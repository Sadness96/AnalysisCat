using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AnalysisCat.Helper.Utils
{
    /// <summary>
    /// Excel帮助类
    /// 创建日期:2017年5月27日
    /// </summary>
    public class ExcelHelper
    {
        /// <summary>
        /// 创建Excel(Office2003)
        /// </summary>
        /// <param name="strDataSourcePath">新建Excel的路径.xls</param>
        /// <param name="strSheetName">Sheet名称,如果为空则创建三个默认Sheet页</param>
        /// <returns>成功返回true,失败返回false</returns>
        public static bool CreateExcel_Office2003(string strDataSourcePath, string strSheetName)
        {
            try
            {
                if (string.IsNullOrEmpty(strDataSourcePath))
                {
                    return false;
                }
                HSSFWorkbook WorkBook2003 = new HSSFWorkbook();
                if (string.IsNullOrEmpty(strSheetName))
                {
                    WorkBook2003.CreateSheet("Sheet1");
                    WorkBook2003.CreateSheet("Sheet2");
                    WorkBook2003.CreateSheet("Sheet3");
                }
                else
                {
                    WorkBook2003.CreateSheet(strSheetName);
                }
                FileStream fileStream2003 = new FileStream(Path.ChangeExtension(strDataSourcePath, "xls"), FileMode.Create);
                WorkBook2003.Write(fileStream2003);
                fileStream2003.Close();
                WorkBook2003.Close();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 创建Excel(Office2007)
        /// </summary>
        /// <param name="strDataSourcePath">新建Excel的路径.xlsx</param>
        /// <param name="strSheetName">Sheet名称,如果为空则创建三个默认Sheet页</param>
        /// <returns>成功返回true,失败返回false</returns>
        public static bool CreateExcel_Office2007(string strDataSourcePath, string strSheetName)
        {
            try
            {
                if (string.IsNullOrEmpty(strDataSourcePath))
                {
                    return false;
                }
                XSSFWorkbook WorkBook2007 = new XSSFWorkbook();
                if (string.IsNullOrEmpty(strSheetName))
                {
                    WorkBook2007.CreateSheet("Sheet1");
                    WorkBook2007.CreateSheet("Sheet2");
                    WorkBook2007.CreateSheet("Sheet3");
                }
                else
                {
                    WorkBook2007.CreateSheet(strSheetName);
                }
                FileStream fileStream2007 = new FileStream(Path.ChangeExtension(strDataSourcePath, "xlsx"), FileMode.Create);
                WorkBook2007.Write(fileStream2007);
                fileStream2007.Close();
                WorkBook2007.Close();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 在指定Excel中添加分页
        /// </summary>
        /// <param name="strDataSourcePath">Excel文件路径</param>
        /// <param name="strSheetName">需要添加的Sheet名称</param>
        /// <returns>成功返回true,失败返回false</returns>
        public static bool CreateExcelSheet(string strDataSourcePath, string strSheetName)
        {
            try
            {
                if (string.IsNullOrEmpty(strDataSourcePath) || string.IsNullOrEmpty(strSheetName) || !File.Exists(strDataSourcePath))
                {
                    return false;
                }
                IWorkbook iWorkBook = null;
                FileStream fileStream = new FileStream(strDataSourcePath, FileMode.Open, FileAccess.Read);
                if (System.IO.Path.GetExtension(strDataSourcePath) == ".xls")
                {
                    iWorkBook = new HSSFWorkbook(fileStream);
                    iWorkBook.CreateSheet(strSheetName);
                    FileStream fileStream2003 = new FileStream(Path.ChangeExtension(strDataSourcePath, "xls"), FileMode.Create);
                    iWorkBook.Write(fileStream2003);
                    fileStream2003.Close();
                    iWorkBook.Close();
                }
                else if (System.IO.Path.GetExtension(strDataSourcePath) == ".xlsx")
                {
                    iWorkBook = new XSSFWorkbook(fileStream);
                    iWorkBook.CreateSheet(strSheetName);
                    FileStream fileStream2007 = new FileStream(Path.ChangeExtension(strDataSourcePath, "xlsx"), FileMode.Create);
                    iWorkBook.Write(fileStream2007);
                    fileStream2007.Close();
                    iWorkBook.Close();
                }
                else
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 在指定Excel中添加分页
        /// </summary>
        /// <param name="strDataSourcePath">Excel文件路径</param>
        /// <param name="strSheetName">需要添加的Sheet名称</param>
        /// <returns>成功返回Excel工作表,失败返回null</returns>
        public static ISheet CreateExcelSheetAt(string strDataSourcePath, string strSheetName, out IWorkbook iWorkBook)
        {
            try
            {
                iWorkBook = null;
                if (string.IsNullOrEmpty(strDataSourcePath) || string.IsNullOrEmpty(strSheetName) || !File.Exists(strDataSourcePath))
                {
                    return null;
                }
                ISheet iSheet = null;
                FileStream fileStream = new FileStream(strDataSourcePath, FileMode.Open, FileAccess.Read);
                if (Path.GetExtension(strDataSourcePath) == ".xls")
                {
                    iWorkBook = new HSSFWorkbook(fileStream);
                    iSheet = iWorkBook.CreateSheet(strSheetName);
                }
                else if (Path.GetExtension(strDataSourcePath) == ".xlsx")
                {
                    iWorkBook = new XSSFWorkbook(fileStream);
                    iSheet = iWorkBook.CreateSheet(strSheetName);
                }
                else
                {
                    return null;
                }
                return iSheet;
            }
            catch (Exception ex)
            {
                iWorkBook = null;
                return null;
            }
        }

        /// <summary>
        /// 在指定Excel中删除分页(至少有一个Sheet分页文件才能打开)
        /// </summary>
        /// <param name="strDataSourcePath">Excel文件路径</param>
        /// <param name="strSheetName">需要删除的Sheet名称</param>
        /// <returns>成功返回true,失败返回false</returns>
        public static bool RemoveExcelSheet(string strDataSourcePath, string strSheetName)
        {
            try
            {
                if (string.IsNullOrEmpty(strDataSourcePath) || string.IsNullOrEmpty(strSheetName) || !File.Exists(strDataSourcePath))
                {
                    return false;
                }
                IWorkbook iWorkBook = null;
                FileStream fileStream = new FileStream(strDataSourcePath, FileMode.Open, FileAccess.Read);
                if (Path.GetExtension(strDataSourcePath) == ".xls")
                {
                    iWorkBook = new HSSFWorkbook(fileStream);
                    iWorkBook.RemoveSheetAt(iWorkBook.GetSheetIndex(strSheetName));
                    FileStream fileStream2003 = new FileStream(Path.ChangeExtension(strDataSourcePath, "xls"), FileMode.Create);
                    iWorkBook.Write(fileStream2003);
                    fileStream2003.Close();
                    iWorkBook.Close();
                }
                else if (Path.GetExtension(strDataSourcePath) == ".xlsx")
                {
                    iWorkBook = new XSSFWorkbook(fileStream);
                    iWorkBook.RemoveSheetAt(iWorkBook.GetSheetIndex(strSheetName));
                    FileStream fileStream2007 = new FileStream(Path.ChangeExtension(strDataSourcePath, "xlsx"), FileMode.Create);
                    iWorkBook.Write(fileStream2007);
                    fileStream2007.Close();
                    iWorkBook.Close();
                }
                else
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 获得指定Excel中所有Sheet
        /// </summary>
        /// <param name="strDataSourcePath">Excel文件路径</param>
        /// <returns>Excel中所有Sheet字典(序号,Sheet名)</returns>
        public static Dictionary<int, string> GetExcelAllSheet(string strDataSourcePath)
        {
            try
            {
                if (string.IsNullOrEmpty(strDataSourcePath) || !File.Exists(strDataSourcePath))
                {
                    return new Dictionary<int, string>();
                }
                Dictionary<int, string> dicAllSheet = new Dictionary<int, string>();
                IWorkbook iWorkBook = null;
                FileStream fileStream = new FileStream(strDataSourcePath, FileMode.Open, FileAccess.Read);
                if (Path.GetExtension(strDataSourcePath) == ".xls")
                {
                    iWorkBook = new HSSFWorkbook(fileStream);
                }
                else if (Path.GetExtension(strDataSourcePath) == ".xlsx")
                {
                    iWorkBook = new XSSFWorkbook(fileStream);
                }
                else
                {
                    return new Dictionary<int, string>();
                }
                for (int iNumberOfSheets = 0; iNumberOfSheets < iWorkBook.NumberOfSheets; iNumberOfSheets++)
                {
                    dicAllSheet.Add(iNumberOfSheets, iWorkBook.GetSheetName(iNumberOfSheets));
                }
                iWorkBook.Close();
                return dicAllSheet;
            }
            catch (Exception ex)
            {
                return new Dictionary<int, string>();
            }
        }

        /// <summary>
        /// 获得指定Excel中的指定Sheet页
        /// </summary>
        /// <param name="strDataSourcePath">Excel文件路径</param>
        /// <param name="strSheetName">Excel中所有Sheet名</param>
        /// <returns>成功返回Excel工作表,失败返回null</returns>
        public static ISheet GetExcelSheetAt(string strDataSourcePath, string strSheetName, out IWorkbook iWorkBook)
        {
            try
            {
                iWorkBook = null;
                if (string.IsNullOrEmpty(strDataSourcePath) || !File.Exists(strDataSourcePath) || string.IsNullOrEmpty(strSheetName))
                {
                    return null;
                }
                FileStream fileStream = new FileStream(strDataSourcePath, FileMode.Open, FileAccess.Read);
                if (Path.GetExtension(strDataSourcePath) == ".xls")
                {
                    iWorkBook = new HSSFWorkbook(fileStream);
                }
                else if (Path.GetExtension(strDataSourcePath) == ".xlsx")
                {
                    iWorkBook = new XSSFWorkbook(fileStream);
                }
                else
                {
                    return null;
                }
                return iWorkBook.GetSheet(strSheetName);
            }
            catch (Exception ex)
            {
                iWorkBook = null;
                return null;
            }
        }

        /// <summary>
        /// 获得指定Excel中的指定Sheet页
        /// </summary>
        /// <param name="strDataSourcePath">Excel文件路径</param>
        /// <param name="iNumberOfSheet">Excel中所有Sheet序号</param>
        /// <returns>成功返回Excel工作表,失败返回null</returns>
        public static ISheet GetExcelSheetAt(string strDataSourcePath, int iNumberOfSheet, out IWorkbook iWorkBook)
        {
            try
            {
                iWorkBook = null;
                if (string.IsNullOrEmpty(strDataSourcePath) || !File.Exists(strDataSourcePath) || iNumberOfSheet < 0)
                {
                    return null;
                }
                FileStream fileStream = new FileStream(strDataSourcePath, FileMode.Open, FileAccess.Read);
                if (Path.GetExtension(strDataSourcePath) == ".xls")
                {
                    iWorkBook = new HSSFWorkbook(fileStream);
                }
                else if (Path.GetExtension(strDataSourcePath) == ".xlsx")
                {
                    iWorkBook = new XSSFWorkbook(fileStream);
                }
                else
                {
                    return null;
                }
                return iWorkBook.GetSheetAt(iNumberOfSheet);
            }
            catch (Exception ex)
            {
                iWorkBook = null;
                return null;
            }
        }

        /// <summary>
        /// 在指定Excel中指定Sheet指定位置填充文本
        /// </summary>
        /// <param name="strDataSourcePath">Excel文件路径(如果文件不存在则重新创建)</param>
        /// <param name="strSheetName">需要填充的Sheet名称(如果没有则添加,如果冲突则使用冲突Sheet)</param>
        /// <param name="strTXT">需要填充的文本</param>
        /// <param name="iRows">填充行</param>
        /// <param name="iColumn">填充列</param>
        /// <returns>成功返回true,失败返回false</returns>
        public static bool FillString(string strDataSourcePath, string strSheetName, string strTXT, int iRows, int iColumn)
        {
            try
            {
                if (string.IsNullOrEmpty(strDataSourcePath) || string.IsNullOrEmpty(strSheetName) || string.IsNullOrEmpty(strTXT))
                {
                    return false;
                }
                if (File.Exists(strDataSourcePath))
                {
                    FileStream fileStream = new FileStream(strDataSourcePath, FileMode.Open, FileAccess.Read);
                    Dictionary<int, string> dicAllSheet = GetExcelAllSheet(strDataSourcePath);
                    if (Path.GetExtension(strDataSourcePath) == ".xls")
                    {
                        //获取指定Sheet页
                        IWorkbook iWorkBook = new HSSFWorkbook(fileStream);
                        ISheet iSheet = null;
                        if (dicAllSheet.ContainsValue(strSheetName))
                        {
                            iSheet = iWorkBook.GetSheet(strSheetName);
                        }
                        else
                        {
                            iSheet = iWorkBook.CreateSheet(strSheetName);
                        }
                        //获取指定单元格
                        IRow iRow = iSheet.GetRow(iRows);
                        ICell iCell = null;
                        if (iRow == null)
                        {
                            //如果没有搜索到指定行则创建单元格
                            iRow = iSheet.CreateRow(iRows);
                            iCell = iRow.CreateCell(iColumn);
                        }
                        else
                        {
                            iCell = iRow.GetCell(iColumn);
                        }
                        iCell.SetCellValue(strTXT);
                        FileStream fileStream2003 = new FileStream(Path.ChangeExtension(strDataSourcePath, "xls"), FileMode.Create);
                        iWorkBook.Write(fileStream2003);
                        fileStream2003.Close();
                        iWorkBook.Close();
                    }
                    else if (Path.GetExtension(strDataSourcePath) == ".xlsx")
                    {
                        //获取指定Sheet页
                        IWorkbook iWorkBook = new XSSFWorkbook(fileStream);
                        ISheet iSheet = null;
                        if (dicAllSheet.ContainsValue(strSheetName))
                        {
                            iSheet = iWorkBook.GetSheet(strSheetName);
                        }
                        else
                        {
                            iSheet = iWorkBook.CreateSheet(strSheetName);
                        }
                        //获取指定单元格
                        IRow iRow = iSheet.GetRow(iRows);
                        ICell iCell = null;
                        if (iRow == null)
                        {
                            //如果没有搜索到指定行则创建单元格
                            iRow = iSheet.CreateRow(iRows);
                            iCell = iRow.CreateCell(iColumn);
                        }
                        else
                        {
                            iCell = iRow.GetCell(iColumn);
                        }
                        iCell.SetCellValue(strTXT);
                        FileStream fileStream2007 = new FileStream(Path.ChangeExtension(strDataSourcePath, "xlsx"), FileMode.Create);
                        iWorkBook.Write(fileStream2007);
                        fileStream2007.Close();
                        iWorkBook.Close();
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    if (Path.GetExtension(strDataSourcePath) == ".xls")
                    {
                        bool bCreare = CreateExcel_Office2003(strDataSourcePath, strSheetName);
                        bool bFill = FillString(strDataSourcePath, strSheetName, strTXT, iColumn, iRows);
                        if (bCreare && bFill)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else if (Path.GetExtension(strDataSourcePath) == ".xlsx")
                    {
                        bool bCreare = CreateExcel_Office2007(strDataSourcePath, strSheetName);
                        bool bFill = FillString(strDataSourcePath, strSheetName, strTXT, iColumn, iRows);
                        if (bCreare && bFill)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 在指定Excel中指定Sheet指定位置填充DataTable(仅拷贝,不提供保存方法)
        /// </summary>
        /// <param name="iDataSourceSheet">指定Excel元数据Sheet页</param>
        /// <param name="dtSourceData">DataTable数据</param>
        /// <param name="WhetherThereFieldName">是否有列名(true保留DataTable字段名)</param>
        /// <param name="iRows">起始行</param>
        /// <param name="iColumn">起始列</param>
        /// <returns>成功返回拷贝后的Sheet页,失败返回null</returns>
        public static ISheet FillDataTable(ISheet iDataSourceSheet, DataTable dtSourceData, bool WhetherThereFieldName, int iRows, int iColumn)
        {
            try
            {
                if (iDataSourceSheet == null)
                {
                    return null;
                }
                if (WhetherThereFieldName)
                {
                    IRow rowDataTableField = iDataSourceSheet.CreateRow(iRows);
                    for (int iDataTableColumns = 0; iDataTableColumns < dtSourceData.Columns.Count; iDataTableColumns++)
                    {
                        ICell cellErrstatist = rowDataTableField.CreateCell(iDataTableColumns + iColumn);
                        cellErrstatist.SetCellValue(dtSourceData.Columns[iDataTableColumns].ColumnName);
                    }
                    for (int iDataTableRows = 0; iDataTableRows < dtSourceData.Rows.Count; iDataTableRows++)
                    {
                        IRow rowDataTable = iDataSourceSheet.CreateRow(iDataTableRows + iRows + 1);
                        for (int iDataTableColumns = 0; iDataTableColumns < dtSourceData.Columns.Count; iDataTableColumns++)
                        {
                            ICell cellErrstatist = rowDataTable.CreateCell(iDataTableColumns + iColumn);
                            string strSourceData = dtSourceData.Rows[iDataTableRows][iDataTableColumns].ToString();
                            Regex regexIsNumeric = new Regex(@"^(-?\d+)(\.\d+)?$");
                            if (regexIsNumeric.IsMatch(strSourceData))
                            {
                                cellErrstatist.SetCellValue(double.Parse(strSourceData));
                            }
                            else
                            {
                                cellErrstatist.SetCellValue(strSourceData);
                            }
                        }
                    }
                }
                else
                {
                    for (int iDataTableRows = 0; iDataTableRows < dtSourceData.Rows.Count; iDataTableRows++)
                    {
                        IRow rowDataTable = iDataSourceSheet.CreateRow(iDataTableRows + iRows);
                        for (int iDataTableColumns = 0; iDataTableColumns < dtSourceData.Columns.Count; iDataTableColumns++)
                        {
                            ICell cellErrstatist = rowDataTable.CreateCell(iDataTableColumns + iColumn);
                            string strSourceData = dtSourceData.Rows[iDataTableRows][iDataTableColumns].ToString();
                            Regex regexIsNumeric = new Regex(@"^(-?\d+)(\.\d+)?$");
                            if (regexIsNumeric.IsMatch(strSourceData))
                            {
                                cellErrstatist.SetCellValue(double.Parse(strSourceData));
                            }
                            else
                            {
                                cellErrstatist.SetCellValue(strSourceData);
                            }
                        }
                    }
                }
                return iDataSourceSheet;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 在指定Excel中指定Sheet指定位置填充DataTable
        /// </summary>
        /// <param name="strDataSourcePath">Excel文件路径(如果文件不存在则重新创建)</param>
        /// <param name="strSheetName">需要填充的Sheet名称(如果没有则添加,如果冲突则使用冲突Sheet)</param>
        /// <param name="dtSourceData">DataTable数据</param>
        /// <param name="WhetherThereFieldName">是否有列名(true保留DataTable字段名)</param>
        /// <param name="iRows">起始行</param>
        /// <param name="iColumn">起始列</param>
        /// <returns>成功返回true,失败返回false</returns>
        public static bool FillDataTable(string strDataSourcePath, string strSheetName, DataTable dtSourceData, bool WhetherThereFieldName, int iRows, int iColumn)
        {
            try
            {
                if (string.IsNullOrEmpty(strDataSourcePath) || string.IsNullOrEmpty(strSheetName) || dtSourceData.Rows.Count < 1)
                {
                    return false;
                }
                if (File.Exists(strDataSourcePath))
                {
                    FileStream fileStream = new FileStream(strDataSourcePath, FileMode.Open, FileAccess.Read);
                    Dictionary<int, string> dicAllSheet = GetExcelAllSheet(strDataSourcePath);
                    if (System.IO.Path.GetExtension(strDataSourcePath) == ".xls")
                    {
                        IWorkbook iWorkBook = new HSSFWorkbook(fileStream);
                        ISheet iSheet = null;
                        if (dicAllSheet.ContainsValue(strSheetName))
                        {
                            iSheet = iWorkBook.GetSheet(strSheetName);
                        }
                        else
                        {
                            iSheet = iWorkBook.CreateSheet(strSheetName);
                        }
                        if (WhetherThereFieldName)
                        {
                            IRow rowDataTableField = iSheet.CreateRow(iRows);
                            for (int iDataTableColumns = 0; iDataTableColumns < dtSourceData.Columns.Count; iDataTableColumns++)
                            {
                                ICell cellErrstatist = rowDataTableField.CreateCell(iDataTableColumns + iColumn);
                                cellErrstatist.SetCellValue(dtSourceData.Columns[iDataTableColumns].ColumnName);
                            }
                            for (int iDataTableRows = 0; iDataTableRows < dtSourceData.Rows.Count; iDataTableRows++)
                            {
                                IRow rowDataTable = iSheet.CreateRow(iDataTableRows + iRows + 1);
                                for (int iDataTableColumns = 0; iDataTableColumns < dtSourceData.Columns.Count; iDataTableColumns++)
                                {
                                    ICell cellErrstatist = rowDataTable.CreateCell(iDataTableColumns + iColumn);
                                    string strSourceData = dtSourceData.Rows[iDataTableRows][iDataTableColumns].ToString();
                                    Regex regexIsNumeric = new Regex(@"^(-?\d+)(\.\d+)?$");
                                    if (regexIsNumeric.IsMatch(strSourceData))
                                    {
                                        cellErrstatist.SetCellValue(double.Parse(strSourceData));
                                    }
                                    else
                                    {
                                        cellErrstatist.SetCellValue(strSourceData);
                                    }
                                }
                            }
                        }
                        else
                        {
                            for (int iDataTableRows = 0; iDataTableRows < dtSourceData.Rows.Count; iDataTableRows++)
                            {
                                IRow rowDataTable = iSheet.CreateRow(iDataTableRows + iRows);
                                for (int iDataTableColumns = 0; iDataTableColumns < dtSourceData.Columns.Count; iDataTableColumns++)
                                {
                                    ICell cellErrstatist = rowDataTable.CreateCell(iDataTableColumns + iColumn);
                                    string strSourceData = dtSourceData.Rows[iDataTableRows][iDataTableColumns].ToString();
                                    Regex regexIsNumeric = new Regex(@"^(-?\d+)(\.\d+)?$");
                                    if (regexIsNumeric.IsMatch(strSourceData))
                                    {
                                        cellErrstatist.SetCellValue(double.Parse(strSourceData));
                                    }
                                    else
                                    {
                                        cellErrstatist.SetCellValue(strSourceData);
                                    }
                                }
                            }
                        }
                        FileStream fileStream2003 = new FileStream(Path.ChangeExtension(strDataSourcePath, "xls"), FileMode.Create);
                        iWorkBook.Write(fileStream2003);
                        fileStream2003.Close();
                        iWorkBook.Close();
                    }
                    else if (System.IO.Path.GetExtension(strDataSourcePath) == ".xlsx")
                    {
                        IWorkbook iWorkBook = new XSSFWorkbook(fileStream);
                        ISheet iSheet = null;
                        if (dicAllSheet.ContainsValue(strSheetName))
                        {
                            iSheet = iWorkBook.GetSheet(strSheetName);
                        }
                        else
                        {
                            iSheet = iWorkBook.CreateSheet(strSheetName);
                        }
                        if (WhetherThereFieldName)
                        {
                            IRow rowDataTableField = iSheet.CreateRow(iRows);
                            for (int iDataTableColumns = 0; iDataTableColumns < dtSourceData.Columns.Count; iDataTableColumns++)
                            {
                                ICell cellErrstatist = rowDataTableField.CreateCell(iDataTableColumns + iColumn);
                                cellErrstatist.SetCellValue(dtSourceData.Columns[iDataTableColumns].ColumnName);
                            }
                            for (int iDataTableRows = 0; iDataTableRows < dtSourceData.Rows.Count; iDataTableRows++)
                            {
                                IRow rowDataTable = iSheet.CreateRow(iDataTableRows + iRows + 1);
                                for (int iDataTableColumns = 0; iDataTableColumns < dtSourceData.Columns.Count; iDataTableColumns++)
                                {
                                    ICell cellErrstatist = rowDataTable.CreateCell(iDataTableColumns + iColumn);
                                    string strSourceData = dtSourceData.Rows[iDataTableRows][iDataTableColumns].ToString();
                                    Regex regexIsNumeric = new Regex(@"^(-?\d+)(\.\d+)?$");
                                    if (regexIsNumeric.IsMatch(strSourceData))
                                    {
                                        cellErrstatist.SetCellValue(double.Parse(strSourceData));
                                    }
                                    else
                                    {
                                        cellErrstatist.SetCellValue(strSourceData);
                                    }
                                }
                            }
                        }
                        else
                        {
                            for (int iDataTableRows = 0; iDataTableRows < dtSourceData.Rows.Count; iDataTableRows++)
                            {
                                IRow rowDataTable = iSheet.CreateRow(iDataTableRows + iRows);
                                for (int iDataTableColumns = 0; iDataTableColumns < dtSourceData.Columns.Count; iDataTableColumns++)
                                {
                                    ICell cellErrstatist = rowDataTable.CreateCell(iDataTableColumns + iColumn);
                                    string strSourceData = dtSourceData.Rows[iDataTableRows][iDataTableColumns].ToString();
                                    Regex regexIsNumeric = new Regex(@"^(-?\d+)(\.\d+)?$");
                                    if (regexIsNumeric.IsMatch(strSourceData))
                                    {
                                        cellErrstatist.SetCellValue(double.Parse(strSourceData));
                                    }
                                    else
                                    {
                                        cellErrstatist.SetCellValue(strSourceData);
                                    }
                                }
                            }
                        }
                        FileStream fileStream2007 = new FileStream(Path.ChangeExtension(strDataSourcePath, "xlsx"), FileMode.Create);
                        iWorkBook.Write(fileStream2007);
                        fileStream2007.Close();
                        iWorkBook.Close();
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    if (Path.GetExtension(strDataSourcePath) == ".xls")
                    {
                        bool bCreare = CreateExcel_Office2003(strDataSourcePath, strSheetName);
                        bool bFill = FillDataTable(strDataSourcePath, strSheetName, dtSourceData, WhetherThereFieldName, iColumn, iRows);
                        if (bCreare && bFill)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else if (Path.GetExtension(strDataSourcePath) == ".xlsx")
                    {
                        bool bCreare = CreateExcel_Office2007(strDataSourcePath, strSheetName);
                        bool bFill = FillDataTable(strDataSourcePath, strSheetName, dtSourceData, WhetherThereFieldName, iColumn, iRows);
                        if (bCreare && bFill)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// DataTable转换为Excel
        /// 存在文件则新建DataTableName的分页(如果分页名冲突则或为空则使用默认名称)
        /// 不存在文件则新建(Excel,名称为DataTableName,如果没有则使用默认名称)
        /// </summary>
        /// <param name="strDataSourcePath">Excel文件路径</param>
        /// <param name="dtSourceData">DataTable数据</param>
        /// <returns>成功返回true,失败返回false</returns>
        public static bool DataTableConversionExcel(string strDataSourcePath, DataTable dtSourceData)
        {
            try
            {
                if (string.IsNullOrEmpty(strDataSourcePath) || dtSourceData.Rows.Count < 1)
                {
                    return false;
                }
                Dictionary<int, string> dicAllSheet = GetExcelAllSheet(strDataSourcePath);
                string strTableName = string.IsNullOrEmpty(dtSourceData.TableName) ? string.Format("Sheet{0}", dicAllSheet.Count + 1) : dtSourceData.TableName;
                if (dicAllSheet.ContainsValue(dtSourceData.TableName))
                {
                    RemoveExcelSheet(strDataSourcePath, dtSourceData.TableName);
                }
                if (FillDataTable(strDataSourcePath, strTableName, dtSourceData, true, 0, 0))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// DataSet转换为Excel
        /// 存在文件则新建DataTableName的分页(如果分页名冲突则或为空则使用默认名称)
        /// 不存在文件则新建(Excel,名称为DataTableName,如果没有则使用默认名称)
        /// </summary>
        /// <param name="strDataSourcePath">Excel文件路径</param>
        /// <param name="dsSourceData">DataTable数据</param>
        /// <returns>成功返回true,失败返回false</returns>
        public static bool DataSetConversionExcel(string strDataSourcePath, DataSet dsSourceData)
        {
            try
            {
                if (string.IsNullOrEmpty(strDataSourcePath) || dsSourceData.Tables.Count < 1)
                {
                    return false;
                }
                foreach (DataTable dtSourceData in dsSourceData.Tables)
                {
                    Dictionary<int, string> dicAllSheet = GetExcelAllSheet(strDataSourcePath);
                    string strTableName = string.IsNullOrEmpty(dtSourceData.TableName) ? string.Format("Sheet{0}", dicAllSheet.Count + 1) : dtSourceData.TableName;
                    if (dicAllSheet.ContainsValue(dtSourceData.TableName))
                    {
                        RemoveExcelSheet(strDataSourcePath, dtSourceData.TableName);
                    }
                    if (!FillDataTable(strDataSourcePath, strTableName, dtSourceData, true, 0, 0))
                    {
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 获得指定Excel指定分页指定起始终止位置的DataTable
        /// </summary>
        /// <param name="iDataSourceSheet">指定Excel元数据Sheet页</param>
        /// <param name="WhetherThereFieldName">是否有列名(true保留DataTable字段名)</param>
        /// <param name="iStartRows">起始行</param>
        /// <param name="iStartColumn">起始列</param>
        /// <param name="iStopRows">终止行(如果小于等于0则默认Length)</param>
        /// <param name="iStopColumn">终止列(如果小于等于0则默认Length)</param>
        /// <returns>成功返回Excel的DataTable,失败返回NULL</returns>
        public static DataTable GetDataTable(ISheet iDataSourceSheet, bool WhetherThereFieldName, int iStartRows, int iStartColumn, int iStopRows, int iStopColumn)
        {
            try
            {
                if (iDataSourceSheet == null)
                {
                    return null;
                }
                DataTable dtTargetData = new DataTable();
                if (WhetherThereFieldName)
                {
                    //构建DataTable列(第一行为列名)
                    IRow iRowFirst = iDataSourceSheet.GetRow(iStartRows);
                    for (int iFirst = iStartColumn; iFirst <= (iStopColumn <= 0 ? (iRowFirst.LastCellNum) : iStopColumn); iFirst++)
                    {
                        ICell iCell = iRowFirst.GetCell(iFirst);
                        if (iCell != null)
                        {
                            if (iCell.StringCellValue != null)
                            {
                                DataColumn dColumn = new DataColumn(iCell.StringCellValue);
                                dtTargetData.Columns.Add(dColumn);
                            }
                        }
                    }
                    //构建DataTable行(第二行往下为数据)
                    for (int iRowNum = iStartRows + 1; iRowNum <= (iStopRows <= 0 ? iDataSourceSheet.LastRowNum : iStopRows); iRowNum++)
                    {
                        IRow iRowData = iDataSourceSheet.GetRow(iRowNum);
                        if (iRowData == null) continue;
                        DataRow drTargetData = dtTargetData.NewRow();
                        for (int iRowCell = iStartColumn; iRowCell <= (iStopColumn <= 0 ? (iRowFirst.LastCellNum) : iStopColumn); iRowCell++)
                        {
                            ICell iCell = iRowData.GetCell(iRowCell);
                            if (iCell != null)
                            {
                                iCell.SetCellType(CellType.String);
                                if (iCell.StringCellValue != null)
                                {
                                    int iNumberIsColums = iRowCell - iStartColumn;
                                    if (iNumberIsColums < dtTargetData.Columns.Count)
                                    {
                                        DataColumn dColumn = new DataColumn(iCell.StringCellValue);
                                        drTargetData[iNumberIsColums] = dColumn;
                                    }
                                }
                            }
                        }
                        dtTargetData.Rows.Add(drTargetData);
                    }
                }
                else
                {
                    //构建DataTable列,以读取第一行的长度填充列名(使用默认命名初始化列名Column1)
                    IRow iRowFirst = iDataSourceSheet.GetRow(iStartRows);
                    for (int iFirst = iStartColumn; iFirst <= (iStopColumn <= 0 ? (iRowFirst.LastCellNum) : iStopColumn); iFirst++)
                    {
                        ICell iCell = iRowFirst.GetCell(iFirst);
                        if (iCell != null)
                        {
                            if (iCell.StringCellValue != null)
                            {
                                dtTargetData.Columns.Add(string.Format("Column{0}", iFirst - iStartColumn));
                            }
                        }
                    }
                    //构建DataTable行(第一行往下为数据)
                    for (int iRowNum = iStartRows; iRowNum <= (iStopRows <= 0 ? iDataSourceSheet.LastRowNum : iStopRows); iRowNum++)
                    {
                        IRow iRowData = iDataSourceSheet.GetRow(iRowNum);
                        if (iRowData == null) continue;
                        DataRow drTargetData = dtTargetData.NewRow();
                        for (int iRowCell = iStartColumn; iRowCell <= (iStopColumn <= 0 ? (iRowFirst.LastCellNum) : iStopColumn); iRowCell++)
                        {
                            ICell iCell = iRowData.GetCell(iRowCell);
                            if (iCell != null)
                            {
                                iCell.SetCellType(CellType.String);
                                if (iCell.StringCellValue != null)
                                {
                                    int iNumberIsColums = iRowCell - iStartColumn;
                                    if (iNumberIsColums < dtTargetData.Columns.Count)
                                    {
                                        DataColumn dColumn = new DataColumn(iCell.StringCellValue);
                                        drTargetData[iNumberIsColums] = dColumn;
                                    }
                                }
                            }
                        }
                        dtTargetData.Rows.Add(drTargetData);
                    }
                }
                return dtTargetData;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 获得指定Excel指定分页指定起始终止位置的DataTable
        /// </summary>
        /// <param name="strDataSourcePath">Excel文件路径</param>
        /// <param name="strSheetName">分页Sheet名称</param>
        /// <param name="WhetherThereFieldName">是否有列名(true保留DataTable字段名)</param>
        /// <param name="iStartRows">起始行</param>
        /// <param name="iStartColumn">起始列</param>
        /// <param name="iStopRows">终止行(如果小于等于0则默认Length)</param>
        /// <param name="iStopColumn">终止列(如果小于等于0则默认Length)</param>
        /// <returns>成功返回Excel的DataTable,失败返回NULL</returns>
        public static DataTable GetDataTable(string strDataSourcePath, string strSheetName, bool WhetherThereFieldName, int iStartRows, int iStartColumn, int iStopRows, int iStopColumn)
        {
            try
            {
                if (string.IsNullOrEmpty(strDataSourcePath) || !File.Exists(strDataSourcePath))
                {
                    return null;
                }
                DataTable dtTargetData = new DataTable();
                FileStream fileStream = new FileStream(strDataSourcePath, FileMode.Open, FileAccess.Read);
                IWorkbook iWorkBook = null;
                if (Path.GetExtension(strDataSourcePath) == ".xls")
                {
                    iWorkBook = new HSSFWorkbook(fileStream);
                }
                else if (Path.GetExtension(strDataSourcePath) == ".xlsx")
                {
                    iWorkBook = new XSSFWorkbook(fileStream);
                }
                ISheet iSheet = null;
                if (string.IsNullOrEmpty(strSheetName))
                {
                    Dictionary<int, string> dicAllSheet = GetExcelAllSheet(strDataSourcePath);
                    iSheet = iWorkBook.GetSheet(dicAllSheet[0]);
                    dtTargetData.TableName = dicAllSheet[0];
                }
                else
                {
                    iSheet = iWorkBook.GetSheet(strSheetName);
                    dtTargetData.TableName = strSheetName;
                }
                if (WhetherThereFieldName)
                {
                    //构建DataTable列(第一行为列名)
                    IRow iRowFirst = iSheet.GetRow(iStartRows);
                    for (int iFirst = iStartColumn; iFirst <= (iStopColumn <= 0 ? (iRowFirst.LastCellNum) : iStopColumn); iFirst++)
                    {
                        ICell iCell = iRowFirst.GetCell(iFirst);
                        if (iCell != null)
                        {
                            if (iCell.StringCellValue != null)
                            {
                                DataColumn dColumn = new DataColumn(iCell.StringCellValue);
                                dtTargetData.Columns.Add(dColumn);
                            }
                        }
                    }
                    //构建DataTable行(第二行往下为数据)
                    for (int iRowNum = iStartRows + 1; iRowNum <= (iStopRows <= 0 ? iSheet.LastRowNum : iStopRows); iRowNum++)
                    {
                        IRow iRowData = iSheet.GetRow(iRowNum);
                        if (iRowData == null) continue;
                        DataRow drTargetData = dtTargetData.NewRow();
                        for (int iRowCell = iStartColumn; iRowCell <= (iStopColumn <= 0 ? (iRowFirst.LastCellNum) : iStopColumn); iRowCell++)
                        {
                            ICell iCell = iRowData.GetCell(iRowCell);
                            if (iCell != null)
                            {
                                iCell.SetCellType(CellType.String);
                                if (iCell.StringCellValue != null)
                                {
                                    int iNumberIsColums = iRowCell - iStartColumn;
                                    if (iNumberIsColums < dtTargetData.Columns.Count)
                                    {
                                        DataColumn dColumn = new DataColumn(iCell.StringCellValue);
                                        drTargetData[iNumberIsColums] = dColumn;
                                    }
                                }
                            }
                        }
                        dtTargetData.Rows.Add(drTargetData);
                    }
                }
                else
                {
                    //构建DataTable列,以读取第一行的长度填充列名(使用默认命名初始化列名Column1)
                    IRow iRowFirst = iSheet.GetRow(iStartRows);
                    for (int iFirst = iStartColumn; iFirst <= (iStopColumn <= 0 ? (iRowFirst.LastCellNum) : iStopColumn); iFirst++)
                    {
                        ICell iCell = iRowFirst.GetCell(iFirst);
                        if (iCell != null)
                        {
                            if (iCell.StringCellValue != null)
                            {
                                dtTargetData.Columns.Add(string.Format("Column{0}", iFirst - iStartColumn));
                            }
                        }
                    }
                    //构建DataTable行(第一行往下为数据)
                    for (int iRowNum = iStartRows; iRowNum <= (iStopRows <= 0 ? iSheet.LastRowNum : iStopRows); iRowNum++)
                    {
                        IRow iRowData = iSheet.GetRow(iRowNum);
                        if (iRowData == null) continue;
                        DataRow drTargetData = dtTargetData.NewRow();
                        for (int iRowCell = iStartColumn; iRowCell <= (iStopColumn <= 0 ? (iRowFirst.LastCellNum) : iStopColumn); iRowCell++)
                        {
                            ICell iCell = iRowData.GetCell(iRowCell);
                            if (iCell != null)
                            {
                                iCell.SetCellType(CellType.String);
                                if (iCell.StringCellValue != null)
                                {
                                    int iNumberIsColums = iRowCell - iStartColumn;
                                    if (iNumberIsColums < dtTargetData.Columns.Count)
                                    {
                                        DataColumn dColumn = new DataColumn(iCell.StringCellValue);
                                        drTargetData[iNumberIsColums] = dColumn;
                                    }
                                }
                            }
                        }
                        dtTargetData.Rows.Add(drTargetData);
                    }
                }
                return dtTargetData;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Excel指定分页转换为DataTable(如果分页为空,默认第一个分页)
        /// </summary>
        /// <param name="strDataSourcePath">Excel文件路径</param>
        /// <param name="strSheetName">分页Sheet名称</param>
        /// <returns>成功返回Excel的DataTable,失败返回NULL</returns>
        public static DataTable ExcelConversionDataTable(string strDataSourcePath, string strSheetName)
        {
            try
            {
                if (string.IsNullOrEmpty(strDataSourcePath) || !File.Exists(strDataSourcePath))
                {
                    return null;
                }
                return GetDataTable(strDataSourcePath, strSheetName, true, 0, 0, 0, 0);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Excel所有分页转换为DataSet
        /// </summary>
        /// <param name="strDataSourcePath">Excel文件路径</param>
        /// <returns>成功返回Excel的DataSet,失败返回NULL</returns>
        public static DataSet ExcelConversionDataSet(string strDataSourcePath)
        {
            try
            {
                if (string.IsNullOrEmpty(strDataSourcePath) || !File.Exists(strDataSourcePath))
                {
                    return null;
                }
                DataSet dsTargetData = new DataSet();
                Dictionary<int, string> dicAllSheet = GetExcelAllSheet(strDataSourcePath);
                foreach (var vAllSheet in dicAllSheet)
                {
                    DataTable dtTargetData = new DataTable();
                    dtTargetData.TableName = vAllSheet.Value;
                    dtTargetData = ExcelConversionDataTable(strDataSourcePath, vAllSheet.Value);
                    if (dtTargetData == null)
                    {
                        continue;
                    }
                    dsTargetData.Tables.Add(dtTargetData);
                }
                return dsTargetData;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 拷贝Sheet页到另一个Sheet页(浅拷贝,不提供保存方法)
        /// Office2003单Sheet页仅支持4000个样式
        /// </summary>
        /// <param name="iSourceWorkbook">源Excel工作簿</param>
        /// <param name="iFromSheet">源Sheet页</param>
        /// <param name="iTargetWorkbook">目标Excel工作簿</param>
        /// <param name="iToSheet">目标Sheet页</param>
        /// <returns>成功返回true,失败返回false</returns>
        public static bool CopySheetAt(IWorkbook iSourceWorkbook, ISheet iFromSheet, IWorkbook iTargetWorkbook, ISheet iToSheet)
        {
            try
            {
                //拷贝数据
                DataTable dtExcelFromData = GetDataTable(iFromSheet, false, 0, 0, 0, 0);
                iToSheet = FillDataTable(iToSheet, dtExcelFromData, false, 0, 0);
                //拷贝单元格合并
                for (int iMergedRegions = 0; iMergedRegions < iFromSheet.NumMergedRegions; iMergedRegions++)
                {
                    iToSheet.AddMergedRegion(iFromSheet.GetMergedRegion(iMergedRegions));
                }
                //拷贝样式(遍历Sheet页行)
                List<ICellStyle> listCellStyle = new List<ICellStyle>();
                for (int iRowNum = 0; iRowNum <= iFromSheet.LastRowNum; iRowNum++)
                {
                    IRow iFromRowData = iFromSheet.GetRow(iRowNum);
                    IRow iToRowData = iToSheet.GetRow(iRowNum);
                    if (iFromRowData == null || iToRowData == null)
                    {
                        continue;
                    }
                    //设置行高
                    short sFromHeight = iFromRowData.Height;
                    iToRowData.Height = sFromHeight;
                    //遍历Sheet页列
                    for (int iRowCell = 0; iRowCell <= iFromRowData.LastCellNum; iRowCell++)
                    {
                        //设置列宽
                        int iFromColumnWidth = iFromSheet.GetColumnWidth(iRowNum) / 256;
                        iToSheet.SetColumnWidth(iRowNum, iFromColumnWidth * 256);
                        //复制数据
                        ICell iFromCell = iFromRowData.GetCell(iRowCell);
                        if (iFromCell != null)
                        {
                            //获得源Sheet页的样式
                            ICellStyle iFromCellStyle = iFromCell.CellStyle;
                            //获得目标Excel指定Cell
                            ICell iToCell = iToRowData.GetCell(iRowCell);
                            if (iToCell == null) continue;
                            #region 复制单元格样式
                            //指定Cell创新目标Excel工作簿新样式
                            ICellStyle iToNewCellStyle = null;
                            foreach (ICellStyle vCellStyle in listCellStyle)
                            {
                                IFont iVToFont = vCellStyle.GetFont(iTargetWorkbook);
                                IFont iFromFont = iFromCellStyle.GetFont(iSourceWorkbook);
                                if (vCellStyle.Alignment == iFromCellStyle.Alignment &&
                                    vCellStyle.BorderBottom == iFromCellStyle.BorderBottom &&
                                    vCellStyle.BorderLeft == iFromCellStyle.BorderLeft &&
                                    vCellStyle.BorderRight == iFromCellStyle.BorderRight &&
                                    vCellStyle.BorderTop == iFromCellStyle.BorderTop &&
                                    vCellStyle.BottomBorderColor == iFromCellStyle.BottomBorderColor &&
                                    vCellStyle.DataFormat == iFromCellStyle.DataFormat &&
                                    vCellStyle.FillBackgroundColor == iFromCellStyle.FillBackgroundColor &&
                                    vCellStyle.FillForegroundColor == iFromCellStyle.FillForegroundColor &&
                                    vCellStyle.FillPattern == iFromCellStyle.FillPattern &&
                                    vCellStyle.Indention == iFromCellStyle.Indention &&
                                    vCellStyle.IsHidden == iFromCellStyle.IsHidden &&
                                    vCellStyle.IsLocked == iFromCellStyle.IsLocked &&
                                    vCellStyle.LeftBorderColor == iFromCellStyle.LeftBorderColor &&
                                    vCellStyle.RightBorderColor == iFromCellStyle.RightBorderColor &&
                                    vCellStyle.Rotation == iFromCellStyle.Rotation &&
                                    vCellStyle.TopBorderColor == iFromCellStyle.TopBorderColor &&
                                    vCellStyle.VerticalAlignment == iFromCellStyle.VerticalAlignment &&
                                    vCellStyle.WrapText == iFromCellStyle.WrapText &&
                                    //字体比对
                                    iVToFont.Color == iFromFont.Color &&
                                    iVToFont.FontHeightInPoints == iFromFont.FontHeightInPoints &&
                                    iVToFont.FontName == iFromFont.FontName &&
                                    iVToFont.IsBold == iFromFont.IsBold &&
                                    iVToFont.IsItalic == iFromFont.IsItalic &&
                                    iVToFont.IsStrikeout == iFromFont.IsStrikeout &&
                                    iVToFont.Underline == iFromFont.Underline)
                                {
                                    iToNewCellStyle = vCellStyle;
                                    break;
                                }
                            }
                            if (iToNewCellStyle == null)
                            {
                                //创建新样式
                                iToNewCellStyle = iTargetWorkbook.CreateCellStyle();
                                //复制样式
                                iToNewCellStyle.Alignment = iFromCellStyle.Alignment;//对齐
                                iToNewCellStyle.BorderBottom = iFromCellStyle.BorderBottom;//下边框
                                iToNewCellStyle.BorderLeft = iFromCellStyle.BorderLeft;//左边框
                                iToNewCellStyle.BorderRight = iFromCellStyle.BorderRight;//右边框
                                iToNewCellStyle.BorderTop = iFromCellStyle.BorderTop;//上边框
                                iToNewCellStyle.BottomBorderColor = iFromCellStyle.BottomBorderColor;//下边框颜色
                                iToNewCellStyle.DataFormat = iFromCellStyle.DataFormat;//数据格式
                                iToNewCellStyle.FillBackgroundColor = iFromCellStyle.FillBackgroundColor;//填充背景色
                                iToNewCellStyle.FillForegroundColor = iFromCellStyle.FillForegroundColor;//填充前景色
                                iToNewCellStyle.FillPattern = iFromCellStyle.FillPattern;//填充图案
                                iToNewCellStyle.Indention = iFromCellStyle.Indention;//压痕
                                iToNewCellStyle.IsHidden = iFromCellStyle.IsHidden;//隐藏
                                iToNewCellStyle.IsLocked = iFromCellStyle.IsLocked;//锁定
                                iToNewCellStyle.LeftBorderColor = iFromCellStyle.LeftBorderColor;//左边框颜色
                                iToNewCellStyle.RightBorderColor = iFromCellStyle.RightBorderColor;//右边框颜色
                                iToNewCellStyle.Rotation = iFromCellStyle.Rotation;//旋转
                                iToNewCellStyle.TopBorderColor = iFromCellStyle.TopBorderColor;//上边框颜色
                                iToNewCellStyle.VerticalAlignment = iFromCellStyle.VerticalAlignment;//垂直对齐
                                iToNewCellStyle.WrapText = iFromCellStyle.WrapText;//文字换行
                                //复制字体
                                IFont iFromFont = iFromCellStyle.GetFont(iSourceWorkbook);
                                IFont iToFont = iTargetWorkbook.CreateFont();
                                iToFont.Color = iFromFont.Color;//颜色
                                iToFont.FontHeightInPoints = iFromFont.FontHeightInPoints;//字号
                                iToFont.FontName = iFromFont.FontName;//字体
                                iToFont.IsBold = iFromFont.IsBold;//加粗
                                iToFont.IsItalic = iFromFont.IsItalic;//斜体
                                iToFont.IsStrikeout = iFromFont.IsStrikeout;//删除线
                                iToFont.Underline = iFromFont.Underline;//下划线
                                iToNewCellStyle.SetFont(iToFont);
                                //保存到缓存集合中
                                listCellStyle.Add(iToNewCellStyle);
                            }
                            //复制样式到指定表格中
                            iToCell.CellStyle = iToNewCellStyle;
                            #endregion
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 拷贝Sheet页到另一个Sheet页
        /// </summary>
        /// <param name="strSourceExcelPath">源Excel路径</param>
        /// <param name="strFromSheetName">源Excel拷贝Sheet</param>
        /// <param name="strTargetExcelPath">目标Excel路径</param>
        /// <param name="strToSheetName">目标Excel拷贝Sheet</param>
        /// <returns>成功返回true,失败返回false</returns>
        public static bool CopySheet(string strSourceExcelPath, string strFromSheetName, string strTargetExcelPath, string strToSheetName)
        {
            try
            {
                if (string.IsNullOrEmpty(strSourceExcelPath) || string.IsNullOrEmpty(strTargetExcelPath) || !File.Exists(strSourceExcelPath))
                {
                    //TXTHelper.Logs(string.Format("源数据和目标数据参数为空或文件不存在!"));
                    return false;
                }
                if (string.IsNullOrEmpty(strFromSheetName) || string.IsNullOrEmpty(strToSheetName))
                {
                    //TXTHelper.Logs(string.Format("源Sheet页和目标Sheet页参数为空!"));
                    return false;
                }
                //获得源数据和目标数据的Sheet页
                IWorkbook iSourceWorkbook = null;
                ISheet iSourceSheet = GetExcelSheetAt(strSourceExcelPath, strFromSheetName, out iSourceWorkbook);
                IWorkbook iTargetWorkbook = null;
                ISheet iTargetSheet = null;
                if (iSourceSheet == null)
                {
                    //TXTHelper.Logs(string.Format("指定源数据Sheet页为空!"));
                    return false;
                }
                if (!File.Exists(strTargetExcelPath))
                {
                    //如果文件不存在则创建Excel
                    if (Path.GetExtension(strTargetExcelPath) == ".xls")
                    {
                        bool bCreare = CreateExcel_Office2003(strTargetExcelPath, strToSheetName);
                    }
                    else if (Path.GetExtension(strTargetExcelPath) == ".xlsx")
                    {
                        bool bCreare = CreateExcel_Office2007(strTargetExcelPath, strToSheetName);
                    }
                    else
                    {
                        //TXTHelper.Logs(string.Format("指定目标Excel文件路径格式错误!"));
                        return false;
                    }
                    iTargetSheet = GetExcelSheetAt(strTargetExcelPath, strToSheetName, out iTargetWorkbook);
                }
                else
                {
                    //如果文件存在则判断是否存在执行Sheet
                    Dictionary<int, string> dicAllSheet = GetExcelAllSheet(strTargetExcelPath);
                    if (dicAllSheet.ContainsValue(strToSheetName))
                    {
                        iTargetSheet = GetExcelSheetAt(strTargetExcelPath, strToSheetName, out iTargetWorkbook);
                    }
                    else
                    {
                        iTargetSheet = CreateExcelSheetAt(strTargetExcelPath, strToSheetName, out iTargetWorkbook);
                    }
                }
                //调用Sheet拷贝Sheet方法
                bool bCopySheet = CopySheetAt(iSourceWorkbook, iSourceSheet, iTargetWorkbook, iTargetSheet);
                if (bCopySheet)
                {
                    if (Path.GetExtension(strTargetExcelPath) == ".xls")
                    {
                        FileStream fileStream2003 = new FileStream(Path.ChangeExtension(strTargetExcelPath, "xls"), FileMode.Create);
                        iTargetWorkbook.Write(fileStream2003);
                        fileStream2003.Close();
                        iTargetWorkbook.Close();
                    }
                    else if (Path.GetExtension(strTargetExcelPath) == ".xlsx")
                    {
                        FileStream fileStream2007 = new FileStream(Path.ChangeExtension(strTargetExcelPath, "xlsx"), FileMode.Create);
                        iTargetWorkbook.Write(fileStream2007);
                        fileStream2007.Close();
                        iTargetWorkbook.Close();
                    }
                    return true;
                }
                else
                {
                    //TXTHelper.Logs(string.Format("拷贝失败!"));
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
