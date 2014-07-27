using System;
using System.Collections;

namespace NBarCodes {

  [Serializable]
  abstract class ThicknessBarCode : BarCode {
    private float wideWidth = Defaults.WideWidth;
    private float narrowWidth = Defaults.NarrowWidth;

    public override void ImportSettings(BarCode barCode) {
      base.ImportSettings(barCode);
      if (barCode is ThicknessBarCode) {
        this.WideWidth = ((ThicknessBarCode)barCode).WideWidth;
        this.NarrowWidth = ((ThicknessBarCode)barCode).NarrowWidth;
      }
    }

    public float WideWidth {
      get { return wideWidth; }
      set { wideWidth = value; }
    }

    public float NarrowWidth {
      get { return narrowWidth; }
      set { narrowWidth = value; }
    }

    protected float DrawSymbols(IBarCodeBuilder builder, float x, float y, float height, BitArray[] symbols) { // base class??
      foreach (BitArray arr in symbols) {
        x = DrawSymbol(builder, x, y, height, arr);
      }

      return x;
    }

    protected virtual float DrawSymbol(IBarCodeBuilder builder, float x, float y, float height, BitArray symbol) {
      // the symbol for Wide is encoded as 1 and
      // the symbol for Narrow is encoded as 0
      // the symbols encode bars and spaces, starting with a bar

      bool drawBar = true; // start drawing a bar
      foreach (bool bit in symbol) {
        float width = bit ? WideWidth : NarrowWidth;
        if (drawBar) builder.DrawRectangle(BarColor, x, y, width, height);
        drawBar = !drawBar; // draw the other element next time
        x += width; // skip element (bar or space)
      }

      return x;
    }

  }

}
