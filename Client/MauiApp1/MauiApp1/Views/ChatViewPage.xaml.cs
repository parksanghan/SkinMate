using MauiApp1.ModelViews;

namespace MauiApp1.Views;

public partial class ChatViewPage : ContentPage
{
    private readonly ChatViewModel viewModel;
	public ChatViewPage()
	{
		InitializeComponent();
         viewModel = new ChatViewModel();
        BindingContext = viewModel;
    }
    private async void OnSendClicked(object sender, EventArgs e)
    {
        string message = chatEntry.Text;
        if (!string.IsNullOrWhiteSpace(message))
        {
            await viewModel.simulation(message);  // ✨ 여기서 호출!
            chatEntry.Text = "";  // 입력창 비우기
        }
    }
        
}