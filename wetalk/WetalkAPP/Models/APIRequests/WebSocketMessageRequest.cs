namespace WetalkAPP.Models.APIRequests
{
    public class WebSocketMessageRequest
    {
        public int ChatID { get; set; }

        public int SenderID { get; set; }

        public string Message { get; set; }
    }
}
