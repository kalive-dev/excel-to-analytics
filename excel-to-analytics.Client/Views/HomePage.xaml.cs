using System;
using System.Collections.Generic;
using System.IO;

namespace excel_to_analytics.Client.Views
{
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {
            InitializeComponent();
        }

        async void OnSelectExcelFileClicked(object sender, EventArgs e)
        {
            var file = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "Select Excel File"
            });

            if (file != null)
            {
                var excelData = ReadExcelFile(file.FullPath);
                DataListView.ItemsSource = excelData;
            }
        }

        List<string> ReadExcelFile(string filePath)
        {
            List<string> excelData = new List<string>();

            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    excelData.Add(line);
                }
            }

            return excelData;
        }

    }
}
