using System;
using System.Drawing;
using com.google.zxing;
using com.google.zxing.common;
using NBarCodes;

namespace NBarCodes.Tests.Readers {
  class ZXingBarCodeReader : IBarCodeReader {
  
    public BarCodeReaderResult ReadBarCode(Bitmap image) {
      var binarizer = new GlobalHistogramBinarizer(new RGBLuminanceSource(image, image.Width, image.Height));
      var binaryBitmap = new BinaryBitmap(binarizer);
      var reader = new MultiFormatReader();
      var result = reader.decode(binaryBitmap);
      return new BarCodeReaderResult { 
        Data = result.Text, 
        Type = ConvertType(result.BarcodeFormat) 
      };
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
