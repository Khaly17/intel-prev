using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Flurl.Http;
using Microsoft.Extensions.Logging;
using Soditech.IntelPrev.Proxy.Extensions;
using Sensor6ty.Results;

namespace Soditech.IntelPrev.Proxy;

public class ProxyService : IProxyService
{
    private readonly ILogger<ProxyService> _logger;
    public async Task<TResult<T>> GetAsync<T>(string url, CancellationToken cancellationToken = default)
    {
        try
        {
            return await url.GetAsync(cancellationToken: cancellationToken)
                .GetResponseAsync<T>();
        }
        catch (FlurlHttpException ex)
        {
            _logger.LogError(ex, "Error while getting data from {Url}", url);
            if (ex.StatusCode == 401)
            {
                //TODO: use refresh token or go to login page
                return Result.Failure<T>(new Error("401", "Token expired."));
            }

            return Result.Failure<T>(new Error("500", ex.Message));
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogError(ex, "Error while getting data from {Url}", url);

            //TODO: use refresh token or go to login page
            return Result.Failure<T>(new Error("401", ex.Message));
        }

        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while getting data from {Url}", url);
            //TODO: log error
            return Result.Failure<T>(new Error("500", ex.Message));
        }
    }

    public async Task<TResult<T>> PostAsync<T>(string url, object data, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await  url.PostJsonAsync(data, cancellationToken: cancellationToken)
                .GetResponseAsync<T>();
            
            return response;
        }
        catch (FlurlHttpException ex)
        {
            _logger.LogError(ex, "Error while posting data from {Url}", url);
            if (ex.StatusCode == 401)
            {
                //TODO: use refresh token or go to login page
                return Result.Failure<T>(new Error("401", "Token expired."));
            }

            return Result.Failure<T>(new Error("500", ex.Message));
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogError(ex, "Error while posting data from {Url}", url);

            //TODO: use refresh token or go to login page
            return Result.Failure<T>(new Error("401", ex.Message));
        }
        catch (Exception ex)
        {
            //TODO: log error
            return Result.Failure<T>(new Error("500", ex.Message));
        }

    }

    public async Task<Result> PostAsync(string url, object data, CancellationToken cancellationToken = default)
    {
        try
        {
            return  await url.PostJsonAsync(data, cancellationToken: cancellationToken)
                .GetResponseAsync();
        }
        catch (FlurlHttpException ex)
        {
            _logger.LogError(ex, "Error while posting data from {Url}", url);

            if (ex.StatusCode == 401)
            {
                //TODO: use refresh token or go to login page
                return Result.Failure(new Error("401", "Token expired."));
            }

            return Result.Failure(new Error("500", ex.Message));
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogError(ex, "Error while posting data from {Url}", url);
            //TODO: use refresh token or go to login page
            return Result.Failure(new Error("401", ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while posting data from {Url}", url);
            //TODO: log error
            return Result.Failure(new Error("500", ex.Message));
        }
    }

    public async Task<TResult<T>> PutAsync<T>(string url, object data, CancellationToken cancellationToken = default)
    {
        try
        {
            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            return  await url.PutAsync(content, cancellationToken: cancellationToken)
                .GetResponseAsync<T>();
        }
        catch (FlurlHttpException ex)
        {
            _logger.LogError(ex, "Error while putting data from {Url}", url);
            if (ex.StatusCode == 401)
            {
                //TODO: use refresh token or go to login page
                return Result.Failure<T>(new Error("401", "Token expired."));
            }

            return Result.Failure<T>(new Error("500", ex.Message));
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogError(ex, "Error while putting data from {Url}", url);
            //TODO: use refresh token or go to login page
            return Result.Failure<T>(new Error("401", ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while putting data from {Url}", url);
            //TODO: log error
            return Result.Failure<T>(new Error("500", ex.Message));
        }
    }

    public async Task<Result> PutAsync(string url, object data, CancellationToken cancellationToken = default)
    {
        try
        {
            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            return  await url.PutAsync(content, cancellationToken: cancellationToken)
                .GetResponseAsync();
        }
        catch (FlurlHttpException ex)
        {
            _logger.LogError(ex, "Error while putting data from {Url}", url);
            if (ex.StatusCode == 401)
            {
                //TODO: use refresh token or go to login page
                return Result.Failure(new Error("401", "Token expired."));
            }

            return Result.Failure(new Error("500", ex.Message));
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogError(ex, "Error while putting data from {Url}", url);
            //TODO: use refresh token or go to login page
            return Result.Failure(new Error("401", ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while putting data from {Url}", url);
            //TODO: log error
            return Result.Failure(new Error("500", ex.Message));
        }
    }

    public async Task<Result> DeleteAsync(string url, CancellationToken cancellationToken = default)
    {
        try
        {
            return  await url.DeleteAsync(cancellationToken: cancellationToken)
                .GetResponseAsync();
        }
        catch (FlurlHttpException ex)
        {
            _logger.LogError(ex, "Error while deleting data from {Url}", url);
            if (ex.StatusCode == 401)
            {
                //TODO: use refresh token or go to login page
                return Result.Failure(new Error("401", "Token expired."));
            }

            return Result.Failure(new Error("500", ex.Message));
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogError(ex, "Error while deleting data from {Url}", url);
            //TODO: use refresh token or go to login page
            return Result.Failure(new Error("401", ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while deleting data from {Url}", url);
            //TODO: log error
            return Result.Failure(new Error("500", ex.Message));
        }
    }
}