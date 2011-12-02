using System;

namespace NBarCodes {
	
	/// <summary>
	/// Class to convert between units of measure of barcodes 
	/// (<see cref="BarCodeType"/>).
	/// </summary>
	public sealed class UnitConverter {

		/// <summary>Default DPI of the rendering device (screen monitor).</summary>
		private const float DEFAULT_DPI = 96f;

		// conversion ratios
		private const float IN2CM = 2.54f;					// inch to centimeter ratio
		private const float CM2IN = 1/IN2CM;				// centimeter to inch ratio
		private const float CM2MM = 10f;						// centimeter to millimeter ratio
		private const float MM2CM = 1/CM2MM;				// milimeter to centimeter ratio
		private const float IN2MM = 1/CM2IN*CM2MM;	// inch to milimeter ratio
		private const float MM2IN = 1/CM2MM*CM2IN;	// milimeter to inch ratio

		// this table is indexed by the BarCodeUnit values
		private static float[,] ConversionTable = {
		/*				cm				mm				in	*/
		/*cm*/	{ 1,				CM2MM,		CM2IN }, 
		/*mm*/	{ MM2CM,		1,				MM2IN },
		/*in*/	{ IN2CM,		IN2MM,		1			}
		};

		/// <summary>Uninstantiatable.</summary>
		private UnitConverter() {}

		/// <summary>
		/// Converts a value between measuring units.
		/// The default DPI for conversions is set at 96.
		/// </summary>
		/// <param name="value">Value to be converted.</param>
		/// <param name="sourceUnit">The unit to convert from.</param>
		/// <param name="targetUnit">The unit to convert to.</param>
		/// <returns>The converted unit.</returns>
		public static float Convert(float value, BarCodeUnit sourceUnit, BarCodeUnit targetUnit) {
			return Convert(value, sourceUnit, targetUnit, DEFAULT_DPI);
		}

		/// <summary>
		/// Converts a value between measuring units.
		/// </summary>
		/// <param name="value">Value to be converted.</param>
		/// <param name="sourceUnit">The unit to convert from.</param>
		/// <param name="targetUnit">The unit to convert to.</param>
		/// <param name="dpi">The DPI of the rendering device.</param>
		/// <returns>The converted unit.</returns>
		public static float Convert(float value, BarCodeUnit sourceUnit, BarCodeUnit targetUnit, float dpi) {
			if (dpi <= 0) {
				throw new ArgumentException("DPI must be greater than zero.");
			}

			if (sourceUnit == targetUnit) {
				return value;
			}

			// keep track of the value being converted
			float converted = value;

			int sourceUnitIndex = (int)sourceUnit;
			if (sourceUnit == BarCodeUnit.Pixel) {
				// convert the value from pixels to inches
				converted /= dpi;
				sourceUnitIndex = (int)BarCodeUnit.Inch;
			}

			int targetUnitIndex = (int)targetUnit;
			if (targetUnit == BarCodeUnit.Pixel) {
				// apply the inch to pixel ratio (the DPI)
				converted *= dpi;
				// convert to inches rather than to pixels
				targetUnitIndex = (int)BarCodeUnit.Inch;
			}

			converted *= ConversionTable [sourceUnitIndex, targetUnitIndex];

			return converted;
		}

	}
}
