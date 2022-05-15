using AnalysisCat.Helper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalysisCat.Helper.Asterix
{
    public class Cat010
    {
        /// <summary>
        /// 解析I010_140日时间
        /// </summary>
        /// <param name="byteData">二进制数据</param>
        /// <returns></returns>
        public static string I010_140(byte[] byteData)
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
        /// 解析I010/245目标识别
        /// </summary>
        /// <param name="byteData">二进制数据</param>
        /// <returns></returns>
        public static string I010_245(byte[] byteData)
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
                    if (Constants.flightNumberMap.ContainsKey(flno2BinaryStr))
                    {
                        string flightNumberValue = Constants.flightNumberMap[flno2BinaryStr];
                        if (!string.IsNullOrEmpty(flightNumberValue))
                        {
                            result += flightNumberValue;
                        }
                    }
                    flno2BinaryStr = "";
                }
            }
            return result;
        }

        /// <summary>
        /// 解析I010_041在WGS-84中的坐标位置
        /// </summary>
        /// <param name="byteData">二进制数据</param>
        /// <returns></returns>
        public static double[] I010_041(byte[] byteData)
        {
            double[] relDataArray = new double[2];
            if (byteData.Length == 8)
            {
                // 16进制转成10进制（4位一转）
                string xCoordinate10 = byteData[0].ToString("X2") + byteData[1].ToString("X2") + byteData[2].ToString("X2") + byteData[3].ToString("X2");
                string yCoordinate10 = byteData[4].ToString("X2") + byteData[5].ToString("X2") + byteData[6].ToString("X2") + byteData[7].ToString("X2");
                // 10进制计算规则（xCoordinate10 * 180 /2^31）
                relDataArray[0] = double.Parse(Convert.ToInt32(xCoordinate10, 16).ToString()) * 180 / Math.Pow(2, 31);
                relDataArray[1] = double.Parse(Convert.ToInt32(yCoordinate10, 16).ToString()) * 180 / Math.Pow(2, 31);
                return relDataArray;
            }
            return null;
        }
    }
}
