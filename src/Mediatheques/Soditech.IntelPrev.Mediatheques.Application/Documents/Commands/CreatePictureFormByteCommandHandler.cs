using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensor6ty.Results;
using Soditech.IntelPrev.Mediatheques.Application.Services;
using Soditech.IntelPrev.Mediatheques.Shared.Documents;

namespace Soditech.IntelPrev.Mediatheques.Application.Documents.Commands;

public class CreateFileFormByteCommandHandler(IServiceProvider serviceProvider) : IRequestHandler<CreateFileFormByteCommand, Result>
{
    private readonly IFileService _fileService = serviceProvider.GetRequiredService<IFileService>();
    private readonly ILogger<CreateFileFormByteCommandHandler> _logger = serviceProvider.GetRequiredService<ILogger<CreateFileFormByteCommandHandler>>();


    public async Task<Result> Handle(CreateFileFormByteCommand req, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(req.FileName))
        {
            _logger.LogError("File name is required: \n User x try to upload file without a name.");
            return Result.Failure(new Error("400", "File name is required"));
        }

        if (req.BlobFile.Count != 0)
            return await _fileService.CreateFileAsync(req.FileName, [.. req.BlobFile], cancellationToken);
        
        _logger.LogError("File content is required: \n User x try to upload file without a content.");
        return Result.Failure(new Error("400", "File content is empty"));

    }
}
