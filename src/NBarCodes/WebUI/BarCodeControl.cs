using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.HtmlControls;

namespace NBarCodes.WebUI {

  /// <include file='Documentation\BarCodeControl.xml' path='Documentation[@for="BarCodeControl"]'/>
  [DefaultProperty("Data"),
   Description("Web control to render barcodes."), 
   Designer(typeof(BarCodeControlDesigner)),
   ToolboxData("<{0}:BarCodeControl runat=\"server\"></{0}:BarCodeControl>"),
   PersistChildren(false), ParseChildren(true)]
  public class BarCodeControl : Control, IBarCodeSettings {
    private BarCodeGenerator _generator;

    /// <summary>
    /// Creates a new instance of the <see cref="BarCodeControl"/> class.
    /// Will validate the license of the control.
    /// </summary>
    public BarCodeControl() {
      //LicenseManager.Validate(typeof(BarCodeControl));

      _generator = new BarCodeGenerator(this);
    }

    #region Properties

    /// <summary>
    /// The type of the barcode to render.
    /// </summary>
    [Description("The type of barcode to render."),
     Category("Appearance"), DefaultValue(BarCodeType.Code128)]
    public BarCodeType Type {
      get {
        object o = ViewState["Type"];
        return o == null ? BarCodeType.Code128 : (BarCodeType)o;
      }
      set {
        ViewState["Type"] = value;
      }
    }

    /// <summary>
    /// The data to render in the barcode.
    /// </summary>
    [Bindable(true), Category("Appearance"), DefaultValue("12345"), 
     Description("The data to render in the barcode.")]
    public string Data {
      get {
        object o = ViewState["Data"];
        return o == null ? "12345" : (string)o;
      }
      set { 
        if (value == null) {
          throw new ArgumentNullException();
        }
        ViewState["Data"] = value;
      }
    }

    /// <summary>
    /// The unit to use when rendering the barcode. Affects all sizing properties.
    /// </summary>
    [Description("The unit to use when rendering the barcode. Affects all sizing properties."), 
    Category("Appearance"), 
    RefreshProperties(RefreshProperties.All),
    DefaultValue(BarCodeUnit.Pixel)] 
    public BarCodeUnit Unit {
      get { 
        object o = ViewState["Unit"];
        return o == null ? BarCodeUnit.Pixel : (BarCodeUnit)o;
      }
      set { 
        BarCodeUnit oldUnit = Unit;
        ViewState["Unit"] = value; 
        if (oldUnit != value) {
          _generator.ConvertValues(oldUnit, value);
        }
      }
    }

    /// <summary>
    /// The DPi (dots per inch) to use when rendering the barcode. Affects all sizing properties.
    /// </summary>
    [Description("The DPI (dots per inch) to use when rendering the barcode. Affects all sizing properties."),
    Category("Appearance"),
    RefreshProperties(RefreshProperties.All)]
    public int Dpi {
      get {
        object o = ViewState["DPI"];
        return o == null ? UnitConverter.ScreenDpi : (int)o;
      }
      set {
        int oldDpi = Dpi;
        ViewState["DPI"] = value;
        if (oldDpi != value) {
          _generator.ConvertDpi(oldDpi, value);
        }
      }
    }
    
    /// <summary>
    /// The back color of the barcode.
    /// </summary>
    [TypeConverter(typeof(ColorConverter)), Bindable(true), Category("Appearance"), DefaultValue(typeof(Color), "White"), Description("The back color of the barcode.")]
    public Color BackColor { 
      get { 
        object o = ViewState["BackColor"];
        return o == null ? Color.White : (Color)o;
      }
      set { 
        ViewState["BackColor"] = value; 
      }
    }

    /// <summary>
    /// The color of the bar of the barcode.
    /// </summary>
    [TypeConverter(typeof(ColorConverter)), Bindable(true), Category("Appearance"), DefaultValue(typeof(Color), "Black"), Description("The color of the bar of the barcode.")]
    public Color BarColor {
      get { 
        object o = ViewState["BarColor"];
        return o == null ? Color.Black : (Color)o;
      }
      set { 
        ViewState["BarColor"] = value; 
      }
    }

    /// <summary>
    /// The height of the bar.
    /// </summary>
    [Description("The height of the bar."), Category("Appearance"), Bindable(true), DefaultValue(50f)]
    public float BarHeight {
      get { 
        object o = ViewState["BarHeight"];
        return o == null ? 50f : (float)o;
      }
      set {
        ViewState["BarHeight"] = value; 
      }
    }

    /// <summary>
    /// The font color of the barcode.
    /// </summary>
    [TypeConverter(typeof(ColorConverter)), Bindable(true), Category("Appearance"), DefaultValue(typeof(Color), "Black"), Description("The font color of the barcode.")]
    public Color FontColor { 
      get { 
        object o = ViewState["FontColor"];
        return o == null ? Color.Black : (Color)o;
      }
      set { 
        ViewState["FontColor"] = value; 
      }
    }

