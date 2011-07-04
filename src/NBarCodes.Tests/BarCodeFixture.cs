using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using NBarCodes.Tests.Readers;
using NUnit.Framework;

namespace NBarCodes.Tests {

	[TestFixture]
	[Category("Acceptance")]
	public class BarCodeFixture {

    [Test, TestCaseSource(typeof(BarCodeTestCaseFactory), "LoadTestCases")]
    public void BarCodeGenerationTest(BarCodeTestInput input) {
			Trace.WriteLine(string.Format("Testing: {0}, {1}", input.Type, input.Data));

      BarCodeGenerator generator = new BarCodeGenerator(new BarCodeSettings { 
        Type = input.Type,
        Data = input.Data
      });

      using (var image = generator.GenerateImage()) {
        // "recognize" the barcode
        var reader = CreateReader(input.Reader);
        var result = reader.ReadBarCode((Bitmap)image);

        Assert.AreEqual(input.Type, result.Type, "Type of barcode differs!");
        Assert.AreEqual(input.Expected, result.Data, "Barcode data differs!");
      }
		}

    private IBarCodeReader CreateReader(string reader) {
      return (IBarCodeReader)Activator.CreateInstance(Type.GetType("NBarCodes.Tests.Readers." + reader + "BarCodeReader"));
    }

  }

  class BarCodeTestCaseFactory {
  
    public IEnumerable<TestCaseData> LoadTestCases() {
      var data = new List<TestCaseData>();
      foreach (var bcs in RetrieveTestData()) {
        data.Add(new TestCaseData(bcs).SetName(string.Format("{0} {1}", bcs.Type, bcs.Data)));
      }
      return data;
    }

    #region Retrieve Test Data

    private const string TestDataResource = "NBarCodes.Tests.BarCodeTestData.txt";

    private IEnumerable<BarCodeTestInput> RetrieveTestData() {
      var inputRead = new List<BarCodeTestInput>();

      using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(TestDataResource))
      using (var fileReader = new StreamReader(stream)) {
        while (true) {
          string line = fileReader.ReadLine();
          if (line == null) break;

          var input = ParseInput(line);
          if (input != null) {
            inputRead.Add(input);
          }
        }
      }
      return inputRead;
    }

    private BarCodeTestInput ParseInput(string input) {
      // expected format: 
      // "[Barcode reader], [Barcode type], [Barcode data], [Expected output]"
      // e.g.: "ZXing, Code128, 1234567890"
      // comments take a WHOLE line and begin with '#'
      // the expected output is optional and defaults to the barcode data

      if (input.Trim().Length == 0) {
        return null;
      }
      if (input.Trim().StartsWith("#")) {
        return null;
      }

      var components = input.Split(',');

      if (components.Length < 3 || components.Length > 4) {
        Assert.Fail("Incorrent settings format: '{0}'", input);
      }

      // extract test data
      string reader = components[0].Trim();
      string type = components[1].Trim();
      string data = components[2].Trim();
      string expected = components.Length == 3 ? data : components[3].Trim();

      return new BarCodeTestInput {
        Type = (BarCodeType)Enum.Parse(typeof(BarCodeType), type),
        Data = data,
        Expected = expected,
        Reader = reader
      };
    }

    #endregion

  }

  public class BarCodeTestInput {
    public BarCodeType Type { get; set; }
    public string Data { get; set; }
    public string Expected { get; set; }
    public string Reader { get; set; }
  }

}
