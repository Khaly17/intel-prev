namespace Soditech.IntelPrev.Web.Models;

public class DateFilter
{
    public DateTime StartDate { get; set; } = DateTime.Today;
    public DateTime EndDate { get; set; } = DateTime.UtcNow;
}
