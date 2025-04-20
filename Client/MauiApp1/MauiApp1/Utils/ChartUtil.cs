using Microcharts.Maui;
using Microcharts;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.Utils
{
    public class ChartUtil
    {
        public static void SetRadarChartData(ChartView chartView, Dictionary<string, float> data, string colorHex = "#F37F64")
        {
            var entries = data.Select(kvp =>
                new ChartEntry(kvp.Value)
                {
                    Label = kvp.Key,
                    ValueLabel = kvp.Value.ToString("F1"),
                    Color = SKColor.Parse(colorHex)
                }).ToList();

            chartView.Chart = new RadarChart
            {
                LabelTextSize = 30,
                Entries = entries,
                BackgroundColor = SKColors.Black
            };
        }
    }

}
