using System;
using System.Collections;

namespace NBarCodes {

  class Code128Encoder : ISymbolEncoder {

    // consider using this on code 39!!
    private class SymbolWrap {
      private readonly BitArray symbol;
      private readonly int value;

      public SymbolWrap(BitArray s, int v) {
        symbol = s;
        value = v;
      }

      public SymbolWrap(string s, int v) {
        symbol = BitArrayHelper.ToBitArray(s);
        value = v;
      }

      public BitArray Symbol {
        get { return symbol; }
      }
      public int Value {
        get { return value; }
      }
    }

    private static readonly Hashtable codeA;
    private static readonly Hashtable codeB;
    private static readonly Hashtable codeC;

    static Code128Encoder() {
      codeA = new Hashtable(105);
      codeB = new Hashtable(105);
      codeC = new Hashtable(105);

      codeA[" "]=codeB[" "]=codeC["00"]=new SymbolWrap("11011001100",0);
      codeA["!"]=codeB["!"]=codeC["01"]=new SymbolWrap("11001101100",1);
      codeA["\""]=codeB["\""]=codeC["02"]=new SymbolWrap("11001100110",2);
      codeA["#"]=codeB["#"]=codeC["03"]=new SymbolWrap("10010011000",3);
      codeA["$"]=codeB["$"]=codeC["04"]=new SymbolWrap("10010001100",4);
      codeA["%"]=codeB["%"]=codeC["05"]=new SymbolWrap("10001001100",5);
      codeA["&"]=codeB["&"]=codeC["06"]=new SymbolWrap("10011001000",6);
      codeA["'"]=codeB["'"]=codeC["07"]=new SymbolWrap("10011000100",7);
      codeA["("]=codeB["("]=codeC["08"]=new SymbolWrap("10001100100",8);
      codeA[")"]=codeB[")"]=codeC["09"]=new SymbolWrap("11001001000",9);
      codeA["*"]=codeB["*"]=codeC["10"]=new SymbolWrap("11001000100",10);
      codeA["+"]=codeB["+"]=codeC["11"]=new SymbolWrap("11000100100",11);
      codeA[","]=codeB[","]=codeC["12"]=new SymbolWrap("10110011100",12);
      codeA["-"]=codeB["-"]=codeC["13"]=new SymbolWrap("10011011100",13);
      codeA["."]=codeB["."]=codeC["14"]=new SymbolWrap("10011001110",14);
      codeA["/"]=codeB["/"]=codeC["15"]=new SymbolWrap("10111001100",15);
      codeA["0"]=codeB["0"]=codeC["16"]=new SymbolWrap("10011101100",16);
      codeA["1"]=codeB["1"]=codeC["17"]=new SymbolWrap("10011100110",17);
      codeA["2"]=codeB["2"]=codeC["18"]=new SymbolWrap("11001110010",18);
      codeA["3"]=codeB["3"]=codeC["19"]=new SymbolWrap("11001011100",19);
      codeA["4"]=codeB["4"]=codeC["20"]=new SymbolWrap("11001001110",20);
      codeA["5"]=codeB["5"]=codeC["21"]=new SymbolWrap("11011100100",21);
      codeA["6"]=codeB["6"]=codeC["22"]=new SymbolWrap("11001110100",22);
      codeA["7"]=codeB["7"]=codeC["23"]=new SymbolWrap("11101101110",23);
      codeA["8"]=codeB["8"]=codeC["24"]=new SymbolWrap("11101001100",24);
      codeA["9"]=codeB["9"]=codeC["25"]=new SymbolWrap("11100101100",25);
      codeA[":"]=codeB[":"]=codeC["26"]=new SymbolWrap("11100100110",26);
      codeA[";"]=codeB[";"]=codeC["27"]=new SymbolWrap("11101100100",27);
      codeA["<"]=codeB["<"]=codeC["28"]=new SymbolWrap("11100110100",28);
      codeA["="]=codeB["="]=codeC["29"]=new SymbolWrap("11100110010",29);
      codeA[">"]=codeB[">"]=codeC["30"]=new SymbolWrap("11011011000",30);
      codeA["?"]=codeB["?"]=codeC["31"]=new SymbolWrap("11011000110",31);
      codeA["@"]=codeB["@"]=codeC["32"]=new SymbolWrap("11000110110",32);
      codeA["A"]=codeB["A"]=codeC["33"]=new SymbolWrap("10100011000",33);
      codeA["B"]=codeB["B"]=codeC["34"]=new SymbolWrap("10001011000",34);
      codeA["C"]=codeB["C"]=codeC["35"]=new SymbolWrap("10001000110",35);
      codeA["D"]=codeB["D"]=codeC["36"]=new SymbolWrap("10110001000",36);
      codeA["E"]=codeB["E"]=codeC["37"]=new SymbolWrap("10001101000",37);
      codeA["F"]=codeB["F"]=codeC["38"]=new SymbolWrap("10001100010",38);
      codeA["G"]=codeB["G"]=codeC["39"]=new SymbolWrap("11010001000",39);
      codeA["H"]=codeB["H"]=codeC["40"]=new SymbolWrap("11000101000",40);
      codeA["I"]=codeB["I"]=codeC["41"]=new SymbolWrap("11000100010",41);
      codeA["J"]=codeB["J"]=codeC["42"]=new SymbolWrap("10110111000",42);
      codeA["K"]=codeB["K"]=codeC["43"]=new SymbolWrap("10110001110",43);
      codeA["L"]=codeB["L"]=codeC["44"]=new SymbolWrap("10001101110",44);
      codeA["M"]=codeB["M"]=codeC["45"]=new SymbolWrap("10111011000",45);
      codeA["N"]=codeB["N"]=codeC["46"]=new SymbolWrap("10111000110",46);
      codeA["O"]=codeB["O"]=codeC["47"]=new SymbolWrap("10001110110",47);
      codeA["P"]=codeB["P"]=codeC["48"]=new SymbolWrap("11101110110",48);
      codeA["Q"]=codeB["Q"]=codeC["49"]=new SymbolWrap("11010001110",49);
      codeA["R"]=codeB["R"]=codeC["50"]=new SymbolWrap("11000101110",50);
      codeA["S"]=codeB["S"]=codeC["51"]=new SymbolWrap("11011101000",51);
      codeA["T"]=codeB["T"]=codeC["52"]=new SymbolWrap("11011100010",52);
      codeA["U"]=codeB["U"]=codeC["53"]=new SymbolWrap("11011101110",53);
      codeA["V"]=codeB["V"]=codeC["54"]=new SymbolWrap("11101011000",54);
      codeA["W"]=codeB["W"]=codeC["55"]=new SymbolWrap("11101000110",55);
      codeA["X"]=codeB["X"]=codeC["56"]=new SymbolWrap("11100010110",56);
      codeA["Y"]=codeB["Y"]=codeC["57"]=new SymbolWrap("11101101000",57);
      codeA["Z"]=codeB["Z"]=codeC["58"]=new SymbolWrap("11101100010",58);
      codeA["["]=codeB["["]=codeC["59"]=new SymbolWrap("11100011010",59);
      codeA["\\"]=codeB["\\"]=codeC["60"]=new SymbolWrap("11101111010",60);
      codeA["]"]=codeB["]"]=codeC["61"]=new SymbolWrap("11001000010",61);
      codeA["^"]=codeB["^"]=codeC["62"]=new SymbolWrap("11110001010",62);
      codeA["_"]=codeB["_"]=codeC["63"]=new SymbolWrap("10100110000",63);
      codeA[((char)0).ToString()]=codeB["`"]=codeC["64"]=new SymbolWrap("10100001100",64);
      codeA[((char)1).ToString()]=codeB["a"]=codeC["65"]=new SymbolWrap("10010110000",65);
      codeA[((char)2).ToString()]=codeB["b"]=codeC["66"]=new SymbolWrap("10010000110",66);
      codeA[((char)3).ToString()]=codeB["c"]=codeC["67"]=new SymbolWrap("10000101100",67);
      codeA[((char)4).ToString()]=codeB["d"]=codeC["68"]=new SymbolWrap("10000100110",68);
      codeA[((char)5).ToString()]=codeB["e"]=codeC["69"]=new SymbolWrap("10110010000",69);
      codeA[((char)6).ToString()]=codeB["f"]=codeC["70"]=new SymbolWrap("10110000100",70);
      codeA[((char)7).ToString()]=codeB["g"]=codeC["71"]=new SymbolWrap("10011010000",71);
      codeA[((char)8).ToString()]=codeB["h"]=codeC["72"]=new SymbolWrap("10011000010",72);
      codeA[((char)9).ToString()]=codeB["i"]=codeC["73"]=new SymbolWrap("10000110100",73);
      codeA[((char)10).ToString()]=codeB["j"]=codeC["74"]=new SymbolWrap("10000110010",74);
      codeA[((char)11).ToString()]=codeB["k"]=codeC["75"]=new SymbolWrap("11000010010",75);
      codeA[((char)12).ToString()]=codeB["l"]=codeC["76"]=new SymbolWrap("11001010000",76);
      codeA[((char)13).ToString()]=codeB["m"]=codeC["77"]=new SymbolWrap("11110111010",77);
      codeA[((char)14).ToString()]=codeB["n"]=codeC["78"]=new SymbolWrap("11000010100",78);
      codeA[((char)15).ToString()]=codeB["o"]=codeC["79"]=new SymbolWrap("10001111010",79);
      codeA[((char)16).ToString()]=codeB["p"]=codeC["80"]=new SymbolWrap("10100111100",80);
      codeA[((char)17).ToString()]=codeB["q"]=codeC["81"]=new SymbolWrap("10010111100",81);
      codeA[((char)18).ToString()]=codeB["r"]=codeC["82"]=new SymbolWrap("10010011110",82);
      codeA[((char)19).ToString()]=codeB["s"]=codeC["83"]=new SymbolWrap("10111100100",83);
      codeA[((char)20).ToString()]=codeB["t"]=codeC["84"]=new SymbolWrap("10011110100",84);
      codeA[((char)21).ToString()]=codeB["u"]=codeC["85"]=new SymbolWrap("10011110010",85);
      codeA[((char)22).ToString()]=codeB["v"]=codeC["86"]=new SymbolWrap("11110100100",86);
      codeA[((char)23).ToString()]=codeB["w"]=codeC["87"]=new SymbolWrap("11110010100",87);
      codeA[((char)24).ToString()]=codeB["x"]=codeC["88"]=new SymbolWrap("11110010010",88);
      codeA[((char)25).ToString()]=codeB["y"]=codeC["89"]=new SymbolWrap("11011011110",89);
      codeA[((char)26).ToString()]=codeB["z"]=codeC["90"]=new SymbolWrap("11011110110",90);
      codeA[((char)27).ToString()]=codeB["{"]=codeC["91"]=new SymbolWrap("11110110110",91);
      codeA[((char)28).ToString()]=codeB["|"]=codeC["92"]=new SymbolWrap("10101111000",92);
      codeA[((char)29).ToString()]=codeB["}"]=codeC["93"]=new SymbolWrap("10100011110",93);
      codeA[((char)30).ToString()]=codeB["~"]=codeC["94"]=new SymbolWrap("10001011110",94);
      codeA[((char)31).ToString()]=codeB[((char)127).ToString()]=codeC["95"]=new SymbolWrap("10111101000",95);
      codeA[FNC3.ToString()]=codeB[FNC3.ToString()]=codeC["96"]=new SymbolWrap("10111100010",96);
      codeA[FNC2.ToString()]=codeB[FNC2.ToString()]=codeC["97"]=new SymbolWrap("11110101000",97);
      codeA[SHIFT.ToString()]=codeB[SHIFT.ToString()]=codeC["98"]=new SymbolWrap("11110100010",98);
      codeA[CodeC.ToString()]=codeB[CodeC.ToString()]=codeC["99"]=new SymbolWrap("10111011110",99);
      codeA[CodeB.ToString()]=codeB[FNC4.ToString()]=codeC[CodeB.ToString()]=new SymbolWrap("10111101110",100);
      codeA[FNC4.ToString()]=codeB[CodeA.ToString()]=codeC[CodeA.ToString()]=new SymbolWrap("11101011110",101);
      codeA[FNC1.ToString()]=codeB[FNC1.ToString()]=codeC[FNC1.ToString()]=new SymbolWrap("11110101110",102);
      codeA[StartA.ToString()]=codeB[StartA.ToString()]=codeC[StartA.ToString()]=new SymbolWrap("11010000100",103);
      codeA[StartB.ToString()]=codeB[StartB.ToString()]=codeC[StartB.ToString()]=new SymbolWrap("11010010000",104);
      codeA[StartC.ToString()]=codeB[StartC.ToString()]=codeC[StartC.ToString()]=new SymbolWrap("11010011100",105);
    }

