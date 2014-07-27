using System;
using System.Drawing;

namespace NBarCodes {
  // NOTE: altering this interface ripples through 
  // at least 6 other places:
  // NBarCodes.WebUI.BarCodeControl
  // NBarCodes.Forms.BarCodeControl
  // BarCodeSettings
  // (these are not so obvious)
  // QueryStringSerializer.ParseQueryString
  // QueryStringSerializer.ToQueryString
  // BarCode (and its descendants)

  /// <summary>
  /// Defines barcode settings.
  /// </summary>
  public interface IBarCodeSettings {

    /// <summary>
    /// The type of the barcode.
    /// </summary>
    BarCodeType Type {
      get; set;
    }

    /// <summary>
    /// The data to render with the barcode.
    /// </summary>
    string Data {
      get; set;
    }

    /// <summary>
    /// The unit of measure of the barcode's measurable properties.
    /// </summary>
    BarCodeUnit Unit {
      get; set;
    }

    /// <summary>
    /// The DPI (dots per inch) of the barcode.
    /// </summary>
    int Dpi {
      get; set;
    }

    /// <summary>
    /// The back color of the barcode.
    /// </summary>
    Color BackColor { 
      get; set;
    }

    /// <summary>
    /// The color of the bar of the barcode.
    /// </summary>
    Color BarColor {
      get; set;
    }

    /// <summary>
    /// The height of the barcode. Affected by <see cref="Unit"/>.
    /// </summary>
    float BarHeight {
      get; set;
    }

    /// <summary>
    /// The color of the font to render text in the barcode.
    /// </summary>
    Color FontColor { 
      get; set;
    }

    /// <summary>
    /// The height of the extra height of the bar for EAN like barcodes.
    /// Affected by <see cref="Unit"/>.
    /// </summary>
    float GuardExtraHeight {
      get; set;
    }

    /// <summary>
    /// The width of bar of the barcode for module-based barcodes (<see cref="ModuleBarCode"/>).
    /// Affected by <see cref="Unit"/>.
    /// </summary>
    float ModuleWidth {
      get; set;
    }

    /// <summary>
    /// The width of the narrow component of a thickness-based barcode (<see cref="ThicknessBarCode"/>).
    /// Affected by <see cref="Unit"/>.
    /// </summary>
    float NarrowWidth {
      get; set;
    }

    /// <summary>
    /// The width of the wide component of a thickness-based barcode (<see cref="ThicknessBarCode"/>).
    /// Affected by <see cref="Unit"/>.
    /// </summary>
    float WideWidth {
      get; set;
    }

    /// <summary>
    /// The vertical (top and bottom) offset height of the barcode to the border.
    /// Affected by <see cref="Unit"/>.
    /// </summary>
    float OffsetHeight {
      get; set;
    }

    /// <summary>
    /// The horizontal (left and right) offset width of the barcode to the border.
    /// Affected by <see cref="Unit"/>.
    /// </summary>
    float OffsetWidth {
      get; set;
    }

    /// <summary>
    /// The font used to render the text inside the barcode.
    /// </summary>
    Font Font {
      get; set;
    }

    /// <summary>
    /// The position of the text rendered in the barcode.
    /// </summary>
    TextPosition TextPosition {
      get; set;
    }

    /// <summary>
    /// Whether the barcode will use an (optional) checksum.
    /// Not every barcode requires a checksum, and others mandate it.
    /// </summary>
    bool UseChecksum {
      get; set;
    }

  }
}
