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

      float temp = 0;
      int dpi = 96;

      // cm to cm
      AssertFloat(234, UnitConverter.Convert(234, BarCodeUnit.Centimeter, BarCodeUnit.Centimeter, dpi));
      // cm to mm
      AssertFloat(2340, UnitConverter.Convert(234, BarCodeUnit.Centimeter, BarCodeUnit.Millimeter, dpi));
      // cm to in
      AssertFloat(100, UnitConverter.Convert(254, BarCodeUnit.Centimeter, BarCodeUnit.Inch, dpi));
      // cm to px
      temp = UnitConverter.Convert(1, BarCodeUnit.Inch, BarCodeUnit.Centimeter, dpi);
      AssertFloat(dpi, UnitConverter.Convert(temp, BarCodeUnit.Centimeter, BarCodeUnit.Pixel, dpi));

      // mm to cm
      AssertFloat(234.5f, UnitConverter.Convert(2345, BarCodeUnit.Millimeter, BarCodeUnit.Centimeter, dpi));
      // mm to mm
      AssertFloat(2345, UnitConverter.Convert(2345, BarCodeUnit.Millimeter, BarCodeUnit.Millimeter, dpi));
      // mm to in
      AssertFloat(92.322835f, UnitConverter.Convert(2345, BarCodeUnit.Millimeter, BarCodeUnit.Inch, dpi));
      // mm to px
      temp = UnitConverter.Convert(1, BarCodeUnit.Inch, BarCodeUnit.Millimeter, dpi);
      AssertFloat(dpi, UnitConverter.Convert(temp, BarCodeUnit.Millimeter, BarCodeUnit.Pixel, dpi));
      
      // in to cm
      AssertFloat(1432.56f, UnitConverter.Convert(564, BarCodeUnit.Inch, BarCodeUnit.Centimeter, dpi));
      // in to mm
      AssertFloat(14325.6f, UnitConverter.Convert(564, BarCodeUnit.Inch, BarCodeUnit.Millimeter, dpi));
      // in to in
      AssertFloat(14325.6f, UnitConverter.Convert(14325.6f, BarCodeUnit.Inch, BarCodeUnit.Inch, dpi));
      // in to px
      AssertFloat(dpi, UnitConverter.Convert(1, BarCodeUnit.Inch, BarCodeUnit.Pixel, dpi));
      
      // px to cm
      temp = UnitConverter.Convert(dpi, BarCodeUnit.Pixel, BarCodeUnit.Centimeter, dpi);
      AssertFloat(1, UnitConverter.Convert(temp, BarCodeUnit.Centimeter, BarCodeUnit.Inch, dpi));
      // px to mm
      temp = UnitConverter.Convert(dpi, BarCodeUnit.Pixel, BarCodeUnit.Millimeter, dpi);
      AssertFloat(1, UnitConverter.Convert(temp, BarCodeUnit.Millimeter, BarCodeUnit.Inch, dpi));
      // px to in
      AssertFloat(1, UnitConverter.Convert(dpi, BarCodeUnit.Pixel, BarCodeUnit.Inch, dpi));
      // px to px
      AssertFloat(123, UnitConverter.Convert(123, BarCodeUnit.Pixel, BarCodeUnit.Pixel, dpi));

      dpi = 312;

      // cm to px
      temp = UnitConverter.Convert(1, BarCodeUnit.Inch, BarCodeUnit.Centimeter, dpi);
      AssertFloat(dpi, UnitConverter.Convert(temp, BarCodeUnit.Centimeter, BarCodeUnit.Pixel, dpi));
      // mm to px
      temp = UnitConverter.Convert(1, BarCodeUnit.Inch, BarCodeUnit.Millimeter, dpi);
      AssertFloat(dpi, UnitConverter.Convert(temp, BarCodeUnit.Millimeter, BarCodeUnit.Pixel, dpi));
      // in to px
      AssertFloat(dpi, UnitConverter.Convert(1, BarCodeUnit.Inch, BarCodeUnit.Pixel, dpi));

      // px to cm
      temp = UnitConverter.Convert(dpi, BarCodeUnit.Pixel, BarCodeUnit.Centimeter, dpi);
      AssertFloat(1, UnitConverter.Convert(temp, BarCodeUnit.Centimeter, BarCodeUnit.Inch, dpi));
      // px to mm
      temp = UnitConverter.Convert(dpi, BarCodeUnit.Pixel, BarCodeUnit.Millimeter, dpi);
      AssertFloat(1, UnitConverter.Convert(temp, BarCodeUnit.Millimeter, BarCodeUnit.Inch, dpi));
      // px to in
      AssertFloat(1, UnitConverter.Convert(dpi, BarCodeUnit.Pixel, BarCodeUnit.Inch, dpi));

    }

    /// <summary>
    /// Tests DIP conversions.
    /// </summary>
    [Test]
    public void TestDpiConversions() {

      // non-pixel units don't change to maintain the aspect ratio
      AssertFloat(123, UnitConverter.ConvertDpi(123, BarCodeUnit.Inch, 96, 300));
      AssertFloat(123, UnitConverter.ConvertDpi(123, BarCodeUnit.Centimeter, 96, 300));
      AssertFloat(123, UnitConverter.ConvertDpi(123, BarCodeUnit.Millimeter, 96, 300));

      // pixels
      AssertFloat(300, UnitConverter.ConvertDpi(96, BarCodeUnit.Pixel, 96, 300));
      AssertFloat(1035, UnitConverter.ConvertDpi(331, BarCodeUnit.Pixel, 96, 300));

    }

    [Test]
    public void TestScreenDpi() {

      // 60 is for old CRTs, so this might be a safe assumption
      Assert.True(UnitConverter.ScreenDpi > 60);
    
    }

    /// <summary>
    /// Tests chained conversions.
    /// </summary>
    [Test]
    public void TestChainedConversions() {
      float[] values = { 2987645f, 0.0001f, 1f, 10f, 500f, 1.00231f };
      float[] epsilons = { 1f, 0.0001f, 0.0001f, 0.0001f, 0.0001f, 0.0001f };

      int index = 0;
      int dpi = 96;
      foreach (float value in values) {
        float temp = value;
        temp = ChainConversions(temp, dpi, BarCodeUnit.Centimeter, BarCodeUnit.Inch, BarCodeUnit.Millimeter, BarCodeUnit.Centimeter);
        AssertFloat(value, temp, epsilons[index]);

        temp = value;
        temp = ChainConversions(temp, dpi, BarCodeUnit.Inch, BarCodeUnit.Millimeter, BarCodeUnit.Centimeter, BarCodeUnit.Inch, BarCodeUnit.Centimeter, BarCodeUnit.Millimeter, BarCodeUnit.Centimeter, BarCodeUnit.Inch, BarCodeUnit.Centimeter, BarCodeUnit.Inch);
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
    /// <param name="dpi">The DPI to use.</param>
    /// <param name="units">Conversions to apply. The first unit is considered to be the value's current unit.</param>
    /// <returns>Converted value.</returns>
    private float ChainConversions(float value, int dpi, params BarCodeUnit[] units) {
      float temp = value;
      for (int i = 1; i < units.Length; ++i) {
        temp = UnitConverter.Convert(temp, units[i-1], units[i], dpi);
      }
      return temp;
    }

  }

}

