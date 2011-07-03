using System;
using System.Collections;

namespace NBarCodes {

	[Serializable]
	class Code128 : ModuleBarCode {
		private readonly static BitArray StopGuard = BitArrayHelper.ToBitArray("11000111010");
		private readonly static BitArray EndGuard = BitArrayHelper.ToBitArray("11");

		private readonly static ISymbolEncoder Encoder = new Code128Encoder();

		public Code128() {
			TextPosition = TextPosition.Bottom;
		}

		private float GuardWidth {
			get { return ModuleWidth * 13; }
		}

		protected override void Draw(IBarCodeBuilder builder, string data) {
      ValidateCharacters(data);

			string coded = new Code128Coder().Code(data);

      coded = AppendChecksum(coded);

			BitArray encoded = Encoder.Encode(coded);

			float totalWidth = ModuleWidth * encoded.Count + GuardWidth + OffsetWidth * 2 + QuietZone * 2;

			// set the canvas size
			builder.Prepare(totalWidth, TotalHeight);

			// draw the background
			builder.DrawRectangle(BackColor, 0, 0, totalWidth, TotalHeight);

			// draw the barcode
			float x = 0, y = 0;
			float textX = x + totalWidth / 2;
			x += OffsetWidth + QuietZone;
			y += OffsetHeight + ExtraTopHeight;
			x = DrawSymbol(builder, x, y, BarHeight, encoded);
			x = DrawSymbol(builder, x, y, BarHeight, StopGuard);
			x = DrawSymbol(builder, x, y, BarHeight, EndGuard);

			DrawText(builder, true, new float[] {textX}, y - TextHeight, data);
		}

    private void ValidateCharacters(string data) {
      foreach (char c in data) {
        if (c >= 128) {
          throw new BarCodeFormatException("The barcode has invalid data.");
        }
      }
    }

    private string AppendChecksum(string coded) {
      if (Checksum == null) {
        Checksum = new Code128Checksum();
      }
      // append the checksum - note: the checksum won't appear in the text string
      coded += Checksum.Calculate(coded);
      return coded;
    }

	}
}
