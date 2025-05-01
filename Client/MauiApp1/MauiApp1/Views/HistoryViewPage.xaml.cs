using Microcharts.Maui; // ChartView만 쓰는 경우
using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;
using Microcharts;
using MauiApp1.Services;
using MauiApp1.Utils; // ChartEntry, LineChart 등
using MauiApp1.Data;
namespace MauiApp1.Views;

public partial class HistoryViewPage : ContentPage
{
    public HistoryViewPage()
    {
        InitializeComponent();
        //HttpService.Instance.ContextInit();
     
        
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (DiagnosisDataStore.Instance.IsUpdated)
        {
            DiagnosisResult? result = DiagnosisDataStore.Instance.LatestResult;
            if (result != null) {
                var classificationDict = new Dictionary<string, int>
                {{ "이마 주름", result.Classification?.ForeheadWrinkle ?? 0  },
                { "미간 주름", result.Classification?.FrownWrinkle ?? 0  },
                { "눈가 주름", result.Classification?.EyesWrinkle ?? 0  },
                { "볼 모공", result.Classification?.CheekPore ?? 0},
                { "입술 건조", result.Classification?.LipsDryness ?? 0  },
                { "턱 처짐", result.Classification?.JawSagging  ??  0}};
                var regressionDict = new Dictionary<string, float>
                {{ "얼굴 전체", result.Regression?.Face  ?? 0 },
                { "이마 수분", result.Regression?.ForeheadMoisture  ?? 0},
                { "이마 탄력", result.Regression?.ForeheadElasticity ?? 0 },
                { "눈가 주름", result.Regression?.EyesWrinkle ?? 0  },
                { "볼 수분", result.Regression?.CheekMoisture ?? 0 },
                { "볼 탄력", result.Regression?.CheekElasticity ??0},
                { "볼 모공", result.Regression?.CheekPore ?? 0  },
                { "턱 수분", result.Regression?.JawMoisture ??  0},
                { "턱 탄력", result.Regression?.JawElasticity ?? 0}
    };
                await ChartUtil.SetRadarChartData(classChartView, classificationDict, "#68B9C0");
                await ChartUtil.SetRadarChartDataFloat(regrssionChartview, regressionDict, "#F37F64");
            }
        }
    }
    //private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs e)
    //{
    //    var canvas = e.Surface.Canvas;
    //    canvas.Clear(SKColors.White);

    //    var center = new SKPoint(e.Info.Width / 2, e.Info.Height / 2);
    //    float radius = 100;

    //    int sides = 5;
    //    float angleStep = 360f / sides;
    //    var paint = new SKPaint
    //    {
    //        Color = SKColors.Black,
    //        StrokeWidth = 2,
    //        Style = SKPaintStyle.Stroke,
    //        IsAntialias = true
    //    };

    //    var path = new SKPath();
    //    for (int i = 0; i < sides; i++)
    //    {
    //        float angle = (float)(Math.PI * 2 * i / 5 - Math.PI / 2);
    //        var point = new SKPoint(
    //            center.X + radius * (float)Math.Cos(angle),
    //            center.Y + radius * (float)Math.Sin(angle));

    //        if (i == 0)
    //            path.MoveTo(point);
    //        else
    //            path.LineTo(point);
    //    }

    //    path.Close();
    //    canvas.DrawPath(path, paint);
    //}
    public void Draw_graph()
    {
        var classDataDict = new Dictionary<string, int>
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

        ChartUtil.SetRadarChartData(classChartView, classDataDict, "#68B9C0");
        ChartUtil.SetRadarChartDataFloat(regrssionChartview, regressionData, "#F37F64");
    }
    public void Draw_graph1()
    {
        classChartView.Chart = new RadarChart
        {
            Entries = new[]
    {
                new ChartEntry(80) { Label = "수분", ValueLabel = "80", Color = SKColor.Parse("#266489") },
                new ChartEntry(60) { Label = "주름", ValueLabel = "60", Color = SKColor.Parse("#68B9C0") },
                new ChartEntry(70) { Label = "탄력", ValueLabel = "70", Color = SKColor.Parse("#90D585") },
                new ChartEntry(50) { Label = "모공", ValueLabel = "50", Color = SKColor.Parse("#F3C151") },
                new ChartEntry(90) { Label = "피부톤", ValueLabel = "90", Color = SKColor.Parse("#F37F64") },
                new ChartEntry(65) { Label = "기미", ValueLabel = "65", Color = SKColor.Parse("#424856") },
                new ChartEntry(85) { Label = "잡티", ValueLabel = "85", Color = SKColor.Parse("#8F97A4") },
                new ChartEntry(75) { Label = "피지", ValueLabel = "75", Color = SKColor.Parse("#DAC096") },
            }
        };
    }
}
