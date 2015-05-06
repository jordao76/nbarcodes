using System;
using System.Collections;
using System.Text.RegularExpressions;

namespace NBarCodes {

  [Serializable]
  abstract class EanUpc : ModuleBarCode {

    private const float supplementOffsetRatio = 11;

    private float guardExtraHeight = Defaults.GuardExtraHeight;

    protected readonly static BitArray LeftGuard = BitArrayHelper.ToBitArray("101");
    protected readonly static BitArray CenterGuard = BitArrayHelper.ToBitArray("01010");
    protected readonly static BitArray RightGuard = BitArrayHelper.ToBitArray("101");

    protected readonly static ISymbolEncoder LeftOddEncoder = new EanLeftOddEncoder();
    protected readonly static ISymbolEncoder LeftEvenEncoder = new EanLeftEvenEncoder();
    protected readonly static ISymbolEncoder RightEncoder = new EanRightEncoder();
    protected readonly static ISymbolEncoder ParityEncoder = new EanParityEncoder();

    public override float QuietZone {
      get {
        return ModuleWidth * 11;
      }
    }

    public override void ImportSettings(BarCode barCode) {
      base.ImportSettings(barCode);
      if (barCode is EanUpc) {
        this.guardExtraHeight = 
          ((EanUpc)barCode).GuardExtraHeight;
      }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public EanUpc() {
      TextPosition = TextPosition.Bottom;
    }

    public float GuardExtraHeight {
      get {
        if (TextPosition == TextPosition.None) return 0;
        return guardExtraHeight;
      }
      set { guardExtraHeight = value; }
    }

    public float SupplementOffset {
      get {
        return ModuleWidth * supplementOffsetRatio;
      }
    }

    protected override float ExtraHeight {
      get {
        // guard height and text height overlap, so we get the bigger
        float height; 
        if (TextPosition != TextPosition.None) {
          height = TextHeight;
        }
        else {
          height = 0;
        }

        return GuardExtraHeight > height ? GuardExtraHeight : height;
      }
    }

    public override TextPosition TextPosition {
      get { return base.TextPosition; }
      set { 
        if ((value & TextPosition.Top) != 0) {
          throw new ArgumentException("TextPosition must be on bottom or none.");
        }
        base.TextPosition = value;
      }
    }

    public abstract float TotalWidth { get; }

    protected string Validate(string data, int fullLength) {
      // check for non digits
      if (!new Regex(@"^\d+$").IsMatch(data))
        throw new BarCodeFormatException("The barcode has non-numeric data.");

      // check the length, it can be full or missing the check digit
      if (data.Length < fullLength - 1 || data.Length > fullLength)
        throw new BarCodeFormatException("Invalid length for barcode.");

      // check or calculate and append the check digit
      if (Checksum == null) {
        Checksum = new Modulo10Checksum();
      }
      if (data.Length == fullLength) {
        // check if the check digit in the string is right
        string checksum = Checksum.Calculate(data.Substring(0, fullLength - 1));
        if (data[fullLength - 1].ToString() != checksum)
          throw new BarCodeFormatException("Invalid check digit.");
      }
      else {
        // append the check digit
        data += Checksum.Calculate(data);
      }

      return data;
    }

    protected internal static BitArray[] EncodeLeft(string data, BitArray parity) {
      if (data.Length != parity.Count) throw new ArgumentException();

      BitArray[] bits = new BitArray[data.Length];
      for (int i = 0; i < data.Length; ++i) {
        bits[i] = parity[i] ? LeftOddEncoder.Encode(data[i]) : LeftEvenEncoder.Encode(data[i]);
      }

      return bits;
    }

    protected static BitArray[] EncodeRight(string data) {
      BitArray[] bits = new BitArray[data.Length];
      for (int i = 0; i < data.Length; ++i) {
        bits[i] = RightEncoder.Encode(data[i]);
      }

      return bits;
    }

    internal EanUpcSupplement ResolveSupplement(string supplement) {
      EanUpcSupplement supp = null;
      if (supplement != null && supplement.Length > 0) {
        supp = EanUpcSupplement.GetSupplementBarCode(this, supplement);
      }
      return supp;
    }

    protected void Draw(IBarCodeBuilder builder, string data, int length) {
      // TODO: BUG: account for digit! can have 6 different sizes:
      // (2 supplements + 1 non-supp) * 2 modes (with or without digit)
      if (data.Length > length) {
        Draw(builder, data.Substring(0, length), data.Substring(length));
      }
      else {
        Draw(builder, data, null);
      }
    }

    public abstract void Draw(IBarCodeBuilder builder, string data, string supplement);
  }

}
