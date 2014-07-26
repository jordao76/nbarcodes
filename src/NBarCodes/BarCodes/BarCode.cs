using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.ComponentModel;

namespace NBarCodes {

  /// <summary>
  /// Base class for barcodes. Defines common functionality.
  /// </summary>
  /// <remarks>
  /// This is the base class for creating barcodes, it defines common functionality like 
  /// bar measures, text position and colors.
  /// </remarks>
  [TypeConverter(typeof(ExpandableObjectConverter))]
  [Serializable]
  abstract class BarCode {
    private float barHeight = 50f / 96f;
    private float offsetWidth = 5f / 96f;
    private float offsetHeight = 5f / 96f;
    private TextPosition textPosition = TextPosition.None;
    [NonSerialized] private IChecksum checksum = null;
    private Color barColor = Color.Black;
    private Color backColor = Color.White;
    private Color fontColor = Color.Black;
    private Font font = new Font("verdana", 15);
    private BarCodeUnit unit = BarCodeUnit.Inch;
    private int dpi = UnitConverter.ScreenDpi;

    public virtual void ImportSettings(BarCode barCode) {
      barHeight = barCode.barHeight;
      offsetWidth = barCode.offsetWidth;
      offsetHeight = barCode.offsetHeight;
      textPosition = barCode.textPosition;
      barColor = barCode.barColor;
      backColor = barCode.backColor;
      fontColor = barCode.fontColor;
      font = barCode.font;
      if (this is IOptionalChecksum && barCode is IOptionalChecksum) {
        ((IOptionalChecksum)this).UseChecksum = 
          ((IOptionalChecksum)barCode).UseChecksum;
      }
    }

    [DefaultValue(50f / 96f), NotifyParentProperty(true)]
    public float BarHeight {
      get { return barHeight; }
      set { barHeight = value; }
    }

    /// <summary>
    /// Gets or sets the margin offset width.
    /// </summary>
    /// <remarks>
    /// Gets or sets the margin offset width of the barcode.
    /// The side of the barcode (left or right) for which the margin offset width affects depends on the particular
    /// barcode implementation.
    /// </remarks>
    [DefaultValue(5f / 96f), NotifyParentProperty(true)]
    public float OffsetWidth {
      get { return offsetWidth; }
      set { offsetWidth = value; }
    }

    /// <summary>
    /// Gets or sets the margin offset height.
    /// </summary>
    /// <remarks>
    /// Gets or sets the margin offset height of the barcode.
    /// The side of the barcode (top or bottom) for which the margin offset height affects depends on the particular
    /// barcode implementation.
    /// </remarks>
    [DefaultValue(5f / 96f), NotifyParentProperty(true)]
    public float OffsetHeight {
      get { return offsetHeight; }
      set { offsetHeight = value; }
    }

    protected float TotalHeight {
      get { return OffsetHeight * 2 + BarHeight + ExtraHeight; }
    }

    protected virtual float ExtraHeight {
      get { 
        if (TextPosition == TextPosition.All) 
          return TextHeight * 2;
        return TextHeight;
      }
    }

    protected virtual float ExtraTopHeight {
      get { 
        if ((TextPosition & TextPosition.Top) != 0) 
          return TextHeight;
        return 0;
      }
    }

    [DefaultValue(TextPosition.None), NotifyParentProperty(true)]
    public virtual TextPosition TextPosition {
      get { return textPosition; }
      set { textPosition = value; }
    }

    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public virtual IChecksum Checksum {
      get { return checksum; }
      set { checksum = value; }
    }

    public abstract float QuietZone { get; }

    [DefaultValue(typeof(Color), "White"), NotifyParentProperty(true)]
    public Color BackColor {
      get { return backColor; }
      set { backColor = value; }
    }
    
    [DefaultValue(typeof(Color), "Black"), NotifyParentProperty(true)]
    public Color BarColor {
      get { return barColor; }
      set { barColor = value; }
    }
    
    [DefaultValue(typeof(Color), "Black"), NotifyParentProperty(true)]
    public Color FontColor {
      get { return fontColor; }
      set { fontColor = value; }
    }

    [Browsable(false)]
    public Color ForeColor {
      get { return barColor; }
      set { barColor = fontColor = value; }
    }

    [DefaultValue(typeof(Font), "Verdana, 15pt"), NotifyParentProperty(true)]
    public Font Font {
      get { return font; }
      set { font = value; }
    }

    protected internal float TextHeight {
      get {
        if (textPosition != TextPosition.None) {
          return UnitConverter.Convert(
            font.GetHeight(Dpi), 
            BarCodeUnit.Pixel, 
            unit,
            Dpi);
        }
        return 0;
      }
    }

    protected float TextWidth {
      get {
        if (textPosition != TextPosition.None) {
          return UnitConverter.Convert(
            font.SizeInPoints / 72f, // a point is 1/72 inch
            BarCodeUnit.Inch,
            unit,
            Dpi);
        }
        return 0;
      }
    }

    [Description("The unit to use when rendering the barcode. Affects all sizing properties."), 
      Category("Behavior"), NotifyParentProperty(true), DefaultValue(BarCodeUnit.Inch)]
    public BarCodeUnit Unit {
      get { return unit; }
      set { unit = value; }
    }

    [Description("The DPI (dots per inch) to use when rendering the barcode. Affects all sizing properties."),
      Category("Behavior"), NotifyParentProperty(true)]
    public int Dpi {
      get { return dpi; }
      set { dpi = value; }
    }

    public virtual void Build(IBarCodeBuilder builder, string data) {
      builder.Dpi = Dpi;
      builder.Unit = Unit;
      Draw(builder, data);
    }

    protected abstract void Draw(IBarCodeBuilder builder, string data);

    protected void DrawText(IBarCodeBuilder builder, float[] x, float y, params string[] data) {
      DrawText(builder, false, x, y, data);
    }

    protected void DrawText(IBarCodeBuilder builder, bool centered, float[] x, float y, params string[] data) {
      if ((TextPosition & TextPosition.Top) != 0) {
        for (int i = 0; i < x.Length; ++i) {
          builder.DrawString(Font, FontColor, centered, data[i], x[i], y);
        }
      }
      if ((TextPosition & TextPosition.Bottom) != 0) {
        for (int i = 0; i < x.Length; ++i) {
          builder.DrawString(Font, FontColor, centered, data[i], x[i], y + TextHeight + BarHeight);
        }
      }
    }

    public Image DrawImage(string data) {
      if (string.IsNullOrEmpty(data)) throw new BarCodeFormatException("No data to render.");
      var builder = new ImageBuilder();
      Build(builder, data);
      return builder.GetImage();
    }

    // TODO: Extra permissions!!
    public void SaveImage(string data, string fileName, ImageFormat format) {
      if (string.IsNullOrEmpty(data)) throw new BarCodeFormatException("No data to render.");
      using (var barCode = DrawImage(data)) {
        barCode.Save(fileName, format);
      }
    }

    public bool TestRender(string data, out string errorMessage) {
      if (string.IsNullOrEmpty(data)) { 
        errorMessage = "No data to render.";
        return false;
      }
      try {
        NullBuilder builder = new NullBuilder();
        Build(builder, data);
        errorMessage = null;
        return true;
      }
      catch (BarCodeFormatException ex) {
        errorMessage = ex.Message;
        return false;
      }
    }

    // TODO: SaveGif, DrawSvg, DrawHtml, etc ...
  }
}
