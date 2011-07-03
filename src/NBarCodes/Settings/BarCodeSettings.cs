using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Web.UI.WebControls;

namespace NBarCodes {

	/// <summary>
	/// Packages settings for barcodes.
	/// Canonical implementation of <see cref="IBarCodeSettings"/>.
	/// </summary>
	[Serializable]
	public class BarCodeSettings : IBarCodeSettings {

		/// <summary>
		/// The type of the barcode.
		/// </summary>
		public BarCodeType Type {
			get { return _type; }
			set { _type = value; }
		} BarCodeType _type = BarCodeType.Code128;

		/// <summary>
		/// The data to render with the barcode.
		/// </summary>
		public string Data {
			get { return _data; }
			set { _data = value; }
		} string _data = "12345";

		/// <summary>
		/// The unit of measure of the barcode's measurable properties.
		/// </summary>
		public BarCodeUnit Unit {
			get { return _unit; }
			set { _unit = value; }
		} BarCodeUnit _unit = BarCodeUnit.Pixel;

		/// <summary>
		/// The back color of the barcode.
		/// </summary>
		public Color BackColor { 
			get { return _backColor; } 
			set { _backColor = value; }
		} Color _backColor = Color.White;

		/// <summary>
		/// The color of the bar of the barcode.
		/// </summary>
		public Color BarColor {
			get { return _barColor; } 
			set { _barColor = value; }
		} Color _barColor = Color.Black;

		/// <summary>
		/// The height of the barcode. Affected by <see cref="Unit"/>.
		/// </summary>
		public float BarHeight {
			get { return _barHeight; } 
			set { _barHeight = value; }
		} float _barHeight = 50f;

		/// <summary>
		/// The color of the font to render text in the barcode.
		/// </summary>
		public Color FontColor { 
			get { return _fontColor; } 
			set { _fontColor = value; }
		} Color _fontColor = Color.Black;

		/// <summary>
		/// The height of the extra height of the bar for EAN like barcodes.
		/// Affected by <see cref="Unit"/>.
		/// </summary>
		public float GuardExtraHeight {
			get { return _guardExtraHeight; } 
			set { _guardExtraHeight = value; }
		} float _guardExtraHeight = 10f;

		/// <summary>
		/// The width of bar of the barcode for module-based barcodes (<see cref="ModuleBarCode"/>).
		/// Affected by <see cref="Unit"/>.
		/// </summary>
		public float ModuleWidth {
			get { return _moduleWidth; } 
			set { _moduleWidth = value; }
		} float _moduleWidth = 1f;

		/// <summary>
		/// The width of the narrow component of a thickness-based barcode (<see cref="ThicknessBarCode"/>).
		/// Affected by <see cref="Unit"/>.
		/// </summary>
		public float NarrowWidth {
			get { return _narrowWidth; } 
			set { _narrowWidth = value; }
		} float _narrowWidth = 1f;

		/// <summary>
		/// The width of the wide component of a thickness-based barcode (<see cref="ThicknessBarCode"/>).
		/// Affected by <see cref="Unit"/>.
		/// </summary>
		public float WideWidth {
			get { return _wideWidth; } 
			set { _wideWidth = value; }
		} float _wideWidth = 3f;

		/// <summary>
		/// The vertical (top and bottom) offset height of the barcode to the border.
		/// Affected by <see cref="Unit"/>.
		/// </summary>
		public float OffsetHeight {
			get { return _offsetHeight; } 
			set { _offsetHeight = value; }
		} float _offsetHeight = 5f;

		/// <summary>
		/// The horizontal (left and right) offset width of the barcode to the border.
		/// Affected by <see cref="Unit"/>.
		/// </summary>
		public float OffsetWidth {
			get { return _offsetWidth; } 
			set { _offsetWidth = value; }
		} float _offsetWidth = 5f;

		/// <summary>
		/// The width of the horizontal (left and right) quiet zone of the barcode.
		/// Affected by <see cref="Unit"/>.
		/// </summary>
		public float QuietZone {
			get { return _quietZone; } 
			set { _quietZone = value; }
		} float _quietZone = 0;

		/// <summary>
		/// The font used to render the text inside the barcode.
		/// </summary>
		public Font Font {
			get { return _font; } 
			set { _font = value; }
		} Font _font = new Font("verdana", 8);

		/// <summary>
		/// The position of the text rendered in the barcode.
		/// </summary>
		public TextPosition TextPosition {
			get { return _textPosition; } 
			set { _textPosition = value; }
		} TextPosition _textPosition = TextPosition.Bottom;

		/// <summary>
		/// Whether the barcode will use an (optional) checksum.
		/// Not every barcode requires a checksum, and others mandate it.
		/// </summary>
		public bool UseChecksum {
			get { return _useChecksum; } 
			set { _useChecksum = value; }
		} bool _useChecksum = false;

	}
}
