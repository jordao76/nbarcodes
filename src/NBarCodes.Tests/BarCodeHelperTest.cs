using System;
using System.Collections.Specialized;
using System.Drawing;
using NUnit.Framework;

namespace NBarCodes.Tests {

	/// <summary>
	/// Unit test for the <see cref="BarCodeHelper"/> class.
	/// </summary>
	[TestFixture]
	public class BarCodeHelperTest {

		#region Utils

		/// <summary>
		/// Returns barcode settings for testing.
		/// </summary>
		/// <returns>Settings for testing.</returns>
		private BarCodeSettings CreateSettings() {
			BarCodeSettings settings = new BarCodeSettings();

			settings.Type = BarCodeType.Code128;
			settings.Data = "123456";
			settings.Unit = BarCodeUnit.Centimeter;
			settings.BackColor = Color.BlanchedAlmond;
			settings.BarColor = Color.Honeydew;
			settings.BarHeight = 4.54f;
			settings.FontColor = Color.OldLace;
			settings.GuardExtraHeight = .543f;
			settings.ModuleWidth = .654f;
			settings.NarrowWidth = .345f;
			settings.WideWidth = .634f;
			settings.OffsetHeight = 1.234f;
			settings.OffsetWidth = 0.489f;
			settings.QuietZone = 0.001f;
			settings.Font = new Font("verdana", 8f, FontStyle.Italic);
			settings.TextPosition = TextPosition.All;
			settings.UseChecksum = true;

			return settings;
		}

		/// <summary>
		/// Asserts that two <see cref="IBarCodeSettings"/> are property-wise equal.
		/// </summary>
		/// <param name="expected">Expected settings.</param>
		/// <param name="actual">Actual settings.</param>
		private void AssertSettingsEqual(IBarCodeSettings expected, IBarCodeSettings actual) {
			Assert.AreEqual(expected.Type, actual.Type);
			Assert.AreEqual(expected.Data, actual.Data);
			Assert.AreEqual(expected.Unit, actual.Unit);
			Assert.AreEqual(expected.BackColor.ToArgb(), actual.BackColor.ToArgb());
			Assert.AreEqual(expected.BarColor.ToArgb(), actual.BarColor.ToArgb());
			Assert.AreEqual(expected.BarHeight, actual.BarHeight);
			Assert.AreEqual(expected.FontColor.ToArgb(), actual.FontColor.ToArgb());
			Assert.AreEqual(expected.GuardExtraHeight, actual.GuardExtraHeight);
			Assert.AreEqual(expected.ModuleWidth, actual.ModuleWidth);
			Assert.AreEqual(expected.NarrowWidth, actual.NarrowWidth);
			Assert.AreEqual(expected.WideWidth, actual.WideWidth);
			Assert.AreEqual(expected.OffsetHeight, actual.OffsetHeight);
			Assert.AreEqual(expected.OffsetWidth, actual.OffsetWidth);
			Assert.AreEqual(expected.QuietZone, actual.QuietZone);
			Assert.AreEqual(expected.Font, actual.Font);
			Assert.AreEqual(expected.TextPosition, actual.TextPosition);
			Assert.AreEqual(expected.UseChecksum, actual.UseChecksum);
		}

		#endregion

		#region Test Contructor

		/// <summary>
		/// Tests passing a null argument to the contructor of <see cref="BarCodeHelper"/>.
		/// </summary>
		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestNullSettings() {
			new BarCodeHelper(null);
		}

		#endregion

		#region Test ParseQueryString(NameValueCollection)

		/// <summary>
		/// Verifies a null argument exception when parsing a null querystring.
		/// </summary>
		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestParsingNull() {
			BarCodeHelper.ParseQueryString(null);
		}

		/// <summary>
		/// Tests parsing a minimal querystring.
		/// </summary>
		[Test]
		public void TestParsingMinimal() {
			BarCodeType type = BarCodeType.Code128;
			string data = "123456";
			NameValueCollection querystring = new NameValueCollection();
			querystring.Add(BarCodeHelper.TYPE_KEY, type.ToString());
			querystring.Add(BarCodeHelper.DATA_KEY, data);

			IBarCodeSettings settings = BarCodeHelper.ParseQueryString(querystring);
			Assert.AreEqual(type, settings.Type, "Barcode type differs");
			Assert.AreEqual(data, settings.Data, "Barcode data differs");
		}

		/// <summary>
		/// Tests parsing an incomplete querystring.
		/// </summary>
		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void TestParsingBelowMinimal() {
			BarCodeType type = BarCodeType.Interleaved25;
			NameValueCollection querystring = new NameValueCollection();
			querystring.Add(BarCodeHelper.TYPE_KEY, type.ToString());

			IBarCodeSettings settings = BarCodeHelper.ParseQueryString(querystring);
		}

