using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Text;

namespace NBarCodes {

  /// <summary>
  /// Operates on <see cref="IBarCodeSettings"/>
  /// to provide barcode rendering and conversion services.
  /// </summary>
  [Serializable]
  public sealed class BarCodeGenerator {

    /// <summary>
    /// Creates a new instance of the <see cref="BarCodeGenerator"/> class.
    /// </summary>
    /// <param name="settings">The settings to use.</param>
    /// <exception cref="ArgumentNullException">
    /// If the settings passed are <c>null</c>.
    /// </exception>
    public BarCodeGenerator(IBarCodeSettings settings) {
      if (settings == null) {
        throw new ArgumentNullException("settings");
      }

      _settings = settings;
    }

    #region BarCode Assembling

    /// <summary>
    /// Tests if the barcode would render with the given settings (<see cref="IBarCodeSettings"/>), without format errors.
    /// </summary>
    /// <param name="errorMessage">Outputs the error message if a <see cref="BarCodeFormatException"/> was thrown.</param>
    /// <returns><c>True</c> if the barcode rendering test went ok, <c>false</c> otherwise.</returns>
    public bool TestRender(out string errorMessage) {
      BarCode bc = AssembleBarCode();
      return bc.TestRender(_settings.Data, out errorMessage);
    }

    /// <summary>
    /// Generates an image with the rendered barcode based on the settings (<see cref="IBarCodeSettings"/>).
    /// </summary>
    /// <returns>The generated barcode image.</returns>
    /// <exception cref="BarCodeFormatException">
    /// If the data in the settings can't be rendered by the selected barcode.
    /// </exception>
    public Image GenerateImage() {
      BarCode bc = AssembleBarCode();
      return bc.DrawImage(_settings.Data);
    }

    /// <summary>
    /// Instantiates the barcode type asked by the settings.
    /// </summary>
    /// <returns>The instantiated barcode type.</returns>
    /// <exception cref="InvalidOperationException">
    /// If the settings contain an invalid barcode type.
    /// </exception>
    private BarCode GetBarCode() {
      switch (_settings.Type) {
        case BarCodeType.Standard25: return new Standard25();
        case BarCodeType.Interleaved25: return new Interleaved25();
        case BarCodeType.Code39: return new Code39();
        case BarCodeType.Code128: return new Code128();
        case BarCodeType.Ean8: return new Ean8();
        case BarCodeType.Ean13: return new Ean13();
        case BarCodeType.Upca: return new Upca();
        case BarCodeType.Upce: return new Upce();
        case BarCodeType.PostNet: return new PostNet();
      }

      throw new InvalidOperationException(string.Format("Cannot convert '{0}' to type BarCode.", _settings.Type));
    }

    /// <summary>
    /// Instantiates and sets the barcode properties based on the settings.
    /// </summary>
    /// <returns>Instantiated and set up barcode.</returns>
    private BarCode AssembleBarCode() {
      BarCode barCode = GetBarCode();
      barCode.Unit = _settings.Unit;
      barCode.Dpi = _settings.Dpi;
      barCode.BarHeight = _settings.BarHeight;
      barCode.OffsetWidth = _settings.OffsetWidth;
      barCode.OffsetHeight = _settings.OffsetHeight;
      barCode.TextPosition = _settings.TextPosition;
      barCode.QuietZone = _settings.QuietZone;
      barCode.BarColor = _settings.BarColor;
      barCode.BackColor = _settings.BackColor;
      barCode.FontColor = _settings.FontColor;
      barCode.Font = _settings.Font;
      if (barCode is IOptionalChecksum) {
        ((IOptionalChecksum)barCode).UseChecksum = _settings.UseChecksum;
      }
      if (barCode is EanUpc) {
        ((EanUpc)barCode).GuardExtraHeight = _settings.GuardExtraHeight;
      }
      if (barCode is ModuleBarCode) {
        ((ModuleBarCode)barCode).ModuleWidth = _settings.ModuleWidth;
      }
      if (barCode is ThicknessBarCode) {
        ((ThicknessBarCode)barCode).NarrowWidth = _settings.NarrowWidth;
        ((ThicknessBarCode)barCode).WideWidth = _settings.WideWidth;
      }
      return barCode;
    }

    #endregion

