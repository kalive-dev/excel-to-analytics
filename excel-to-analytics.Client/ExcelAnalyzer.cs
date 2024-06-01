using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml;
using MathNet.Numerics.Statistics;

namespace excel_to_analytics.Client
{
    public class ExcelAnalyzer
    {
        public static Dictionary<string, List<(DateTime Date, double Sales)>> LoadData(string filePath)
        {
            var data = new Dictionary<string, List<(DateTime Date, double Sales)>>();

            using (var package = new ExcelPackage(new System.IO.FileInfo(filePath)))
            {
                var worksheet = package.Workbook.Worksheets[0];

                for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
                {
                    var product = worksheet.Cells[row, 1].Text;
                    var date = DateTime.Parse(worksheet.Cells[row, 2].Text);
                    var sales = double.Parse(worksheet.Cells[row, 3].Text);

                    if (!data.ContainsKey(product))
                    {
                        data[product] = new List<(DateTime Date, double Sales)>();
                    }
                    data[product].Add((date, sales));
                }
            }

            return data;
        }
        public static (DateTime MaxItem, double MaxValue) AnalyzeSeasonality(List<(DateTime Date, double Sales)> salesData)
        {
            var salesByDate = salesData.GroupBy(x => x.Date)
                                       .Select(g => new { Date = g.Key, Sales = g.Sum(x => x.Sales) })
                                       .ToList();

            var meanSales = salesByDate.Average(x => x.Sales);
            var seasonalPeriods = salesByDate.Select(x => new { x.Date, Deviation = x.Sales - meanSales })
                                             .OrderByDescending(x => x.Deviation)
                                             .FirstOrDefault();

            return (seasonalPeriods.Date, seasonalPeriods.Deviation);
        }

        static void GenerateReport(Dictionary<string, string> classification)
        {
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Seasonal Classification");

                worksheet.Cells[1, 1].Value = "Product";
                worksheet.Cells[1, 2].Value = "Seasonality";

                int row = 2;
                foreach (var item in classification)
                {
                    worksheet.Cells[row, 1].Value = item.Key;
                    worksheet.Cells[row, 2].Value = item.Value;
                    row++;
                }

                package.SaveAs(new System.IO.FileInfo("seasonal_classification_report.xlsx"));
            }
        }
    }
}
