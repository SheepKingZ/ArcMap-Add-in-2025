using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ForestResourcePlugin.Utils
{
    /// <summary>
    /// 使用 NPOI 库导出数据到 Excel 文件的工具类
    /// </summary>
    public static class ExcelExporter
    {
        /// <summary>
        /// 导出一个对象列表到 Excel 文件。
        /// </summary>
        /// <typeparam name="T">要导出的数据类型。</typeparam>
        /// <param name="data">要导出的数据集合。</param>
        /// <param name="headers">一个字典，键是数据对象的属性名，值是 Excel 中的列标题。</param>
        /// <param name="filePath">Excel 文件的保存路径。</param>
        public static void ExportToExcel<T>(List<T> data, Dictionary<string, string> headers, string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentNullException(nameof(filePath), "文件路径不能为空。");
            }
            if (data == null)
            {
                data = new List<T>(); // 允许导出空数据表
            }
            if (headers == null)
            {
                headers = new Dictionary<string, string>();
            }

            IWorkbook workbook = new XSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("数据报告");

            // 创建表头行
            IRow headerRow = sheet.CreateRow(0);
            int headerIndex = 0;
            foreach (var headerValue in headers.Values)
            {
                headerRow.CreateCell(headerIndex++).SetCellValue(headerValue);
            }

            // 填充数据行
            var properties = typeof(T).GetProperties()
                                      .Where(p => headers.ContainsKey(p.Name))
                                      .ToList();

            for (int i = 0; i < data.Count; i++)
            {
                IRow dataRow = sheet.CreateRow(i + 1);
                int cellIndex = 0;
                foreach (var propName in headers.Keys)
                {
                    var prop = properties.FirstOrDefault(p => p.Name == propName);
                    if (prop != null)
                    {
                        object value = prop.GetValue(data[i], null);
                        dataRow.CreateCell(cellIndex).SetCellValue(value?.ToString() ?? "");
                    }
                    cellIndex++;
                }
            }

            // 自动调整列宽
            for (int j = 0; j < headers.Count; j++)
            {
                sheet.AutoSizeColumn(j);
            }

            // 将工作簿写入文件
            try
            {
                using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    workbook.Write(fileStream);
                }
            }
            catch (Exception ex)
            {
                throw new IOException($"导出 Excel 文件时出错: {ex.Message}", ex);
            }
        }
    }
}