using System;

namespace NBarCodes {

	class Code128Checksum : IChecksum {

		public string Calculate(string data) {
			char currCode = Code128Encoder.ResolveStartCode(data[0]);

			// start character
			int sum = Code128Encoder.SymbolValue(data[0].ToString(), currCode);

			for (int i = 1, pos = 1; i < data.Length; ++i, ++pos) {
				string curr = data[i].ToString();
				int value = 0;

				if (currCode == Code128Encoder.CodeC && char.IsNumber(data[i]))
					value = Code128Encoder.SymbolValue(curr+data[++i].ToString(), currCode);
				else
					value = Code128Encoder.SymbolValue(curr, currCode);

				sum += pos * value;

				// detect change in current code
				switch (data[i]) {
					case Code128Encoder.CodeA: 
					case Code128Encoder.CodeB: 
					case Code128Encoder.CodeC: 
						currCode = data[i];
						break;
				}
			}
			
			return Code128Encoder.SymbolString(sum % 103, currCode);
		}

	}
}
