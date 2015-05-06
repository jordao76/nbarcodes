using System.Diagnostics;
using NUnit.Framework;

namespace NBarCodes.Tests {

  /// <summary>
  /// Helper for <see cref="IChecksum"/> based tests.
  /// </summary>
  class ChecksumTestDriver {

    /// <summary>
    /// Creates a new instance of the <see cref="ChecksumTestDriver"/> class.
    /// </summary>
    /// <param name="checksum"><see cref="IChecksum"/> to use in calculations.</param>
    public ChecksumTestDriver(IChecksum checksum) {
      Debug.Assert(checksum != null);
      _checksum = checksum;
    }

    /// <summary>
    /// Asserts the checksum calculation.
    /// </summary>
    /// <param name="input">The input data for the calculation.</param>
    /// <param name="expected">The expected result for the calculation.</param>
    public void AssertCalculation(string input, string expected) {
      string actual = _checksum.Calculate(input);
      Assert.AreEqual(expected, actual, "Checksum error with {0}.", _checksum.GetType());
    }

    IChecksum _checksum;
  }

}