    // Code 128 Special characters
    // The last seven characters of Code Sets A and B (character values 96 - 102) 
    // and the last three characters of Code Set C (character values 100 - 102) 
    // are special non-data characters with no ASCII character equivalents, 
    // which have particular significance to the barcode reading device.
    // We use high-order ascii codes for these special chars not to clash 
    // with other chars.
    public const char StartA = (char)200;
    public const char StartB = (char)201;
    public const char StartC = (char)202;
    public const char CodeA = (char)203;
    public const char CodeB = (char)204;
    public const char CodeC = (char)205;
    public const char FNC1 = (char)206;
    public const char FNC2 = (char)207;
    public const char FNC3 = (char)208;
    public const char FNC4 = (char)209;
    public const char SHIFT = (char)210;

    internal static bool CanCode(string data, char code) {
      switch (code) {
        case CodeA: return codeA.ContainsKey(data);
        case CodeB: return codeB.ContainsKey(data);
        case CodeC: return codeC.ContainsKey(data);
      }
      throw new ArgumentException("Invalid data.");
    }
    
    internal static int SymbolValue(string data, char code) {
      switch (code) {
        case CodeA: return ((SymbolWrap)codeA[data]).Value;
        case CodeB: return ((SymbolWrap)codeB[data]).Value;
        case CodeC: return ((SymbolWrap)codeC[data]).Value;
      }
      throw new ArgumentException("Invalid coding.");
    }

