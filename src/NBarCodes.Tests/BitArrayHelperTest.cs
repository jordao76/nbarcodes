using System;
using System.Collections;
using System.Diagnostics;
using NUnit.Framework;

namespace NBarCodes.Tests {

	/// <summary>
	/// Tests for the <see cref="BitArrayHelper"/> class.
	/// </summary>
	[TestFixture]
	public class BitArrayHelperTest {

		/// <summary>
		/// Tests popping of front BitArray.
		/// </summary>
		[Test]
		public void TestPopFront() {
			BitArray[] bits = new BitArray[3];
			BitArray first = bits[0] = new BitArray(new int[] { 1 });
			BitArray second = bits[1] = new BitArray(new int[] { 2 });
			BitArray third = bits[2] = new BitArray(new int[] { 4 });

			BitArray result = BitArrayHelper.PopFront(ref bits);

			Assert.AreSame(first, result);
			Assert.AreEqual(2, bits.Length);
			Assert.AreSame(second, bits[0]);
			Assert.AreSame(third, bits[1]);
		}

		/// <summary>
		/// Tests popping of back BitArray.
		/// </summary>
		[Test]
		public void TestPopBack() {
			BitArray[] bits = new BitArray[3];
			BitArray first = bits[0] = new BitArray(new int[] { 1 });
			BitArray second = bits[1] = new BitArray(new int[] { 2 });
			BitArray last = bits[2] = new BitArray(new int[] { 4 });

			BitArray result = BitArrayHelper.PopBack(ref bits);

			Assert.AreSame(last, result);
			Assert.AreEqual(2, bits.Length);
			Assert.AreSame(first, bits[0]);
			Assert.AreSame(second, bits[1]);
		}

		/// <summary>
		/// Tests conversion from string to BitArray.
		/// </summary>
		[Test]
		public void TestToBitArray() {
			string data = "10001101001";
			BitArray bits = BitArrayHelper.ToBitArray(data);
			AssertBits(data, bits);
		}

		/// <summary>
		/// Tests conversion from string with bad data to BitArray.
		/// </summary>
		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void TestToBitArrayWrongData() {
			BitArrayHelper.ToBitArray("10010010100102");
		}

		/// <summary>
		/// Tests conversion from string array to BitArray array.
		/// </summary>
		[Test]
		public void TestToBitMatrix() {
			string[] data = new string[3];
			data[0] = "1001001001000111010";
			data[1] = "1111100000";
			data[2] = new string('1', 700);

			BitArray[] bits = BitArrayHelper.ToBitMatrix(data);
			Assert.AreEqual(data.Length, bits.Length);
			for (int i = 0; i < data.Length; ++i) {
				AssertBits(data[i], bits[i]);
			}
		}

		/// <summary>
		/// Asserts that the 'bits' of a string match the bits of a BitArray.
		/// </summary>
		/// <param name="data">Bit-string, string composed with '1's and '0's.</param>
		/// <param name="bits">BitArray to compare.</param>
		private void AssertBits(string data, BitArray bits) {
			Assert.AreEqual(data.Length, bits.Length);

			for (int i = 0; i < data.Length; ++i) {
				if (data[i] == '1') {
					Assert.IsTrue(bits[i]);
				}
				else if (data[i] == '0') {
					Assert.IsFalse(bits[i]);
				}
				else {
					Assert.Fail("Wrong data!");
				}
			}
		}

	}
}

