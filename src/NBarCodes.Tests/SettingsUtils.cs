using System.Drawing;
using NUnit.Framework;

namespace NBarCodes.Tests {

  /// <summary>
  /// Helper methods to test bar code settings (<see cref="IBarCodeSettings"/>).
  /// </summary>
  public static class SettingsUtils {

    /// <summary>
    /// Returns barcode settings for testing.
    /// </summary>
    /// <returns>Settings for testing.</returns>
    public static BarCodeSettings CreateTestSettings() {
      BarCodeSettings settings = new BarCodeSettings();

      settings.Type = BarCodeType.Code128;
      settings.Data = "123456";
      settings.Unit = BarCodeUnit.Centimeter;
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
      settings.QuietZone = 0.001f;
      settings.Font = new Font("verdana", 8f, FontStyle.Italic);
      settings.TextPosition = TextPosition.All;
      settings.UseChecksum = true;

      return settings;
    }

    /// <summary>
    /// Asserts that two <see cref="IBarCodeSettings"/> are property-wise equal.
    /// </summary>
    /// <param name="expected">Expected settings.</param>
    /// <param name="actual">Actual settings.</param>
    public static void AssertSettingsEqual(IBarCodeSettings expected, IBarCodeSettings actual) {
      Assert.AreEqual(expected.Type, actual.Type);
      Assert.AreEqual(expected.Data, actual.Data);
      Assert.AreEqual(expected.Unit, actual.Unit);
      Assert.AreEqual(expected.BackColor.ToArgb(), actual.BackColor.ToArgb());
      Assert.AreEqual(expected.BarColor.ToArgb(), actual.BarColor.ToArgb());
      Assert.AreEqual(expected.BarHeight, actual.BarHeight);
      Assert.AreEqual(expected.FontColor.ToArgb(), actual.FontColor.ToArgb());
      Assert.AreEqual(expected.GuardExtraHeight, actual.GuardExtraHeight);
      Assert.AreEqual(expected.ModuleWidth, actual.ModuleWidth);
      Assert.AreEqual(expected.NarrowWidth, actual.NarrowWidth);
      Assert.AreEqual(expected.WideWidth, actual.WideWidth);
      Assert.AreEqual(expected.OffsetHeight, actual.OffsetHeight);
      Assert.AreEqual(expected.OffsetWidth, actual.OffsetWidth);
      Assert.AreEqual(expected.QuietZone, actual.QuietZone);
      Assert.AreEqual(expected.Font, actual.Font);
      Assert.AreEqual(expected.TextPosition, actual.TextPosition);
      Assert.AreEqual(expected.UseChecksum, actual.UseChecksum);
    }

  }

}
