using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.IO;
using System.Web.UI.HtmlControls;

namespace NBarCodes.WebUI {

  /// <summary>
  /// Helper class to render HTML tags for barcodes. Useful from ASP.NET MVC.
  /// </summary>
  public static class BarCodeHtmlHelper {

    /// <summary>
    /// Generates the barcode tags to render to barcode expressed in the input <paramref name="settings"/>.
    /// </summary>
    /// <param name="settings">Settings for the barcode to render.</param>
    /// <param name="barCodeHandlerUrl">
    /// URL for the barcode image rendering HTTP handler. 
    /// Must be registered in web.config, pointing to <see cref="BarCodeHandler"/>.
    /// </param>
    /// <returns></returns>
    public static string BarCode(IBarCodeSettings settings, string barCodeHandlerUrl) {
      if (settings == null) throw new ArgumentNullException("settings");
      if (string.IsNullOrEmpty(barCodeHandlerUrl)) throw new ArgumentNullException("barCodeHandlerUrl");
      var sb = new StringBuilder();
      using (var sw = new StringWriter(sb))
      using (var output = new HtmlTextWriter(sw)) {
        RenderImageTag(output, settings, barCodeHandlerUrl);
      }
      return sb.ToString();
    }

    internal static void RenderImageTag(HtmlTextWriter output, IBarCodeSettings settings, string barCodeHandlerUrl) {
      if (settings.Data != null && settings.Data.Trim().Length > 0) {
        string errorMessage;
        var helper = new BarCodeHelper(settings);
        if (helper.TestRender(out errorMessage)) {
          HtmlImage htmlImage = new HtmlImage();
          htmlImage.Alt = settings.Data;
          htmlImage.Src = GetBarCodeHandlerCall(helper, barCodeHandlerUrl);
          htmlImage.RenderControl(output);
        }
        else {
          output.Write("<span class='barCodeError'>{0}</span>", errorMessage);
        }
      }
      else {
        output.Write("<span class='barCodeError'>Empty barcode data.</span>");
      }
    }

    private static string GetBarCodeHandlerCall(BarCodeHelper helper, string barCodeHandlerUrl) {
      return
        string.Format("{0}?{1}",
          barCodeHandlerUrl,
          helper.ToQueryString());
    }
  }

}
