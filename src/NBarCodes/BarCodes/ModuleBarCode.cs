using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;

namespace NBarCodes {

  [Serializable]
  abstract class ModuleBarCode : BarCode {
    private float moduleWidth = .02f;

    public override void ImportSettings(BarCode barCode) {
      base.ImportSettings(barCode);
      if (barCode is ModuleBarCode) {
        this.ModuleWidth = 
          ((ModuleBarCode)barCode).ModuleWidth;
      }
    }

    [DefaultValue(.02f), NotifyParentProperty(true)]
    public virtual float ModuleWidth {
      get { return moduleWidth; }
      set { moduleWidth = value; }
    }

    protected float DrawSymbols(IBarCodeBuilder builder, float x, float y, float height, BitArray[] symbols) {
      foreach (BitArray arr in symbols) {
        x = DrawSymbol(builder, x, y, height, arr);
      }

      return x;
    }

    protected float DrawSymbol(IBarCodeBuilder builder, float x, float y, float height, BitArray symbol) {
      return DrawSymbol(builder, x, y, ModuleWidth, height, symbol, BarColor);
    }

    internal static float DrawSymbol(IBarCodeBuilder builder, float x, float y, float moduleWidth, float height, BitArray symbol, Color barColor) {
      float start = x;
      bool wasSpace = true;
      foreach (bool bit in symbol) {
        if (bit) {
          if (wasSpace) {
            start = x;
            wasSpace = false;
          }
        }
        else {
          if (!wasSpace) {
            builder.DrawRectangle(barColor, start, y, x-start, height);
            wasSpace = true;
          }
        }
        x += moduleWidth;
      }
      if (!wasSpace) {
        builder.DrawRectangle(barColor, start, y, x-start, height);
      }

      return x;
    }
  
  }

}
