using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace NBarCodes {

	/// <summary>
	/// Builder to render barcodes as images.
	/// </summary>
  class ImageBuilder : IBarCodeBuilder {

		private const int MAX_WIDTH = 1000;
		private const int MAX_HEIGHT = 700;
		private const int MIN_WIDTH = 50;
		private const int MIN_HEIGHT = 35;
		private const float DEFAULT_DPI = 96f;

		private Bitmap _barCodeImage;
		private StringFormat _drawFormat;
		private BarCodeUnit _unit;
		private float _dpi;

		/// <summary>
		/// Creates a new instance of the <see cref="ImageBuilder"/> class.
		/// </summary>
		public ImageBuilder() {
			_drawFormat = new StringFormat();
			_drawFormat.Alignment = StringAlignment.Center;
			_unit = BarCodeUnit.Pixel;
			_dpi = DEFAULT_DPI;
		}

		/// <summary>
		/// The unit to use when rendering.
		/// </summary>
		public BarCodeUnit Unit { 
			get { return _unit; }
			set { _unit = value; }
		}

		// TODO: put DPI in interface!!
		/// <summary>
		/// The DPI (dots per inch) to use when rendering.
		/// </summary>
		public float Dpi {
			get { return _dpi; }
			set { _dpi = value; }
		}

		/// <summary>
		/// Prepares the surface for rendering by allocating an image of the 
		/// desirable dimensions, trimming than to reasonable defaults if
		/// necessary.
		/// </summary>
		/// <param name="width">Width of the drawing canvas.</param>
		/// <param name="height">Height of the drawing canvas.</param>
		public void Prepare(float width, float height) {
			if (_barCodeImage != null) {
				// will flush the current image
				_barCodeImage.Dispose();
			}

			// have to convert width and height to pixels
			int widthInPixels = (int)ConvertToPixels(width);
			int heightInPixels = (int)ConvertToPixels(height);

			if (widthInPixels > MAX_WIDTH) {
				widthInPixels = MAX_WIDTH;
			}
			if (widthInPixels < MIN_WIDTH) {
				widthInPixels = MIN_WIDTH;
			}
			if (heightInPixels > MAX_HEIGHT) {
				heightInPixels = MAX_HEIGHT;
			}
			if (heightInPixels < MIN_HEIGHT) {
				heightInPixels = MIN_HEIGHT;
			}

			// TODO: check for Bitmap constructor that takes Graphics for setting DPI!!
			_barCodeImage = new Bitmap(widthInPixels, heightInPixels);
    }

		/// <summary>
		/// Draws a string of text.
		/// </summary>
		/// <param name="font">The font to use.</param>
		/// <param name="fontColor">The color of the font.</param>
		/// <param name="centered">Whether the text is centered.</param>
		/// <param name="data">The data to print.</param>
		/// <param name="x">The horizontal component of the start of the text.</param>
		/// <param name="y">The vertical component of the start of the text.</param>
		public void DrawString(Font font, Color fontColor, bool centered, string data, float x, float y) {
			using (Graphics canvas = Graphics.FromImage(_barCodeImage))
			using (Brush brush = new SolidBrush(fontColor)) {
				float xInPixels = ConvertToPixels(x);
				float yInPixels = ConvertToPixels(y);

				if (centered) {
					canvas.DrawString(data, font, brush, xInPixels, yInPixels, _drawFormat);
				}
				else {
					canvas.DrawString(data, font, brush, xInPixels, yInPixels);
				}
			}
    }

		/// <summary>
		/// Draws a rectangle. Used to draw bars.
		/// </summary>
		/// <param name="color">The fill color of the rectangle.</param>
		/// <param name="x">The horizontal component of the start of the rectangle.</param>
		/// <param name="y">The vertical component of the start of the rectangle.</param>
		/// <param name="width">The width of the rectangle.</param>
		/// <param name="height">The height of the rectangle.</param>
		public void DrawRectangle(Color color, float x, float y, float width, float height) {
			using (Graphics canvas = Graphics.FromImage(_barCodeImage))
			using (Brush brush = new SolidBrush(color)) {
				float xInPixels = ConvertToPixels(x);
				float yInPixels = ConvertToPixels(y);
				float widthInPixels = ConvertToPixels(width);
				float heightInPixels = ConvertToPixels(height);
				canvas.FillRectangle(brush, xInPixels, yInPixels, widthInPixels, heightInPixels);
			}
		}

		/// <summary>
		/// Used to retrieve the image built.
		/// </summary>
		/// <returns>The image of the rendered content.</returns>
    public Bitmap GetImage() {
      return _barCodeImage;
    }

		/// <summary>
		/// Converts a value to pixels. The value is based on the unit stored in <see cref="Unit"/>.
		/// </summary>
		/// <param name="value">Value to convert.</param>
		/// <returns>Value converted to pixels.</returns>
		private float ConvertToPixels(float value) {
			return UnitConverter.Convert(value, _unit, BarCodeUnit.Pixel, _dpi);
		}

  }

}
