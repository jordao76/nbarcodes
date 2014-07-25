using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using System.Drawing;

namespace NBarCodes.WebUI {

  /// <summary>
  /// Parses and formats bar code settings (<see cref="IBarCodeSettings"/>)
  /// as query strings.
  /// </summary>
  public class QueryStringSerializer {

    #region Constants
    /// <summary>Type of barcode key.</summary>
    public const string TYPE_KEY = "Type";
    /// <summary>Data key.</summary>
    public const string DATA_KEY = "Data";
    /// <summary>Unit key.</summary>
    public const string UNIT_KEY = "Unit";
    /// <summary>DPI key.</summary>
    public const string DPI_KEY = "DPI";
    /// <summary>Back color key.</summary>
    public const string BACKCOLOR_KEY = "BackColor";
    /// <summary>Bar color key.</summary>
    public const string BARCOLOR_KEY = "BarColor";
    /// <summary>Bar haight key.</summary>
    public const string BARHEIGHT_KEY = "BarHeight";
    /// <summary>Font color key.</summary>
    public const string FONTCOLOR_KEY = "FontColor";
    /// <summary>Guard extra height key.</summary>
    public const string GUARDEXTRAHEIGHT_KEY = "GuardExtraHeight";
    /// <summary>Module width key.</summary>
    public const string MODULEWIDTH_KEY = "ModuleWidth";
    /// <summary>Narrow width key.</summary>
    public const string NARROWWIDTH_KEY = "NarrowWidth";
    /// <summary>Wide width key.</summary>
    public const string WIDEWIDTH_KEY = "WideWidth";
    /// <summary>Offset height key.</summary>
    public const string OFFSETHEIGHT_KEY = "OffsetHeight";
    /// <summary>Offset width key.</summary>
    public const string OFFSETWIDTH_KEY = "OffsetWidth";
    /// <summary>Quiet zone key.</summary>
    public const string QUIETZONE_KEY = "QuietZone";
    /// <summary>Font key.</summary>
    public const string FONT_KEY = "Font";
    /// <summary>Text position key.</summary>
    public const string TEXTPOSITION_KEY = "TextPosition";
    /// <summary>Use checksum key.</summary>
    public const string USECHECKSUM_KEY = "UseChecksum";
    #endregion

    /// <summary>
    /// Parses a querystring-type parameter for <see cref="IBarCodeSettings"/> properties.
    /// The minimum properties expected are "Type", for the type of the barcode,
    /// and "Data", for the data to render with the barcode.
    /// </summary>
    /// <param name="queryString">The collection of key-value pairs for parsing.</param>
    /// <returns>The assembled <see cref="IBarCodeSettings"/>.</returns>
    /// <exception cref="ArgumentNullException">
    ///  If the querystring parameter is <c>null</c>.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// If the querystring parameter has any invalid property for the settings.
    /// </exception>
    public static IBarCodeSettings ParseQueryString(NameValueCollection queryString) {
      if (queryString == null) {
        throw new ArgumentNullException("queryString");
      }
      if (queryString[TYPE_KEY] == null || queryString[DATA_KEY] == null) {
        throw new ArgumentException("Query String must contain Type and Data keys");
      }

      BarCodeSettings bcs = new BarCodeSettings();

      foreach (string key in queryString.Keys) {
        string value = queryString[key];
        switch (key) {
          case TYPE_KEY:
            bcs.Type = (BarCodeType)Enum.Parse(typeof(BarCodeType), value);
            break;
          case DATA_KEY:
            bcs.Data = value;
            break;
          case UNIT_KEY:
            bcs.Unit = (BarCodeUnit)Enum.Parse(typeof(BarCodeUnit), value);
            break;
          case DPI_KEY:
            bcs.Dpi = int.Parse(value);
            break;
          case BACKCOLOR_KEY:
            bcs.BackColor = Color.FromArgb(int.Parse(value));
            break;
          case BARCOLOR_KEY:
            bcs.BarColor = Color.FromArgb(int.Parse(value));
            break;
          case BARHEIGHT_KEY:
            bcs.BarHeight = float.Parse(value);
            break;
          case FONTCOLOR_KEY:
            bcs.FontColor = Color.FromArgb(int.Parse(value));
            break;
          case GUARDEXTRAHEIGHT_KEY:
            bcs.GuardExtraHeight = float.Parse(value);
            break;
          case MODULEWIDTH_KEY:
            bcs.ModuleWidth = float.Parse(value);
            break;
          case NARROWWIDTH_KEY:
            bcs.NarrowWidth = float.Parse(value);
            break;
          case WIDEWIDTH_KEY:
            bcs.WideWidth = float.Parse(value);
            break;
          case OFFSETHEIGHT_KEY:
            bcs.OffsetHeight = float.Parse(value);
            break;
          case OFFSETWIDTH_KEY:
            bcs.OffsetWidth = float.Parse(value);
            break;
          case QUIETZONE_KEY:
            bcs.QuietZone = float.Parse(value);
            break;
          case FONT_KEY:
            bcs.Font = (Font)new FontConverter().ConvertFrom(value);
            break;
          case TEXTPOSITION_KEY:
            bcs.TextPosition = (TextPosition)Enum.Parse(typeof(TextPosition), value);
            break;
          case USECHECKSUM_KEY:
            bcs.UseChecksum = bool.Parse(value);
            break;
          default:
            throw new InvalidOperationException("Invalid property found!");
        }
      }

      return bcs;
    }

