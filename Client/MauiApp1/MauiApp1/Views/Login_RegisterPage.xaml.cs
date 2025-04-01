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
    private void OnLoginClicked(object sender, EventArgs e)
    {
        LoginRequest req = new LoginRequest
        {
            UserId = usernameEntry.Text,
            Password = passwordEntry.Text
        };
        Task<string> res=authController.LoginAsync(req);
     
         // 로그인 기능 구현
    }
    private void OnRegisterClicked(object sender, EventArgs e)
    {
        LoginRequest req = new LoginRequest
        {
            UserId = usernameEntry.Text,
            Password = passwordEntry.Text
        };
        // 가입 기능 구현

    }

}