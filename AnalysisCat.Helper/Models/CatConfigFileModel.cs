using AnalysisCat.Helper.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalysisCat.Helper.Models
{
    /// <summary>
    /// Cat 配置文件模型
    /// </summary>
    public class CatConfigFileModel
    {
        /// <summary>
        /// 编号
        /// </summary>
        public string FRN { get; set; }

        /// <summary>
        /// 数据项
        /// </summary>
        public string DataItem { get; set; }

        /// <summary>
        /// 简介
        /// </summary>
        public string Information { get; set; }

        /// <summary>
        /// 长度
        /// </summary>
        public string Length { get; set; }
    }
}
