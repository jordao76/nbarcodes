using System;
using System.Drawing;

namespace NBarCodes {
 
  static class Defaults {

    public const BarCodeUnit Unit = BarCodeUnit.Inch;
    public readonly static int Dpi = UnitConverter.ScreenDpi;

    public const String BackColorName = "White";
    public readonly static Color BackColor = Color.FromName(BackColorName);

    public const String BarColorName = "Black";
    public readonly static Color BarColor = Color.FromName(BarColorName);

    public const String FontColorName = "Black";
    public readonly static Color FontColor = Color.FromName(FontColorName);

    public const String FontName = "Verdana, 15pt";
    public readonly static Font Font = new Font("verdana", 15); // 15 points = 15/72 inches = 0.208 inches

    public const TextPosition TextPos = TextPosition.Bottom;

    public const float BarHeight = .5f;
    public const float GuardExtraHeight = .1f;
    public const float ModuleWidth = .02f;
    public const float NarrowWidth = .02f;
    public const float WideWidth = .06f;
    public const float OffsetHeight = .05f;
    public const float OffsetWidth = .05f;

  }

}
