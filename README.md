#NBarCodes

NBarCodes is a .NET component to generate common one-dimensional barcodes. It includes an API, an ASP.NET control and a Windows Forms control. Supported barcode types include Code 128, Code 39, Standard 2 of 5, Interleaved 2 of 5, EAN-13, EAN-8, UPC-A, UPC-E and Postnet.

You can download NBarCodes binaries from the [releases](https://github.com/jordao76/nbarcodes/releases) page.

##ASP.NET control

To use the NBarCodes ASP.NET control, you can follow these steps:

*Step 1.* Reference the *NBarCodes.dll* assembly from your ASP.NET project.

*Step 2.* Register the barcode HTTP handler on the relevant section (configuration/system.webServer) of the web.config file:

```xml
<handlers>
  <add verb="GET" path="BarCodeHandler.axd" name="BarCodeHandler"
    type="NBarCodes.WebUI.BarCodeHandler, NBarCodes"/>
</handlers>
```

The barcode control uses this handler to generate the barcode image.

*Step 3.* In the ASP.NET page that you want to use the control, add it by first registering its namespace on the top of the page:

```xml
<%@ Register TagPrefix="nbc" Namespace="NBarCodes.WebUI"
  Assembly="NBarCodes" %>
```

And then include the control in the page hierarchy:

```xml
<nbc:BarCodeControl id="BarCodeControl1" runat="server"
  Data="NBarCodes" Type="Code128" />
```

You can work with the control programmatically or using the designer surface.

Alternatively to steps 1 and 3, you can add the control to the Visual Studio toolbox. Its full name is `NBarCodes.WebUI.BarCodeControl`, and it's in the *NBarCodes.dll* assembly.

You can find a simple ASP.NET application showcasing the control in the *src\NBarCodes.Samples.AspNet* folder.

##Windows Forms control

To use the NBarCodes Windows Forms control, follow these steps:

*Step 1.* Make sure that your Windows Forms project doesn't target any Client Profile framework. NBarCodes references assemblies that are not present in these target frameworks. You can change this setting by going to your project properties page.

*Step 2.* Add the control to the Visual Studio toolbox. Its full name is `NBarCodes.Forms.BarCodeControl`, and it's in the *NBarCodes.dll* assembly.

*Step 3.* Use the `BarCodesControl` from the toolbox.

Alternatively to steps 2 and 3, you can simply reference the *NBarCodes.dll* assembly from your Windows Forms project and use the control programmatically in your code.

You can find a simple Windows Forms application showcasing the control in the *src\NBarCodes.Samples.WinForms* folder. It has a sample form with the control and it also demonstrates sending the rendered barcode to the printer.

##API

To generate barcode images programmatically, without using one of the provided controls, first reference the *NBarCodes.dll* assembly from your project. You also need to reference the *System.Drawing* assembly to work with images.

You can then generate barcode images with code like the following:

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

The `BarCodeSettings` object holds the barcode information. This class has many properties that affect the rendering of the barcode, like the barcode type, the data to render, bar measures, text position and colors. Required properties are simply `Type`, for the barcode type, and `Data`, for the data to render. The `BarCodeGenerator` class is used to generate the right barcode from the provided barcode settings.

A `BarCodeFormatException` can be thrown if the settings provided have errors for the target barcode. If you expect errors, you can check the settings before calling `GenerateImage` with the method `TestRender` on the `BarCodeGenerator` class.
