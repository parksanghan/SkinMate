using MauiApp1.Controller;
using MauiApp1.Model;

namespace MauiApp1.Views;

public partial class Login_RegisterPage : ContentPage
{
    private readonly AuthController authController;
        //<Entry x:Name="usernameEntry" Placeholder="아이디 입력" />
        //<Entry x:Name="passwordEntry" Placeholder="비밀번호 입력" IsPassword="True" />
	public Login_RegisterPage()
	{
		InitializeComponent();
        authController = new AuthController();  

    }
    private async void OnLoginClicked(object sender, EventArgs e)
    {
        try
        {
            LoginRequest req = new LoginRequest
            {
                UserId = usernameEntry.Text,
                Password = passwordEntry.Text
            };
            string res = await authController.LoginAsync(req); 
            if (res.Trim().ToLower() == "ok") await DisplayAlert("로그인", res, "확인");
        }
        catch (Exception ex)
        {
            await DisplayAlert("에러", ex.Message, "확인");
        }
        // 로그인 기능 구현
    }
    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        try
        {
            RegisterRequest req = new RegisterRequest
            {
                UserId = usernameEntry.Text,
                Password = passwordEntry.Text
                ,Name = usernameEntry.Text  
            };
            string res = await authController.RegisterAsync(req);
            if (res.Trim().ToLower() == "ok") await DisplayAlert("가입", res, "확인");
        }
        catch (Exception ex)
        {
            await DisplayAlert("에러", ex.Message, "확인");
        }
        // 가입 기능 구현

    }

}