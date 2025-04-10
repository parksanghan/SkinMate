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
        var shakeAnim = new Animation();
        shakeAnim.WithConcurrent(new Animation(v => chatBtn.TranslationX = v, 0, -15), 0, 0.1);
        shakeAnim.WithConcurrent(new Animation(v => chatBtn.TranslationX = v, -15, 15), 0.1, 0.3);
        shakeAnim.WithConcurrent(new Animation(v => chatBtn.TranslationX = v, 15, -15), 0.3, 0.5);
        shakeAnim.WithConcurrent(new Animation(v => chatBtn.TranslationX = v, -15, 0), 0.5, 0.6);
        chatBtn.Animate("shake", shakeAnim, 16, 250);
        Vibration.Default.Vibrate(TimeSpan.FromMilliseconds(100));
        //if (!string.IsNullOrWhiteSpace(message))
        //{
        //    await viewModel.simulation(message);  // ✨ 여기서 호출!
        //    chatEntry.Text = "";  // 입력창 비우기
        //}
    }
        
}