using System;
using System.Collections;

namespace NBarCodes { 

	[Serializable]
	class Interleaved25 : Base25 {
		private static readonly BitArray start	= BitArrayHelper.ToBitArray("1010");
		private static readonly BitArray end		= BitArrayHelper.ToBitArray("1101");

		protected override BitArray Start { get { return start; } }
		protected override BitArray End { get { return end; } }

		protected override float SymbolWidth { 
			// 2 of 5 : 2 are wide, 3 are narrow, for bars and spaces (*2)
			// but we devided by 2 because the barcode is interleaved (/2)
			get { return (2 * WideWidth + 3 * NarrowWidth); } 
		}

		protected override float GuardWidth { 
			get { return NarrowWidth * 8; }
		}

		protected override void Draw(IBarCodeBuilder builder, string data) {
			int count = data.Length + (UseChecksum ? 1 : 0);
			if (count % 2 != 0)
				throw new BarCodeFormatException("Interleaved 2 of 5 must encode an even number of elements.");

			base.Draw(builder, data);
		}

		protected override float DrawSymbol(IBarCodeBuilder builder, float x, float y, float height, BitArray symbol) {
			// the symbol for Wide is encoded as 1 and
			// the symbol for Narrow is encoded as 0
			// five elements denote a symbol
			// symbols are interleaved with the next symbol
			// so one symbol represents the bars and the next, the spaces

			for (int i = 0; i < symbol.Count; i += 10) {
				for (int j = 0; j < 5; ++j) {
					// draw a bar
					float width = symbol[i+j] ? WideWidth : NarrowWidth;
					builder.DrawRectangle(BarColor, x, y, width, height);
					x += width;
					// skip a space
					x += symbol[i+j+5] ? WideWidth : NarrowWidth;
				}
			}

			return x;
		}
	}

}
