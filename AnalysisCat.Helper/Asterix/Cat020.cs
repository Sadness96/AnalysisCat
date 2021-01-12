using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AnalysisCat.Helper.Asterix
{
    public class Cat020
    {
        /// <summary>
        /// 解析I020_140日时间
        /// </summary>
        /// <param name="byteData">二进制数据</param>
        /// <returns></returns>
        public static string I020_140(byte[] byteData)
        {
            // 16进制转成10进制
            string timeDec = (((uint)byteData[0] << 16) + ((uint)byteData[1] << 8) + byteData[2]).ToString();

            // 字符串转数值/128 * 1000 总毫秒数
            long ms = (long)((double.Parse(timeDec) / 128) * 1000);

            int ss = 1000;
            int mi = ss * 60;
            int hh = mi * 60;

            long hour = ms / hh;
            long minute = (ms - hour * hh) / mi;
            long second = (ms - hour * hh - minute * mi) / ss;
            long milliSecond = ms - hour * hh - minute * mi - second * ss;

            // 小时
            string strHour = hour < 10 ? "0" + hour : "" + hour;
            // 分钟
            string strMinute = minute < 10 ? "0" + minute : "" + minute;
            // 秒
            string strSecond = second < 10 ? "0" + second : "" + second;
            // 毫秒
            string strMilliSecond = milliSecond < 10 ? "0" + milliSecond : "" + milliSecond;
            strMilliSecond = milliSecond < 100 ? "0" + strMilliSecond : "" + strMilliSecond;
            //增加UTC时间
            strHour = int.Parse(strHour) + 8 > 24 ? (int.Parse(strHour) + 8 - 24).ToString() : (int.Parse(strHour) + 8).ToString();
            return $"{DateTime.Now.ToShortDateString()} {strHour}:{strMinute}:{strSecond}.{strMilliSecond}";
        }

        /// <summary>
        /// 解析I020/245目标识别
        /// </summary>
        /// <param name="byteData">二进制数据</param>
        /// <returns></returns>
        public static string I020_245(byte[] byteData)
        {
            string str = "";
            for (int i = 1; i < byteData.Length; i++)
            {
                // 把第一位去掉
                str += Convert.ToString(byteData[i], 2).PadLeft(8, '0');
            }

            char[] strCharArray = str.ToCharArray();
            string flno2BinaryStr = "";
            string result = "";

            for (int i = 0; i < strCharArray.Length; i++)
            {
                flno2BinaryStr += strCharArray[i] + "";
                if ((i + 1) % 6 == 0)
                {
                    string flightNumberValue = flightNumberMap[flno2BinaryStr];
                    if (!string.IsNullOrEmpty(flightNumberValue))
                    {
                        result += flightNumberValue;
                    }
                    flno2BinaryStr = "";
                }
            }
            return result;
        }

        /// <summary>
        /// 解析I020_041在WGS-84中的坐标位置
        /// </summary>
        /// <param name="byteData">二进制数据</param>
        /// <returns></returns>
        public static double[] I020_041(byte[] byteData)
        {
            double[] relDataArray = new double[2];
            if (byteData.Length == 8)
            {
                // 16进制转成10进制（4位一转）
                string xCoordinate10 = byteData[0].ToString("X2") + byteData[1].ToString("X2") + byteData[2].ToString("X2") + byteData[3].ToString("X2");
                string yCoordinate10 = byteData[4].ToString("X2") + byteData[5].ToString("X2") + byteData[6].ToString("X2") + byteData[7].ToString("X2");
                // 10进制计算规则（xCoordinate10 * 180 /2^25）
                relDataArray[0] = double.Parse(Convert.ToInt32(xCoordinate10, 16).ToString()) * 180 / 33554432;
                relDataArray[1] = double.Parse(Convert.ToInt32(yCoordinate10, 16).ToString()) * 180 / 33554432;
                return relDataArray;
            }
            return null;
        }

        /// <summary>
        /// 解析I020_042轨道位置(直角)
        /// </summary>
        /// <param name="byteData">二进制数据</param>
        /// <returns></returns>
        public static double[] I020_042(byte[] byteData)
        {
            double[] relDataArray = new double[2];
            if (byteData.Length == 6)
            {
                // 16进制转成10进制
                string xAngle16 = byteData[0].ToString("X2") + byteData[1].ToString("X2") + byteData[2].ToString("X2");
                string yAngle16 = byteData[3].ToString("X2") + byteData[4].ToString("X2") + byteData[5].ToString("X2");
                string xAngle10 = Convert.ToInt32(xAngle16, 16).ToString();
                string yAngle10 = Convert.ToInt32(yAngle16, 16).ToString();
                // 10进制计算规则（xAngle10 * 0.5）
                relDataArray[0] = double.Parse(xAngle10) * 0.5;
                relDataArray[1] = double.Parse(yAngle10) * 0.5;
                return relDataArray;
            }
            return null;
        }

        /// <summary>
        /// 解析I020_161
        /// </summary>
        /// <param name="byteData">二进制数据</param>
        /// <returns></returns>
        public static int I020_161(byte[] byteData)
        {
            return Convert.ToInt32(byteData[0].ToString("X2") + byteData[1].ToString("X2"), 16);
        }

        /// <summary>
        /// 解析I020_110
        /// </summary>
        /// <param name="byteData">二进制数据</param>
        /// <returns></returns>
        public static double I020_110(byte[] byteData)
        {
            string strByteData = byteData[0].ToString("X2") + byteData[1].ToString("X2");
            double dByteData = (double)Convert.ToInt32(strByteData, 16);

            if (Convert.ToString(byteData[0], 2).Substring(0, 1).Equals("1"))
            {
                // 如果2进制长度为16，说明第16位一定为1，则为负数
                return -(Math.Pow(2, 16) - dByteData) * 6.25;
            }
            else
            {
                // 如果2进制长度不为16，说明第16位一定为0，则为正数
                return dByteData * 6.25;
            }
        }

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
