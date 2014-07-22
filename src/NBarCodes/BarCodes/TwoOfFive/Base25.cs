using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Collections;
using System.ComponentModel;

namespace NBarCodes {

  [Serializable]
  abstract class Base25 : ThicknessBarCode, IOptionalChecksum {
    private static readonly ISymbolEncoder Encoder = new Encoder25();

    protected abstract BitArray Start { get; }
    protected abstract BitArray End { get; }

    protected abstract float SymbolWidth { get; }
    protected abstract float GuardWidth { get; }

    private bool useChecksum = true;

    [DefaultValue(true), NotifyParentProperty(true)]
    public bool UseChecksum {
      get { return useChecksum; }
      set { useChecksum = value; }
    }

    // fixed width and height in base class??
    private float FixedWidth { 
      get { return OffsetWidth * 2 + QuietZone * 2 + GuardWidth; }
    }

    protected override void Draw(IBarCodeBuilder builder, string data) {
      ValidateCharacters(data);

      data = AppendChecksum(data);

      BitArray encoded = Encoder.Encode(data);

      float totalWidth = FixedWidth + SymbolWidth * data.Length;

      // set the canvas size
      builder.Prepare(totalWidth, TotalHeight);

      // draw the background
      builder.DrawRectangle(BackColor, 0, 0, totalWidth, TotalHeight);

      // draw the barcode
      float x = 0, y = 0;
      float textX = x + totalWidth / 2;
      x += OffsetWidth + QuietZone;
      y += OffsetHeight + ExtraTopHeight;
      x = ModuleBarCode.DrawSymbol(builder, x, y, NarrowWidth, BarHeight, Start, BarColor);
      x = DrawSymbol(builder, x, y, BarHeight, encoded);
      x = ModuleBarCode.DrawSymbol(builder, x, y, NarrowWidth, BarHeight, End, BarColor);

      // draw the text strings
      DrawText(builder, true, new float[] {textX}, y - TextHeight, new string[] {data});
    }

    private void ValidateCharacters(string data) {
      // check for non digits
      if (!new Regex(@"^\d+$").IsMatch(data)) {
        throw new BarCodeFormatException("The barcode has non-numeric data.");
      }
    }

    private string AppendChecksum(string data) {
      // append the checksum if necessary
      if (useChecksum) {
        if (Checksum == null) {
          Checksum = new Modulo10Checksum();
        }
        data += Checksum.Calculate(data);
      }
      return data;
    }

  }

}
