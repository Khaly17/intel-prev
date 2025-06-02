using Sensor6ty.Results;

namespace Soditech.IntelPrev.Web.Services.Proxy
{
    public interface IProxyService
    {
        Task<TResult<T>> GetAsync<T>(string url, CancellationToken cancellationToken = default);
        Task<TResult<T>> PostAsync<T>(string url, object data, CancellationToken cancellationToken = default);
        Task<Result> PostAsync(string url, object data, CancellationToken cancellationToken = default);
        Task<TResult<T>> PutAsync<T>(string url, object data, CancellationToken cancellationToken = default);
        Task<Result> PutAsync(string url, object data, CancellationToken cancellationToken = default);
        Task<Result> DeleteAsync(string url, CancellationToken cancellationToken = default);
    }
}
