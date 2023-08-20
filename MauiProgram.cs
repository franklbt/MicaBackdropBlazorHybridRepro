using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;
using Microsoft.UI.Xaml.Media;

namespace MicaBackdropBlazorHybridRepro;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>();
        builder.Services.AddMauiBlazorWebView();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        // Access windows event lifecycle hooks in maui startup
        builder.ConfigureLifecycleEvents(events =>
        {
#if WINDOWS
            events.AddWindows(b =>
            {
                b.OnWindowCreated(win =>
                {
                    win.SystemBackdrop = new MicaBackdrop();
                    var transparent = Windows.UI.Color.FromArgb(0, 0, 0, 0);
                    win.AppWindow.TitleBar.ButtonBackgroundColor = transparent;
                });
            });
#endif
        });

        return builder.Build();
    }
}