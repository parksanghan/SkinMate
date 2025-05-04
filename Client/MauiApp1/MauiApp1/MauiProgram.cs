using CommunityToolkit.Maui;
using MauiApp1.Controller;
using Microcharts.Maui; // ← 반드시 필요
#if ANDROID
using Android.Webkit; // ✅ 이거 추가!
using MauiApp1.Platforms.Android.AdHandler;
#endif
using Microsoft.Extensions.Logging;

namespace MauiApp1
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder.ConfigureMauiHandlers(handlers =>
            {

#if ANDROID
    Microsoft.Maui.Handlers.WebViewHandler.Mapper.AppendToMapping("EnableJavaScript", (handler, view) =>
    {
        if (handler.PlatformView is Android.Webkit.WebView androidWebView)
        {
            androidWebView.Settings.JavaScriptEnabled = true;
            androidWebView.Settings.DomStorageEnabled = true;
            androidWebView.Settings.JavaScriptCanOpenWindowsAutomatically = true;
            androidWebView.Settings.SetSupportMultipleWindows(true);
            androidWebView.SetWebChromeClient(new WebChromeClient());
        }
    });
#endif
                ;
            })
                .UseMauiApp<Views.App>()
                .UseMauiCommunityToolkitCamera()
                .UseMicrocharts()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
