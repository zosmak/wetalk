using WetalkAPP.Services;
using System;
using System.Windows.Forms;

namespace WetalkAPP.Pages.Auth
{
    public partial class Register : Form
    {
        public Register()
        {
            InitializeComponent();
        }

        private void LoginLabel_Click(object sender, EventArgs e)
        {
            Hide();
            Login LoginForm = new Login();
            LoginForm.Show();
            Close();
        }

        private async void CreateButton_Click(object sender, EventArgs e)
        {
            try
            {
                // validate if all fields are written
                if (FirstNameTextBox.TextLength > 0 && LastNameTextBox.TextLength > 0 && UsernameTextBox.TextLength > 0 && PasswordTextBox.TextLength > 0)
                {
                    var APIService = new APIService();
                    await APIService.Register(FirstNameTextBox.Text, LastNameTextBox.Text, UsernameTextBox.Text, PasswordTextBox.Text);
                    Hide();
                    Menu.Menu MenuForm = new Menu.Menu();
                    MenuForm.Show();
                }
            }
            catch (Exception error)
            {
                ErrorLabel.Text = error.Message;
            }
        }
    }
}
