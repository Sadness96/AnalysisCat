using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalysisCat.Helper.Asterix
{
    public class Cat021
    {
        /// <summary>
        /// 计算日时间项(I021/030)对应的值
        /// </summary>
        /// <param name="byteData">二进制数据</param>
        /// <returns></returns>
        public static string I021_030(byte[] byteData)
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
        /// 计算日时间项(I021/071)对应的值
        /// </summary>
        /// <param name="byteData">二进制数据</param>
        /// <returns></returns>
        public static string I021_071(byte[] byteData)
        {
            return I021_030(byteData);
        }

        /// <summary>
        /// 计算位置坐标(WGS-84中)项(I021/130)对应的值
        /// </summary>
        /// <param name="byteData">二进制数据</param>
        /// <returns></returns>
        public static double[] I021_130(byte[] byteData)
        {
            if (byteData.Length == 6)
            {
                return I021_131(byteData);
            }
            else if (byteData.Length == 8)
            {
                double[] res = { 0, 0 };
                int value1;
                //将容器中前4个字节合并为一个字节，用以计算纬度。
                value1 = (byteData[0] << 24) + (byteData[1] << 16) + (byteData[2] << 8) + byteData[3];
                double temp1 = value1 * (5.364418e-6);
                //Console.WriteLine($"坐标值:纬度值{temp1}");
                res[1] = temp1;
                int value0;
                //将容器中后4个字节合并为一个字节，用以计算经度。
                value0 = (byteData[4] << 24) + (byteData[5] << 16) + (byteData[6] << 8) + byteData[7];
                double temp0 = value0 * (5.364418e-6);
                //Console.WriteLine($"经度值{temp0}");
                res[0] = temp0;
                return res;
            }
            return null;
        }

        /// <summary>
        /// 解析(I062_105)经纬度坐标
        /// </summary>
        /// <param name="byteData">二进制数据</param>
        /// <returns></returns>
        public static double[] I021_131(byte[] byteData)
        {
            var len = byteData.Length;
            var startIndex = 0;
            var Len6Ruler = 180 / Math.Pow(2, 23);
            var Len8Ruler = 180 / Math.Pow(2, 30);
            //根据长度确定转换标尺
            var ruler = len == 6 ? Len6Ruler : Len8Ruler;
            var res = new double[] { 0, 0 };

            int startValue = 0;
            byte lshBit = 0;
            //将容器中前一半字节合并为一个字节，用以计算纬度。
            for (int i = startIndex + len / 2 - 1; i >= startIndex; i--)
            {
                startValue += (int)byteData[i] << lshBit;
                lshBit += 8;
            }
            res[1] = startValue * ruler;

            int endValue = 0;
            lshBit = 0;
            //将容器中后一半字节合并为一个字节，用以计算经度。
            for (int i = startIndex + len - 1; i >= startIndex + len / 2; i--)
            {
                endValue += (int)byteData[i] << lshBit;
                lshBit += 8;
            }

            res[0] = endValue * ruler;
            return res;
        }

        /// <summary>
        /// 计算目标地址项(I021/080)对应的值
        /// </summary>
        /// <param name="byteData">二进制数据</param>
        /// <returns></returns>
        public static string I021_080(byte[] byteData)
        {
            uint rhs = ((uint)byteData[0] << 16) + ((uint)byteData[1] << 8) + byteData[2];
            return string.Format("{0:X}", rhs);
        }

        /// <summary>
        /// 计算几何垂直速率项(I021/157)对应的值
        /// </summary>
        /// <param name="byteData">二进制数据</param>
        /// <returns></returns>
        public static double I021_157(byte[] byteData)
        {
            uint rhs = byteData[0] + (uint)byteData[1];
            return rhs * 6.25;
        }

        /// <summary>
        /// 计算目标识别项(I021/170)对应的值
        /// </summary>
        /// <param name="byteData">二进制数据</param>
        /// <returns></returns>
        public static string I021_170(byte[] byteData)
        {
            string res = "";
            //将6个独立字节合并为一个字节
            long rhs = ((long)byteData[0] << 40) + ((long)byteData[1] << 32) + ((long)byteData[2] << 24) + ((long)byteData[3] << 16) + ((long)byteData[4] << 8) + byteData[5];
            //取出第42~47位
            long value0 = (rhs >> 42) & 63;
            //取出新的二进制数的第5位，并判断为0还是1.
            long value01 = (value0 >> 5) & 1;
            if (value01 == 1)
            {
                char value02 = (char)value0;
                res += value02;
            }
            else
            {
                //value0 = (value0^(1 << 6));
                //如果第5位为1，则将第6位取反。
                value0 ^= (1 << 6);
                char value03 = (char)value0;
                res += value03;
            }
            //取出第36~41位
            long value1 = (rhs >> 36) & 63;
            long value11 = (value1 >> 5) & 1;
            if (value11 == 1)
            {
                char value12 = (char)value1;
                res += value12;
            }
            else
            {
                value1 ^= (1 << 6);
                char value13 = (char)value1;
                res += value13;
            }
            //取出第30~35位
            long value2 = (rhs >> 30) & 63;
            long value21 = (value2 >> 5) & 1;
            if (value21 == 1)
            {
                char value22 = (char)value2;
                res += value22;
            }
            else
            {
                value2 ^= (1 << 6);
                char value23 = (char)value2;
                res += value23;
            }
            //取出第24~29位
            long value3 = (rhs >> 24) & 63;
            long value31 = (value3 >> 5) & 1;
            if (value31 == 1)
            {
                char value32 = (char)value3;
                res += value32;
            }
            else
            {
                value3 ^= (1 << 6);
                char value33 = (char)value3;
                res += value33;
            }
            //取出第18~23位
            long value4 = (rhs >> 18) & 63;
            long value41 = (value4 >> 5) & 1;
            if (value41 == 1)
            {
                char value42 = (char)value4;
                res += value42; ;
            }
            else
            {
                value4 ^= (1 << 6);
                char value43 = (char)value4;
                res += value43;
            }
            //取出第12~17位
            long value5 = (rhs >> 12) & 63;
            long value51 = (value5 >> 5) & 1;
            if (value51 == 1)
            {
                char value52 = (char)value5;
                res += value52;
            }
            else
            {
                value5 ^= (1 << 6);
                char value53 = (char)value5;
                res += value53;
            }
            //取出第6~11位
            long value6 = (rhs >> 6) & 63;
            long value61 = (value6 >> 5) & 1;
            if (value61 == 1)
            {
                char value62 = (char)value6;
                res += value62;
            }
            else
            {
                value6 ^= (1 << 6);
                char value63 = (char)value6;
                res += value63;
            }
            //取出第0~5位
            long value7 = rhs & 63;
            long value71 = (value7 >> 5) & 1;
            if (value71 == 1)
            {
                char value72 = (char)value7;
                res += value72;
            }
            else
            {
                value7 ^= (1 << 6);
                char value73 = (char)value7;
                res += value73;
            }
            return res;
        }

        /// <summary>
        /// 计算几何高度项(I021/140)对应的值
        /// </summary>
        /// <param name="byteData">二进制数据</param>
        /// <returns></returns>
        public static double I021_140(byte[] byteData)
        {
            uint rhs = ((uint)byteData[0] << 8) + byteData[1];
            return rhs * 6.25;
        }
    }
}
