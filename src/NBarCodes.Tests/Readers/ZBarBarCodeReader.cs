using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using NBarCodes;

namespace NBarCodes.Tests.Readers {
  
  class ZBarBarCodeReader : IBarCodeReader {
  
    public BarCodeReaderResult ReadBarCode(Bitmap image) {
      using (var tempFile = new TemporaryFile()) {
        image.Save(tempFile.FilePath, ImageFormat.Png);
        return ReadBarCode(tempFile.FilePath);
      }
    }

    private BarCodeReaderResult ReadBarCode(string filePath) {
      var zbar = Process.Start(new ProcessStartInfo {
        FileName = @"..\..\..\..\lib\ZBar-0.10\bin\zbarimg.exe",
        Arguments = "--quiet \"" + filePath + "\"",
        UseShellExecute = false,
        RedirectStandardOutput = true,
        CreateNoWindow = true
      });
      if (!zbar.WaitForExit(5000)) {
        zbar.Kill();
        throw new Exception("ZBar took too long.");
      }
      if (zbar.ExitCode != 0) {
        throw new Exception("ZBar failed");
      }
      var zbarResult = zbar.StandardOutput.ReadToEnd().Trim();
      Trace.WriteLine("RESULT from ZBar: " + zbarResult);
      string[] components = zbarResult.Split(':');
      return new BarCodeReaderResult {
        Type = ConvertType(components[0]),
        Data = components[1]
      };
    }

    private BarCodeType ConvertType(string type) {
      switch (type) {
        case "CODE-128": return BarCodeType.Code128;
        case "EAN-13": return BarCodeType.Ean13;
        case "EAN-8": return BarCodeType.Ean8;
        case "I2/5": return BarCodeType.Interleaved25;
        case "CODE-39": return BarCodeType.Code39;
      }
      throw new NotSupportedException("Unmatched barcode type: " + type);
    }
  
  }

}
