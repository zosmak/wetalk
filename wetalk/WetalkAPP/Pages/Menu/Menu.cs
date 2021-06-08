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
            Chat.Chat ChatForm = new Chat.Chat();
            ChatForm.Show();
        }

        private void FilesButton_Click(object sender, EventArgs e)
        {
            //Hide();
        }

        private void ManageButton_Click(object sender, EventArgs e)
        {
            //Hide();
        }

        private void QuitButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