		/// <summary>
		/// Tests parsing a querystring with all the barcode settings' properties.
		/// </summary>
		[Test]
		public void TestParsingFull() {
			BarCodeSettings inputSettings = CreateSettings();

			NameValueCollection querystring = new NameValueCollection();
			querystring.Add(BarCodeHelper.TYPE_KEY, inputSettings.Type.ToString());
			querystring.Add(BarCodeHelper.DATA_KEY, inputSettings.Data);
			querystring.Add(BarCodeHelper.UNIT_KEY, inputSettings.Unit.ToString());
			querystring.Add(BarCodeHelper.BACKCOLOR_KEY, inputSettings.BackColor.ToArgb().ToString());
			querystring.Add(BarCodeHelper.BARCOLOR_KEY, inputSettings.BarColor.ToArgb().ToString());
			querystring.Add(BarCodeHelper.BARHEIGHT_KEY, inputSettings.BarHeight.ToString());
			querystring.Add(BarCodeHelper.FONTCOLOR_KEY, inputSettings.FontColor.ToArgb().ToString());
			querystring.Add(BarCodeHelper.GUARDEXTRAHEIGHT_KEY, inputSettings.GuardExtraHeight.ToString());
			querystring.Add(BarCodeHelper.MODULEWIDTH_KEY, inputSettings.ModuleWidth.ToString());
			querystring.Add(BarCodeHelper.NARROWWIDTH_KEY, inputSettings.NarrowWidth.ToString());
			querystring.Add(BarCodeHelper.WIDEWIDTH_KEY, inputSettings.WideWidth.ToString());
			querystring.Add(BarCodeHelper.OFFSETHEIGHT_KEY, inputSettings.OffsetHeight.ToString());
			querystring.Add(BarCodeHelper.OFFSETWIDTH_KEY, inputSettings.OffsetWidth.ToString());
			querystring.Add(BarCodeHelper.QUIETZONE_KEY, inputSettings.QuietZone.ToString());
			querystring.Add(BarCodeHelper.FONT_KEY, (string)new FontConverter().ConvertTo(inputSettings.Font, typeof(string)));
			querystring.Add(BarCodeHelper.TEXTPOSITION_KEY, inputSettings.TextPosition.ToString());
			querystring.Add(BarCodeHelper.USECHECKSUM_KEY, inputSettings.UseChecksum.ToString());

			IBarCodeSettings settings = BarCodeHelper.ParseQueryString(querystring);
			AssertSettingsEqual(inputSettings, settings);
		}

		/// <summary>
		/// Tests parsing a querystring with an invalid key.
		/// </summary>
		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public void TestParsingInvalidKey() {
			BarCodeType type = BarCodeType.Code128;
			string data = "123456";
			NameValueCollection querystring = new NameValueCollection();
			querystring.Add(BarCodeHelper.TYPE_KEY, type.ToString());
			querystring.Add(BarCodeHelper.DATA_KEY, data);
			querystring.Add("InvalidKey", "whatever");

			IBarCodeSettings settings = BarCodeHelper.ParseQueryString(querystring);
		}

		#endregion

		#region Test ToQueryString()
		
		/// <summary>
		/// Tests the query string generation.
		/// </summary>
		[Test]
		public void TestQueryString() {
			BarCodeSettings settings = CreateSettings();

			BarCodeHelper helper = new BarCodeHelper(settings);
			string queryString = helper.ToQueryString();

			StringAssert.Contains(MakePair(BarCodeHelper.TYPE_KEY, settings.Type), queryString);
			StringAssert.Contains(MakePair(BarCodeHelper.DATA_KEY, settings.Data), queryString);
			StringAssert.Contains(MakePair(BarCodeHelper.UNIT_KEY, settings.Unit), queryString);
			StringAssert.Contains(MakePair(BarCodeHelper.BACKCOLOR_KEY, settings.BackColor.ToArgb()), queryString);
			StringAssert.Contains(MakePair(BarCodeHelper.BARCOLOR_KEY, settings.BarColor.ToArgb()), queryString);
			StringAssert.Contains(MakePair(BarCodeHelper.BARHEIGHT_KEY, settings.BarHeight), queryString);
			StringAssert.Contains(MakePair(BarCodeHelper.FONTCOLOR_KEY, settings.FontColor.ToArgb()), queryString);
			StringAssert.Contains(MakePair(BarCodeHelper.GUARDEXTRAHEIGHT_KEY, settings.GuardExtraHeight), queryString);
			StringAssert.Contains(MakePair(BarCodeHelper.MODULEWIDTH_KEY, settings.ModuleWidth), queryString);
			StringAssert.Contains(MakePair(BarCodeHelper.NARROWWIDTH_KEY, settings.NarrowWidth), queryString);
			StringAssert.Contains(MakePair(BarCodeHelper.WIDEWIDTH_KEY, settings.WideWidth), queryString);
			StringAssert.Contains(MakePair(BarCodeHelper.OFFSETHEIGHT_KEY, settings.OffsetHeight), queryString);
			StringAssert.Contains(MakePair(BarCodeHelper.OFFSETWIDTH_KEY, settings.OffsetWidth), queryString);
			StringAssert.Contains(MakePair(BarCodeHelper.QUIETZONE_KEY, settings.QuietZone), queryString);
			StringAssert.Contains(MakePair(BarCodeHelper.FONT_KEY, (string)new FontConverter().ConvertTo(settings.Font, typeof(string))), queryString);
			StringAssert.Contains(MakePair(BarCodeHelper.TEXTPOSITION_KEY, settings.TextPosition), queryString);
			StringAssert.Contains(MakePair(BarCodeHelper.USECHECKSUM_KEY, settings.UseChecksum), queryString);
		}

