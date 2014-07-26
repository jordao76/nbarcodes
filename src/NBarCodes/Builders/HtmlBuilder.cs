using System;
using System.Drawing;
using System.Text;

namespace NBarCodes {

  /// <summary>
  /// Builder to render barcodes in a pure-HTML &lt;div&gt; tag.
  /// </summary>
  class HtmlBuilder : IBarCodeBuilder {

    private StringBuilder _htmlBuilder = new StringBuilder();
    private Size size;

    public int Dpi {
      get { return 96; }
      set { }
    }

    public BarCodeUnit Unit { 
      get { return BarCodeUnit.Inch; }
      set {}
    }

    public void Prepare(float width, float height) {
      // flush current string builder
      _htmlBuilder.Length = 0;
      size = new Size((int)width, (int)height);
    }

    public void DrawString(Font font, Color fontColor, bool centered, string data, float x, float y) {
      float width = font.SizeInPoints / 0.75f * data.Length / 2;
      float height = font.GetHeight();
      if (centered) x -= width / 2;

      _htmlBuilder.AppendFormat(@"
          <div style=""position: absolute; top: {0}; left: {1}; width: {2}; height: {3};"">
            <center><span style=""font-family: {4}; font-size: {5}px; color: {6}"">{7}</span></center>
          </div>
        ", (int)y, (int)x, (int)width, (int)height, font.Name, font.Size, fontColor.Name, HtmlEncode(data)
      );
    }

    public string HtmlEncode(string data) {
      return
        System.Web.HttpUtility.HtmlEncode(data);
    }

    public void DrawRectangle(Color color, float x, float y, float width, float height) {
      if ((int)width == size.Width && (int)height == size.Height) {
        // background, should be the first call to this method on current barcode // enforce!!
        _htmlBuilder.AppendFormat("<div style=\"position: relative; background-color: {0}; width: {1}; height: {2};\">\n", color.Name, (int)width, (int)height);
      }
      else {
        _htmlBuilder.AppendFormat("<div style=\"position: absolute; background-color: {0}; top: {1}; left: {2}; width: {3}; height: {4};\"></div>\n", color.Name, (int)y, (int)x, (int)width, (int)height);
      }
    }

    public string GetHtml() {
      _htmlBuilder.Append("</div>\n");
      return _htmlBuilder.ToString();
    }

  }

}
