using System.Drawing;

namespace NBarCodes.Tests {

  /// <summary>
  /// Helper methods to test bar code settings (<see cref="IBarCodeSettings"/>).
  /// </summary>
  public static class SettingsUtils {

    /// <summary>
    /// Returns barcode settings for testing with non-default values.
    /// </summary>
    /// <returns>Settings for testing.</returns>
    public static BarCodeSettings CreateTestSettings() {
      BarCodeSettings settings = new BarCodeSettings();

      settings.Type = BarCodeType.Interleaved25;
      settings.Data = "1234567";
      settings.Unit = BarCodeUnit.Centimeter;
      settings.Dpi = 300;
      settings.BackColor = Color.BlanchedAlmond;
      settings.BarColor = Color.Honeydew;
      settings.BarHeight = 4.54f;
      settings.FontColor = Color.OldLace;
      settings.GuardExtraHeight = .543f;
      settings.ModuleWidth = .654f;
      settings.NarrowWidth = .345f;
      settings.WideWidth = .634f;
      settings.OffsetHeight = 1.234f;
      settings.OffsetWidth = 0.489f;
      settings.Font = new Font("verdana", 15f, FontStyle.Italic);
      settings.TextPosition = TextPosition.All;
      settings.UseChecksum = true;

      return settings;
    }

  }

}
