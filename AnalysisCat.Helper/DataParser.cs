using AnalysisCat.Helper.Models;
using AnalysisCat.Helper.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalysisCat.Helper
{
    /// <summary>
    /// 数据解析类
    /// </summary>
    public static class DataParser
    {
        /// <summary>
        /// 分析数据
        /// </summary>
        public static CatDataModel Analysis(string data)
        {
            // 判断数据有效性
            if (!string.IsNullOrEmpty(data) && IsAsterix(data))
            {
                // 十六进制数据转为 byte
                var vCatByteData = StringToByte(data.Replace(" ", ""));
                if (vCatByteData != null && vCatByteData.Length >= 10)
                {
                    // 解析数据
                    return AnalysisData(vCatByteData);
                }
            }
            return new CatDataModel() { CatDataType = CatType.unknown };
        }

        /// <summary>
        /// 校验数据是否符合 Asterix 标准
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns>数据符合标准返回true</returns>
        private static bool IsAsterix(string data)
        {
            //Cat 16 进制数据开头
            List<string> listCat16X = new List<string>() { "14", "15", "3E" };
            //判断是否符合标准数据
            if (data.Length >= 10)
            {
                var dataStart = data.Substring(0, 2);
                if (listCat16X.Contains(dataStart))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 十六进制文本数据转为 byte
        /// </summary>
        /// <param name="str">十六进制文本数据</param>
        /// <returns>byte数据</returns>
        private static byte[] StringToByte(string str)
        {
            if (str.Length % 2 != 0)
            {
                str += "0";
            }
            byte[] temp = new byte[str.Length / 2];
            for (int i = 0; i < temp.Length; i++)
            {
                temp[i] = Convert.ToByte(str.Substring(i * 2, 2), 16);
            }
            return temp;
        }

        /// <summary>
        /// 解析数据
        /// </summary>
        /// <param name="byteCat">二进制数据</param>
        /// <returns>解析结果</returns>
        private static CatDataModel AnalysisData(byte[] byteCat)
        {
            CatDataModel catDataModel = new CatDataModel();
            // 获取数据类型
            switch (byteCat[0])
            {
                case 20: catDataModel.CatDataType = CatType.Cat020; break;
                case 21: catDataModel.CatDataType = CatType.Cat021; break;
                case 62: catDataModel.CatDataType = CatType.Cat062; break;
                default: catDataModel.CatDataType = CatType.unknown; break;
            }
            // 获取数据长度
            catDataModel.DataStartLength = byteCat[1];
            catDataModel.DataStopLength = byteCat[2];

            // 分析数据
            var vConfigCatFileT = GetCatConfigFile.DicConfigCatFileT[catDataModel.CatDataType];
            if (vConfigCatFileT != null && vConfigCatFileT.Count >= 1)
            {
                // 根据配置文件创建数据模型
                List<CatDataItemModel> catDataItemModels = new List<CatDataItemModel>();
                foreach (var vDataItem in vConfigCatFileT)
                {
                    CatDataItemModel catDataItemModel = new CatDataItemModel();
                    catDataItemModel.DataItemInfo = vDataItem;
                    catDataItemModels.Add(catDataItemModel);
                }
                // 解析出标识符开关所占的字节
                byte[] byteFspecBytes = GetFspecBytes(byteCat);
                catDataModel.FspecBytes = byteFspecBytes;
                for (int iByteFspecBytes = 0; iByteFspecBytes < byteFspecBytes.Length; iByteFspecBytes++)
                {
                    var vByteItem2 = Convert.ToString(byteFspecBytes[iByteFspecBytes], 2).PadLeft(8, '0');
                    for (int iByteItem2 = 0; iByteItem2 < vByteItem2.Length; iByteItem2++)
                    {
                        if (int.Parse(vByteItem2[iByteItem2].ToString()) == 1)
                        {
                            int iIndexes = iByteFspecBytes * 8 + iByteItem2;
                            if (catDataItemModels.Count() > iIndexes)
                            {
                                catDataItemModels[iIndexes].IsEnable = true;
                            }
                        }
                    }
                }
                // 拆分数据字节
                byte[] byteDate = new byte[byteCat.Length - 3 - byteFspecBytes.Length];
                Array.Copy(byteCat, 3 + byteFspecBytes.Length, byteDate, 0, byteDate.Length);
                int iByteNum = 0;
                foreach (var item in catDataItemModels.Where(o => o.IsEnable))
                {
                    int itemByteLength;
                    if (item.DataItemInfo.Length == null || item.DataItemInfo.Length.Equals("-") || string.IsNullOrEmpty(item.DataItemInfo.Length) || item.DataItemInfo.Length.Contains("N"))
                    {
                        continue;
                    }
                    if (item.DataItemInfo.Length.Contains("+"))
                    {
                        int iLength = int.Parse(item.DataItemInfo.Length.Substring(0, item.DataItemInfo.Length.IndexOf("+")));
                        int iFspecLength = GetFspecLength(byteDate, iByteNum + iLength - 1) - iByteNum + 1;
                        byte[] bytes = new byte[iFspecLength];
                        Array.Copy(byteDate, iByteNum, bytes, 0, bytes.Length);
                        item.CatByteData = bytes;
                        iByteNum += iFspecLength;
                        continue;
                    }
                    int.TryParse(item.DataItemInfo.Length, out itemByteLength);
                    if (itemByteLength >= 1)
                    {
                        byte[] bytes = new byte[itemByteLength];
                        Array.Copy(byteDate, iByteNum, bytes, 0, bytes.Length);
                        item.CatByteData = bytes;
                        iByteNum += itemByteLength;
                        continue;
                    }
                }
                // 解析字段
                foreach (var item in catDataItemModels.Where(o => o.IsEnable && o.CatByteData != null && o.CatByteData.Length >= 1))
                {
                    var parameters = new object[] { item.CatByteData };
                    var vResultData = ReflectHelper.RunMethod(catDataModel.CatDataType.ToString(), item.DataItemInfo.DataItem.Replace("/", "_"), parameters);
                    if (vResultData != null)
                    {
                        item.CatAnalysisData = vResultData;
                    }
                }

                catDataModel.CatDataItem = catDataItemModels;
            }
            return catDataModel;
        }

        /// <summary>
        /// 解析出标识符所占的字节
        /// </summary>
        /// <param name="dates"></param>
        /// <returns></returns>
        private static byte[] GetFspecBytes(byte[] dates)
        {
            int count = 3;
            byte[] fspecbytes;
            //如果下一个字节是标识符
            while (IsMoreFspec(dates[count]))
            {
                count++;
            }
            //确定标识符字节数
            fspecbytes = new byte[count - 2];
            Array.Copy(dates, 3, fspecbytes, 0, fspecbytes.Length);
            return fspecbytes;
        }

        /// <summary>
        /// 获取占用字节长度
        /// </summary>
        /// <param name="dates"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static int GetFspecLength(byte[] dates, int count)
        {
            //如果下一个字节是标识符
            while (IsMoreFspec(dates[count]))
            {
                count++;
            }
            return count;
        }

        /// <summary>
        /// 判断下一个字节是否是符号字节
        /// </summary>
        /// <param name="temp"></param>
        /// <returns></returns>
        private static bool IsMoreFspec(byte temp)
        {
            byte[] tempbytes = new byte[4];
            temp <<= 7;
            temp >>= 7;
            tempbytes[0] = temp;
            if (BitConverter.ToInt32(tempbytes, 0) == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
