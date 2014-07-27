using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using NBarCodes;

namespace NBarCodes.Forms {

  /// <summary>
  /// Control to render bar codes.
  /// </summary>
  [//ToolboxBitmap(typeof(BarCodeControl), "BarCode.ico"),
    // Code...attribute, not to appear the visual designer
    Designer(typeof(BarCodeControlDesigner)),
    DefaultProperty("Data"),
    Description("Control to render barcodes.")]
  public class BarCodeControl : Control, IBarCodeSettings {
    private Font _errorFont;
    private Brush _errorBrush;
    private BarCodeGenerator _generator;

    /// <summary>
    /// Creates a new instance of the <see cref="BarCodeControl"/>.
    /// </summary>
    public BarCodeControl() {
      _errorFont = new Font("verdana", 16f, FontStyle.Bold);
      _errorBrush = Brushes.Red;
      _generator = new BarCodeGenerator(this);

      BackColor = Defaults.BackColor;
      Font = Defaults.Font;
    }

    #region Properties

    /// <summary>
    /// The type of barcode to render.
    /// </summary>
    [Description("The type of barcode to render."),
      Category("Appearance"), DefaultValue(BarCodeType.Code128)]
    public BarCodeType Type {
      get {
        return _type;
      }
      set {
        _type = value;
        Refresh();
      }
    } BarCodeType _type = BarCodeType.Code128;

    /// <summary>
    /// The data to render in the barcode.
    /// </summary>
    [Bindable(true), Category("Appearance"), DefaultValue("12345"), 
      Description("The data to render in the barcode.")]
    public string Data {
      get {
        return _data;
      }
      set { 
        if (value == null) {
          throw new ArgumentNullException();
        }
        _data = value;
        Refresh();
      }
    } string _data = "12345";

    /// <summary>
    /// The unit to use when rendering the barcode. Affects all sizing properties.
    /// </summary>
    [Description("The unit to use when rendering the barcode. Affects all sizing properties."), 
      Category("Appearance"), 
      RefreshProperties(RefreshProperties.All),
      DefaultValue(Defaults.Unit)] 
    public BarCodeUnit Unit {
      get { return _unit; }
      set { 
        BarCodeUnit oldUnit = _unit;
        _unit = value; 
        if (oldUnit != _unit) {
          _generator.ConvertValues(oldUnit, _unit);
          Refresh();
        }
      }
    } BarCodeUnit _unit = Defaults.Unit;

    /// <summary>
    /// The DPI (dots per inch) to use when rendering the barcode. Affects all sizing properties.
    /// </summary>
    [Description("The DPI (dots per inch) to use when rendering the barcode. Affects all sizing properties."),
      Category("Appearance"),
      RefreshProperties(RefreshProperties.All)]
    public int Dpi {
      get { return _dpi; }
      set {
        int oldDpi = _dpi;
        _dpi = value;
        if (oldDpi != _dpi) {
          _generator.ConvertDpi(oldDpi, _dpi);
          Refresh();
        }
      }
    } int _dpi = Defaults.Dpi;

    /// <summary>
    /// The back color of the barcode.
    /// </summary>
    [TypeConverter(typeof(ColorConverter)), Bindable(true), Category("Appearance"), DefaultValue(typeof(Color), Defaults.BackColorName), Description("The back color of the barcode.")]
    public override Color BackColor { 
      get { return base.BackColor; } 
      set { base.BackColor = value; }
    }

    /// <summary>
    /// The color of the bar of the barcode.
    /// </summary>
    [TypeConverter(typeof(ColorConverter)), Bindable(true), Category("Appearance"), DefaultValue(typeof(Color), Defaults.BarColorName), Description("The color of the bar of the barcode.")]
    public Color BarColor {
      get { return _barColor; } 
      set { 
        _barColor = value; 
        Refresh();
      }
    } Color _barColor = Defaults.BarColor;

    /// <summary>
    /// The height of the bar.
    /// </summary>
    [Description("The height of the bar."), Category("Appearance"), Bindable(true), DefaultValue(Defaults.BarHeight)]
    public float BarHeight {
      get { return _barHeight; } 
      set { 
        _barHeight = value; 
        Refresh();
      }
    } float _barHeight = Defaults.BarHeight;

    /// <summary>
    /// The font color of the barcode.
    /// </summary>
    [TypeConverter(typeof(ColorConverter)), Bindable(true), Category("Appearance"), DefaultValue(typeof(Color), Defaults.FontColorName), Description("The font color of the barcode.")]
    public Color FontColor { 
      get { return _fontColor; } 
      set { 
        _fontColor = value; 
        Refresh();
      }
    } Color _fontColor = Defaults.FontColor;

