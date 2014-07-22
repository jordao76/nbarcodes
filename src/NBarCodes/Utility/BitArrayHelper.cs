using System;
using System.Collections;
using System.Diagnostics;

namespace NBarCodes {

  /// <summary>
  /// Utility class for BitArray operations.
  /// </summary>
  public static class BitArrayHelper {

    /// <summary>
    /// Removes the first <see cref="BitArray"/> of an array of
    /// BitArrays and returns it.
    /// </summary>
    /// <param name="bits">Array of bit arrays to work on. On return, will lack the first element.</param>
    /// <returns>The first element of the BitArray array parameter.</returns>
    public static BitArray PopFront(ref BitArray[] bits) {
      Debug.Assert(bits != null, "Can't operate on null array");
      Debug.Assert(bits.Length != 0, "Can't operate on empty array");

      BitArray popped = bits[0];
      BitArray[] altered = new BitArray[bits.Length - 1];
      for (int i = 1; i < bits.Length; ++i) {
        altered[i - 1] = bits[i];
      }
      // switch old with new
      bits = altered;
      return popped;
    }

    /// <summary>
    /// Removes the last <see cref="BitArray"/> of an array of
    /// BitArrays and returns it.
    /// </summary>
    /// <param name="bits">Array of bit arrays to work on. On return, will lack the last element.</param>
    /// <returns>The last element of the BitArray array parameter.</returns>
    public static BitArray PopBack(ref BitArray[] bits) {
      Debug.Assert(bits != null, "Can't operate on null array");
      Debug.Assert(bits.Length != 0, "Can't operate on empty array");

      BitArray popped = bits[bits.Length - 1];
      BitArray[] altered = new BitArray[bits.Length - 1];
      for (int i = 0; i < bits.Length - 1; ++i) {
        altered[i] = bits[i];
      }
      // switch old with new
      bits = altered;
      return popped;
    }

    /// <summary>
    /// Converts a string of data consisting of '1's and '0's
    /// into a <see cref="BitArray"/>.
    /// </summary>
    /// <param name="data">Input data.</param>
    /// <returns>BitArray of input data.</returns>
    public static BitArray ToBitArray(string data) {
      Debug.Assert(!StringHelper.IsNullOrEmpty(data), "Can't operate on empty data");
      
      BitArray bits = new BitArray(data.Length);
      for (int i = 0; i < data.Length; ++i) {
        switch (data[i]) {
          case '1': bits[i] = true; break;
          case '0': bits[i] = false; break;
          default: throw new ArgumentException("Incorrect character found");
        }
      }
      return bits;
    }

    /// <summary>
    /// Converts an array of strings of data consisting of '1's and '0's
    /// into an array of corresponding <see cref="BitArray"/>s.
    /// </summary>
    /// <param name="data">Input strings.</param>
    /// <returns>Bit matrix (array of BitArrays) created.</returns>
    public static BitArray[] ToBitMatrix(params string[] data) {
      Debug.Assert(data != null, "Can't operate on null data");
      Debug.Assert(data.Length != 0, "Can't operate on empty data");

      BitArray[] bits = new BitArray[data.Length];
      for (int i = 0; i < data.Length; ++i) {
        bits[i] = ToBitArray(data[i]);
      }
      return bits;
    }

  }
}
