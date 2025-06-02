using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Sensor6ty.Sessions;
using Soditech.IntelPrev.Preventions.Persistence.Models;
using Soditech.IntelPrev.Preventions.Shared.ProPrevSetting;

namespace Soditech.IntelPrev.Preventions.Application.ProPrevSetting.Queries;

public class GetMyLibraryContentQueryHandler(IServiceProvider serviceProvider) : IRequestHandler<MyLibraryContentQuery,TResult<ProPrevContentResult>>
{
    private readonly IRepository<ProPrevSettings> _proPrevSettingsRepository =
        serviceProvider.GetRequiredService<IRepository<ProPrevSettings>>();
    private readonly ISensor6tySession _session = serviceProvider.GetRequiredService<ISensor6tySession>();
    private readonly ILogger<GetMyLibraryContentQueryHandler> _logger =
        serviceProvider.GetRequiredService<ILogger<GetMyLibraryContentQueryHandler>>();

    /// <inheritdoc />
    public async Task<TResult<ProPrevContentResult>> Handle(MyLibraryContentQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var proPrevSettings = await _proPrevSettingsRepository.GetAll
                .FirstOrDefaultAsync(x => x.TenantId == _session.TenantId, cancellationToken: cancellationToken);

            if (proPrevSettings == null)
            {
                return Result.Failure<ProPrevContentResult>(new Error("404","My Library Content not found"));
            }
            
            var result = new ProPrevContentResult
            {
                Content = proPrevSettings.MyLibraryContent,
                CreatedAt = proPrevSettings.CreatedAt,
                CreatorId = proPrevSettings.CreatorId
            };
        
            return Result.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while getting MyLibrary Content");
            return Result.Failure<ProPrevContentResult>(new Error("500","Error while getting MyLibrary Content"));
        }
    }
}