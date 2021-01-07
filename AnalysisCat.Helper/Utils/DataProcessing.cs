using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AnalysisCat.Helper.Utils
{
    /// <summary>
    /// 数据处理类
    /// 创建日期:2016年12月22日
    /// </summary>
    public class DataProcessing
    {
        /// <summary>
        /// 对DataTable进行筛选过滤
        /// </summary>
        /// <param name="dtDataSource">源数据(DataTable)</param>
        /// <param name="sqlFilterConditions">过滤条件(WHERE语句)</param>
        /// <returns>筛选过滤后的DataTable</returns>
        public static DataTable DataTableFiltered(DataTable dtDataSource, string sqlFilterConditions)
        {
            DataTable dtDataResulting = new DataTable();
            try
            {
                if (dtDataSource.Rows.Count < 1 || string.IsNullOrEmpty(sqlFilterConditions)) return null;
                dtDataResulting = dtDataSource.Select(sqlFilterConditions).CopyToDataTable();
            }
            catch (Exception ex)
            {
            }
            return dtDataResulting;
        }

        /// <summary>
        /// 删除DataTable中的空行
        /// 弱引用,可直接修改参数
        /// </summary>
        /// <param name="dtDataSource">源数据(DataTable)</param>
        /// <returns>删除空行后的DataTable</returns>
        public static DataTable RemoveEmpty(DataTable dtDataSource)
        {
            try
            {
                List<DataRow> listRemove = new List<DataRow>();
                for (int i = 0; i < dtDataSource.Rows.Count; i++)
                {
                    bool IsNull = true;
                    for (int j = 0; j < dtDataSource.Columns.Count; j++)
                    {
                        if (!string.IsNullOrEmpty(dtDataSource.Rows[i][j].ToString().Trim()))
                        {
                            IsNull = false;
                        }
                    }
                    if (IsNull)
                    {
                        listRemove.Add(dtDataSource.Rows[i]);
                    }
                }
                for (int i = 0; i < listRemove.Count; i++)
                {
                    dtDataSource.Rows.Remove(listRemove[i]);
                }
            }
            catch (Exception ex)
            {
            }
            return dtDataSource;
        }

        /// <summary>
        /// DataTable转换为List
        /// </summary>
        /// <typeparam name="T">数据模型</typeparam>
        /// <param name="dtDataSource">源数据(DataTable)</param>
        /// <returns>成功返回List,失败返回null</returns>
        public static List<T> ConvertToList<T>(DataTable dtDataSource) where T : class, new()
        {
            try
            {
                List<T> listT = new List<T>();
                foreach (DataRow drDataSource in dtDataSource.Rows)
                {
                    T t = new T();
                    var propertyInfos = t.GetType().GetProperties();
                    foreach (PropertyInfo propertyInfo in propertyInfos)
                    {
                        string tempName = propertyInfo.Name;
                        if (dtDataSource.Columns.Contains(tempName))
                        {
                            if (!propertyInfo.CanWrite) continue;
                            object value = drDataSource[tempName];
                            if (value != DBNull.Value)
                            {
                                if (propertyInfo.GetMethod.ReturnParameter.ParameterType.Name == "Int32")
                                {
                                    value = Convert.ToInt32(value);
                                }
                                propertyInfo.SetValue(t, value, null);
                            }
                        }
                    }
                    listT.Add(t);
                }
                return listT;
            }
            catch (Exception ex)
            {
            }
            return null;
        }

        /// <summary>
        /// List转换为DataTable
        /// </summary>
        /// <param name="listDataSource">源数据</param>
        /// <returns>成功返回DataTable,失败返回null</returns>
        public static DataTable ConvertDataTable(IList listDataSource)
        {
            try
            {
                DataTable dataTable = new DataTable();
                if (listDataSource.Count > 0)
                {
                    PropertyInfo[] propertyInfos = listDataSource[0].GetType().GetProperties();
                    foreach (PropertyInfo propertyInfo in propertyInfos)
                    {
                        dataTable.Columns.Add(propertyInfo.Name, propertyInfo.PropertyType);
                    }
                    foreach (var vDataSource in listDataSource)
                    {
                        ArrayList arrayList = new ArrayList();
                        foreach (PropertyInfo propertyInfo in propertyInfos)
                        {
                            arrayList.Add(propertyInfo.GetValue(vDataSource, null));
                        }
                        dataTable.LoadDataRow(arrayList.ToArray(), true);
                    }
                }
                return dataTable;
            }
            catch (Exception ex)
            {
            }
            return null;
        }

        /// <summary>
        /// DataTable分页查询
        /// </summary>
        /// <param name="dtDataSource">源数据(DataTable)</param>
        /// <param name="iPageSize">每页条数</param>
        /// <returns>每页保存在DataSet中</returns>
        public static DataSet PagingQuery(DataTable dtDataSource, int iPageSize)
        {
            try
            {
                DataSet dataSet = new DataSet();
                int iDataSourceCount = dtDataSource.Rows.Count;
                int iNumberPages = (iDataSourceCount / iPageSize) + (iDataSourceCount % iPageSize > 0 ? 1 : 0);
                for (int iPages = 1; iPages <= iNumberPages; iPages++)
                {
                    //填充数据
                    DataTable dtPageData = dtDataSource.Clone();
                    for (int iRows = (iPages - 1) * iPageSize; iRows < iPages * iPageSize && iRows < dtDataSource.Rows.Count; iRows++)
                    {
                        var newRow = dtPageData.NewRow();
                        var oldRow = dtDataSource.Rows[iRows];
                        foreach (DataColumn dataColumn in dtDataSource.Columns)
                        {
                            newRow[dataColumn.ColumnName] = oldRow[dataColumn.ColumnName];
                        }
                        dtPageData.Rows.Add(newRow);
                    }
                    dataSet.Tables.Add(dtPageData);
                }
                return dataSet;
            }
            catch (Exception ex)
            {
            }
            return null;
        }

        /// <summary>
        /// DataTable分页查询
        /// </summary>
        /// <param name="dtDataSource">源数据(DataTable)</param>
        /// <param name="iPageNo">页码</param>
        /// <param name="iPageSize">每页条数</param>
        /// <returns>指定页码的DataTable数据</returns>
        public static DataTable PagingQuery(DataTable dtDataSource, int iPageNo, int iPageSize)
        {
            try
            {
                int iDataSourceCount = dtDataSource.Rows.Count;
                int iNumberPages = (iDataSourceCount / iPageSize) + (iDataSourceCount % iPageSize > 0 ? 1 : 0);
                iPageNo = iPageNo <= 0 ? 1 : iPageNo;
                //填充数据
                DataTable dtPageData = dtDataSource.Clone();
                for (int iRows = (iPageNo - 1) * iPageSize; iRows < iPageNo * iPageSize && iRows < dtDataSource.Rows.Count; iRows++)
                {
                    var newRow = dtPageData.NewRow();
                    var oldRow = dtDataSource.Rows[iRows];
                    foreach (DataColumn dataColumn in dtDataSource.Columns)
                    {
                        newRow[dataColumn.ColumnName] = oldRow[dataColumn.ColumnName];
                    }
                    dtPageData.Rows.Add(newRow);
                }
                return dtPageData;
            }
            catch (Exception ex)
            {
            }
            return null;
        }
    }
}
