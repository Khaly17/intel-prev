namespace Soditech.IntelPrev.Mobile.Models
{
    public class PreventionTopic
    {
        public string Title { get; set; }
        public string DocumentPath { get; set; }
        public string DocumentTitle { get; set; }
        public bool HasDocument => !string.IsNullOrEmpty(DocumentPath);
    }
}