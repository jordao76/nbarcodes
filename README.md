#NBarCodes

**NBarCodes-api** is a .NET library to generate common one-dimensional barcodes. Supported barcode types include Code 128, Code 39, Standard 2 of 5, Interleaved 2 of 5, EAN-13, EAN-8, UPC-A, UPC-E and Postnet.

##Quick intro

![NBarCodes](NBarCodes.png)

(C#)

```csharp
using NBarCodes;

...

var settings = new BarCodeSettings {
  Type = BarCodeType.Code128,
  Data = "NBarCodes"
};

var generator = new BarCodeGenerator(settings);

using (var barcodeImage = generator.GenerateImage()) {

  // use the generated barcodeImage

}
```

(VB.NET)

```vbnet
Imports NBarCodes

...

Dim settings = New BarCodeSettings With {
  .Type = BarCodeType.Code128,
  .Data = "NBarCodes"
}

Dim generator = New BarCodeGenerator(settings)

Using barcodeImage = generator.GenerateImage()

  ' use the generated barcodeImage

End Using
```

(F#)

```fsharp
open NBarCodes

...

let settings = new BarCodeSettings(Type = BarCodeType.Code128, Data = "NBarCodes")
let generator = new BarCodeGenerator(settings)

use barcodeImage = generator.GenerateImage()
// use the generated barcodeImage
```

The `BarCodeSettings` object holds the barcode information. This class has many properties that affect the rendering of the barcode, like the barcode type, the data to render, bar measures, text position and colors. Required properties are simply `Type`, for the barcode type, and `Data`, for the data to render. The `BarCodeGenerator` class is used to generate the right barcode from the provided barcode settings.

A `BarCodeFormatException` can be thrown if the settings provided have errors for the target barcode. The settings can be checked before calling `GenerateImage` with the method `TestRender` on the `BarCodeGenerator` class.
