using CommunityToolkit.Maui;
using MauiApp1.Controller;
using Microcharts.Maui; // ← 반드시 필요
#if ANDROID
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
               
#endif

                ;            })
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
