using System;
using System.Drawing;
using ZXing;
using ZXing.Common;
using NBarCodes;

namespace NBarCodes.Tests.Readers {
  class ZXingBarCodeReader : IBarCodeReader {
  
    public BarCodeReaderResult ReadBarCode(Bitmap image) {
      var reader = new BarcodeReader();
      var result = reader.Decode(image);
      return new BarCodeReaderResult { 
        Data = ResolveText(result), 
        Type = ConvertType(result.BarcodeFormat) 
      };
    }

    private string ResolveText(Result result) {
      string text = result.Text;
      if (result.ResultMetadata.ContainsKey(ResultMetadataType.UPC_EAN_EXTENSION)) {
        text += result.ResultMetadata[ResultMetadataType.UPC_EAN_EXTENSION];
      }
      return text;
    }

    private BarCodeType ConvertType(BarcodeFormat type) {
      // these types are missing...
      //"Standard 2 of 5"
      //"Interleaved 2 of 5"
      //"Postnet"

      if (type == BarcodeFormat.CODE_39) return BarCodeType.Code39;
      if (type == BarcodeFormat.CODE_128) return BarCodeType.Code128;
      if (type == BarcodeFormat.EAN_8) return BarCodeType.Ean8;
      if (type == BarcodeFormat.EAN_13) return BarCodeType.Ean13;
      if (type == BarcodeFormat.UPC_A) return BarCodeType.Upca;
      if (type == BarcodeFormat.UPC_E) return BarCodeType.Upce;

      throw new NotSupportedException("Unmatched barcode type: " + type);
    }

  }
}