    internal static string SymbolString(int value, char code) {
      Hashtable hash = null;
      switch (code) {
        case CodeA: hash = codeA; break;
        case CodeB: hash = codeB; break;
        case CodeC: hash = codeC; break;
        default: throw new ArgumentException("Invalid coding.");
      }
      
      // not optimized!
      foreach (string key in hash.Keys) {
        if (((SymbolWrap)hash[key]).Value == value) return key;
      }

      throw new ArgumentException("Invalid value.");
    }

    internal static char ResolveStartCode(char code) {
      switch (code) {
        case StartA: return CodeA;
        case StartB: return CodeB;
        case StartC: return CodeC;
        default: throw new ArgumentException("Invalid starting code.");
      }
    }

    public BitArray Encode(string data) {
      BitArray bits = new BitArray(11 * data.Length); // maximum number of elements
      int count = 0; // actual count of elements

      // first char should be the code to use
      char currCode = ResolveStartCode(data[0]);

      for (int i = 0; i < data.Length; ++i) {
        char currChr = data[i];
        string curr = currChr.ToString();

        if (i > 0 && (curr == StartA.ToString() || curr == StartB.ToString() || curr == StartC.ToString())) 
          throw new ArgumentException("The data has invalid coding.");

        BitArray currArr = null;
        switch (currCode) {
          case CodeA: currArr = ((SymbolWrap)codeA[curr]).Symbol; break;
          case CodeB: currArr = ((SymbolWrap)codeB[curr]).Symbol; break;
          case CodeC: 
            if (char.IsNumber(currChr))
              currArr = ((SymbolWrap)codeC[curr+data[++i].ToString()]).Symbol;
            else
              currArr = ((SymbolWrap)codeC[curr]).Symbol;
            break;
        }

        for (int j = 0; j < currArr.Count; ++j) {
          bits[count+j] = currArr[j];
        }
        count += currArr.Count;

        if (curr == CodeA.ToString() || curr == CodeB.ToString() || curr == CodeC.ToString()) 
          currCode = curr[0];
      }

      bits.Length = count;
      return bits;
    }

    BitArray ISymbolEncoder.Encode(char datum) {
      // can't encode single datum
      throw new InvalidOperationException("Can't encode single char datum.");
    }
  }

}
