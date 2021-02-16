namespace DevIO.App.ViewModels
{
    public class ErrorViewModel
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public int RequestId { get; set; }
        public bool ShowRequestId => RequestId == 0;
    }
}