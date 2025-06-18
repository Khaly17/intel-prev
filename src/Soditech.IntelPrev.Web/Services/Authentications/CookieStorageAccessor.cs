using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Soditech.IntelPrev.Web.Services.Authentications;

public interface ICookieStorageAccessor
{
    Task<T> GetValueAsync<T>(string key);
    Task SetValueAsync<T>(string key, T value, int? expireMinutes = 1440);
}

public class CookieStorageAccessor(IJSRuntime js) : ICookieStorageAccessor, IAsyncDisposable
{
    private Lazy<IJSObjectReference> _accessorJsRef = new();
    private IJSRuntime Js { get; } = js;

    private async Task WaitForReference()
    {
        if (_accessorJsRef.IsValueCreated is false)
        {
            _accessorJsRef = new(await Js.InvokeAsync<IJSObjectReference>("import", "/assets/js/CookieStorageAccessor.js"));
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_accessorJsRef.IsValueCreated)
        {
            await _accessorJsRef.Value.DisposeAsync();
        }
    }
    public async Task<T> GetValueAsync<T>(string key)
    {
        await WaitForReference();
        var result = await _accessorJsRef.Value.InvokeAsync<T>("get", key);
        return result;
    }

    public async Task SetValueAsync<T>(string key, T value, int? expireMinutes = 1440)
    {
        await WaitForReference();
        await _accessorJsRef.Value.InvokeVoidAsync("set", key, value, expireMinutes);
    }
}