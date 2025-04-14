using MauiApp1.Services;

namespace MauiApp1.Views;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        //  앱 시작시 context 초기화
        HttpService.Instance.ContextInit();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new ChatViewPage());
    }
}