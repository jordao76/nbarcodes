using System;
using System.Collections;
using System.Diagnostics;
using NUnit.Framework;

namespace NBarCodes.Tests {

  /// <summary>
  /// Tests for the <see cref="UnitConverter"/> class.
  /// </summary>
  [TestFixture]
  public class UnitConverterTest {

    /// <summary>
    /// Tests various conversions.
    /// </summary>
    [Test]
    public void TestConversions() {
      // tests conversions between cm, mm, inches and pixels
      // with the default 96 DPI, 1 inch = 96 pixels

      float temp = 0;
      float defaultDpi = 96;

      // cm to cm
      AssertFloat(234, UnitConverter.Convert(234, BarCodeUnit.Centimeter, BarCodeUnit.Centimeter));
      // cm to mm
      AssertFloat(2340, UnitConverter.Convert(234, BarCodeUnit.Centimeter, BarCodeUnit.Millimeter));
      // cm to in
      AssertFloat(100, UnitConverter.Convert(254, BarCodeUnit.Centimeter, BarCodeUnit.Inch));
      // cm to px
      temp = UnitConverter.Convert(1, BarCodeUnit.Inch, BarCodeUnit.Centimeter);
      AssertFloat(defaultDpi, UnitConverter.Convert(temp, BarCodeUnit.Centimeter, BarCodeUnit.Pixel));
      // mm to cm
      AssertFloat(234.5f, UnitConverter.Convert(2345, BarCodeUnit.Millimeter, BarCodeUnit.Centimeter));
      // mm to mm
      AssertFloat(2345, UnitConverter.Convert(2345, BarCodeUnit.Millimeter, BarCodeUnit.Millimeter));
      // mm to in
      AssertFloat(92.322835f, UnitConverter.Convert(2345, BarCodeUnit.Millimeter, BarCodeUnit.Inch));
      // mm to px
      temp = UnitConverter.Convert(1, BarCodeUnit.Inch, BarCodeUnit.Millimeter);
      AssertFloat(defaultDpi, UnitConverter.Convert(temp, BarCodeUnit.Millimeter, BarCodeUnit.Pixel));
      // in to cm
      AssertFloat(1432.56f, UnitConverter.Convert(564, BarCodeUnit.Inch, BarCodeUnit.Centimeter));
      // in to mm
      AssertFloat(14325.6f, UnitConverter.Convert(564, BarCodeUnit.Inch, BarCodeUnit.Millimeter));
      // in to in
      AssertFloat(14325.6f, UnitConverter.Convert(14325.6f, BarCodeUnit.Inch, BarCodeUnit.Inch));
      // in to px
      AssertFloat(defaultDpi, UnitConverter.Convert(1, BarCodeUnit.Inch, BarCodeUnit.Pixel));
      // px to cm
      temp = UnitConverter.Convert(defaultDpi, BarCodeUnit.Pixel, BarCodeUnit.Centimeter);
      AssertFloat(1, UnitConverter.Convert(temp, BarCodeUnit.Centimeter, BarCodeUnit.Inch));
      // px to mm
      temp = UnitConverter.Convert(defaultDpi, BarCodeUnit.Pixel, BarCodeUnit.Millimeter);
      AssertFloat(1, UnitConverter.Convert(temp, BarCodeUnit.Millimeter, BarCodeUnit.Inch));
      // px to in
      AssertFloat(1, UnitConverter.Convert(defaultDpi, BarCodeUnit.Pixel, BarCodeUnit.Inch));
      // px to px
      AssertFloat(123, UnitConverter.Convert(123, BarCodeUnit.Pixel, BarCodeUnit.Pixel));
    }

