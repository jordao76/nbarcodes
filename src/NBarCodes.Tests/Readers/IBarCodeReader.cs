using System.Drawing;
using NBarCodes;

namespace NBarCodes.Tests.Readers {

  interface IBarCodeReader {
    BarCodeReaderResult ReadBarCode(Bitmap image);
  }

  class BarCodeReaderResult {
    public BarCodeType Type { get; set; }
    public string Data { get; set; }
  }

}
