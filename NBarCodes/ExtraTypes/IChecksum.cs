namespace NBarCodes {

  /// <summary>
  /// Interface to calculate checksums on barcode data.
  /// </summary>
  /// <remarks>
  /// Classes that calculate checksums on barcode data should implement this interface.
  /// It makes for a pluggable checksum architecture.
  /// </remarks>
  public interface IChecksum {

    /// <summary>
    /// Calculates the checksum of a string of data.
    /// </summary>
    /// <param name="data">The data to calculate the checksum for.</param>
    /// <returns>The calculated checksum.</returns>
    string Calculate(string data);
  }

  /// <summary>
  /// Determines whether the checksum calculation in a barcode is optional.
  /// </summary>
  interface IOptionalChecksum {

    /// <summary>
    /// Determines whether the barcode will calculate a checksum on its data.
    /// </summary>
    bool UseChecksum { get; set; }
  }

}
