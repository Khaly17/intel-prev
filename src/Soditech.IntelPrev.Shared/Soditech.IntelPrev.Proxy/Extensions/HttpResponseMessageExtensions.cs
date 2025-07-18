using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json;
using System.Threading.Tasks;
using Flurl.Http;
using Sensor6ty.Results;

namespace Soditech.IntelPrev.Proxy.Extensions;

public static class FlurlResponseExtensions
{
    public static async Task<TResult<T>> GetResponseAsync<T>(this Task<IFlurlResponse> responseTask)
    {
        try
        {
            var response = await responseTask.ConfigureAwait(false);

            switch (response.StatusCode)
            {
                case 200:
                    {
                        var resultSting = await response.GetStringAsync();

                        // if resulString contains "IsSuccess", then parse it with TResult<T>
                        if (resultSting.Contains("isSuccess"))
                        {
                            var tResult = await response.GetJsonAsync<TResult<T>>();
                            return tResult ?? Result.Failure<T>(new Error("DeserializationError", $"Cannot deserialize {typeof(T).Name}"));
                        }

                        var result = JsonSerializer.Deserialize<T>(resultSting);
                        if (!EqualityComparer<T>.Default.Equals(result, default)) // Fix for S2955, result != null is not sufficient for reference types
                        {
                            return Result.Success(result);
                        }
                        return Result.Failure<T>(new Error("DeserializationError", $"Cannot deserialize {typeof(T).Name}"));
                    }
                case 401:
                    return Result.Failure<T>(new Error("401", response.ResponseMessage.ToString()));
                default:
                    return Result.Failure<T>(new Error(response.StatusCode.ToString(), "Error occured !!"));
            }
        }
        catch (FlurlHttpException e)
        {
            if (e.StatusCode == 401)
            {
                return Result.Failure<T>(new Error("401", e.Message));
            }

            Debug.WriteLine(e);
            return Result.Failure<T>(new Error(e.StatusCode.ToString() ?? "DeserializationError", $"Cannot deserialize {typeof(T).Name}"));
        }
        catch (Exception e)
        {
            Debug.Write(e);
            return Result.Failure<T>(new Error("DeserializationError", $"Cannot deserialize {typeof(T).Name}"));
        }
    }
    
    public static async Task<Result> GetResponseAsync(this Task<IFlurlResponse> responseTask)
    {
        try
        {
            var response = await responseTask.ConfigureAwait(false);
            
            switch (response.StatusCode)
            {
                case 200:
                {
                    var resultSting = await response.GetStringAsync();
                
                    // if resulString contains "IsSuccess", then parse it with TResult<T>
                    if (resultSting.Contains("IsSuccess"))
                    {
                        var tResult = JsonSerializer.Deserialize<Result>(resultSting);
                        return tResult ?? Result.Failure(new Error("DeserializationError",$"Cannot deserialize {nameof(Result)}"));
                    }

                    return Result.Success();
                }
                case 401:
                    return Result.Failure(new Error("401", response.ResponseMessage.ToString()));
                default:
                    return Result.Failure(new Error(response.StatusCode.ToString(), "Error occured !!"));
            }
        }
        catch (FlurlHttpException e)
        {
            if (e.StatusCode == 401)
            {
                return Result.Failure(new Error("401", e.Message));   
            }
            
            return Result.Failure(new Error(e.StatusCode.ToString() ?? "DeserializationError",e.Message));
        }
        catch (Exception e)
        {
            //TODO: use ILogger
            Debug.Write(e);
           return Result.Failure(new Error("DeserializationError",$"Cannot deserialize {nameof(Result)}"));
        }
    }

}