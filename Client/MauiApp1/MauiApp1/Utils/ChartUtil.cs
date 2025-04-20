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
        public static void Draw_graph()
        {
            // 예시 JSON에서 파싱된 결과를 직접 삽입
            var classData = new Dictionary<string, int>
    {
        { "이마 주름", 2 },
        { "미간 주름", 3 },
        { "눈가 주름", 4 },
        { "볼 모공", 3 },
        { "입술 건조도", 2 },
        { "턱 처짐", 4 }
    };

            var regressionData = new Dictionary<string, float>
    {
        { "이마 수분", 72.5f },
        { "이마 탄력", 64.2f },
        { "눈가 주름", 55.3f },
        { "볼 수분", 68.0f },
        { "볼 탄력", 70.1f },
        { "볼 모공", 50.5f },
        { "턱 수분", 63.3f },
        { "턱 탄력", 69.7f }
    };

            var classEntries = classData.Select(kvp =>
                new ChartEntry(kvp.Value)
                {
                    Label = kvp.Key,
                    ValueLabel = kvp.Value.ToString(),
                    Color = SKColor.Parse("#68B9C0")
                }).ToList();

            var regressionEntries = regressionData.Select(kvp =>
                new ChartEntry(kvp.Value)
                {
                    Label = kvp.Key,
                    ValueLabel = kvp.Value.ToString("F1"),
                    Color = SKColor.Parse("#F37F64")
                }).ToList();

            classChartView.Chart = new RadarChart
            {
                LabelTextSize = 30,
                Entries = classEntries,
                BackgroundColor = SKColors.Black
            };

            regrssionChartview.Chart = new RadarChart
            {
                LabelTextSize = 30,
                Entries = regressionEntries,
                BackgroundColor = SKColors.Black
            };
        }
    }
}
