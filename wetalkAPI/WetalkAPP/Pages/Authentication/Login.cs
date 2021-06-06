using System;
using System.Windows.Forms;
using WetalkAPP.Services;

namespace WetalkAPP.Pages.Auth
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }
        private void RegisterLabel_Click(object sender, EventArgs e)
        {
            Hide();
            Register registerForm = new Register();
            registerForm.Show();
        }

        private async void LoginButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (UsernameTextBox.TextLength > 0 && PasswordTextBox.TextLength > 0)
                {
                    var APIService = new APIService();
                    await APIService.Login(UsernameTextBox.Text, PasswordTextBox.Text);
                    Hide();
                    //TODO: go to main page and hide this page
                    //Main.Movieseek MovieseekForm = new Main.Movieseek();
                    //MovieseekForm.Show();
                }
            }
            catch (Exception error)
            {
                ErrorLabel.Text = error.Message;
            }
        }
    }
}
