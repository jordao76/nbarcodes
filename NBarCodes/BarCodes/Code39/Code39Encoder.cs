using System.Text;
using System.Collections;

namespace NBarCodes {

  class Code39Encoder : TableEncoder {
    private readonly static Hashtable symbols;

    static Code39Encoder() {
      symbols = new Hashtable(44);
      symbols.Add('0', BitArrayHelper.ToBitArray("0001101000"));
      symbols.Add('1', BitArrayHelper.ToBitArray("1001000010"));
      symbols.Add('2', BitArrayHelper.ToBitArray("0011000010"));
      symbols.Add('3', BitArrayHelper.ToBitArray("1011000000"));
      symbols.Add('4', BitArrayHelper.ToBitArray("0001100010"));
      symbols.Add('5', BitArrayHelper.ToBitArray("1001100000"));
      symbols.Add('6', BitArrayHelper.ToBitArray("0011100000"));
      symbols.Add('7', BitArrayHelper.ToBitArray("0001001010"));
      symbols.Add('8', BitArrayHelper.ToBitArray("1001001000"));
      symbols.Add('9', BitArrayHelper.ToBitArray("0011001000"));
      symbols.Add('A', BitArrayHelper.ToBitArray("1000010010"));
      symbols.Add('B', BitArrayHelper.ToBitArray("0010010010"));
      symbols.Add('C', BitArrayHelper.ToBitArray("1010010000"));
      symbols.Add('D', BitArrayHelper.ToBitArray("0000110010"));
      symbols.Add('E', BitArrayHelper.ToBitArray("1000110000"));
      symbols.Add('F', BitArrayHelper.ToBitArray("0010110000"));
      symbols.Add('G', BitArrayHelper.ToBitArray("0000011010"));
      symbols.Add('H', BitArrayHelper.ToBitArray("1000011000"));
      symbols.Add('I', BitArrayHelper.ToBitArray("0010011000"));
      symbols.Add('J', BitArrayHelper.ToBitArray("0000111000"));
      symbols.Add('K', BitArrayHelper.ToBitArray("1000000110"));
      symbols.Add('L', BitArrayHelper.ToBitArray("0010000110"));
      symbols.Add('M', BitArrayHelper.ToBitArray("1010000100"));
      symbols.Add('N', BitArrayHelper.ToBitArray("0000100110"));
      symbols.Add('O', BitArrayHelper.ToBitArray("1000100100"));
      symbols.Add('P', BitArrayHelper.ToBitArray("0010100100"));
      symbols.Add('Q', BitArrayHelper.ToBitArray("0000001110"));
      symbols.Add('R', BitArrayHelper.ToBitArray("1000001100"));
      symbols.Add('S', BitArrayHelper.ToBitArray("0010001100"));
      symbols.Add('T', BitArrayHelper.ToBitArray("0000101100"));
      symbols.Add('U', BitArrayHelper.ToBitArray("1100000010"));
      symbols.Add('V', BitArrayHelper.ToBitArray("0110000010"));
      symbols.Add('W', BitArrayHelper.ToBitArray("1110000000"));
      symbols.Add('X', BitArrayHelper.ToBitArray("0100100010"));
      symbols.Add('Y', BitArrayHelper.ToBitArray("1100100000"));
      symbols.Add('Z', BitArrayHelper.ToBitArray("0110100000"));
      symbols.Add('-', BitArrayHelper.ToBitArray("0100001010"));
      symbols.Add('.', BitArrayHelper.ToBitArray("1100001000"));
      symbols.Add(' ', BitArrayHelper.ToBitArray("0110001000"));
      symbols.Add('*', BitArrayHelper.ToBitArray("0100101000"));
      symbols.Add('$', BitArrayHelper.ToBitArray("0101010000"));
      symbols.Add('/', BitArrayHelper.ToBitArray("0101000100"));
      symbols.Add('+', BitArrayHelper.ToBitArray("0100010100"));
      symbols.Add('%', BitArrayHelper.ToBitArray("0001010100"));
    }

    // will work with a char too
    protected override BitArray LookUp(int index) {
      return (BitArray) symbols[(char)index];
    }

    public override BitArray Encode(char datum) {
      // encode the symbol
      // may throw IndexOutOfRangeException
      BitArray bits = (BitArray) LookUp(datum).Clone();

      // return the encoded symbol
      return bits;
    }

    public static bool CanEncode(string data) {
      foreach (char datum in data) {
        if (datum == '*' || !symbols.ContainsKey(datum)) return false;
      }
      return true;
    }

  }

  class Code39Translator {

    private Code39Translator() {}

    private readonly static char[] keys = new char[] {
      '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D',
      'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R',
      'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '-', '.', ' ', '$', '/', '+',
      '%', '*' // <= asterisk is part of the keys?
    };

    /// <summary>Mapping for all ascii codes less than 128.</summary>
    private readonly static string[] mapping;

    static Code39Translator() {
      mapping = new string[] { // indexed by ascii code
        "%U", "$A", "$B", "$C", "$D", "$E", "$F", "$G", "$H", "$I", // 0-9
        "$J", "$K", "$L", "$M", "$N", "$O", "$P", "$Q", "$R", "$S", // 10-19
        "$T", "$U", "$V", "$W", "$X", "$Y", "$Z", "%A", "%B", "%C", // 20-29
        "%D", "%E", " ",  "/A", "/B", "/C", "/D", "/E", "/F", "/G", // 30-39
        "/H", "/I", "/J", "/K", "/L", "-",  ".",  "/O", "0",  "1",  // 40-49
        "2",  "3",  "4",  "5",  "6",  "7",  "8",  "9",  "/Z", "%F", // 50-59
        "%G", "%H", "%I", "%J", "%V", "A",  "B",  "C",  "D",  "E",  // 60-69
        "F",  "G",  "H",  "I",  "J",  "K",  "L",  "M",  "N",  "O",  // 70-79
        "P",  "Q",  "R",  "S",  "T",  "U",  "V",  "W",  "X",  "Y",  // 80-89
        "Z",  "%K", "%L", "%M", "%N", "%O", "%W", "+A", "+B", "+C", // 90-99
        "+D", "+E", "+F", "+G", "+H", "+I", "+J", "+K", "+L", "+M", // 100-109
        "+N", "+O", "+P", "+Q", "+R", "+S", "+T", "+U", "+V", "+W", // 110-119
        "+X", "+Y", "+Z", "%P", "%Q", "%R", "%S",                   // 120-126
        "%T" // <- 127 - DEL, could also be encoded as %X, %Y, %Z
      };
    }

    public static int CheckValue(char key) {
      for (int i = 0; i < keys.Length; ++i) {
        if (key == keys[i]) return i; // use a hashtable for performance??
      }
      return -1;
    }

    public static string TranslateExtended(string data) {
      var sb = new StringBuilder();
      foreach (char c in data) {
        sb.Append(mapping[c]);
      }
      return sb.ToString();
    }
  
  }

}
