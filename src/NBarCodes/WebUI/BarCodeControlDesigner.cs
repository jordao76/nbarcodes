using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Design;
using System.Web.UI.Design;
using NBarCodes;

namespace NBarCodes.WebUI {
  class BarCodeControlDesigner : ControlDesigner {

    private string _fileName;

    public BarCodeControlDesigner() {
      _fileName = Path.GetTempFileName();
    }

    protected override void Dispose(bool disposing) {
      try {
        File.Delete(_fileName);
      } 
      catch {}

      base.Dispose(disposing);
    }

    public override string GetDesignTimeHtml() {
      BarCodeControl bcc = (BarCodeControl)Component;

      BarCodeGenerator generator = new BarCodeGenerator(bcc);

      string errorMessage;
      bool succeeded = 
        generator.TestRender(out errorMessage);

      if (succeeded) {
        using (var image = generator.GenerateImage()) {
          // TODO: put the image format in the settings
          SaveBarCode(image, ImageFormat.Png);
          return GetImageHtml();
        }
      }
      else {
        return
          string.Format("<span class='barCodeError'>{0}</span>", errorMessage);
      }
    }

    private void SaveBarCode(Image barCodeImage, ImageFormat imageFormat) {
      using (FileStream fs = new FileStream(_fileName, FileMode.Open, FileAccess.Write, FileShare.Read)) {
        barCodeImage.Save(fs, imageFormat);
      }
    }

    private string GetImageHtml() {
      return string.Format(
        "<img src='file://{0}'>", 
        _fileName);
    }
  }
}
