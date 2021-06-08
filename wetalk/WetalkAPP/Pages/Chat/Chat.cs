using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;
using WetalkAPP.Models.APIRequests;
using WetalkAPP.Models.APIResponses;
using WetalkAPP.Services;

namespace WetalkAPP.Pages.Chat
{
    public partial class Chat : Form
    {
        #region constructor
        public APIService _APIService;
        private int? selectedChat = null;
        private List<ChatResponse> myChats = new List<ChatResponse>();
        private IHubProxy _hubProxy;

        public Chat()
        {
            _APIService = new APIService();
            InitializeComponent();
            ConnectToSockets();
            UpdateMyChatsDataSource();
        }
        #endregion

        private async Task ConnectToSockets()
        {
            try
            {
                using (var hubConnection = new HubConnection($"{ConfigurationManager.AppSettings["API.Address"]}"))
                {
                    _hubProxy = hubConnection.CreateHubProxy("notification");
                    //_hubProxy.On<string, string>("ReceiveMessage", async (name, message) =>
                    //{
                    //    await UpdateMyChatsDataSource();
                    //    UpdateChatMessagesDataSource();
                    //});
                    await hubConnection.Start();
                }
            }
            catch (Exception e)
            {
                //throw;
            }

        }

        private async Task UpdateMyChatsDataSource()
        {
            // configure columns
            DataTable dt = new DataTable();
            dt.Columns.Add("ID");
            dt.Columns.Add("Name");

            myChats = await _APIService.GetUserChats();

            // read the movies from file
            foreach (var item in myChats)
            {
                dt.Rows.Add(item.ID, item.Name);
            }

            chatsGrid.DataSource = dt;
            chatsGrid.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private void UpdateChatMessagesDataSource()
        {
            // configure columns
            DataTable dt = new DataTable();
            dt.Columns.Add("ID");
            dt.Columns.Add("READ");
            dt.Columns.Add("FROM");
            dt.Columns.Add("MESSAGE");

            if (selectedChat != null)
            {
                var currentChat = myChats.Find(x => x.ID == selectedChat);
                foreach (var item in currentChat.Messages)
                {
                    dt.Rows.Add(item.ID, item.Read, item.SenderID, item.Description);
                }
            }

            messageGrid.DataSource = dt;
            messageGrid.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private void BackClick(object sender, EventArgs e)
        {
            Hide();
            Menu.Menu MenuForm = new Menu.Menu();
            MenuForm.Show();
        }

        private void chatsGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var selectedRow = chatsGrid.Rows[e.RowIndex];
            if (selectedRow.Cells.Count > 0)
            {
                selectedChat = int.Parse(selectedRow.Cells[0].FormattedValue.ToString());
                UpdateChatMessagesDataSource();
            }
        }

        private async void SendButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(MessageTextBox.Text) || string.IsNullOrWhiteSpace(MessageTextBox.Text) || selectedChat == null)
                return;

            await _APIService.SendChatMessage((int)selectedChat, MessageTextBox.Text);
            await UpdateMyChatsDataSource();
            UpdateChatMessagesDataSource();


            WebSocketMessageRequest websocketMessage = new WebSocketMessageRequest()
            {
                ChatID = (int)selectedChat,
                Message = MessageTextBox.Text,
                SenderID = APIService.user.id
            };
            //await _hubProxy.Invoke("UpdateUserChats", websocketMessage);
        }

        private async void messageGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var selectedRow = messageGrid.Rows[e.RowIndex];
            if (selectedRow.Cells.Count > 0 && selectedRow.Cells[0] != null)
            {
                int selectedMessage = int.Parse(selectedRow.Cells[0].FormattedValue.ToString());
                await _APIService.MarkMessageAsRead(selectedMessage);
                await UpdateMyChatsDataSource();
                UpdateChatMessagesDataSource();
            }
        }
    }
}