    /// <summary>
    /// The extra height of the guard on EAN or UPC barcodes.
    /// </summary>
    [Description("The extra height of the guard on EAN or UPC barcodes."), Category("Appearance"), Bindable(true), DefaultValue(10f)]
    public float GuardExtraHeight {
      get { 
        object o = ViewState["GuardExtraHeight"];
        return o == null ? 10f : (float)o;
      }
      set {
        ViewState["GuardExtraHeight"] = value; 
      }
    }

    /// <summary>
    /// The width of a bar in module-based barcodes.
    /// </summary>
    [Description("The width of a bar in module-based barcodes."), Category("Appearance"), Bindable(true), DefaultValue(1f)]
    public float ModuleWidth {
      get { 
        object o = ViewState["ModuleWidth"];
        return o == null ? 1f : (float)o;
      }
      set {
        ViewState["ModuleWidth"] = value; 
      }
    }

    /// <summary>
    /// The width of a narrow bar in thickness-based barcodes.
    /// </summary>
    [Description("The width of a narrow bar in thickness-based barcodes."), Category("Appearance"), Bindable(true), DefaultValue(1f)]
    public float NarrowWidth {
      get { 
        object o = ViewState["NarrowWidth"];
        return o == null ? 1f : (float)o;
      }
      set {
        ViewState["NarrowWidth"] = value; 
      }
    }

    /// <summary>
    /// The width of a wide bar in thickness-based barcodes.
    /// </summary>
    [Description("The width of a wide bar in thickness-based barcodes."), Category("Appearance"), Bindable(true), DefaultValue(3f)]
    public float WideWidth {
      get { 
        object o = ViewState["WideWidth"];
        return o == null ? 3f : (float)o;
      }
      set {
        ViewState["WideWidth"] = value; 
      }
    }

    /// <summary>
    /// The height offset of the barcode.
    /// </summary>
    [Description("The height offset of the barcode."), Category("Appearance"), Bindable(true), DefaultValue(5f)]
    public float OffsetHeight {
      get { 
        object o = ViewState["OffsetHeight"];
        return o == null ? 5f : (float)o;
      }
      set {
        ViewState["OffsetHeight"] = value; 
      }
    }

    /// <summary>
    /// The width offset of the barcode.
    /// </summary>
    [Description("The width offset of the barcode."), Category("Appearance"), Bindable(true), DefaultValue(5f)]
    public float OffsetWidth {
      get { 
        object o = ViewState["OffsetWidth"];
        return o == null ? 5f : (float)o;
      }
      set {
        ViewState["OffsetWidth"] = value; 
      }
    }

    /// <summary>
    /// The quiet zone for the barcode.
    /// </summary>
    [Description("The quiet zone for the barcode."), Category("Appearance"), Bindable(true), DefaultValue(0f)]
    public float QuietZone {
      get { 
        object o = ViewState["QuietZone"];
        return o == null ? 0f : (float)o;
      }
      set {
        ViewState["QuietZone"] = value; 
      }
    }

    /// <summary>
    /// The font of the barcode text.
    /// </summary>
    [DefaultValue(typeof(Font), "Verdana, 8pt"), Description("The font of the barcode text."), Category("Appearance")]
    public Font Font {
      get { 
        object o = ViewState["Font"];
        if (o == null) {
          return new Font("verdana", 8);
        }
        return (Font)o;
      }
      set {
        ViewState["Font"] = value; 
      }
    }

    /// <summary>
    /// The position of the text.
    /// </summary>
    [Description("The position of the text."), Category("Appearance"), Bindable(true), DefaultValue(TextPosition.Bottom)]
    public TextPosition TextPosition {
      get { 
        object o = ViewState["TextPosition"];
        return o == null ? TextPosition.Bottom : (TextPosition)o;
      }
      set {
        ViewState["TextPosition"] = value; 
      }
    }

    /// <summary>
    /// Whether to use a checksum on barcodes where it is optional.
    /// </summary>
    [Description("Whether to use a checksum on barcodes where it is optional."), Category("Behavior"), Bindable(true), DefaultValue(false)]
    public bool UseChecksum {
      get { 
        object o = ViewState["UseChecksum"];
        return o == null ? false : (bool)o;
      }
      set {
        ViewState["UseChecksum"] = value; 
      }
    }

    #endregion

    /// <summary>
    /// Barcode HTTP handler URL.
    /// </summary>
    [Category("Behavior"), DefaultValue("BarCodeHandler.axd")]
    [Description("Barcode HTTP handler URL."), Editor(typeof(UrlEditor), typeof(UITypeEditor))]
    public string BarCodeHandlerUrl {
      get {
        if (_barCodeHandlerUrl == null || _barCodeHandlerUrl.Trim().Length == 0) {
          return "BarCodeHandler.axd";
        }
        return _barCodeHandlerUrl;
      }
      set {
        _barCodeHandlerUrl = value; 
      }
    } private string _barCodeHandlerUrl;

    /// <summary> 
    /// Render this control to the output parameter specified.
    /// </summary>
    /// <param name="output">The HTML writer to write out to.</param>
    protected override void Render(HtmlTextWriter output) {
      RenderImageTag(output);
    }

    private void RenderImageTag(HtmlTextWriter output) {
      BarCodeHtmlHelper.RenderImageTag(output, this, BarCodeHandlerUrl);
    }

  }
}
