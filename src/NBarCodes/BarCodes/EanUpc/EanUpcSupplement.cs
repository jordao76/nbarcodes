using System;
using System.Text.RegularExpressions;
using System.Collections;

namespace NBarCodes {

  abstract class EanUpcSupplement : ModuleBarCode {
    protected readonly static BitArray LeftGuard = BitArrayHelper.ToBitArray("1011");
    protected readonly static BitArray SeparatorBar = BitArrayHelper.ToBitArray("01");

    public override float QuietZone {
      get {
        return 0;
      }
    }

    public EanUpcSupplement(EanUpc baseBarCode) {
      ImportSettings(baseBarCode);
      BarHeight = baseBarCode.BarHeight + baseBarCode.GuardExtraHeight - baseBarCode.TextHeight;

      // text position on top
      if (baseBarCode.TextPosition == TextPosition.Bottom) {
        TextPosition = TextPosition.Top;
      }
      else {
        TextPosition = baseBarCode.TextPosition;
      }
    }

    internal static EanUpcSupplement GetSupplementBarCode(EanUpc baseBarCode, string data) {
      switch (data.Length) {
        case 2: return new EanUpc2DigitSupplement(baseBarCode);
        case 5: return new EanUpc5DigitSupplement(baseBarCode);
        default: throw new BarCodeFormatException("Supplement barcode has incorrect length.");
      }
    }

    public abstract float TotalWidth { get; }

    protected void Validate(string data, int length) {
      // check for non digits
      if (!new Regex(@"^\d+$").IsMatch(data))
        throw new BarCodeFormatException("The barcode supplement has non-numeric data.");

      if (data.Length != length) 
        throw new BarCodeFormatException("Invalid barcode supplement length.");
    }

    protected abstract ISymbolEncoder ParityEncoder { get; }

    protected override void Draw(IBarCodeBuilder builder, string data) { // never used
      Draw(builder, data, 0, 0);
    }
      
    public abstract void Draw(IBarCodeBuilder builder, string data, float x, float y);

    protected void Draw(IBarCodeBuilder builder, string data, string parity, float x, float y) {
      // encode the symbol
      BitArray[] encoded = EanUpc.EncodeLeft(data, ParityEncoder.Encode(parity));

      // draw the bars; size and background have already been set by the base barcode
      float textX = x + TotalWidth / 2;
      y += OffsetHeight + TextHeight;
      x = DrawSymbol(builder, x, y, BarHeight, LeftGuard);
      x = DrawSymbol(builder, x, y, BarHeight, encoded[0]);
      for (int i = 1; i < encoded.Length; ++i) {
        x = DrawSymbol(builder, x, y, BarHeight, SeparatorBar);
        x = DrawSymbol(builder, x, y, BarHeight, encoded[i]);
      }

      // draw the text string
      DrawText(builder, true, new float[] {textX}, y - TextHeight, new string[] {data});
    }

  }

  class EanUpc2DigitSupplement : EanUpcSupplement {
    private readonly static ISymbolEncoder parityEncoder = new EanUpc2DigitSupplementParityEncoder();

    protected override ISymbolEncoder ParityEncoder { 
      get { return parityEncoder; }
    }

    public override float TotalWidth {
      // only the width of the barcode
      get { return 20 * ModuleWidth; }
    }

    public EanUpc2DigitSupplement(EanUpc baseBarCode) : base(baseBarCode) {}

    public override void Draw(IBarCodeBuilder builder, string data, float x, float y) {
      // validate the supplement data
      Validate(data, 2);

      // calculate the parity
      string parity = (int.Parse(data) % 4).ToString();

      Draw(builder, data, parity, x, y);
    }
  }

  class EanUpc5DigitSupplement : EanUpcSupplement {
    private readonly static ISymbolEncoder parityEncoder = new EanUpc5DigitSupplementParityEncoder();

    protected override ISymbolEncoder ParityEncoder { 
      get { return parityEncoder; }
    }

    public override float TotalWidth {
      // only the width of the barcode
      get { return 47 * ModuleWidth; }
    }

    public EanUpc5DigitSupplement(EanUpc baseBarCode) : 
      base(baseBarCode) {
      Checksum = new EanUpc5DigitSupplementChecksum();
    }

    public override void Draw(IBarCodeBuilder builder, string data, float x, float y) {
      // validate the supplement data
      Validate(data, 5);

      // calculate checksum to use in parity
      string parity = Checksum.Calculate(data);

      Draw(builder, data, parity, x, y);
    }
  }

}
