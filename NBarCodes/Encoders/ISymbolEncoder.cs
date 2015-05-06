using System.Collections;

namespace NBarCodes {

  /// <summary>
  /// Encoder interface for barcode symbols.
  /// </summary>
  /// <remarks>
  /// Classes that encode barcode data into arrays of bits (<see cref="BitArray"/>) should
  /// implement this interface.
  /// </remarks>
  interface ISymbolEncoder {
    
    /// <summary>
    /// Encodes a string of barcode data.
    /// </summary>
    /// <param name="data">The string of data to be encoded.</param>
    /// <returns>The encoded data.</returns>
    BitArray Encode(string data);

    /// <summary>
    /// Encodes a character of barcode data.
    /// </summary>
    /// <param name="datum">The character of data to be encoded.</param>
    /// <returns>The encoded data.</returns>
    BitArray Encode(char datum);
  }

}
