using AnalysisCat.Helper.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalysisCat.Helper.Utils
{
    /// <summary>
    /// 获取 Cat 配置文件类
    /// </summary>
    public class GetCatConfigFile
    {
        private static Dictionary<CatType, string> _dicConfigCatFilePath;
        /// <summary>
        /// Cat 配置参考文档路径
        /// </summary>
        public static Dictionary<CatType, string> DicConfigCatFilePath
        {
            get
            {
                if (_dicConfigCatFilePath == null)
                {
                    _dicConfigCatFilePath = new Dictionary<CatType, string>()
                    {
                        { CatType.Cat020,"Cat020pt14ed19.xlsx"},
                        { CatType.Cat021,"Cat021p12ed026.xlsx"},
                        { CatType.Cat062,"Cat062p9ed118.xlsx"}
                    };
                }
                return _dicConfigCatFilePath;
            }
        }

        private static Dictionary<CatType, DataTable> _dicConfigCatFile;
        /// <summary>
        /// Cat 配置参考文档
        /// </summary>
        public static Dictionary<CatType, DataTable> DicConfigCatFile
        {
            get
            {
                if (_dicConfigCatFile == null)
                {
                    _dicConfigCatFile = new Dictionary<CatType, DataTable>();
                    foreach (var item in DicConfigCatFilePath)
                    {
                        var vCatConfigPath = $@"{AppDomain.CurrentDomain.BaseDirectory}CatConfig\{item.Value}";
                        if (File.Exists(vCatConfigPath))
                        {
                            _dicConfigCatFile.Add(item.Key, ExcelHelper.ExcelConversionDataTable(vCatConfigPath, null));
                        }
                        else
                        {
                            _dicConfigCatFile.Add(item.Key, null);
                        }
                    }
                }
                return _dicConfigCatFile;
            }
        }


        private static Dictionary<CatType, List<CatConfigFileModel>> _dicConfigCatFileT;
        /// <summary>
        /// Cat 配置参考文档(泛型数据)
        /// </summary>
        public static Dictionary<CatType, List<CatConfigFileModel>> DicConfigCatFileT
        {
            get
            {
                if (_dicConfigCatFileT == null)
                {
                    _dicConfigCatFileT = new Dictionary<CatType, List<CatConfigFileModel>>();
                    foreach (var item in DicConfigCatFile)
                    {
                        _dicConfigCatFileT.Add(item.Key, DataProcessing.ConvertToList<CatConfigFileModel>(item.Value));
                    }
                }
                return _dicConfigCatFileT;
            }
        }
    }
}
