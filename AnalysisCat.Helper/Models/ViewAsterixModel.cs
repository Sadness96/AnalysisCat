using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalysisCat.Helper.Models
{
    /// <summary>
    /// 用于表格中显示结果
    /// </summary>
    public class ViewAsterixModel
    {
        /// <summary>
        /// 报文数据
        /// </summary>
        public string AsterixData { get; set; }

        /// <summary>
        /// 处理结果状态
        /// </summary>
        public AsterixState AsterixState { get; set; }

        /// <summary>
        /// Analysis 数据类型
        /// </summary>
        public CatType CatDataType { get; set; }

        /// <summary>
        /// 数据起始长度
        /// </summary>
        public int StartLength { get; set; }

        /// <summary>
        /// 数据终止长度
        /// </summary>
        public int StopLength { get; set; }

        /// <summary>
        /// 解析结果(Json 序列化)
        /// </summary>
        public string AnalysisData { get; set; }
    }
}