    /// <summary>
    /// Copy settings between <see cref="IBarCodeSettings"/>.
    /// </summary>
    /// <param name="settingsToCopyFrom">Settings to copy from.</param>
    /// <param name="settingsToCopyTo">Settings to copy to.</param>
    public static void CopySettings(IBarCodeSettings settingsToCopyFrom, IBarCodeSettings settingsToCopyTo) {
      settingsToCopyTo.Type = settingsToCopyFrom.Type;
      settingsToCopyTo.Data = settingsToCopyFrom.Data;
      settingsToCopyTo.BackColor = settingsToCopyFrom.BackColor;
      settingsToCopyTo.BarColor = settingsToCopyFrom.BarColor;
      settingsToCopyTo.BarHeight = settingsToCopyFrom.BarHeight;
      settingsToCopyTo.FontColor = settingsToCopyFrom.FontColor;
      settingsToCopyTo.GuardExtraHeight = settingsToCopyFrom.GuardExtraHeight;
      settingsToCopyTo.ModuleWidth = settingsToCopyFrom.ModuleWidth;
      settingsToCopyTo.NarrowWidth = settingsToCopyFrom.NarrowWidth;
      settingsToCopyTo.WideWidth = settingsToCopyFrom.WideWidth;
      settingsToCopyTo.OffsetHeight = settingsToCopyFrom.OffsetHeight;
      settingsToCopyTo.OffsetWidth = settingsToCopyFrom.OffsetWidth;
      settingsToCopyTo.QuietZone = settingsToCopyFrom.QuietZone;
      settingsToCopyTo.Font = settingsToCopyFrom.Font;
      settingsToCopyTo.TextPosition = settingsToCopyFrom.TextPosition;
      settingsToCopyTo.UseChecksum = settingsToCopyFrom.UseChecksum;
      settingsToCopyTo.Unit = settingsToCopyFrom.Unit;
    }

    /// <summary>
    /// Converts and stores the settings' convertible properties to another unit of measure.
    /// </summary>
    /// <param name="fromUnit">The unit the settings properties are.</param>
    /// <param name="toUnit">the unit to convert to.</param>
    public void ConvertValues(BarCodeUnit fromUnit, BarCodeUnit toUnit) {
      int dpi = _settings.Dpi;
      _settings.BarHeight = UnitConverter.Convert(_settings.BarHeight, fromUnit, toUnit, dpi);
      _settings.GuardExtraHeight = UnitConverter.Convert(_settings.GuardExtraHeight, fromUnit, toUnit, dpi);
      _settings.ModuleWidth = UnitConverter.Convert(_settings.ModuleWidth, fromUnit, toUnit, dpi);
      _settings.NarrowWidth = UnitConverter.Convert(_settings.NarrowWidth, fromUnit, toUnit, dpi);
      _settings.WideWidth = UnitConverter.Convert(_settings.WideWidth, fromUnit, toUnit, dpi);
      _settings.OffsetHeight = UnitConverter.Convert(_settings.OffsetHeight, fromUnit, toUnit, dpi);
      _settings.OffsetWidth = UnitConverter.Convert(_settings.OffsetWidth, fromUnit, toUnit, dpi);
      _settings.QuietZone = UnitConverter.Convert(_settings.QuietZone, fromUnit, toUnit, dpi);
    }

    /// <summary>
    /// Converts the DPI for all convertible properties.
    /// <param name="fromDpi">DPI to convert from.</param>
    /// <param name="toDpi">DPI to convert to.</param>
    /// </summary>
    public void ConvertDpi(int fromDpi, int toDpi) {
      var unit = _settings.Unit;
      _settings.BarHeight = UnitConverter.ConvertDpi(_settings.BarHeight, unit, fromDpi, toDpi);
      _settings.GuardExtraHeight = UnitConverter.ConvertDpi(_settings.GuardExtraHeight, unit, fromDpi, toDpi);
      _settings.ModuleWidth = UnitConverter.ConvertDpi(_settings.ModuleWidth, unit, fromDpi, toDpi);
      _settings.NarrowWidth = UnitConverter.ConvertDpi(_settings.NarrowWidth, unit, fromDpi, toDpi);
      _settings.WideWidth = UnitConverter.ConvertDpi(_settings.WideWidth, unit, fromDpi, toDpi);
      _settings.OffsetHeight = UnitConverter.ConvertDpi(_settings.OffsetHeight, unit, fromDpi, toDpi);
      _settings.OffsetWidth = UnitConverter.ConvertDpi(_settings.OffsetWidth, unit, fromDpi, toDpi);
      _settings.QuietZone = UnitConverter.ConvertDpi(_settings.QuietZone, unit, fromDpi, toDpi);
    }

    IBarCodeSettings _settings;
  }
}
