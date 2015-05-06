using System.Drawing;

namespace NBarCodes {

  /// <summary>
  /// Builder interface to render barcodes.
  /// </summary>
  interface IBarCodeBuilder {

    /// <summary>
    /// The DPI (dots per inch) used for rendering.
    /// </summary>
    int Dpi { get; set; }
    
    /// <summary>
    /// The unit used for rendering.
    /// </summary>
    BarCodeUnit Unit { get; set; }

    /// <summary>
    /// Prepares the builder for rendering by properly setting the bounds of
    /// the canvas surface to be used. Depends on the implementing class.
    /// </summary>
    /// <param name="width">Width of the canvas.</param>
    /// <param name="height">Height of the canvas.</param>
    void Prepare(float width, float height);
    
    /// <summary>
    /// Draws a string of text.
    /// </summary>
    /// <param name="font">The font to use.</param>
    /// <param name="fontColor">The color of the font.</param>
    /// <param name="centered">Whether the text is centered.</param>
    /// <param name="data">The data to print.</param>
    /// <param name="x">The horizontal component of the start of the text.</param>
    /// <param name="y">The vertical component of the start of the text.</param>
    void DrawString(Font font, Color fontColor, bool centered, string data, float x, float y);

    /// <summary>
    /// Draws a rectangle. Used to draw bars.
    /// </summary>
    /// <param name="color">The fill color of the rectangle.</param>
    /// <param name="x">The horizontal component of the start of the rectangle.</param>
    /// <param name="y">The vertical component of the start of the rectangle.</param>
    /// <param name="width">The width of the rectangle.</param>
    /// <param name="height">The height of the rectangle.</param>
    void DrawRectangle(Color color, float x, float y, float width, float height);

  }

}
