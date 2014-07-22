using System;

namespace NBarCodes {

  /// <summary>
  /// The exception thrown when the barcode has an incorrect format.
  /// </summary>
  public class BarCodeFormatException : ApplicationException {

    /// <overloads>
    /// <summary>
    /// Creates a new instance of the <see cref="BarCodeFormatException"/> class.
    /// </summary>
    /// </overloads>
    /// <summary>
    /// Creates a new instance of the <see cref="BarCodeFormatException"/> class.
    /// </summary>
    public BarCodeFormatException() {}

    /// <summary>
    /// Creates a new instance of the <see cref="BarCodeFormatException"/> class with the given message.
    /// </summary>
    /// <param name="message">Error message of the exception.</param>
    public BarCodeFormatException(string message) : base(message) {}

    /// <summary>
    /// Creates a new instance of the <see cref="BarCodeFormatException"/> class with the given message and 
    /// the given inner exception.
    /// </summary>
    /// <param name="message">Error message of the exception.</param>
    /// <param name="innerException">Inner exception.</param>
    public BarCodeFormatException(string message, Exception innerException) :
      base(message, innerException) {}
  
  }

}
