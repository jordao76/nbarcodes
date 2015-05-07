using System;
using System.Drawing;

namespace NBarCodes {

  /// <summary>
  /// Default values for barcode settings (<see cref="BarCodeSettings"/>).
  /// </summary>
  public static class Defaults {

    /// <summary>Default unit.</summary>
    public const BarCodeUnit Unit = BarCodeUnit.Inch;
    /// <summary>Default DPI (<see href="UnitConverter.ScreenDpi"/>).</summary>
    public readonly static int Dpi = UnitConverter.ScreenDpi;

    /// <summary>Default back color name.</summary>
    public const String BackColorName = "White";
    /// <summary>Default back color.</summary>
    public readonly static Color BackColor = Color.FromName(BackColorName);

    /// <summary>Default bar color name.</summary>
    public const String BarColorName = "Black";
    /// <summary>Default bar color.</summary>
    public readonly static Color BarColor = Color.FromName(BarColorName);

    /// <summary>Default font color name.</summary>
    public const String FontColorName = "Black";
    /// <summary>Default font color.</summary>
    public readonly static Color FontColor = Color.FromName(FontColorName);

    /// <summary>Default font name.</summary>
    public const String FontName = "Verdana, 15pt";
    /// <summary>Default font.</summary>
    public readonly static Font Font = new Font("verdana", 15); // 15 points = 15/72 inches = 0.208 inches

    /// <summary>Default text position.</summary>
    public const TextPosition TextPos = TextPosition.Bottom;

    /// <summary>Default bar height.</summary>
    public const float BarHeight = .5f;
    /// <summary>Default guard extra height.</summary>
    public const float GuardExtraHeight = .1f;
    /// <summary>Default module width.</summary>
    public const float ModuleWidth = .02f;
    /// <summary>Default narrow width.</summary>
    public const float NarrowWidth = .02f;
    /// <summary>Default wide width.</summary>
    public const float WideWidth = .06f;
    /// <summary>Default offset height.</summary>
    public const float OffsetHeight = .05f;
    /// <summary>Default offset width.</summary>
    public const float OffsetWidth = .05f;

  }

}
