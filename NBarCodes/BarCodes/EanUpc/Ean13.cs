using System;
using System.Collections;

namespace NBarCodes {

  [Serializable]
  class Ean13 : EanUpc {

    public override float QuietZone {
      get {
        return ModuleWidth * 11;
      }
    }

    public override float TotalWidth { 
      get { return 95 * ModuleWidth + OffsetWidth * 2 + TextWidth + QuietZone * 2; }
    }

    protected override void Draw(IBarCodeBuilder builder, string data) {
      Draw(builder, data, 13);
    }

    public override void Draw(IBarCodeBuilder builder, string data, string supplement) {
      // resolve the supplement barcode
      EanUpcSupplement supp = ResolveSupplement(supplement);

      // validate Ean13 data
      data = Validate(data, 13);

      // split the data in its sub components
      string[] text = new string[] {data.Substring(0, 1), data.Substring(1, 6), data.Substring(7, 6)};

      // encode the data
      BitArray[] left = EncodeLeft(text[1], ParityEncoder.Encode(text[0]));
      BitArray[] right = EncodeRight(text[2]);

      // calculate total width
      float totalWidth = TotalWidth + 
        (supp == null ? 0 : SupplementOffset + supp.TotalWidth);

      // set the canvas size
      builder.Prepare(totalWidth, TotalHeight);

      // draw the background
      builder.DrawRectangle(BackColor, 0, 0, totalWidth, TotalHeight);

      // draw the barcode
      float x = 0, y = 0;      
      float[] textX = new float[3];
      x += OffsetWidth + QuietZone;
      y += OffsetHeight;
      textX[0] = x;
      x += TextWidth;
      x = DrawSymbol(builder, x, y, BarHeight + GuardExtraHeight, LeftGuard);
      textX[1] = x;
      x = DrawSymbols(builder, x, y, BarHeight, left);
      x = DrawSymbol(builder, x, y, BarHeight + GuardExtraHeight, CenterGuard);
      textX[2] = x;
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
