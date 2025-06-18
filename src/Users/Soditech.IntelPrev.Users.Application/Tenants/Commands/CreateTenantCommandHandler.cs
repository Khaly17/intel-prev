using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Soditech.IntelPrev.Users.Persistence.Models;
using Soditech.IntelPrev.Users.Shared.Tenants;
using Soditech.IntelPrev.Users.Shared.Users;

namespace Soditech.IntelPrev.Users.Application.Tenants.Commands;


public class CreateTenantCommandHandler(IServiceProvider serviceProvider) : IRequestHandler<CreateTenantCommand, TResult<TenantResult>>
{
    private readonly IRepository<Tenant> _tenantRepository = serviceProvider.GetRequiredService<IRepository<Tenant>>();
    private readonly ILogger<CreateTenantCommandHandler> _logger = serviceProvider.GetRequiredService<ILogger<CreateTenantCommandHandler>>();
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    private readonly IMapper _mapper = serviceProvider.GetRequiredService<IMapper>();
    private readonly IPublisher _publisher = serviceProvider.GetRequiredService<IPublisher>();

    public async Task<TResult<TenantResult>> Handle(CreateTenantCommand request, CancellationToken cancellationToken)
    {
        try
        {
            request.Name = request.Name.ToUpper();
            
            //Check if the tenant already exists
            var tenant = await _tenantRepository.GetAll.FirstOrDefaultAsync(t => t.Name == request.Name, cancellationToken: cancellationToken);
            if (tenant != null)
            {
                return Result.Success(_mapper.Map<TenantResult>(tenant));
            }
        
            tenant = _mapper.Map<Tenant>(request);

            await _tenantRepository.AddAsync(tenant, cancellationToken);
            //TODO: create a tenant manager where we manager the tenant creation and admin for tenant also.
            var result = await _mediator.Send(new CreateUserCommand
            {
                TenantId = tenant.Id,
                Email = request.AdminEmail,
                UserName = "admin", // TODO: Do not hardcode the username, use constants
                FirstName = request.AdminFirstName,
                LastName = request.AdminLastName,
            }, cancellationToken);

            //TODO: create static roles for the tenant
            
            if (result.IsSuccess)
            {
                //TODO: Send a notification to the admin
                
                await _publisher.Publish(_mapper.Map<TenantCreatedEvent>(tenant), cancellationToken);
                
                return Result.Success(_mapper.Map<TenantResult>(tenant));
            }

            _logger.LogError("[{code}]Error while creating tenant, {message}", result.Error.Code, result.Error.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Cannot create tenant");
        }
        
        return Result.Failure<TenantResult>(new Error("500", "Error while creating tenant"));
    }   
}