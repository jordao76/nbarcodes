using System;

namespace NBarCodes {

  class Code39Checksum : IChecksum {

    public string Calculate(string data) {
      int sum = 0;
      foreach (char c in data) {
        sum += Code39Translator.CheckValue(c);
      }
      return (sum % 43).ToString();
    }

  }

}
