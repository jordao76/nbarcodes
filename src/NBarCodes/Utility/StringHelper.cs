using System;

namespace NBarCodes {

  /// <summary>
  /// Utility class for common string operations.
  /// </summary>
  static class StringHelper {

    /// <summary>
    /// Determines if a string is null of empty. It's emptiness is
    /// determined by first trimming it.
    /// </summary>
    /// <param name="stringToCheck">String to check for nullness or emptiness.</param>
    /// <returns><c>True</c> if the string is null or empty, <c>false</c> otherwise.</returns>
    public static bool IsNullOrEmpty(string stringToCheck) {
      return 
        stringToCheck == null ||
        stringToCheck.Trim().Length == 0;
    }

  }
}
