namespace NBarCodes {
  sealed class PostNetChecksum : IChecksum {
    public string Calculate(string data) {
      int sum = 0;
      foreach (char c in data) {
        sum += c - '0';
      }
      return ((10 - sum % 10) % 10).ToString();
    }
  }
}
