using System;
using System.Drawing;

namespace NBarCodes {

  /// <summary>
  /// A null builder used to test barcode renderings (<see cref="BarCode.TestRender"/>).
  /// </summary>
  class NullBuilder : IBarCodeBuilder {

    public int Dpi {
      get { return 96; }
      set { }
    }
    
    public BarCodeUnit Unit
    { 
      get { return BarCodeUnit.Pixel; }
      set {}
    }

    public void Prepare(float width, float height) {
      // no-op
    }

    public void DrawString(Font font, Color fontColor, bool centered, string data, float x, float y) {
      // no-op
    }

    public void DrawRectangle(Color color, float x, float y, float width, float height) {
      // no-op
    }
  }

}
