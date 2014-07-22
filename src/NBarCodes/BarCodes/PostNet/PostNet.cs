using System;
using System.Collections;
using System.Text.RegularExpressions;
using System.ComponentModel;

namespace NBarCodes {

  [Serializable]
  sealed class PostNet : ThicknessBarCode {
    private readonly static BitArray Frame = BitArrayHelper.ToBitArray("1");
    private readonly static ISymbolEncoder Encoder = new PostNetEncoder();

    public override IChecksum Checksum {
      get { 
        if (base.Checksum == null) {
          base.Checksum = new PostNetChecksum();
        }
        return base.Checksum;
      }
      set { base.Checksum = value; }
    }

    private void Validate(string data) {
      if (data == null) throw new ArgumentNullException("data");

      if (!new Regex(@"^\d+$").IsMatch(data)) {
        throw new BarCodeFormatException("The barcode has non-numeric data.");
      }

      if (data.Length != 5 && data.Length != 9 && data.Length != 11) {
        throw new BarCodeFormatException("Invalid length for barcode. Valid lengths are 5, 9 or 11.");
      }
    }

    protected override void Draw(IBarCodeBuilder builder, string data) {
      Validate(data);

      // append the checksum - note: the checksum won't appear in the text string
      string dataWithCheck = data + Checksum.Calculate(data);

      // encode the data
      BitArray encoded = Encoder.Encode(dataWithCheck);

      // calculate total width
      float totalWidth = WideWidth * (encoded.Length + 2/*frames*/) + NarrowWidth * (encoded.Length + 1/*frames*/) + OffsetWidth * 2 + QuietZone * 2;

      // set the canvas size
      builder.Prepare(totalWidth, TotalHeight);

      // draw the background
      builder.DrawRectangle(BackColor, 0, 0, totalWidth, TotalHeight);

      // draw the barcode
      float x = 0, y = 0;
      float textX = x + totalWidth / 2;
      x += OffsetWidth + QuietZone;
      y += OffsetHeight + ExtraTopHeight;
      x = DrawSymbol(builder, x, y, BarHeight, Frame);
      x = DrawSymbol(builder, x, y, BarHeight, encoded);
      x = DrawSymbol(builder, x, y, BarHeight, Frame);

      // draw the text strings
      DrawText(builder, true, new float[] {textX}, y - TextHeight, data);
    }

    protected override float DrawSymbol(IBarCodeBuilder builder, float x, float y, float fullHeight, BitArray symbol) {
      float shortHeight = fullHeight / 2f;
      foreach (bool bit in symbol) {
        if (bit) {
          // tall bar
          builder.DrawRectangle(BarColor, x, y, WideWidth, fullHeight);
        }
        else {
          // short bar
          builder.DrawRectangle(BarColor, x, y + shortHeight, WideWidth, shortHeight);
        }
        // skip bar and space
        x += WideWidth + NarrowWidth;
      }
      return x;
    }
  }
}
