using System;
using System.Drawing;
using NUnit.Framework;

namespace NBarCodes.Tests {

  /// <summary>
  /// Unit test for the <see cref="BarCodeGenerator"/> class.
  /// </summary>
  [TestFixture]
  public class BarCodeGeneratorTest {

    #region Test Contructor

    /// <summary>
    /// Tests passing a null argument to the contructor of <see cref="BarCodeGenerator"/>.
    /// </summary>
    [Test]
    public void TestNullSettings() {
      Assert.Throws<ArgumentNullException>(() =>
        new BarCodeGenerator(null));
    }

    #endregion

    #region Test render

    /// <summary>
    /// Tests a barcode rendering that would succeed.
    /// </summary>
    [Test]
    public void TestValidRender() {
      BarCodeGenerator generator = MakeValidGenerator();
      string errorMessage = null;
      generator.TestRender(out errorMessage);
      Assert.IsNull(errorMessage);
    }

    /// <summary>
    /// Tests a barcode rendering that would fail.
    /// </summary>
    [Test]
    public void TestRenderError() {
      BarCodeGenerator generator = MakeInvalidGenerator();
      string errorMessage = null;
      generator.TestRender(out errorMessage);
      Assert.IsNotNull(errorMessage);
    }

    /// <summary>
    /// Tests a valid rendering of a barcode.
    /// </summary>
    [Test]
    public void TestValidImage() {
      BarCodeSettings settings = SettingsUtils.CreateTestSettings();
      BarCodeGenerator generator = new BarCodeGenerator(settings);
      using (var image = generator.GenerateImage()) {
        AssertImage(image);
        Assert.AreEqual(settings.Dpi, image.HorizontalResolution);
        Assert.AreEqual(settings.Dpi, image.VerticalResolution);
      }
    }

    /// <summary>
    /// Tests an invalid rendering of a barcode.
    /// </summary>
    [Test]
    public void TestInvalidImage() {
      BarCodeGenerator generator = MakeInvalidGenerator();
      Assert.Throws<BarCodeFormatException>(() => {
        using (generator.GenerateImage()) { }
      });
    }

    /// <summary>
    /// Tests valid renderings of all barcode types.
    /// </summary>
    [Test]
    public void TestAllBarcodes() {
      AssertBarcode(BarCodeType.Code128, "testing123");
      AssertBarcode(BarCodeType.Code39, "TESTING123");
      AssertBarcode(BarCodeType.Ean13, "123456789456");
      AssertBarcode(BarCodeType.Ean8, "1234567");
      AssertBarcode(BarCodeType.Interleaved25, "12345678");
      AssertBarcode(BarCodeType.PostNet, "123456789");
      AssertBarcode(BarCodeType.Standard25, "12345678");
      AssertBarcode(BarCodeType.Upca, "12345678912");
      AssertBarcode(BarCodeType.Upce, "12345600006");
    }

    /// <summary>
    /// Makes a <see cref="BarCodeGenerator"/> with valid settings.
    /// </summary>
    /// <returns>Valid barcode generator.</returns>
    private BarCodeGenerator MakeValidGenerator() {
      BarCodeSettings settings = SettingsUtils.CreateTestSettings();
      return new BarCodeGenerator(settings);
    }

    /// <summary>
    /// Makes a <see cref="BarCodeGenerator"/> with invalid settings.
    /// </summary>
    /// <returns>Invalid barcode generator.</returns>
    private BarCodeGenerator MakeInvalidGenerator() {
      BarCodeSettings settings = new BarCodeSettings();
      settings.Type = BarCodeType.Standard25;
      settings.Data = "TEST"; // Standard25 does not accept letters
      return new BarCodeGenerator(settings);
    }

    /// <summary>
    /// Asserts the valid generation of a barcode image.
    /// </summary>
    /// <param name="type">The barcode type to render.</param>
    /// <param name="data">The data to render.</param>
    private void AssertBarcode(BarCodeType type, string data) {
      BarCodeSettings settings = new BarCodeSettings();
      settings.Type = type;
      settings.Data = data;
      BarCodeGenerator generator = new BarCodeGenerator(settings);
      using (var image = generator.GenerateImage()) {
        AssertImage(image);
      }
    }

    /// <summary>
    /// Asserts that an image is not null and not empty (width and height greater than zero)
    /// </summary>
    /// <param name="image">Image to assert.</param>
    private void AssertImage(Image image) {
      Assert.IsNotNull(image);
      Assert.IsTrue(image.Width > 0);
      Assert.IsTrue(image.Height > 0);
    }

    #endregion

  }

}
