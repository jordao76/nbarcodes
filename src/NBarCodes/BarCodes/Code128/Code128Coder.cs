using System;
using System.Diagnostics;
using System.Text;

namespace NBarCodes {
  class Code128Coder {
    
    // flags // use enum??
    private const int 
      codeA = 1,
      codeB = 2,
      codeC = 4,
      codeCFirst = 8,
      codeCSecond = 16;

    private int[] _allCodes, _bestCode, _currCode;
    private int _bestCodeValue;

    // code the 128 barcode data to use as little space as possible
    public string Code(string data) {
      if (data.Length == 0) return "";

      // first determine what codes can be used to code every character
      _allCodes = new int[data.Length];
      for (int i = 0; i < data.Length; ++i) {
        string curr = data[i].ToString();
        if (Code128Encoder.CanCode(curr, Code128Encoder.CodeA)) _allCodes[i] |= codeA;
        if (Code128Encoder.CanCode(curr, Code128Encoder.CodeB)) _allCodes[i] |= codeB;

        // code C will code more than one character (FNC1??)
        //if (Code128Encoder.CanCode(curr, Code128Encoder.CodeC)) _allCodes[i] |= codeC; //FNC1??
        if (i < data.Length - 1 && 
          Code128Encoder.CanCode(curr+data[i+1].ToString(), Code128Encoder.CodeC)) {
          _allCodes[i] |= codeCFirst;
          _allCodes[i+1] |= codeCSecond;
        }
      }

      // backtrack to find the best configuration
      _bestCodeValue = int.MaxValue;
      _bestCode = new int[_allCodes.Length];
      _currCode = new int[_allCodes.Length];
      SolveCode(0);
      for (int i = 0; i < _bestCode.Length; ++i) {
        if (_bestCode[i] > codeC) _bestCode[i] = codeC; // change codeCFirst and codeCSecond to codeC
      }

      DebugCode(data);

      // put code start and code changes in result string
      StringBuilder sb = new StringBuilder();
      int code = _bestCode[0];
      switch (code) {
        case codeA: sb.Append(Code128Encoder.StartA); break;
        case codeB: sb.Append(Code128Encoder.StartB); break;
        case codeC: sb.Append(Code128Encoder.StartC); break;
      }
      for (int i = 0; i < _bestCode.Length; ++i) {
        if (_bestCode[i] != code) {
          code = _bestCode[i];
          switch (code) {
            case codeA: sb.Append(Code128Encoder.CodeA); break;
            case codeB: sb.Append(Code128Encoder.CodeB); break;
            case codeC: sb.Append(Code128Encoder.CodeC); break;
          }
        }
        sb.Append(data[i]);
      }

      return sb.ToString();
    }

    [Conditional("DEBUG")]
    private void DebugCode(string data) {
      Debug.Write(string.Format("Code 128 coding:\n\t{0}\n\t", data));
      foreach (int i in _bestCode) {
        Debug.Write(string.Format("{0}", i == 1 ? "A" : (i == 2 ? "B" : (i == 4 ? "C" : i.ToString()))));
      }
      Debug.WriteLine(string.Empty);
    }

    // backtracking algorithm
    private void SolveCode(int index) {
      for (int i = 1; i <= 16; i <<= 1) { // for all flag codes
        if (index > 0) {
          if (_currCode[index-1] == codeCFirst) {
            if (i != codeCSecond) continue;
          }
          else {
            if (i == codeCSecond) continue;
            
            // now, some optimizing heuristics

            // just allow changing codes if we're changing to code C
            if (_currCode[index-1] == codeA) {
              // disallow change from code A to B if we can continue on A
              if ((_allCodes[index] & codeA) != 0 && i == codeB) continue;
            }
            else if (_currCode[index-1] == codeB) {
              // disallow change from code B to A if we can continue on B
              if ((_allCodes[index] & codeB) != 0 && i == codeA) continue;
            }
            else if (_currCode[index-1] == codeCSecond) {
              // remain in code C if we can
              if ((_allCodes[index] & codeCFirst) != 0 && i != codeCFirst) continue;
            }
          }
        }

        if ((_allCodes[index] & i) != 0) {
          // try this code
          _currCode[index] = i;
          if (index == _allCodes.Length - 1) {
            // check if we improved upon the best
            int currValue = EvaluateCode(_currCode);
            if (currValue < _bestCodeValue) {
              // improved!
              Array.Copy(_currCode, 0, _bestCode, 0, _currCode.Length);
              _bestCodeValue = currValue;
            }
          }
          else {
            SolveCode(index+1);
          }
        }
      }
    }

    private int EvaluateCode(int[] code) {
      float value = 0;
      int curr = (code[0] == codeCFirst || code[0] == codeCSecond) ? codeC : code[0];
      foreach (int i in code) {
        switch (i) {
          default: 
            value += 1 + (i != curr ? 1 : 0);
            curr = i;
            break;
          case codeCFirst: case codeCSecond:
            value += 0.5f + (curr != codeC ? 1 : 0);
            curr = codeC;
            break;
        }
      }
      return (int)value;
    }
  }
}
