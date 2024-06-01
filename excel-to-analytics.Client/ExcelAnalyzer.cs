using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using OfficeOpenXml;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms.TimeSeries;

namespace excel_to_analytics.Client
{
    public class ExcelAnalyzer
    {
        static Dictionary<string, List<SalesData>> LoadData(string filePath)
        {
            var data = new Dictionary<string, List<SalesData>>();

            using (var package = new ExcelPackage(new System.IO.FileInfo(filePath)))
            {
                var worksheet = package.Workbook.Worksheets[0];

                for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
                {
                    var product = worksheet.Cells[row, 1].Text;
                    var date = DateTime.Parse(worksheet.Cells[row, 2].Text);
                    var sales = float.Parse(worksheet.Cells[row, 3].Text);

                    if (!data.ContainsKey(product))
                    {
                        data[product] = new List<SalesData>();
                    }
                    data[product].Add(new SalesData { Product = product, Date = date, Sales = sales });
                }
            }

            return data;
        }

        static List<float> ForecastSales(MLContext mlContext, List<SalesData> salesData)
        {
            var dataView = mlContext.Data.LoadFromEnumerable(salesData);

            var forecastingPipeline = mlContext.Forecasting.ForecastBySsa(
                outputColumnName: nameof(SalesPrediction.ForecastedSales),
                inputColumnName: nameof(SalesData.Sales),
                windowSize: 12,
                seriesLength: salesData.Count,
                trainSize: salesData.Count,
                horizon: 12);

            var model = forecastingPipeline.Fit(dataView);

            var forecastingEngine = model.CreateTimeSeriesEngine<SalesData, SalesPrediction>(mlContext);

            var forecast = forecastingEngine.Predict();

            return forecast.ForecastedSales;
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
        static string ClassifySeasonality(List<float> forecastedSales)
        {
            // Аналіз піків продажів для визначення сезонності
            var maxSales = forecastedSales.Max();
            var maxIndex = forecastedSales.IndexOf(maxSales);

            if (maxIndex == 11) // Припустимо, що 11 місяць - грудень
            {
                return "Christmas";
            }
            else if (maxIndex == 6) // Припустимо, що 6 місяць - липень
            {
                return "Summer";
            }
            else
            {
                return "No clear seasonality";
            }
        }
    }
}
