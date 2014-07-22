using System;
using System.Collections;
using System.ComponentModel;

namespace NBarCodes {

  [Serializable]
  class Code39 : ThicknessBarCode, IOptionalChecksum {
    private static readonly ISymbolEncoder Encoder = new Code39Encoder();

    private bool useChecksum = false;

    [DefaultValue(false), NotifyParentProperty(true)]
    public bool UseChecksum {
      get { return useChecksum; }
      set { useChecksum = value; }
    }

    // in base class??
    private float SymbolWidth {
      // 3 of 9: 3 Wide, 6 Narrow plus 1 Narrow space
      get { return WideWidth * 3 + NarrowWidth * 7; }
    }
    private float GuardWidth {
      get { return SymbolWidth * 2; }
    }

    protected override void Draw(IBarCodeBuilder builder, string data) {
      ValidateCharacters(data);

      // store the current data
      string oldData = data;

      // translate the extended characters
      data = Code39Translator.TranslateExtended(data);

      data = AppendChecksum(data);

      BitArray encoded = Encoder.Encode(data);
      BitArray guard = Encoder.Encode("*");

      float totalWidth = SymbolWidth * data.Length + GuardWidth + OffsetWidth * 2 + QuietZone * 2 + CalculateExtraWidth(guard, guard, encoded);

      // set the canvas size
      builder.Prepare(totalWidth, TotalHeight);

      // draw the background
      builder.DrawRectangle(BackColor, 0, 0, totalWidth, TotalHeight);

      // draw the barcode
      float x = 0, y = 0;
      float textX = x + totalWidth / 2;
      x += OffsetWidth + QuietZone;
      y += OffsetHeight + ExtraTopHeight;
      x = DrawSymbol(builder, x, y, BarHeight, guard);
      x = DrawSymbol(builder, x, y, BarHeight, encoded);
      x = DrawSymbol(builder, x, y, BarHeight, guard);

      DrawText(builder, true, new float[] {textX}, y - TextHeight, new string[] {oldData});
    }

    private void ValidateCharacters(string data) {
      foreach (char c in data) {
        if (c >= 128) {
          throw new BarCodeFormatException("The barcode has invalid data.");
        }
      }
    }

    private string AppendChecksum(string data) {
      if (useChecksum) {
        if (Checksum == null) {
          Checksum = new Code39Checksum();
        }
        data += Checksum.Calculate(data);
      }
      return data;
    }

    /// <summary>
    /// Calculates the extra width rendered with the space padding.
    /// </summary>
    /// <param name="symbols">Symbols to be encoded.</param>
    /// <returns>Extra width rendered.</returns>
    private int CalculateExtraWidth(params BitArray[] symbols) {
      int extraSpace = 0;
      foreach (BitArray symbol in symbols) {
        extraSpace += symbol.Length / 2;
      }

      return extraSpace;
    }

    protected override float DrawSymbol(IBarCodeBuilder builder, float x, float y, float height, BitArray symbol) {
      // the symbol for Wide is encoded as 1 and
      // the symbol for Narrow is encoded as 0
      // the symbols encode bars and spaces, starting with a bar

      bool drawBar = true; // start drawing a bar
      foreach (bool bit in symbol) {
        float width = bit ? WideWidth : NarrowWidth;
        if (drawBar) {
          builder.DrawRectangle(BarColor, x, y, width, height);
        }
        else {
          // spaces must be wider
          width += 1;
        }
        
        x += width; // skip element (bar or space)
        drawBar = !drawBar; // draw the other element next time
      }

      return x;
    }
  
  }

}
