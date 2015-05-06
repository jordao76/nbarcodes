using System.Collections;

namespace NBarCodes {
  sealed class PostNetEncoder : TableEncoder {
    private static BitArray[] Symbols;

    private void Populate() {
      Symbols = BitArrayHelper.ToBitMatrix(
        "11000", "00011", "00101", "00110", "01001",
        "01010", "01100", "10001", "10010", "10100"
      );
    }

    protected override BitArray LookUp(int index) {
      if (Symbols == null) {
        Populate();
      }
      return Symbols[index];
    }
  }
}