    /// <summary>
    /// The extra height of the guard on EAN or UPC barcodes.
    /// </summary>
    [Description("The extra height of the guard on EAN or UPC barcodes."), Category("Appearance"), Bindable(true), DefaultValue(Defaults.GuardExtraHeight)]
    public float GuardExtraHeight {
      get { return _guardExtraHeight; } 
      set { 
        _guardExtraHeight = value; 
        Refresh();
      }
    } float _guardExtraHeight = Defaults.GuardExtraHeight;

    /// <summary>
    /// The width of a bar in module-based barcodes.
    /// </summary>
    [Description("The width of a bar in module-based barcodes."), Category("Appearance"), Bindable(true), DefaultValue(Defaults.ModuleWidth)]
    public float ModuleWidth {
      get { return _moduleWidth; } 
      set { 
        _moduleWidth = value; 
        Refresh();
      }
    } float _moduleWidth = Defaults.ModuleWidth;

    /// <summary>
    /// The width of a narrow bar in thickness-based barcodes.
    /// </summary>
    [Description("The width of a narrow bar in thickness-based barcodes."), Category("Appearance"), Bindable(true), DefaultValue(Defaults.NarrowWidth)]
    public float NarrowWidth {
      get { return _narrowWidth; } 
      set { 
        _narrowWidth = value; 
        Refresh();
      }
    } float _narrowWidth = Defaults.NarrowWidth;

    /// <summary>
    /// The width of a wide bar in thickness-based barcodes.
    /// </summary>
    [Description("The width of a wide bar in thickness-based barcodes."), Category("Appearance"), Bindable(true), DefaultValue(Defaults.WideWidth)]
    public float WideWidth {
      get { return _wideWidth; } 
      set { 
        _wideWidth = value; 
        Refresh();
      }
    } float _wideWidth = Defaults.WideWidth;

    /// <summary>
    /// The height offset of the barcode.
    /// </summary>
    [Description("The height offset of the barcode."), Category("Appearance"), Bindable(true), DefaultValue(Defaults.OffsetHeight)]
    public float OffsetHeight {
      get { return _offsetHeight; } 
      set { 
        _offsetHeight = value; 
        Refresh();
      }
    } float _offsetHeight = Defaults.OffsetHeight;

    /// <summary>
    /// The width offset of the barcode.
    /// </summary>
    [Description("The width offset of the barcode."), Category("Appearance"), Bindable(true), DefaultValue(Defaults.OffsetWidth)]
    public float OffsetWidth {
      get { return _offsetWidth; } 
      set { 
        _offsetWidth = value; 
        Refresh();
      }
    } float _offsetWidth = Defaults.OffsetWidth;

    /// <summary>
    /// The font of the barcode text.
    /// </summary>
    [DefaultValue(typeof(Font), Defaults.FontName), Description("The font of the barcode text."), Category("Appearance")]
    public override Font Font {
      get { return base.Font; } 
      set { 
        base.Font = value; 
        Refresh();
      }
    }

    /// <summary>
    /// The position of the text.
    /// </summary>
    [Description("The position of the text."), Category("Appearance"), Bindable(true), DefaultValue(Defaults.TextPos)]
    public TextPosition TextPosition {
      get { return _textPosition; } 
      set { 
        _textPosition = value; 
        Refresh();
      }
    } TextPosition _textPosition = Defaults.TextPos;

    /// <summary>
    /// Whether to use a checksum on barcodes where it is optional.
    /// </summary>
    [Description("Whether to use a checksum on barcodes where it is optional."), Category("Behavior"), Bindable(true), DefaultValue(false)]
    public bool UseChecksum {
      get { return _useChecksum; } 
      set { 
        _useChecksum = value; 
        Refresh();
      }
    } bool _useChecksum = false;

    #endregion

    /// <summary>
    /// Renders the barcode.
    /// </summary>
    /// <param name="e">Paint event arguments.</param>
    protected override void OnPaint(PaintEventArgs e) {
      DrawBarCode(e.Graphics);
    }

    /// <summary>
    /// Draws the barcode in the canvas passed.
    /// </summary>
    /// <param name="canvas">Canvas to draw barcode into.</param>
    public void DrawBarCode(Graphics canvas) {
      string errorMessage;
      if (_generator.TestRender(out errorMessage)) {
        using (var barCodeImage = _generator.GenerateImage()) {
          Size = new Size(barCodeImage.Width, barCodeImage.Height);
          canvas.DrawImage(barCodeImage, 0, 0);
        }
      }
      else {
        Size = new Size(250, 50);
        canvas.DrawString(errorMessage, _errorFont, _errorBrush, new RectangleF(0, 0, 250, 50));
      }
    }
  }
}
