using Microsoft.Maui.Controls;

using ClosedXML.Excel;
using Microcharts;

using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microcharts.Maui;

namespace excel_to_analytics.Client.Views
{
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {
            InitializeComponent();
        }

        private async void OnPickFileClicked(object sender, EventArgs e)
        {
            var result = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "Pick an Excel file",
            });

            if (result != null)
            {
                var stream = await result.OpenReadAsync();
                var entries = ReadExcelData(stream);
                DisplayChart(entries);
            }
        }

        private List<ChartEntry> ReadExcelData(Stream stream)
        {
            var entries = new List<ChartEntry>();

            using (var workbook = new XLWorkbook(stream))
            {
                var worksheet = workbook.Worksheet(1);
                var rows = worksheet.RangeUsed().RowsUsed().Skip(1); // Skipping header

                foreach (var row in rows)
                {
                    var date = row.Cell(4).GetValue<DateTime>();
                    var quantity = row.Cell(5).GetValue<float>();

                    entries.Add(new ChartEntry(quantity)
                    {
                        Label = date.ToString("dd.MM.yyyy"),
                        ValueLabel = quantity.ToString(),
                        Color = SKColor.Parse("#3498db") // You can set a color of your choice
                    });
                }
            }

            return entries;
        }

        private void DisplayChart(List<ChartEntry> entries)
        {
            var chart = new LineChart { Entries = entries };
            chartView.Chart = chart;
        }
    }
}
