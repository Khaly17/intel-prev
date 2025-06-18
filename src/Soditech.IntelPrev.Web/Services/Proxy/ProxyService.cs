using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sensor6ty.Results;
using Soditech.IntelPrev.Web.Services.Extensions;

namespace Soditech.IntelPrev.Web.Services.Proxy;
public class ProxyService(HttpClient httpClient, ILogger<ProxyService> logger) : IProxyService
{
    public async Task<TResult<T>> GetAsync<T>(string url, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await httpClient.GetAsync(url, cancellationToken);
            return await response.GetResponseAsync<T>(logger, cancellationToken: cancellationToken);
        }
        // catch UnAuthorized error
        catch (UnauthorizedAccessException ex)
        {
            //TODO: use refresh token or go to login page
            return Result.Failure<T>(new Error("401", ex.Message));
        }

        catch (Exception ex)
        {
            logger.LogError(ex, "Error in GetAsync");

            return Result.Failure<T>(new Error("500", ex.Message));
        }
    }

    public async Task<TResult<T>> PostAsync<T>(string url, object data, CancellationToken cancellationToken = default)
    {
        try
        {
            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(url, content, cancellationToken);
            return await response.GetResponseAsync<T>(logger, cancellationToken: cancellationToken);
        }
        catch (UnauthorizedAccessException ex)
        {
            //TODO: use refresh token or go to login page
            return Result.Failure<T>(new Error("401", ex.Message));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error in PostAsync `{url}`", url);
        }

        return Result.Failure<T>(new Error("500", "Cannot post data to server"));
    }

    public async Task<Result> PostAsync(string url, object data, CancellationToken cancellationToken = default)
    {
        try
        {
            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(url, content, cancellationToken);
            return await response.GetResponseAsync(logger, cancellationToken: cancellationToken);
        }
        catch (UnauthorizedAccessException ex)
        {
            //TODO: use refresh token or go to login page
            return Result.Failure(new Error("401", ex.Message));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error in PostAsync `{url}`", url);
        }

        return Result.Failure(new Error("500", "Cannot post data to server"));
    }

    public async Task<TResult<T>> PutAsync<T>(string url, object data, CancellationToken cancellationToken = default)
    {
        try
        {
            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PutAsync(url, content, cancellationToken);
            return await response.GetResponseAsync<T>(logger, cancellationToken: cancellationToken);
        }
        catch (UnauthorizedAccessException ex)
        {
            //TODO: use refresh token or go to login page
            return Result.Failure<T>(new Error("401", ex.Message));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error in PutAsync `{url}`", url);
        }

        return Result.Failure<T>(new Error("500", "Cannot put data to server"));
    }

    public async Task<Result> PutAsync(string url, object data, CancellationToken cancellationToken = default)
    {
        try
        {
            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PutAsync(url, content, cancellationToken);
            return await response.GetResponseAsync(logger, cancellationToken: cancellationToken);
        }
        catch (UnauthorizedAccessException ex)
        {
            //TODO: use refresh token or go to login page
            return Result.Failure(new Error("401", ex.Message));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error in PutAsync `{url}`", url);
        }

        return Result.Failure(new Error("500", "Cannot put data to server"));
    }

    public async Task<Result> DeleteAsync(string url, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await httpClient.DeleteAsync(url, cancellationToken);
            return await response.GetResponseAsync(logger, cancellationToken: cancellationToken);
        }
        catch (UnauthorizedAccessException ex)
        {
            //TODO: use refresh token or go to login page
            return Result.Failure(new Error("401", ex.Message));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error in DeleteAsync `{url}`", url);
        }

        return Result.Failure(new Error("500", "Cannot delete data from server"));
    }
}