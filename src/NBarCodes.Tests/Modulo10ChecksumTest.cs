using System;
using System.Diagnostics;
using NUnit.Framework;

namespace NBarCodes.Tests {

	/// <summary>
	/// Unit test for the internal <b>Modulo10Checksum</b> class.
	/// </summary>
	[TestFixture]
	public class Modulo10ChecksumTest {

		/// <summary>
		/// Sets up the checksum driver to help with checksum assertions.
		/// </summary>
		[SetUp]
		public void Init() {
			_driver = new ChecksumTestDriver(new Modulo10Checksum()); 
		}

		/// <summary>
		/// Tests good outcomes of the checksum calculation.
		/// </summary>
		[Test]
		public void TestGoodChecksums() {
			_driver.AssertCalculation("007567816412", "5");
			_driver.AssertCalculation("123456789123", "1");
			_driver.AssertCalculation("000000000000", "0");
			_driver.AssertCalculation("111111111111", "6");
			_driver.AssertCalculation("576415430248", "3");
			_driver.AssertCalculation("456", "5");
			_driver.AssertCalculation("9", "3");
			_driver.AssertCalculation("1234567", "0");
			_driver.AssertCalculation("5512345", "7");
		}

		ChecksumTestDriver _driver;
	}

}
