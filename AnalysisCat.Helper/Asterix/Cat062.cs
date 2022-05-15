using AnalysisCat.Helper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalysisCat.Helper.Asterix
{
    public class Cat062
    {
        /// <summary>
        /// 计算日时间项(I062/070)对应的值
        /// </summary>
        /// <param name="byteData">二进制数据</param>
        /// <returns></returns>
        public static string I062_070(byte[] byteData)
        {
            //将几个独立字节合并为一个字节
            uint rhs = ((uint)byteData[0] << 16) + ((uint)byteData[1] << 8) + byteData[2];
            //总秒数
            uint value0 = rhs / 128;
            //小时数
            uint value1 = value0 / 3600;
            //分钟数
            uint value2 = (value0 - value1 * 3600) / 60;
            //秒数
            uint value3 = (value0 - value1 * 3600) % 60;
            //毫秒数
            uint value4 = ((rhs % 128) * 1000) / 128;
            return $"{DateTime.Now.ToShortDateString()} {value1}:{value2}:{value3}.{value4}";
        }

        /// <summary>
        /// 解析(I062_245)目标识别
        /// </summary>
        /// <param name="byteData">二进制数据</param>
        /// <returns></returns>
        public static string I062_245(byte[] byteData)
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
        /// 解析(I062_105)经纬度坐标
        /// </summary>
        /// <param name="byteData">二进制数据</param>
        /// <returns></returns>
        public static double[] I062_105(byte[] byteData)
        {
            double[] relDataArray = new double[2];
            if (byteData.Length == 8)
            {
                // 16进制转成10进制（4位一转）
                string xCoordinate10 = byteData[0].ToString("X2") + byteData[1].ToString("X2") + byteData[2].ToString("X2") + byteData[3].ToString("X2");
                string yCoordinate10 = byteData[4].ToString("X2") + byteData[5].ToString("X2") + byteData[6].ToString("X2") + byteData[7].ToString("X2");
                // 10进制计算规则（xCoordinate10 * 180 /2^25）
                relDataArray[0] = double.Parse(Convert.ToInt32(xCoordinate10, 16).ToString()) * 180 / Math.Pow(2, 25);
                relDataArray[1] = double.Parse(Convert.ToInt32(yCoordinate10, 16).ToString()) * 180 / Math.Pow(2, 25);
                return relDataArray;
            }
            return null;
        }

        /// <summary>
        /// 解析(I062_100)卡迪尔坐标
        /// </summary>
        /// <param name="byteData">二进制数据</param>
        /// <returns></returns>
        public static double[] I062_100(byte[] byteData)
        {
            double[] relDataArray = new double[2];
            if (byteData.Length == 6)
            {
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
    }
}
