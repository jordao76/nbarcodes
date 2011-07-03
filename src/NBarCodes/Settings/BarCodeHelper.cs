using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Text;

namespace NBarCodes {

	/// <summary>
	/// A helper class that operates on <see cref="IBarCodeSettings"/>
	/// to provide barcode rendering and convertion services.
	/// </summary>
	[Serializable]
	public sealed class BarCodeHelper {

		#region Constants
		/// <summary>Type of barcode key.</summary>
		public const string TYPE_KEY = "Type";
		/// <summary>Data key.</summary>
    public const string DATA_KEY = "Data";
		/// <summary>Unit key.</summary>
    public const string UNIT_KEY = "Unit";
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
		/// Creates a new instance of the <see cref="BarCodeHelper"/> class.
		/// </summary>
		/// <param name="settings">The settings to use.</param>
		/// <exception cref="ArgumentNullException">
		/// If the settings passed are <c>null</c>.
		/// </exception>
		public BarCodeHelper(IBarCodeSettings settings) {
			if (settings == null) {
				throw new ArgumentNullException("settings");
			}

			_settings = settings;
		}

		#region BarCode Assembling

		/// <summary>
		/// Tests if the barcode would render with the given settings (<see cref="IBarCodeSettings"/>), without format errors.
		/// </summary>
		/// <param name="errorMessage">Outputs the error message if a <see cref="BarCodeFormatException"/> was thrown.</param>
		/// <returns><c>True</c> if the barcode rendering test went ok, <c>false</c> otherwise.</returns>
		public bool TestRender(out string errorMessage) {
			BarCode bc = AssembleBarCode();
			return bc.TestRender(_settings.Data, out errorMessage);
		}

		/// <summary>
		/// Generates an image with the rendered barcode based on the settings (<see cref="IBarCodeSettings"/>).
		/// </summary>
		/// <returns>The generated barcode image.</returns>
		/// <exception cref="BarCodeFormatException">
		/// If the data in the settings can't be rendered by the selected barcode.
		/// </exception>
		public Image GenerateImage() {
			BarCode bc = AssembleBarCode();
			return bc.DrawImage(_settings.Data);
		}

		/// <summary>
		/// Instantiates the barcode type asked by the settings.
		/// </summary>
		/// <returns>The instantiated barcode type.</returns>
		/// <exception cref="InvalidOperationException">
		/// If the settings contain an invalid barcode type.
		/// </exception>
		private BarCode GetBarCode() {
			switch (_settings.Type) {
				case BarCodeType.Standard25: return new Standard25();
				case BarCodeType.Interleaved25: return new Interleaved25();
				case BarCodeType.Code39: return new Code39();
				case BarCodeType.Code128: return new Code128();
				case BarCodeType.Ean8: return new Ean8();
				case BarCodeType.Ean13: return new Ean13();
				case BarCodeType.Upca: return new Upca();
				case BarCodeType.Upce: return new Upce();
				case BarCodeType.PostNet: return new PostNet();
			}

			throw new InvalidOperationException(string.Format("Cannot convert '{0}' to type BarCode.", _settings.Type));
		}

		/// <summary>
		/// Instantiates and sets the barcode properties based on the settings.
		/// </summary>
		/// <returns>Instantiated and set up barcode.</returns>
		private BarCode AssembleBarCode() {
			BarCode barCode = GetBarCode();
			barCode.Unit = _settings.Unit;
			barCode.BarHeight = _settings.BarHeight;
			barCode.OffsetWidth = _settings.OffsetWidth;
			barCode.OffsetHeight = _settings.OffsetHeight;
			barCode.TextPosition = _settings.TextPosition;
			barCode.QuietZone = _settings.QuietZone;
			barCode.BarColor = _settings.BarColor;
			barCode.BackColor = _settings.BackColor;
			barCode.FontColor = _settings.FontColor;
			barCode.Font = _settings.Font;
			if (barCode is IOptionalChecksum) {
				((IOptionalChecksum)barCode).UseChecksum = _settings.UseChecksum;
			}
			if (barCode is EanUpc) {
				((EanUpc)barCode).GuardExtraHeight = _settings.GuardExtraHeight;
			}
			if (barCode is ModuleBarCode) {
				((ModuleBarCode)barCode).ModuleWidth = _settings.ModuleWidth;
			}
			if (barCode is ThicknessBarCode) {
				((ThicknessBarCode)barCode).NarrowWidth = _settings.NarrowWidth;
				((ThicknessBarCode)barCode).WideWidth = _settings.WideWidth;
			}
			return barCode;
		}

