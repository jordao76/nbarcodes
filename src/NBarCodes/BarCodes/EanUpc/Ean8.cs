using System;
using System.Collections;
using System.Drawing;

namespace NBarCodes {

  [Serializable]
  class Ean8 : EanUpc {

    public override float TotalWidth { 
      get { return 67 * ModuleWidth + OffsetWidth * 2 + QuietZone * 2; }
    }

    protected BitArray[] EncodeLeftOdd(string data) {
      BitArray[] bits = new BitArray[data.Length];
      for (int i = 0; i < data.Length; ++i)
        bits[i] = LeftOddEncoder.Encode(data[i]);

      return bits;
    }

    protected override void Draw(IBarCodeBuilder builder, string data) {
      Draw(builder, data, 8);
    }

    public override void Draw(IBarCodeBuilder builder, string data, string supplement) {
      // resolve the supplement barcode
      EanUpcSupplement supp = ResolveSupplement(supplement);
      
      // validate Ean8 data
      data = Validate(data, 8);

      // split the data in its sub components
      string[] text = new string[] {data.Substring(0, 4), data.Substring(4, 4)};

      // encode the data
      BitArray[] left = EncodeLeftOdd(text[0]);
      BitArray[] right = EncodeRight(text[1]);

      // calculate total width
      float totalWidth = TotalWidth + 
        (supp == null ? 0 : SupplementOffset + supp.TotalWidth);

      // set the canvas size
      builder.Prepare(totalWidth, TotalHeight);

      // draw the background
      builder.DrawRectangle(BackColor, 0, 0, totalWidth, TotalHeight);

      // draw the barcode
      float x = 0, y = 0;
      float[] textX = new float[2];
      x += OffsetWidth + QuietZone;
      y += OffsetHeight;
      x = DrawSymbol(builder, x, y, BarHeight + GuardExtraHeight, LeftGuard);
      textX[0] = x;
      x = DrawSymbols(builder, x, y, BarHeight, left);
      x = DrawSymbol(builder, x, y, BarHeight + GuardExtraHeight, CenterGuard);
      textX[1] = x;
      x = DrawSymbols(builder, x, y, BarHeight, right);
      x = DrawSymbol(builder, x, y, BarHeight + GuardExtraHeight, RightGuard);

      // draw the text strings
      DrawText(builder, textX, y - TextHeight, text);

      // draw the supplement barcode, if one is present
      if (supp != null) {
        supp.Draw(builder, supplement, x + SupplementOffset, y - OffsetHeight);
      }
    }

  }

}
