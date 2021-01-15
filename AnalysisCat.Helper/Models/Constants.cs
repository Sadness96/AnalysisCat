using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalysisCat.Helper.Models
{
    public class Constants
    {
        /// <summary>
        /// 存储航班号对应的字母或数字
        /// </summary>
        public static Dictionary<string, string> flightNumberMap = new Dictionary<string, string>()
        {
            {"110000", "0"},
            {"110001", "1"},
            {"110010", "2"},
            {"110011", "3"},
            {"110100", "4"},
            {"110101", "5"},
            {"110110", "6"},
            {"110111", "7"},
            {"111000", "8"},
            {"111001", "9"},
            {"000001", "A"},
            {"000010", "B"},
            {"000011", "C"},
            {"000100", "D"},
            {"000101", "E"},
            {"000110", "F"},
            {"000111", "G"},
            {"001000", "H"},
            {"001001", "I"},
            {"001010", "J"},
            {"001011", "K"},
            {"001100", "L"},
            {"001101", "M"},
            {"001110", "N"},
            {"001111", "O"},
            {"010000", "P"},
            {"010001", "Q"},
            {"010010", "R"},
            {"010011", "S"},
            {"010100", "T"},
            {"010101", "U"},
            {"010110", "V"},
            {"010111", "W"},
            {"011000", "X"},
            {"011001", "Y"},
            {"011010", "Z"},
            {"100000", ""}
        };
    }
}
