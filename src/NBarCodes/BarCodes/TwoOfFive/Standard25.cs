using System;
using System.Collections;

namespace NBarCodes { 

	[Serializable]
	class Standard25 : Base25 {
		private static readonly BitArray start = BitArrayHelper.ToBitArray("11011010");
		private static readonly BitArray end	 = BitArrayHelper.ToBitArray("11010110");

		protected override BitArray Start { get { return start; } }
		protected override BitArray End { get { return end; } }

		protected override float SymbolWidth { 
			// 2 of 5 : 2 are wide, 3 are narrow, plus 5 narrow spaces
			get { return 2 * WideWidth + 8 * NarrowWidth; } 
		}

		protected override float GuardWidth { 
			get { return NarrowWidth * 16; }
		}

		protected override float DrawSymbol(IBarCodeBuilder builder, float x, float y, float height, BitArray symbol) {
			// the symbol for Wide is encoded as 1 and
			// the symbol for Narrow is encoded as 0
			// a space lies between every symbol and
			// has the width of the Narrow bar
			// only bars are encoded in the symbols

			foreach (bool bit in symbol) {
				// draw bar
				float width = bit ? WideWidth : NarrowWidth;
				builder.DrawRectangle(BarColor, x, y, width, height);
				x += width + NarrowWidth; // skip bar and white space
			}

			return x;
		}
	}

}