    /// <summary>
    /// Formats and returns a query string for the input settings (<see cref="IBarCodeSettings"/>).
    /// </summary>
    /// <param name="settings">Input settings.</param>
    /// <returns>Assembled querystring.</returns>
    public static string ToQueryString(IBarCodeSettings settings) {
      const string QUERY_NODE = "{0}={1}";
      const string SEPARATOR = "&";
      StringBuilder queryBuilder = new StringBuilder();

      queryBuilder.AppendFormat(QUERY_NODE, TYPE_KEY, UrlEncode(settings.Type));
      queryBuilder.Append(SEPARATOR);
      queryBuilder.AppendFormat(QUERY_NODE, DATA_KEY, UrlEncode(settings.Data));

      // TODO: check for default values
      queryBuilder.Append(SEPARATOR);
      queryBuilder.AppendFormat(QUERY_NODE, BACKCOLOR_KEY, UrlEncode(settings.BackColor.ToArgb()));
      queryBuilder.Append(SEPARATOR);
      queryBuilder.AppendFormat(QUERY_NODE, BARCOLOR_KEY, UrlEncode(settings.BarColor.ToArgb()));
      queryBuilder.Append(SEPARATOR);
      queryBuilder.AppendFormat(QUERY_NODE, BARHEIGHT_KEY, UrlEncode(settings.BarHeight));
      queryBuilder.Append(SEPARATOR);
      queryBuilder.AppendFormat(QUERY_NODE, FONTCOLOR_KEY, UrlEncode(settings.FontColor.ToArgb()));
      queryBuilder.Append(SEPARATOR);
      queryBuilder.AppendFormat(QUERY_NODE, GUARDEXTRAHEIGHT_KEY, UrlEncode(settings.GuardExtraHeight));
      queryBuilder.Append(SEPARATOR);
      queryBuilder.AppendFormat(QUERY_NODE, MODULEWIDTH_KEY, UrlEncode(settings.ModuleWidth));
      queryBuilder.Append(SEPARATOR);
      queryBuilder.AppendFormat(QUERY_NODE, NARROWWIDTH_KEY, UrlEncode(settings.NarrowWidth));
      queryBuilder.Append(SEPARATOR);
      queryBuilder.AppendFormat(QUERY_NODE, WIDEWIDTH_KEY, UrlEncode(settings.WideWidth));
      queryBuilder.Append(SEPARATOR);
      queryBuilder.AppendFormat(QUERY_NODE, OFFSETHEIGHT_KEY, UrlEncode(settings.OffsetHeight));
      queryBuilder.Append(SEPARATOR);
      queryBuilder.AppendFormat(QUERY_NODE, OFFSETWIDTH_KEY, UrlEncode(settings.OffsetWidth));
      queryBuilder.Append(SEPARATOR);
      queryBuilder.AppendFormat(QUERY_NODE, QUIETZONE_KEY, UrlEncode(settings.QuietZone));
      queryBuilder.Append(SEPARATOR);
      queryBuilder.AppendFormat(QUERY_NODE, FONT_KEY, UrlEncode((string)new FontConverter().ConvertTo(settings.Font, typeof(string))));
      queryBuilder.Append(SEPARATOR);
      queryBuilder.AppendFormat(QUERY_NODE, TEXTPOSITION_KEY, UrlEncode(settings.TextPosition));
      queryBuilder.Append(SEPARATOR);
      queryBuilder.AppendFormat(QUERY_NODE, USECHECKSUM_KEY, UrlEncode(settings.UseChecksum));
      queryBuilder.Append(SEPARATOR);
      queryBuilder.AppendFormat(QUERY_NODE, UNIT_KEY, UrlEncode(settings.Unit));
      queryBuilder.Append(SEPARATOR);
      queryBuilder.AppendFormat(QUERY_NODE, DPI_KEY, UrlEncode(settings.Dpi));

      return queryBuilder.ToString();
    }

    /// <summary>
    /// Url-encodes the input data.
    /// </summary>
    /// <param name="dataToEncode">Data to encode.</param>
    /// <returns>Encoded data.</returns>
    private static string UrlEncode(object dataToEncode) {
      return
        System.Web.HttpUtility.UrlEncode(dataToEncode.ToString());
    }
  }
}
