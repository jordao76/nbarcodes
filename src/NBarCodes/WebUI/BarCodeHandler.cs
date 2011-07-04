using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using NBarCodes;

namespace NBarCodes.WebUI {

	/// <summary>
	/// Http handler to generate bar code images.
	/// </summary>
  public class BarCodeHandler : IHttpHandler {

    void IHttpHandler.ProcessRequest(HttpContext context) {
			IBarCodeSettings settings = 
				QueryStringSerializer.ParseQueryString(context.Request.QueryString);

			BarCodeGenerator generator = new BarCodeGenerator(settings);

			// TODO: put the image format in the settings??
			ImageFormat imageFormat = ImageFormat.Png;
			string contentType = GetContentType(imageFormat);
			context.Response.ContentType = contentType;

      using (var image = generator.GenerateImage()) 
      using (var ms = new MemoryStream()) {
        image.Save(ms, imageFormat);
        ms.Seek(0, SeekOrigin.Begin);
        CopyData(ms, context.Response.OutputStream);
      }
		}

    private void CopyData(Stream input, Stream output) {
      const int Size = 4096;
      byte[] buffer = new byte[Size];
      var bytesRead = 0;
      while ((bytesRead = input.Read(buffer, 0, Size)) > 0) {
        output.Write(buffer, 0, bytesRead);
      }
    }

		private string GetContentType(ImageFormat imageFormat) {
			switch (imageFormat.ToString()) {
				case "Bmp": return "image/bmp";
				case "Emf": return "unknown/unknown";
				case "Exif": return "unknown/unknown";
				case "Gif": return "image/gif";
				case "Icon": return "image/x-icon";
				case "Jpeg": return "image/jpeg";
				case "MemoryBmp": return "image/bmp";
				case "Png": return "image/png";
				case "Tiff": return "image/tiff";
				case "Wmf": return "unknown/unknown";
			}
			throw new Exception("Invalid image format!");
		}

    bool IHttpHandler.IsReusable { 
			get { return true; } 
		}
	}
}
