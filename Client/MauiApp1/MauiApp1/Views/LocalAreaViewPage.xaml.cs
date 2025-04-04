namespace MauiApp1.Views;

public partial class LocalAreaViewPage : ContentPage
{
	public LocalAreaViewPage()
	{
		InitializeComponent();
#if ANDROID

        myWebView.Source = new HtmlWebViewSource
        {
            Html = @""  
        };
#endif


    }
}