using Microcharts.Maui;
using Microcharts;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace MauiApp1.Utils
{
    public class ChartUtil
    {
        //Dictionary<string, float> regressionData = new Dictionary<string, float>
        //{
        //     { "이마 수분", regression.ForeheadMoisture },
        //    { "이마 탄력", regression.ForeheadElasticity },
        //    { "눈가 주름", regression.EyesWrinkle },
        //    { "볼 수분", regression.CheekMoisture },
        //    { "볼 탄력", regression.CheekElasticity },
        //    { "볼 모공", regression.CheekPore },
        //    { "턱 수분", regression.JawMoisture },
        //    { "턱 탄력", regression.JawElasticity }
        //};
        //        var classDataDict = new Dictionary<string, float>
        //{
        //    { "이마 주름", classification.ForeheadWrinkle },
        //    { "미간 주름", classification.FrownWrinkle },
        //    { "눈가 주름", classification.EyesWrinkle },
        //    { "입술 건조도", classification.LipsDryness },
        //    { "턱 처짐", classification.JawSagging },
        //    { "볼 모공", classification.CheekPore }
        //};

        public static async Task SetRadarChartData<T>(ChartView chartView, Dictionary<string, T> data, string colorHex = "#F37F64")
      where T : struct, IConvertible
        {
            var typeface  = await FontUtil.getNaunumFontAsync();
            var entries = data.Select(kvp =>
                new ChartEntry(Convert.ToSingle(kvp.Value)) // int든 float든 OK
                {
                    Label = kvp.Key,
                    ValueLabel = kvp.Value.ToString(),
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