		/// <summary>
		/// Re-parses a <see cref="BarCodeHelper"/> generated querystring.
		/// </summary>
		[Test]
		public void TestParseGenerated() {
			BarCodeSettings settings = CreateSettings();

			BarCodeHelper helper = new BarCodeHelper(settings);
			string queryString = helper.ToQueryString();

			// re-parse querystring
			NameValueCollection queryStringCollection = MakeCollection(queryString);
			IBarCodeSettings parsedSettings = BarCodeHelper.ParseQueryString(queryStringCollection);

			// compare outcome
			AssertSettingsEqual(settings, parsedSettings);
		}

		/// <summary>
		/// Transforms a querystring into its corresponding collection.
		/// </summary>
		/// <param name="queryString">Input querystring.</param>
		/// <returns>Collection with querystring key-value data.</returns>
		private NameValueCollection MakeCollection(string queryString) {
			NameValueCollection queryStringCollection = new NameValueCollection();
			string[] keyValues = queryString.Split('&');
			foreach (string pair in keyValues) {
				string[] components = pair.Split('=');
				string key = components[0];
				string value = components[1];
				queryStringCollection.Add(key, UrlDecode(value));
			}
			return queryStringCollection;
		}

		/// <summary>
		/// Makes a key-value pair for a querystring.
		/// Properly encodes the value.
		/// </summary>
		/// <param name="key">Key.</param>
		/// <param name="value">Value.</param>
		/// <returns>String with key-value pair in the form KEY=VALUE.</returns>
		private string MakePair(string key, object value) {
			return string.Format("{0}={1}", key, UrlEncode(value));
		}

		/// <summary>
		/// Url-encodes the input data.
		/// </summary>
		/// <param name="dataToEncode">Data to encode.</param>
		/// <returns>Encoded data.</returns>
		private string UrlEncode(object dataToEncode) {
			return
				System.Web.HttpUtility.UrlEncode(dataToEncode.ToString());
		}

		/// <summary>
		/// Url-decodes the input data.
		/// </summary>
		/// <param name="dataToDecode">Data to decode.</param>
		/// <returns>Decoded data.</returns>
		private string UrlDecode(string dataToDecode) {
			return
				System.Web.HttpUtility.UrlDecode(dataToDecode);
		}

		#endregion

		#region Test render

		/// <summary>
		/// Tests a barcode rendering that would succeed.
		/// </summary>
		[Test]
		public void TestValidRender() {
			BarCodeHelper helper = MakeValidHelper();
			string errorMessage = null;
			helper.TestRender(out errorMessage);
			Assert.IsNull(errorMessage);
		}

		/// <summary>
		/// Tests a barcode rendering that would fail.
		/// </summary>
		[Test]
		public void TestRenderError() {
			BarCodeHelper helper = MakeInvalidHelper();
			string errorMessage = null;
			helper.TestRender(out errorMessage);
			Assert.IsNotNull(errorMessage);
		}

		/// <summary>
		/// Tests a valid rendering of a barcode.
		/// </summary>
		[Test]
		public void TestValidImage() {
			BarCodeSettings settings = CreateSettings();
			BarCodeHelper helper = new BarCodeHelper(settings);
      using (var image = helper.GenerateImage()) {
        AssertImage(image);
      }
		}

		/// <summary>
		/// Tests an invalid rendering of a barcode.
		/// </summary>
		[Test]
		[ExpectedException(typeof(BarCodeFormatException))]
		public void TestInvalidImage() {
			BarCodeHelper helper = MakeInvalidHelper();
      using (helper.GenerateImage()) { }
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
		/// Makes a <see cref="BarCodeHelper"/> with valid settings.
		/// </summary>
		/// <returns>Valid barcode helper.</returns>
		private BarCodeHelper MakeValidHelper() {
			BarCodeSettings settings = CreateSettings();
			return new BarCodeHelper(settings);
		}

		/// <summary>
		/// Makes a <see cref="BarCodeHelper"/> with invalid settings.
		/// </summary>
		/// <returns>Invalid barcode helper.</returns>
		private BarCodeHelper MakeInvalidHelper() {
			BarCodeSettings settings = new BarCodeSettings();
			settings.Type = BarCodeType.Standard25;
			settings.Data = "TEST"; // Standard25 does not accept letters
			return new BarCodeHelper(settings);
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
			BarCodeHelper helper = new BarCodeHelper(settings);
      using (var image = helper.GenerateImage()) {
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

