using System;
using System.Diagnostics;

namespace NBarCodes {

	/// <summary>
	/// <see cref="IChecksum"/> implementation that calculates a modulo 10 checksum
	/// based on EAN-type or 2-of-5-type barcode checksums.
	/// </summary>
	public class Modulo10Checksum : IChecksum {

		/// <summary>
		/// Calculates a modulo 10 checksum on the input data.
		/// </summary>
		/// <remarks>
		/// <para>
		/// This checksum digit calculation is based on a modulo 10 calculation of the weighted
		/// sum of the values of each of the digits of the input data.
		/// </para>
		/// <para>
		/// First, we take the rightmost digit of the value and consider it to be an "even" 
		/// character. We then move right-to-left, alternating between even and odd. We then sum 
		/// the numeric value of all the odd positions, and sum the numeric value multiplied by three 
		/// of all the even positions. The check digit, then, is the number which, when added to the total 
		/// calculated, results in a number evenly divisible by 10. If the sum is already evenly divisible 
		/// by 10, the check digit is "0".
		/// </para>
		/// </remarks>
		/// <param name="data">The data of which to calculate the checksum.</param>
		/// <returns>The calculated checksum.</returns>
		public string Calculate(string data) {
			Debug.Assert(!StringHelper.IsNullOrEmpty(data), "Attempt to calculate modulo 10 checksum on empty data.");
			AssertOnlyDigits(data);

			int sum = 0, count = 0;
			for (int dataIndex = data.Length - 1; dataIndex >= 0; --dataIndex) {
				int multiplier = count % 2 == 0 ? 3 : 1;
				sum += (data[dataIndex] - '0') * multiplier;
				++count;
			}
			int checksum = (10 - sum % 10) % 10;

			return checksum.ToString();
		}

		/// <summary>
		/// Asserts that the data consists only of digits.
		/// </summary>
		/// <param name="data">Data to test.</param>
		[Conditional("DEBUG")]
		void AssertOnlyDigits(string data) {
			foreach (char digit in data) {
				Debug.Assert(char.IsDigit(digit), "Modulo 10 checksum must operate on only-digit strings.");
			}
		}

	}

}
