using AppliMobile.Models;
using AppliMobile.pages;
using AppliMobile.Services;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Plugin.LocalNotification;

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
                await Shell.Current.DisplayAlert("Warning", "Please input Email & password", "OK");
                return;
            }

            Eleve eleve = await loginrespository.Login(email, password);
            if (eleve != null)
            {
                await Navigation.PushAsync(new Homepage());

                // Notification code
                var random = new Random();
                var notificationId = random.Next(101, 200);
                var request = new NotificationRequest
                {
                    CategoryType = NotificationCategoryType.Status,
                    NotificationId = notificationId,
                    Title = $"Connexion de l'étudiant  " +
                    $" EMAIL : {email}"
                };
                LocalNotificationCenter.Current.Show(request);
            }
            else
            {
                await DisplayAlert("Warning", "Email or password is incorrect", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Login", ex.Message, "OK");
        }
    }
}