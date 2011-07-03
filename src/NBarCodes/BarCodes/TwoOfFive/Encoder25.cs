using System;
using System.Collections;

namespace NBarCodes { 

	class Encoder25 : TableEncoder {
		private readonly static BitArray[] symbols;

		static Encoder25() {
			symbols = new BitArray[] {
				BitArrayHelper.ToBitArray("00110"),
				BitArrayHelper.ToBitArray("10001"),
				BitArrayHelper.ToBitArray("01001"),
				BitArrayHelper.ToBitArray("11000"),
				BitArrayHelper.ToBitArray("00101"),
				BitArrayHelper.ToBitArray("10100"),
				BitArrayHelper.ToBitArray("01100"),
				BitArrayHelper.ToBitArray("00011"),
				BitArrayHelper.ToBitArray("10010"),
				BitArrayHelper.ToBitArray("01010")
			};
		}

		protected override BitArray LookUp(int index) {
			return symbols[index];
		}

	}

}
