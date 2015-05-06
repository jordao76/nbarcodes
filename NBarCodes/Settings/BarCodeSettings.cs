using System;
using System.Drawing;

namespace NBarCodes {

  /// <summary>
  /// Packages settings for barcodes.
  /// Canonical implementation of <see cref="IBarCodeSettings"/>.
  /// </summary>
  [Serializable]
  public class BarCodeSettings : IBarCodeSettings {

    /// <summary>
    /// The type of the barcode.
    /// </summary>
    public BarCodeType Type {
      get { return _type; }
      set { _type = value; }
    } BarCodeType _type = BarCodeType.Code128;

    /// <summary>
    /// The data to render with the barcode.
    /// </summary>
    public string Data {
      get { return _data; }
      set { _data = value; }
    } string _data = "12345";

    /// <summary>
    /// The unit of measure of the barcode's measurable properties.
    /// </summary>
    public BarCodeUnit Unit {
      get { return _unit; }
      set { _unit = value; }
    } BarCodeUnit _unit = Defaults.Unit;

    /// <summary>
    /// The DPI (dots per inch) of the barcode.
    /// </summary>
    public int Dpi {
      get { return _dpi; }
      set { _dpi = value; }
    } int _dpi = Defaults.Dpi;

    /// <summary>
    /// The back color of the barcode.
    /// </summary>
    public Color BackColor {
      get { return _backColor; }
      set { _backColor = value; }
    } Color _backColor = Defaults.BackColor;

    /// <summary>
    /// The color of the bar of the barcode.
    /// </summary>
    public Color BarColor {
      get { return _barColor; }
      set { _barColor = value; }
    } Color _barColor = Defaults.BarColor;

    /// <summary>
    /// The height of the barcode. Affected by <see cref="Unit"/>.
    /// </summary>
    public float BarHeight {
      get { return _barHeight; }
      set { _barHeight = value; }
    } float _barHeight = Defaults.BarHeight;

    /// <summary>
    /// The color of the font to render text in the barcode.
    /// </summary>
    public Color FontColor {
      get { return _fontColor; }
      set { _fontColor = value; }
    } Color _fontColor = Defaults.FontColor;

    /// <summary>
    /// The height of the extra height of the bar for EAN-like barcodes.
    /// Affected by <see cref="Unit"/>.
    /// </summary>
    public float GuardExtraHeight {
      get { return _guardExtraHeight; }
      set { _guardExtraHeight = value; }
    } float _guardExtraHeight = Defaults.GuardExtraHeight;

    /// <summary>
    /// The width of bar of the barcode for module-based barcodes (<see cref="ModuleBarCode"/>).
    /// Affected by <see cref="Unit"/>.
    /// </summary>
    public float ModuleWidth {
      get { return _moduleWidth; }
      set { _moduleWidth = value; }
    } float _moduleWidth = Defaults.ModuleWidth;

    /// <summary>
    /// The width of the narrow component of a thickness-based barcode (<see cref="ThicknessBarCode"/>).
    /// Affected by <see cref="Unit"/>.
    /// </summary>
    public float NarrowWidth {
      get { return _narrowWidth; }
      set { _narrowWidth = value; }
    } float _narrowWidth = Defaults.NarrowWidth;

    /// <summary>
    /// The width of the wide component of a thickness-based barcode (<see cref="ThicknessBarCode"/>).
    /// Affected by <see cref="Unit"/>.
    /// </summary>
    public float WideWidth {
      get { return _wideWidth; }
      set { _wideWidth = value; }
    } float _wideWidth = Defaults.WideWidth;

    /// <summary>
    /// The vertical (top and bottom) offset height of the barcode to the border.
    /// Affected by <see cref="Unit"/>.
    /// </summary>
    public float OffsetHeight {
      get { return _offsetHeight; }
      set { _offsetHeight = value; }
    } float _offsetHeight = Defaults.OffsetHeight;

    /// <summary>
    /// The horizontal (left and right) offset width of the barcode to the border.
    /// Affected by <see cref="Unit"/>.
    /// </summary>
    public float OffsetWidth {
      get { return _offsetWidth; }
      set { _offsetWidth = value; }
    } float _offsetWidth = Defaults.OffsetWidth;

    /// <summary>
    /// The font used to render the text inside the barcode.
    /// </summary>
    public Font Font {
      get { return _font; }
      set { _font = value; }
    } Font _font = Defaults.Font;

    /// <summary>
    /// The position of the text rendered in the barcode.
    /// </summary>
    public TextPosition TextPosition {
      get { return _textPosition; }
      set { _textPosition = value; }
    } TextPosition _textPosition = Defaults.TextPos;

    /// <summary>
    /// Whether the barcode will use an checksum.
    /// Not every barcode requires a checksum, and others mandate it.
    /// </summary>
    public bool UseChecksum {
      get { return _useChecksum; }
      set { _useChecksum = value; }
    } bool _useChecksum = false;

  }
}
