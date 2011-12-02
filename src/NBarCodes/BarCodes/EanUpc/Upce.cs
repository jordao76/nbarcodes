using System;
using System.Collections;
using System.Drawing;

namespace NBarCodes {

	[Serializable]
	class Upce : EanUpc {
		private readonly new static BitArray RightGuard = BitArrayHelper.ToBitArray("010101");
		private readonly new static ISymbolEncoder ParityEncoder = new UpceParityEncoder();

		public override float TotalWidth { 
			get { return 51 * ModuleWidth + OffsetWidth * 2 + TextWidth * 2 + QuietZone * 2; }
		}

		public string FromUpca(string upca) {
			string manufacturer = upca.Substring(1, 5);
			string product = upca.Substring(6, 5);

			int prodNum = int.Parse(product);
			string last3manuf = manufacturer.Substring(2, 3); // last three digits of the manufacturer code
			if (last3manuf == "000" || last3manuf == "100" || last3manuf == "200") {
				if (prodNum < 1000) {
					return manufacturer.Substring(0, 2) + product.Substring(2, 3) + manufacturer[2];
				}
			}
			if (manufacturer.Substring(3, 2) == "00" && prodNum < 100) {
				return manufacturer.Substring(0, 3) + product.Substring(3, 2) + "3";
			}
			if (manufacturer[4] == '0' && prodNum < 10) {
				return manufacturer.Substring(0, 4) + product[4] + "4";
			}
			if (prodNum >= 5 && prodNum < 10) {
				return manufacturer + product[4];
			}

			throw new BarCodeFormatException("Invalid conversion from Upca to Upce.");
		}

		protected override void Draw(IBarCodeBuilder builder, string data) {
			Draw(builder, data, 12);
		}

		public override void Draw(IBarCodeBuilder builder, string data, string supplement) {
			// resolve the supplement barcode
			EanUpcSupplement supp = ResolveSupplement(supplement);

			// the data is Upca data
			data = Validate(data, 12);

			if (data[0] != '0' && data[0] != '1') {
				throw new BarCodeFormatException("Can't encode Upce for Upca Number System.");
			}

			// Upce data to be encoded
			string upce = FromUpca(data);

			// encode the Upce symbol using the number system and check digit of the Upca data
			BitArray[] encoded = EncodeLeft(upce, ParityEncoder.Encode(data[0].ToString() + data[11]));

			// calculate total width
			float totalWidth = TotalWidth + 
				(supp == null ? 0 : SupplementOffset + supp.TotalWidth);

			// set the canvas size
			builder.Prepare(totalWidth, TotalHeight);

			// draw the background
			builder.DrawRectangle(BackColor, 0, 0, totalWidth, TotalHeight);

			// draw the barcode
			float x = 0, y = 0;
			float[] textX = new float[3];
			x += OffsetWidth + QuietZone;
			y += OffsetHeight;
			textX[0] = x;
			x += TextWidth;
			x = DrawSymbol(builder, x, y, BarHeight + GuardExtraHeight, LeftGuard);
			textX[1] = x;
			x = DrawSymbols(builder, x, y, BarHeight, encoded);
			x = DrawSymbol(builder, x, y, BarHeight + GuardExtraHeight, RightGuard);
			textX[2] = x;

			// draw the text strings
			DrawText(builder, textX, y - TextHeight, new string[] {data[0].ToString(), upce, data[11].ToString()});

			// draw the supplement barcode, if one is present
			if (supp != null) supp.Draw(builder, supplement, x + TextWidth + SupplementOffset, y - OffsetHeight);
		}

	}

}
