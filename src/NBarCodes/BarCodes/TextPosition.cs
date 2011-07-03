using System;

namespace NBarCodes {

	/// <summary>
	/// Enumeration to position string elements in barcodes.
	/// </summary>
	/// <remarks>
	/// This enumeration positions string elements in barcodes. It has a Flags (<see cref="FlagsAttribute"/>)
	/// attribute to allow for more than one option (through bitwise or).
	/// </remarks>
	[Flags]
	public enum TextPosition {
		
		/// <summary>
		/// The text won't appear in the rendered barcode.
		/// </summary>
		None = 0,

		/// <summary>
		/// The text will be rendered on top of the barcode.
		/// </summary>
		Top = 1,

		/// <summary>
		/// The text will be rendered on the bottom of the barcode.
		/// </summary>
		Bottom = 2,
		
		/// <summary>
		/// The text will be rendered on top and on the bottom of the barcode.
		/// </summary>
		All = Top | Bottom
	}

}
