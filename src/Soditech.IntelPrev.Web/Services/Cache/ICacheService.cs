namespace Soditech.IntelPrev.Web.Services.Cache;

public interface ICacheService
{
    void Set(string key, object value);
    (bool Exists, object Value) Get(string key);
}