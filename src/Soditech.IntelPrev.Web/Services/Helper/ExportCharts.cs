using Microsoft.JSInterop;

namespace Soditech.IntelPrev.Web.Services.Helper;

public static class ExportCharts
{
    [JSInvokable]
    public static async ValueTask PrintCharts(this IJSRuntime js, string pageTitle , string divId) 
        => await js.InvokeAsync<object>("printCharts", pageTitle, divId);
    
   
    [JSInvokable]
    public static async ValueTask DownloadCharts(this IJSRuntime js, string divId, string fileName) 
        => await js.InvokeAsync<object>("downloadCharts", divId, fileName);
}