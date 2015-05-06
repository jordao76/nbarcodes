using System;
using System.Collections;

namespace NBarCodes {

  class EanLeftOddEncoder : TableEncoder {
    private readonly static BitArray[] leftOddSymbols;

    static EanLeftOddEncoder() {
      leftOddSymbols = new BitArray[] {
        BitArrayHelper.ToBitArray("0001101"),
        BitArrayHelper.ToBitArray("0011001"),
        BitArrayHelper.ToBitArray("0010011"),
        BitArrayHelper.ToBitArray("0111101"),
        BitArrayHelper.ToBitArray("0100011"),
        BitArrayHelper.ToBitArray("0110001"),
        BitArrayHelper.ToBitArray("0101111"),
        BitArrayHelper.ToBitArray("0111011"),
        BitArrayHelper.ToBitArray("0110111"),
        BitArrayHelper.ToBitArray("0001011")
      };
    }

    protected override BitArray LookUp(int index) {
      return leftOddSymbols[index];
    }
  }

  class EanLeftEvenEncoder : TableEncoder {
    private readonly static BitArray[] leftEvenSymbols;

    static EanLeftEvenEncoder() {
      leftEvenSymbols = new BitArray[] {
        BitArrayHelper.ToBitArray("0100111"),
        BitArrayHelper.ToBitArray("0110011"),
        BitArrayHelper.ToBitArray("0011011"),
        BitArrayHelper.ToBitArray("0100001"),
        BitArrayHelper.ToBitArray("0011101"),
        BitArrayHelper.ToBitArray("0111001"),
        BitArrayHelper.ToBitArray("0000101"),
        BitArrayHelper.ToBitArray("0010001"),
        BitArrayHelper.ToBitArray("0001001"),
        BitArrayHelper.ToBitArray("0010111")
      };
    }

    protected override BitArray LookUp(int index) {
      return leftEvenSymbols[index];
    }
  }

  class EanRightEncoder : TableEncoder {
    private readonly static BitArray[] rightSymbols;

    static EanRightEncoder() {
      rightSymbols = new BitArray[] {
        BitArrayHelper.ToBitArray("1110010"),
        BitArrayHelper.ToBitArray("1100110"),
        BitArrayHelper.ToBitArray("1101100"),
        BitArrayHelper.ToBitArray("1000010"),
        BitArrayHelper.ToBitArray("1011100"),
        BitArrayHelper.ToBitArray("1001110"),
        BitArrayHelper.ToBitArray("1010000"),
        BitArrayHelper.ToBitArray("1000100"),
        BitArrayHelper.ToBitArray("1001000"),
        BitArrayHelper.ToBitArray("1110100")
      };
    }

    protected override BitArray LookUp(int index) {
      return rightSymbols[index];
    }
  }

  class EanParityEncoder : TableEncoder {
    private readonly static BitArray[] paritySymbols;

    static EanParityEncoder() {
      paritySymbols = new BitArray[] {
        BitArrayHelper.ToBitArray("111111"),
        BitArrayHelper.ToBitArray("110100"),
        BitArrayHelper.ToBitArray("110010"),
        BitArrayHelper.ToBitArray("110001"),
        BitArrayHelper.ToBitArray("101100"),
        BitArrayHelper.ToBitArray("100110"),
        BitArrayHelper.ToBitArray("100011"),
        BitArrayHelper.ToBitArray("101010"),
        BitArrayHelper.ToBitArray("101001"),
        BitArrayHelper.ToBitArray("100101")
      };
    }

    protected override BitArray LookUp(int index) {
      return paritySymbols[index];
    }
  }

  class UpceParityEncoder : ISymbolEncoder {
    private readonly static BitArray[][] paritySymbols;

    static UpceParityEncoder() {
      paritySymbols = new BitArray[][] {
        new BitArray[] {
          BitArrayHelper.ToBitArray("000111"),
          BitArrayHelper.ToBitArray("001011"),
          BitArrayHelper.ToBitArray("001101"),
          BitArrayHelper.ToBitArray("001110"),
          BitArrayHelper.ToBitArray("010011"),
          BitArrayHelper.ToBitArray("011001"),
          BitArrayHelper.ToBitArray("011100"),
          BitArrayHelper.ToBitArray("010101"),
          BitArrayHelper.ToBitArray("010110"),
          BitArrayHelper.ToBitArray("011010")
        },
        new BitArray[] {
          BitArrayHelper.ToBitArray("111000"),
          BitArrayHelper.ToBitArray("110100"),
          BitArrayHelper.ToBitArray("110010"),
          BitArrayHelper.ToBitArray("110001"),
          BitArrayHelper.ToBitArray("101100"),
          BitArrayHelper.ToBitArray("100110"),
          BitArrayHelper.ToBitArray("100011"),
          BitArrayHelper.ToBitArray("101010"),
          BitArrayHelper.ToBitArray("101001"),
          BitArrayHelper.ToBitArray("100101")
        }
      };
    }

    private BitArray LookUp(int numberSystem, int checkDigit) {
      return paritySymbols[numberSystem][checkDigit];
    }

    public BitArray Encode(string data) {
      // the data must contain 2 items: the number system and the check digit
      if (data.Length != 2) throw new ArgumentException();

      // may throw IndexOutOfRangeException
      return (BitArray) LookUp(data[0] - '0', data[1] - '0').Clone();
    }

    BitArray ISymbolEncoder.Encode(char datum) {
      // can't encode single datum
      throw new InvalidOperationException("Can't encode single char datum.");
    }

  }

  class EanUpc2DigitSupplementParityEncoder : TableEncoder {
    private readonly static BitArray[] paritySymbols;

    static EanUpc2DigitSupplementParityEncoder() {
      paritySymbols = new BitArray[] {
        BitArrayHelper.ToBitArray("11"),
        BitArrayHelper.ToBitArray("10"),
        BitArrayHelper.ToBitArray("01"),
        BitArrayHelper.ToBitArray("00")
      };
    }

    protected override BitArray LookUp(int index) {
      return paritySymbols[index];
    }
  }

  class EanUpc5DigitSupplementParityEncoder : TableEncoder {
    private readonly static BitArray[] paritySymbols;

    static EanUpc5DigitSupplementParityEncoder() {
      paritySymbols = new BitArray[] {
        BitArrayHelper.ToBitArray("00111"),
        BitArrayHelper.ToBitArray("01011"),
        BitArrayHelper.ToBitArray("01101"),
        BitArrayHelper.ToBitArray("01110"),
        BitArrayHelper.ToBitArray("10011"),
        BitArrayHelper.ToBitArray("11001"),
        BitArrayHelper.ToBitArray("11100"),
        BitArrayHelper.ToBitArray("10101"),
        BitArrayHelper.ToBitArray("10110"),
        BitArrayHelper.ToBitArray("11010")
      };
    }

    protected override BitArray LookUp(int index) {
      return paritySymbols[index];
    }
  }

}
