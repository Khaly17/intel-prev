using System.Linq.Expressions;

namespace Soditech.IntelPrev.Preventions.Persistence.Extensions.Linq;

public static class LinqExtensions
{
    public static bool IsNullOrEmpty(this Guid? query)
    {
        return query == null || query == Guid.Empty;
    }
}