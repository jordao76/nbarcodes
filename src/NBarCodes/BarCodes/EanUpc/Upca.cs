using System;
using System.Collections;
using System.Drawing;

namespace NBarCodes {

	[Serializable]
	class Upca : EanUpc {

		public override float TotalWidth { 
			get { return 95 * ModuleWidth + OffsetWidth * 2 + TextWidth * 2 + QuietZone * 2; }
		}
		
		protected override void Draw(IBarCodeBuilder builder, string data) {
			Draw(builder, data, 12);
		}

		public override void Draw(IBarCodeBuilder builder, string data, string supplement) {
			// resolve the supplement barcode
			EanUpcSupplement supp = ResolveSupplement(supplement);
			
			// validate Upca data
			data = Validate(data, 12);

			// split the data in its sub components
			string[] text = new string[] {
        data.Substring(0, 1),
        data.Substring(1, 5),
        data.Substring(6, 5),
        data.Substring(11, 1)
			};

			// encode the data
			BitArray[] left = EncodeLeft(text[0] + text[1], ParityEncoder.Encode("0"));
      BitArray[] right = EncodeRight(text[2] + text[3]);

			// calculate total width
			float totalWidth = TotalWidth + 
				(supp == null ? 0 : SupplementOffset + supp.TotalWidth);

			// set the canvas size
			builder.Prepare(totalWidth, TotalHeight);

			// draw the background
			builder.DrawRectangle(BackColor, 0, 0, totalWidth, TotalHeight);

			// draw the barcode
			float x = 0, y = 0;
			float[] textX = new float[4];
      x += OffsetWidth + QuietZone;
      y += OffsetHeight;
      textX[0] = x;
      x += TextWidth;
      x = DrawSymbol(builder, x, y, BarHeight + GuardExtraHeight, LeftGuard);
      x = DrawSymbol(builder, x, y, BarHeight + GuardExtraHeight, left[0]);
      textX[1] = x;
      BitArrayHelper.PopFront(ref left);
      x = DrawSymbols(builder, x, y, BarHeight, left);
      x = DrawSymbol(builder, x, y, BarHeight + GuardExtraHeight, CenterGuard);
      textX[2] = x;
      BitArray rightmost = BitArrayHelper.PopBack(ref right);
      x = DrawSymbols(builder, x, y, BarHeight, right);
      x = DrawSymbol(builder, x, y, BarHeight + GuardExtraHeight, rightmost);
      x = DrawSymbol(builder, x, y, BarHeight + GuardExtraHeight, RightGuard);
      textX[3] = x;

			// draw the text strings
			DrawText(builder, textX, y - TextHeight, text);

			// draw the supplement barcode, if one is present
			if (supp != null) supp.Draw(builder, supplement, x + TextWidth + SupplementOffset, y - OffsetHeight);
    }

  }

}
