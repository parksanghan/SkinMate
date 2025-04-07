using Microcharts.Maui; // ChartView만 쓰는 경우
using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;
using Microcharts; // ChartEntry, LineChart 등

namespace MauiApp1.Views;

public partial class HistoryViewPage : ContentPage
{
    public HistoryViewPage()
    {
        InitializeComponent();

        chartView.Chart = new LineChart
        {
            Entries = new[]
            {
                new ChartEntry(100) { Label = "월", ValueLabel = "100", Color = SKColor.Parse("#FF1943") },
                new ChartEntry(200) { Label = "화", ValueLabel = "200", Color = SKColor.Parse("#00BFFF") },
                new ChartEntry(150) { Label = "수", ValueLabel = "150", Color = SKColor.Parse("#FF8C00") },
            }
        };
    }

    private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs e)
    {
        var canvas = e.Surface.Canvas;
        canvas.Clear(SKColors.White);

        var center = new SKPoint(e.Info.Width / 2, e.Info.Height / 2);
        float radius = 100;

        int sides = 5;
        float angleStep = 360f / sides;
        var paint = new SKPaint
        {
            Color = SKColors.Black,
            StrokeWidth = 2,
            Style = SKPaintStyle.Stroke,
            IsAntialias = true
        };

        var path = new SKPath();
        for (int i = 0; i < sides; i++)
        {
            float angle = (float)(Math.PI * 2 * i / 5 - Math.PI / 2);
            var point = new SKPoint(
                center.X + radius * (float)Math.Cos(angle),
                center.Y + radius * (float)Math.Sin(angle));

            if (i == 0)
                path.MoveTo(point);
            else
                path.LineTo(point);
        }

        path.Close();
        canvas.DrawPath(path, paint);
    }
}
