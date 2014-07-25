using System;
using System.Drawing;

namespace NBarCodes {
  
  /// <summary>
  /// Class to convert between units of measure of barcodes 
  /// (<see cref="BarCodeType"/>).
  /// </summary>
  public sealed class UnitConverter {

    private static int _screenDpi = 0;

    /// <summary>The screen DPI, rounded up to the nearest integer.</summary>
    public static int ScreenDpi {
      get {
        if (_screenDpi > 0) return _screenDpi;
        using (Bitmap image = new Bitmap(1, 1))
        using (Graphics g = Graphics.FromImage(image)) {
          return _screenDpi = (int)Math.Ceiling(g.DpiX); // DpiY is assumed to be equal to DpiX
        }
      }
    }

    // conversion ratios
    private const float IN2CM = 2.54f;          // inch to centimeter ratio
    private const float CM2IN = 1/IN2CM;        // centimeter to inch ratio
    private const float CM2MM = 10f;            // centimeter to millimeter ratio
    private const float MM2CM = 1/CM2MM;        // milimeter to centimeter ratio
    private const float IN2MM = 1/CM2IN*CM2MM;  // inch to milimeter ratio
    private const float MM2IN = 1/CM2MM*CM2IN;  // milimeter to inch ratio

    // this table is indexed by the BarCodeUnit values
    private static float[,] ConversionTable = {
    /*        cm        mm        in  */
    /*cm*/  { 1,        CM2MM,    CM2IN }, 
    /*mm*/  { MM2CM,    1,        MM2IN },
    /*in*/  { IN2CM,    IN2MM,    1      }
    };

    /// <summary>Uninstantiatable.</summary>
    private UnitConverter() {}

    /// <summary>
    /// Converts a value between measuring units.
    /// If the target unit is pixels, the converted value is rounded up to the nearest integer.
    /// </summary>
    /// <param name="value">Value to be converted.</param>
    /// <param name="sourceUnit">The unit to convert from.</param>
    /// <param name="targetUnit">The unit to convert to.</param>
    /// <param name="dpi">The DPI to use.</param>
    /// <returns>The converted unit.</returns>
    public static float Convert(float value, BarCodeUnit sourceUnit, BarCodeUnit targetUnit, int dpi) {
      if (dpi <= 0) {
        throw new ArgumentException("DPI must be greater than zero.");
      }

      if (sourceUnit == targetUnit) {
        return value;
      }

      // keep track of the value being converted
      float converted = value;

      int sourceUnitIndex = (int)sourceUnit;
      if (sourceUnit == BarCodeUnit.Pixel) {
        // convert the value from pixels to inches
        converted /= dpi;
        sourceUnitIndex = (int)BarCodeUnit.Inch;
      }

      int targetUnitIndex = (int)targetUnit;
      if (targetUnit == BarCodeUnit.Pixel) {
        // apply the inch to pixel ratio (the DPI)
        converted *= dpi;
        // convert to inches rather than to pixels
        targetUnitIndex = (int)BarCodeUnit.Inch;
      }

      converted *= ConversionTable [sourceUnitIndex, targetUnitIndex];

      if (targetUnit == BarCodeUnit.Pixel) {
        converted = (float)Math.Ceiling(converted);
      }

      return converted;
    }

    /// <summary>
    /// Convert a value to another DPI.
    /// If the value is in a non-pixel unit, the same value is returned to maintain the aspect ratio. 
    /// Pixel units are rounded up to the nearest integer.
    /// </summary>
    /// <param name="value">Value to convert.</param>
    /// <param name="unit">Value unit.</param>
    /// <param name="fromDpi">The current DPI of the value.</param>
    /// <param name="toDpi">The target DPI for conversion.</param>
    /// <returns>The value in the new DPI.</returns>
    public static float ConvertDpi(float value, BarCodeUnit unit, int fromDpi, int toDpi) {
      if (fromDpi <= 0 || toDpi <= 0) {
        throw new ArgumentException("DPIs must be greater than zero.");
      }

      if (unit != BarCodeUnit.Pixel) {
        return value;
      }

      var ratio = toDpi / (float)fromDpi;
      return (float)Math.Ceiling(value * ratio);
    }

  }

}