    /// <summary>
    /// Tests various conversions with different DPIs.
    /// </summary>
    [Test]
    public void TestConversionsWithDpi() {
      float dpi = 312;
      
      float temp = 0;

      // cm to px
      temp = UnitConverter.Convert(1, BarCodeUnit.Inch, BarCodeUnit.Centimeter);
      AssertFloat(dpi, UnitConverter.Convert(temp, BarCodeUnit.Centimeter, BarCodeUnit.Pixel, dpi));
      // mm to px
      temp = UnitConverter.Convert(1, BarCodeUnit.Inch, BarCodeUnit.Millimeter);
      AssertFloat(dpi, UnitConverter.Convert(temp, BarCodeUnit.Millimeter, BarCodeUnit.Pixel, dpi));
      // in to px
      AssertFloat(dpi, UnitConverter.Convert(1, BarCodeUnit.Inch, BarCodeUnit.Pixel, dpi));
      // px to cm
      temp = UnitConverter.Convert(dpi, BarCodeUnit.Pixel, BarCodeUnit.Centimeter, dpi);
      AssertFloat(1, UnitConverter.Convert(temp, BarCodeUnit.Centimeter, BarCodeUnit.Inch));
      // px to mm
      temp = UnitConverter.Convert(dpi, BarCodeUnit.Pixel, BarCodeUnit.Millimeter, dpi);
      AssertFloat(1, UnitConverter.Convert(temp, BarCodeUnit.Millimeter, BarCodeUnit.Inch));
      // px to in
      AssertFloat(1, UnitConverter.Convert(dpi, BarCodeUnit.Pixel, BarCodeUnit.Inch, dpi));
    }

    /// <summary>
    /// Tests chained conversions.
    /// </summary>
    [Test]
    public void TestChainedConversions() {
      float[] values = { 2987645f, 0.0001f, 1f, 10f, 500f, 1.00231f };
      float[] epsilons = { 1f, 0.0001f, 0.0001f, 0.0001f, 0.0001f, 0.0001f };

      int index = 0;
      foreach (float value in values) {
        float temp = value;
        temp = ChainConversions(temp, BarCodeUnit.Centimeter, BarCodeUnit.Inch, BarCodeUnit.Millimeter, BarCodeUnit.Pixel, BarCodeUnit.Centimeter);
        AssertFloat(value, temp, epsilons[index]);

        temp = value;
        temp = ChainConversions(temp, BarCodeUnit.Inch, BarCodeUnit.Millimeter, BarCodeUnit.Centimeter, BarCodeUnit.Inch, BarCodeUnit.Centimeter, BarCodeUnit.Millimeter, BarCodeUnit.Pixel, BarCodeUnit.Centimeter, BarCodeUnit.Inch, BarCodeUnit.Centimeter, BarCodeUnit.Pixel, BarCodeUnit.Inch);
        AssertFloat(value, temp, epsilons[index]);

        ++index;
      }
    }

    /// <summary>Default interval to consider when comparing floats.</summary>
    const float EPSILON = 0.0002f;

    /// <summary>
    /// Asserts equality of floats taking into consideration possible deviations.
    /// </summary>
    /// <param name="expected">Expected value (plus or minus <see cref="EPSILON"/>).</param>
    /// <param name="actual">Actual value.</param>
    private void AssertFloat(float expected, float actual) {
      AssertFloat(expected, actual, EPSILON);
    }

    /// <summary>
    /// Asserts equality of floats taking into consideration possible deviations.
    /// </summary>
    /// <param name="expected">Expected value (plus or minus epsilon).</param>
    /// <param name="actual">Actual value.</param>
    /// <param name="epsilon">Interval to consider for equality comparison.</param>
    private void AssertFloat(float expected, float actual, float epsilon) {
      // asserts that [ (expected - epsilon) <= actual <= (expected + epsilon) ]
      if (expected - epsilon <= actual && actual <= expected + epsilon) {
        return;
      }
      Assert.Fail("Actual value {0:0.00000} not within bounds of expected value {1:0.00000}", actual, expected);
    }

    /// <summary>
    /// Chains a value through various conversions.
    /// </summary>
    /// <param name="value">Value to convert.</param>
    /// <param name="units">Conversions to apply. The first unit is considered to be the value's current unit.</param>
    /// <returns>Converted value.</returns>
    private float ChainConversions(float value, params BarCodeUnit[] units) {
      float temp = value;
      for (int i = 1; i < units.Length; ++i) {
        temp = UnitConverter.Convert(temp, units[i-1], units[i]);
      }
      return temp;
    }

  }

}

