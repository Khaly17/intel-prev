namespace Soditech.IntelPrev.Web.Events
{
    public class SiteEventService
    {
        public event Action OnSiteChanged;

        public void NotifySiteChanged()
        {
            OnSiteChanged?.Invoke();
        }
    }

}
