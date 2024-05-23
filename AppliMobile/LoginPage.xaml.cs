using AppliMobile.Models;
using AppliMobile.pages;
using AppliMobile.Services;
using Microsoft.Maui.Controls.PlatformConfiguration;

namespace AppliMobile;

public partial class LoginPage : ContentPage
{
	readonly ILoginRepository loginrespository = new LoginService();
	public LoginPage()
	{
		InitializeComponent();
	}

    private async void Login_Cliked(object sender, EventArgs e)
    {
		try
		{
            string email = txtEmail.Text;
            string password = txtPassword.Text;
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {

                await Shell.Current.DisplayAlert("Warning", "please input Email & password", "ok");
                return;
            }

            Eleve eleve = await loginrespository.Login(email, password);
            if (eleve != null)
            {
                await Navigation.PushAsync(new Homepage());
            }
            else
            {
                await DisplayAlert("Warning", " Email or password is incorrect", "ok");
            }
        }catch(Exception ex)
        {
            await DisplayAlert("Login" , ex.Message, "OK");
        }
    }

  
}