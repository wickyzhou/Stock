using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.Model
{
   public class ExportService
    {
        public void ExportDataTableToExcel(DataTable dataTable, string filePath, string fileName)
        {
            string fullName = Path.Combine(filePath, $"{fileName}{DateTime.Now:yyyy-MM-dd}.xls");

            //如果存在此文件则添加1
            if (File.Exists(fullName))
                fullName = fullName.Replace(".xls", DateTime.Now.ToString("--HH-mm-ss") + ".xls");

            IWorkbook wb = new HSSFWorkbook();
            ISheet sheet = wb.CreateSheet(fileName);
            sheet.ForceFormulaRecalculation = true;

            IRow row0 = sheet.CreateRow(0);
            row0.Height = (short)20 * 20;

            //表头
            for (int i = 0; i < dataTable.Columns.Count; i++)
            {
                row0.CreateCell(i).SetCellValue(dataTable.Columns[i].ColumnName);
            }

            //数据
            for (int j = 0; j < dataTable.Rows.Count; j++)
            {
                IRow row1 = sheet.CreateRow(j + 1);
                row1.Height = (short)15 * 20;
                for (int z = 0; z < dataTable.Columns.Count; z++)
                {
                    var s = dataTable.Columns[z].DataType;
                    if (s.Name == "Int16" || s.Name == "Int32" || s.Name == "Int64" || s.Name == "Float" || s.Name == "Double" || s.Name == "Decimal")
                    {
                        var x = dataTable.Rows[j][z];
                        object value = x.GetType().Name == "DBNull" ? 0 : x;
                        row1.CreateCell(z).SetCellValue(Convert.ToDouble(value));
                    }

                    else if (s.Name == "DateTime")
                        row1.CreateCell(z).SetCellValue(Convert.ToDateTime(dataTable.Rows[j][z]).ToString("yyyy-MM-dd HH:mm:ss"));
                    else
                        row1.CreateCell(z).SetCellValue(Convert.ToString(dataTable.Rows[j][z]));
                }
            }

            FileStream fs = new FileStream(fullName, FileMode.Create);//新建才不会报错
            wb.Write(fs);//会自动关闭流文件  //fs.Flush();
            fs.Close();
        }

    }
}
