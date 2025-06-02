using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensor6ty.Results;
using Soditech.IntelPrev.Mediatheques.Application.Services;
using Soditech.IntelPrev.Mediatheques.Shared.Documents;

namespace Soditech.IntelPrev.Documents.Application.Documents.Queries;

public class GetDocumentBytesQueryHandler(IServiceProvider serviceProvider)
    : IRequestHandler<GetDocumentBytesQuery, TResult<byte[]>>
{
    private readonly IFileService _fileService = serviceProvider.GetRequiredService<IFileService>();
    private readonly ILogger<GetDocumentBytesQueryHandler> _logger = serviceProvider.GetRequiredService<ILogger<GetDocumentBytesQueryHandler>>();


    public async Task<TResult<byte[]>> Handle(GetDocumentBytesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var documentResult = await _fileService.GetFileAsync(request.Path);

            return Result.Success(documentResult);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Cannot get load file `{file}`", request.Path);
            return Result.Failure<byte[]>(new Error("500", "Cannot get load file"));
        }
    }
}