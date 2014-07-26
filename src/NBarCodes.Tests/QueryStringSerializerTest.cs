using System;
using System.Collections.Specialized;
using System.Drawing;
using NBarCodes.WebUI;
using NUnit.Framework;

namespace NBarCodes.Tests {

  /// <summary>
  /// Unit test for the <see cref="QueryStringSerializer"/> class.
  /// </summary>
  public class QueryStringSerializerTest {

    #region Test ParseQueryString(NameValueCollection)

    /// <summary>
    /// Verifies a null argument exception when parsing a null querystring.
    /// </summary>
    [Test]
    [ExpectedException(typeof(ArgumentNullException))]
    public void TestParsingNull() {
      QueryStringSerializer.ParseQueryString(null);
    }

    /// <summary>
    /// Tests parsing a minimal querystring.
    /// </summary>
    [Test]
    public void TestParsingMinimal() {
      BarCodeType type = BarCodeType.Code128;
      string data = "123456";
      NameValueCollection querystring = new NameValueCollection();
      querystring.Add(QueryStringSerializer.TYPE_KEY, type.ToString());
      querystring.Add(QueryStringSerializer.DATA_KEY, data);

      IBarCodeSettings settings = QueryStringSerializer.ParseQueryString(querystring);
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
      querystring.Add(QueryStringSerializer.TYPE_KEY, type.ToString());

      IBarCodeSettings settings = QueryStringSerializer.ParseQueryString(querystring);
    }

    /// <summary>
    /// Tests parsing a querystring with all the barcode settings' properties.
    /// </summary>
    [Test]
    public void TestParsingFull() {
      BarCodeSettings inputSettings = SettingsUtils.CreateTestSettings();

      NameValueCollection querystring = new NameValueCollection();
      querystring.Add(QueryStringSerializer.TYPE_KEY, inputSettings.Type.ToString());
      querystring.Add(QueryStringSerializer.DATA_KEY, inputSettings.Data);
      querystring.Add(QueryStringSerializer.UNIT_KEY, inputSettings.Unit.ToString());
      querystring.Add(QueryStringSerializer.DPI_KEY, inputSettings.Dpi.ToString());
      querystring.Add(QueryStringSerializer.BACKCOLOR_KEY, inputSettings.BackColor.ToArgb().ToString());
      querystring.Add(QueryStringSerializer.BARCOLOR_KEY, inputSettings.BarColor.ToArgb().ToString());
      querystring.Add(QueryStringSerializer.BARHEIGHT_KEY, inputSettings.BarHeight.ToString());
      querystring.Add(QueryStringSerializer.FONTCOLOR_KEY, inputSettings.FontColor.ToArgb().ToString());
      querystring.Add(QueryStringSerializer.GUARDEXTRAHEIGHT_KEY, inputSettings.GuardExtraHeight.ToString());
      querystring.Add(QueryStringSerializer.MODULEWIDTH_KEY, inputSettings.ModuleWidth.ToString());
      querystring.Add(QueryStringSerializer.NARROWWIDTH_KEY, inputSettings.NarrowWidth.ToString());
      querystring.Add(QueryStringSerializer.WIDEWIDTH_KEY, inputSettings.WideWidth.ToString());
      querystring.Add(QueryStringSerializer.OFFSETHEIGHT_KEY, inputSettings.OffsetHeight.ToString());
      querystring.Add(QueryStringSerializer.OFFSETWIDTH_KEY, inputSettings.OffsetWidth.ToString());
      querystring.Add(QueryStringSerializer.FONT_KEY, (string)new FontConverter().ConvertTo(inputSettings.Font, typeof(string)));
      querystring.Add(QueryStringSerializer.TEXTPOSITION_KEY, inputSettings.TextPosition.ToString());
      querystring.Add(QueryStringSerializer.USECHECKSUM_KEY, inputSettings.UseChecksum.ToString());

      IBarCodeSettings settings = QueryStringSerializer.ParseQueryString(querystring);
      SettingsUtils.AssertSettingsEqual(inputSettings, settings);
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
      querystring.Add(QueryStringSerializer.TYPE_KEY, type.ToString());
      querystring.Add(QueryStringSerializer.DATA_KEY, data);
      querystring.Add("InvalidKey", "whatever");

      IBarCodeSettings settings = QueryStringSerializer.ParseQueryString(querystring);
    }

    #endregion

    #region Test ToQueryString()

    /// <summary>
    /// Tests the query string generation.
    /// </summary>
    [Test]
    public void TestQueryString() {
      BarCodeSettings settings = SettingsUtils.CreateTestSettings();

      string queryString = QueryStringSerializer.ToQueryString(settings);

      StringAssert.Contains(MakePair(QueryStringSerializer.TYPE_KEY, settings.Type), queryString);
      StringAssert.Contains(MakePair(QueryStringSerializer.DATA_KEY, settings.Data), queryString);
      StringAssert.Contains(MakePair(QueryStringSerializer.UNIT_KEY, settings.Unit), queryString);
      StringAssert.Contains(MakePair(QueryStringSerializer.DPI_KEY, settings.Dpi), queryString);
      StringAssert.Contains(MakePair(QueryStringSerializer.BACKCOLOR_KEY, settings.BackColor.ToArgb()), queryString);
      StringAssert.Contains(MakePair(QueryStringSerializer.BARCOLOR_KEY, settings.BarColor.ToArgb()), queryString);
      StringAssert.Contains(MakePair(QueryStringSerializer.BARHEIGHT_KEY, settings.BarHeight), queryString);
      StringAssert.Contains(MakePair(QueryStringSerializer.FONTCOLOR_KEY, settings.FontColor.ToArgb()), queryString);
      StringAssert.Contains(MakePair(QueryStringSerializer.GUARDEXTRAHEIGHT_KEY, settings.GuardExtraHeight), queryString);
      StringAssert.Contains(MakePair(QueryStringSerializer.MODULEWIDTH_KEY, settings.ModuleWidth), queryString);
      StringAssert.Contains(MakePair(QueryStringSerializer.NARROWWIDTH_KEY, settings.NarrowWidth), queryString);
      StringAssert.Contains(MakePair(QueryStringSerializer.WIDEWIDTH_KEY, settings.WideWidth), queryString);
      StringAssert.Contains(MakePair(QueryStringSerializer.OFFSETHEIGHT_KEY, settings.OffsetHeight), queryString);
      StringAssert.Contains(MakePair(QueryStringSerializer.OFFSETWIDTH_KEY, settings.OffsetWidth), queryString);
      StringAssert.Contains(MakePair(QueryStringSerializer.FONT_KEY, (string)new FontConverter().ConvertTo(settings.Font, typeof(string))), queryString);
      StringAssert.Contains(MakePair(QueryStringSerializer.TEXTPOSITION_KEY, settings.TextPosition), queryString);
      StringAssert.Contains(MakePair(QueryStringSerializer.USECHECKSUM_KEY, settings.UseChecksum), queryString);
    }

    /// <summary>
    /// Re-parses a <see cref="BarCodeGenerator"/> generated querystring.
    /// </summary>
    [Test]
    public void TestParseGenerated() {
      BarCodeSettings settings = SettingsUtils.CreateTestSettings();

      string queryString = QueryStringSerializer.ToQueryString(settings);

      // re-parse querystring
      NameValueCollection queryStringCollection = MakeCollection(queryString);
      IBarCodeSettings parsedSettings = QueryStringSerializer.ParseQueryString(queryStringCollection);

      // compare outcome
      SettingsUtils.AssertSettingsEqual(settings, parsedSettings);
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
  
  }

}
