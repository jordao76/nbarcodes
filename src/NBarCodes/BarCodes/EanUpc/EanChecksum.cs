namespace NBarCodes {

	class EanUpc5DigitSupplementChecksum : IChecksum {
		public string Calculate(string data) {
			int sum = 0, count = 0;
			for (int i = data.Length - 1; i >= 0; --i) {
				sum += (data[i] - '0') * (count++ % 2 == 0 ? 3 : 9);
			}
			int ret = sum % 10;
			return ret.ToString();
		}
	}

}
