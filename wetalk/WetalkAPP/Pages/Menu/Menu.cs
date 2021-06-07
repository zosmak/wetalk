using System;
using System.Windows.Forms;
using WetalkAPP.Services;

namespace WetalkAPP.Pages.Menu
{
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();
        }

        private void ChatButton_Click(object sender, EventArgs e)
        {
            Hide();
            //Main.Movieseek MovieseekForm = new Main.Movieseek();
            //MovieseekForm.Show();
        }

        private void FilesButton_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void ManageButton_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void QuitButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
