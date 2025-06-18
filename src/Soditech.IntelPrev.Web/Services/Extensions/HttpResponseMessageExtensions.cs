using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sensor6ty.Results;

namespace Soditech.IntelPrev.Web.Services.Extensions;


public static class HttpResponseMessageExtensions
{
    public static async Task<TResult<T>> GetResponseAsync<T>(
        this HttpResponseMessage responseMessage,
        ILogger logger,
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (responseMessage.IsSuccessStatusCode)
            {
                var resultSting = await responseMessage.Content.ReadAsStringAsync(cancellationToken);
                
                // if resulString contains "IsSuccess", then parse it with TResult<T>
                if (resultSting.Contains("IsSuccess", StringComparison.OrdinalIgnoreCase))
                {
                    // var tResult = JsonSerializer.Deserialize<TResult<T>>(resultSting);
                    var tResult = await responseMessage.Content.ReadFromJsonAsync<TResult<T>>(cancellationToken);

                    return tResult ?? Result.Failure<T>(new Error("DeserializationError",$"Cannot deserialize {typeof(T).Name}"));
                }

                var result = JsonSerializer.Deserialize<T>(resultSting);
                if (result != null)
                {
                    return Result.Success(result);
                }
                
                //var result = await responseMessage.Content.ReadFromJsonAsync<TResult<T>>(cancellationToken);

                // if (result != null)
                // {
                //     return result;
                // }

                logger.LogWarning("Cannot deserialize {Type}", typeof(T).Name);

                return Result.Failure<T>(new Error("400", $"Cannot deserialize {typeof(T).Name}"));
            }

            logger.LogWarning("Request failed with status code {StatusCode}: {ReasonPhrase}",
                responseMessage.StatusCode,
                responseMessage.ReasonPhrase);

            return Result.Failure<T>(new Error(responseMessage.StatusCode.ToString(), $"Request failed with status code {responseMessage.StatusCode}"));
        }
        catch (Exception e)
        {
            logger.LogError(e, "An exception occurred while processing the response. Status code: {StatusCode}", responseMessage.StatusCode);
            return Result.Failure<T>(new Error(responseMessage.StatusCode.ToString(), e.Message));
        }
    }
    
    public static async Task<Result> GetResponseAsync(
        this HttpResponseMessage responseMessage,
        ILogger logger,
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (responseMessage.IsSuccessStatusCode)
            {
                var result = await responseMessage.Content.ReadFromJsonAsync<Result>(cancellationToken);

                if (result != null)
                {
                    return result;
                }

                logger.LogWarning("Cannot deserialize {Type}", nameof(Result));

                return Result.Failure(new Error("400", $"Cannot deserialize {nameof(Result)}"));
            }

            logger.LogWarning("Request failed with status code {StatusCode}: {ReasonPhrase}",
                responseMessage.StatusCode,
                responseMessage.ReasonPhrase);

            return Result.Failure(new Error(responseMessage.StatusCode.ToString(), $"Request failed with status code {responseMessage.StatusCode}"));
        }
        catch (Exception e)
        {
            logger.LogError(e, "An exception occurred while processing the response. Status code: {StatusCode}", responseMessage.StatusCode);
            return Result.Failure(new Error(responseMessage.StatusCode.ToString(), e.Message));
        }
    }
}
