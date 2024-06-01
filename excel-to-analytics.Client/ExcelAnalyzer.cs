using System;
using System.Collections.Generic;
using System.IO;
using OfficeOpenXml;

namespace excel_to_analytics.Client
{
    internal class ExcelAnalyzer
    {
        public List<ExcelData> ReadExcelData(string filePath)
        {
            List<ExcelData> dataList = new List<ExcelData>();

            FileInfo fileInfo = new FileInfo(filePath);
            using (ExcelPackage package = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                int rowCount = worksheet.Dimension.Rows;
                int columnCount = worksheet.Dimension.Columns;

                for (int row = 1; row <= rowCount; row++)
                {
                    ExcelData data = new ExcelData();
                    data.Location = worksheet.Cells[row, 1].Value?.ToString();
                    data.Area = worksheet.Cells[row, 2].Value?.ToString();
                    data.ProductDescription = worksheet.Cells[row, 3].Value?.ToString();
                    data.Date = DateTime.ParseExact(worksheet.Cells[row, 4].Value?.ToString(), "dd.MM.yyyy HH:mm:ss", null);
                    data.Quantity = Convert.ToDouble(worksheet.Cells[row, 5].Value);
                    data.Price = Convert.ToDouble(worksheet.Cells[row, 6].Value);

                    dataList.Add(data);
                }
            }

            return dataList;
        }
        public Dictionary<string, string> GetPopularCategoriesByMonth(List<ExcelData> dataList)
        {
            var popularCategories = new Dictionary<string, string>();

            foreach (var data in dataList)
            {
                string monthYear = data.Date.ToString("MMMM yyyy");

                if (!popularCategories.ContainsKey(monthYear))
                {
                    popularCategories[monthYear] = data.ProductDescription;
                }
                else
                {
                    var categories = new Dictionary<string, int>();
                    categories[data.ProductDescription] = 1;

                    if (popularCategories[monthYear] != data.ProductDescription)
                    {
                        categories[popularCategories[monthYear]] = 1;
                    }

                    popularCategories[monthYear] = categories.OrderByDescending(x => x.Value).First().Key;
                }
            }
            return popularCategories;
        }
        public Dictionary<string, string> GetPopularCategoriesByYear(List<ExcelData> dataList)
        {
            var popularCategories = new Dictionary<string, string>();

            foreach (var data in dataList)
            {
                string year = data.Date.Year.ToString();

                if (!popularCategories.ContainsKey(year))
                {
                    popularCategories[year] = data.ProductDescription;
                }
                else
                {
                    var categories = new Dictionary<string, int>();
                    categories[data.ProductDescription] = 1;

                    if (popularCategories[year] != data.ProductDescription)
                    {
                        categories[popularCategories[year]] = 1;
                    }

                    popularCategories[year] = categories.OrderByDescending(x => x.Value).First().Key;
                }
            }
            return popularCategories;
        }
    }

    public class ExcelData
    {
        public string Location { get; set; }
        public string Area { get; set; }
        public string ProductDescription { get; set; }
        public DateTime Date { get; set; }
        public double Quantity { get; set; }
        public double Price { get; set; }
    }
}
