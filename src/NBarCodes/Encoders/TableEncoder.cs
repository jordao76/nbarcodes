using System;
using System.Collections;

namespace NBarCodes {

	/// <summary>
	/// Encoder base class for table-lookup like encoders.
	/// </summary>
  abstract class TableEncoder : ISymbolEncoder {
    
		/// <summary>
		/// Looks up the sought value and returns its encoded equivalent.
		/// Should be properly overriden in derived classes.
		/// </summary>
		/// <param name="index">Index of the value to look up.</param>
		/// <returns>The encoded data.</returns>
		protected abstract BitArray LookUp(int index);

		/// <summary>
		/// Encodes a string of barcode data.
		/// </summary>
		/// <param name="data">The string of data to be encoded.</param>
		/// <returns>The encoded data.</returns>
		public virtual BitArray Encode(string data) {
      BitArray bits = new BitArray(0);

      foreach (char datum in data) {
        BitArray datumBits = Encode(datum);
        int count = bits.Count;
        bits.Length = bits.Count + datumBits.Count;
        for (int j = 0; j < datumBits.Count; ++j) {
          bits[count+j] = datumBits[j];
        }
      }

      return bits;
    }

		/// <summary>
		/// Encodes a character of barcode data. 
		/// </summary>
		/// <remarks>
		/// This method uses the <see cref="LookUp"/> method to resolve 
		/// the character of data to an encoded entity (<see cref="BitArray"/>).
		/// It assumes the datum passed represents a number and converts it before
		/// looking it up.
		/// </remarks>
		/// <param name="datum">The character of data to be encoded.</param>
		/// <returns>The encoded data.</returns>
		public virtual BitArray Encode(char datum) {
      // encode the symbol
			// may throw IndexOutOfRangeException
			BitArray bits = (BitArray) LookUp(datum - '0').Clone();

      // return the encoded symbol
      return bits;
    }

  }

}
