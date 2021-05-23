using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;

namespace Common
{
    public static class DataTableHelper
    {
        /// <summary>
        /// 创建存储采集数据列表的容器DataTable
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static DataTable CreateDataTableByPattern(string pattern,string extraPattern)
        {
            DataTable dt = new DataTable("list");
            Regex rg = new Regex(@"\(\?<(.*?)>", RegexOptions.IgnoreCase);
            if (rg.IsMatch(pattern))
            {
                MatchCollection matches = rg.Matches(pattern);

                for (int i = 0; i < matches.Count; i++)
                {
                    DataColumn dc = new DataColumn { ColumnName = matches[i].Groups[1].Value.ToString() };
                    //正则用“|”将数据统一到同一列上的时候，会需要多次用到同一个列，此处只添加一次
                    if (!dt.Columns.Contains(dc.ColumnName))
                    {
                        dt.Columns.Add(dc);
                    }
                }
                //除了正则列和必须的列以外，列表固定列
                if (rg.IsMatch(extraPattern))
                {
                    MatchCollection matches1 = rg.Matches(extraPattern);
                    for (int i = 0; i < matches1.Count; i++)
                    {
                        DataColumn dc = new DataColumn { ColumnName = matches1[i].Groups[1].Value.ToString() };
                        //正则用“|”将数据统一到同一列上的时候，会需要多次用到同一个列，此处只添加一次
                        if (!dt.Columns.Contains(dc.ColumnName))
                        {
                            dt.Columns.Add(dc);
                        }
                    }
                }
    
            }
            return dt;
        }
    }
}
