using Microcharts;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;
using Entry = Microcharts.ChartEntry;
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
        new Entry(100) { Label = "월", ValueLabel = "100", Color = SKColor.Parse("#FF1943") },
        new Entry(200) { Label = "화", ValueLabel = "200", Color = SKColor.Parse("#00BFFF") },
        new Entry(150) { Label = "수", ValueLabel = "150", Color = SKColor.Parse("#FF8C00") },
    }
        };
    }
    private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs e)
    {
        var canvas = e.Surface.Canvas;
        canvas.Clear(SKColors.White);

        var center = new SKPoint(e.Info.Width / 2, e.Info.Height / 2);
        float radius = 100;

        int sides = 5; // 오각형
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
            float angle = (float)(Math.PI * 2 * i / 5 - Math.PI / 2); // 360도 → 라디안 변환
            var point = new SKPoint(
                center.X + radius * (float)Math.Cos(angle),
                center.Y + radius * (float)Math.Sin(angle));

            if (i == 0)
                path.MoveTo(point);
            else
                path.LineTo(point);
        }
        path.Close(); // 도형 닫기
        canvas.DrawPath(path, paint);

        // 내부 데이터 선도 그릴 수 있음 (ex: 점 찍고 선 잇기)
    }
}