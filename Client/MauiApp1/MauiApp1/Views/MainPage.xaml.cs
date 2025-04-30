namespace MauiApp1.Views;
using CommunityToolkit.Maui.Camera;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Core.Primitives;
using CommunityToolkit.Maui.Views;
using MauiApp1.Views.Content;
using System.Threading.Tasks;

public partial class MainPage : ContentPage
{
    
    
    public MainPage()
    {
        InitializeComponent();
    
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        var popup = new WebMapPopup();
        await this.ShowPopupAsync(popup);    
    }
}
