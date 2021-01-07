using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalysisCat.Helper.Models
{
    /// <summary>
    /// 解析的 Cat 数据模型
    /// </summary>
    public class CatDataModel
    {
        /// <summary>
        /// Analysis 数据类型
        /// </summary>
        public CatType CatDataType { get; set; }

        /// <summary>
        /// 数据起始长度
        /// </summary>
        public int DataStartLength { get; set; }

        /// <summary>
        /// 数据终止长度
        /// </summary>
        public int DataStopLength { get; set; }

        /// <summary>
        /// 数据项信息
        /// </summary>
        public List<CatDataItemModel> CatDataItem { get; set; }
    }

    /// <summary>
    /// 数据项信息模型
    /// </summary>
    public class CatDataItemModel
    {
        /// <summary>
        /// 数据项基础信息
        /// </summary>
        public CatConfigFileModel DataItemInfo { get; set; }

        /// <summary>
        /// 数据项二进制数据
        /// </summary>
        public byte[] CatByteData { get; set; }

        /// <summary>
        /// 解析信息
        /// </summary>
        public object CatAnalysisData { get; set; }
    }
}