		#endregion

		#region Parsing and Formatting

		/// <summary>
		/// Parses a querystring-type parameter for <see cref="IBarCodeSettings"/> properties.
		/// The minimum properties expected are "Type", for the type of the barcode,
		/// and "Data", for the data to render with the barcode.
		/// </summary>
		/// <param name="queryString">The collection of key-value pairs for parsing.</param>
		/// <returns>The assembled <see cref="IBarCodeSettings"/>.</returns>
		/// <exception cref="ArgumentNullException">
		///	If the querystring parameter is <c>null</c>.
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
		/// Formats and returns a query string for the settings (<see cref="IBarCodeSettings"/>).
		/// </summary>
		/// <returns>Assembled querystring.</returns>
		public string ToQueryString() {
			const string QUERY_NODE = "{0}={1}";
			const string SEPARATOR = "&";
			StringBuilder queryBuilder = new StringBuilder();

			queryBuilder.AppendFormat(QUERY_NODE, TYPE_KEY, UrlEncode(_settings.Type));
			queryBuilder.Append(SEPARATOR);
			queryBuilder.AppendFormat(QUERY_NODE, DATA_KEY, UrlEncode(_settings.Data));

			// TODO: check for default values
			queryBuilder.Append(SEPARATOR);
			queryBuilder.AppendFormat(QUERY_NODE, BACKCOLOR_KEY, UrlEncode(_settings.BackColor.ToArgb()));
			queryBuilder.Append(SEPARATOR);
			queryBuilder.AppendFormat(QUERY_NODE, BARCOLOR_KEY, UrlEncode(_settings.BarColor.ToArgb()));
			queryBuilder.Append(SEPARATOR);
			queryBuilder.AppendFormat(QUERY_NODE, BARHEIGHT_KEY, UrlEncode(_settings.BarHeight));
			queryBuilder.Append(SEPARATOR);
			queryBuilder.AppendFormat(QUERY_NODE, FONTCOLOR_KEY, UrlEncode(_settings.FontColor.ToArgb()));
			queryBuilder.Append(SEPARATOR);
			queryBuilder.AppendFormat(QUERY_NODE, GUARDEXTRAHEIGHT_KEY, UrlEncode(_settings.GuardExtraHeight));
			queryBuilder.Append(SEPARATOR);
			queryBuilder.AppendFormat(QUERY_NODE, MODULEWIDTH_KEY, UrlEncode(_settings.ModuleWidth));
			queryBuilder.Append(SEPARATOR);
			queryBuilder.AppendFormat(QUERY_NODE, NARROWWIDTH_KEY, UrlEncode(_settings.NarrowWidth));
			queryBuilder.Append(SEPARATOR);
			queryBuilder.AppendFormat(QUERY_NODE, WIDEWIDTH_KEY, UrlEncode(_settings.WideWidth));
			queryBuilder.Append(SEPARATOR);
			queryBuilder.AppendFormat(QUERY_NODE, OFFSETHEIGHT_KEY, UrlEncode(_settings.OffsetHeight));
			queryBuilder.Append(SEPARATOR);
			queryBuilder.AppendFormat(QUERY_NODE, OFFSETWIDTH_KEY, UrlEncode(_settings.OffsetWidth));
			queryBuilder.Append(SEPARATOR);
			queryBuilder.AppendFormat(QUERY_NODE, QUIETZONE_KEY, UrlEncode(_settings.QuietZone));
			queryBuilder.Append(SEPARATOR);
			queryBuilder.AppendFormat(QUERY_NODE, FONT_KEY, UrlEncode((string)new FontConverter().ConvertTo(_settings.Font, typeof(string))));
			queryBuilder.Append(SEPARATOR);
			queryBuilder.AppendFormat(QUERY_NODE, TEXTPOSITION_KEY, UrlEncode(_settings.TextPosition));
			queryBuilder.Append(SEPARATOR);
			queryBuilder.AppendFormat(QUERY_NODE, USECHECKSUM_KEY, UrlEncode(_settings.UseChecksum));
			queryBuilder.Append(SEPARATOR);
			queryBuilder.AppendFormat(QUERY_NODE, UNIT_KEY, UrlEncode(_settings.Unit));

			return queryBuilder.ToString();
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

		#endregion

		/// <summary>
		/// Copy settings between <see cref="IBarCodeSettings"/>.
		/// </summary>
		/// <param name="settingsToCopyFrom">Settings to copy from.</param>
		/// <param name="settingsToCopyTo">Settings to copy to.</param>
		public static void CopySettings(IBarCodeSettings settingsToCopyFrom, IBarCodeSettings settingsToCopyTo) {
			settingsToCopyTo.Type = settingsToCopyFrom.Type;
			settingsToCopyTo.Data = settingsToCopyFrom.Data;
			settingsToCopyTo.BackColor = settingsToCopyFrom.BackColor;
			settingsToCopyTo.BarColor = settingsToCopyFrom.BarColor;
			settingsToCopyTo.BarHeight = settingsToCopyFrom.BarHeight;
			settingsToCopyTo.FontColor = settingsToCopyFrom.FontColor;
			settingsToCopyTo.GuardExtraHeight = settingsToCopyFrom.GuardExtraHeight;
			settingsToCopyTo.ModuleWidth = settingsToCopyFrom.ModuleWidth;
			settingsToCopyTo.NarrowWidth = settingsToCopyFrom.NarrowWidth;
			settingsToCopyTo.WideWidth = settingsToCopyFrom.WideWidth;
			settingsToCopyTo.OffsetHeight = settingsToCopyFrom.OffsetHeight;
			settingsToCopyTo.OffsetWidth = settingsToCopyFrom.OffsetWidth;
			settingsToCopyTo.QuietZone = settingsToCopyFrom.QuietZone;
			settingsToCopyTo.Font = settingsToCopyFrom.Font;
			settingsToCopyTo.TextPosition = settingsToCopyFrom.TextPosition;
			settingsToCopyTo.UseChecksum = settingsToCopyFrom.UseChecksum;
			settingsToCopyTo.Unit = settingsToCopyFrom.Unit;
		}

		/// <summary>
		/// Converts and stores the settings' convertible properties to another unit of measure.
		/// </summary>
		/// <param name="fromUnit">The unit the settings properties are.</param>
		/// <param name="toUnit">the unit to convert to.</param>
		public void ConvertValues(BarCodeUnit fromUnit, BarCodeUnit toUnit) {
			_settings.BarHeight = UnitConverter.Convert(_settings.BarHeight, fromUnit, toUnit);
			_settings.GuardExtraHeight = UnitConverter.Convert(_settings.GuardExtraHeight, fromUnit, toUnit);
			_settings.ModuleWidth = UnitConverter.Convert(_settings.ModuleWidth, fromUnit, toUnit);
			_settings.NarrowWidth = UnitConverter.Convert(_settings.NarrowWidth, fromUnit, toUnit);
			_settings.WideWidth = UnitConverter.Convert(_settings.WideWidth, fromUnit, toUnit);
			_settings.OffsetHeight = UnitConverter.Convert(_settings.OffsetHeight, fromUnit, toUnit);
			_settings.OffsetWidth = UnitConverter.Convert(_settings.OffsetWidth, fromUnit, toUnit);
			_settings.QuietZone = UnitConverter.Convert(_settings.QuietZone, fromUnit, toUnit);
		}

		IBarCodeSettings _settings;
	}
}
