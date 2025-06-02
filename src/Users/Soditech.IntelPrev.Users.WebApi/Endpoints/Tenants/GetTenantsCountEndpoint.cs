using FastEndpoints;
using MediatR;
using Sensor6ty.Results;
using Soditech.IntelPrev.Users.Shared;
using Soditech.IntelPrev.Users.Shared.Roles;
using Soditech.IntelPrev.Users.Shared.Tenants;
using Soditech.IntelPrev.Users.Shared.Users;

namespace Soditech.IntelPrev.Users.WebApi.Endpoints.Tenants;


[HttpGet(UserRoutes.Tenants.Count)]
[Tags("Tenants")]
public class GetTenantsCountEndpoint(IServiceProvider serviceProvider): EndpointWithoutRequest<TResult<int>>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    
    public override async Task<TResult<int>> HandleAsync(CancellationToken cancellationToken)
    {
        var request = new GetTenantsCountQuery();
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}

[HttpPost(UserRoutes.Tenants.Update)]
[Tags("Tenants")]
public class UpdateTenantEndpoint(IServiceProvider serviceProvider): Endpoint<UpdateTenantCommand, TResult<TenantResult>>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    
    public override async Task<TResult<TenantResult>> HandleAsync(UpdateTenantCommand request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}


[HttpPost(UserRoutes.Tenants.Create)]
[Tags("Tenants")]
public class CreateTenantEndpoint(IServiceProvider serviceProvider): Endpoint<CreateTenantCommand, TResult<TenantResult>>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    
    public override async Task<TResult<TenantResult>> HandleAsync(CreateTenantCommand request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}

[HttpDelete(UserRoutes.Tenants.Delete)]
[Tags("Tenants")]
public class DeleteTenantEndpoint(IServiceProvider serviceProvider): Endpoint<DeleteTenantCommand, Result>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    
    public override async Task<Result> HandleAsync(DeleteTenantCommand request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}


[HttpPatch(UserRoutes.Tenants.Disable)]
[Tags("Tenants")]
public class DisableTenantEndpoint(IServiceProvider serviceProvider): Endpoint<DisableTenantCommand, Result>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    
    public override async Task<Result> HandleAsync(DisableTenantCommand request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}

[HttpPatch(UserRoutes.Tenants.Enable)]
[Tags("Tenants")]
public class EnableTenantEndpoint(IServiceProvider serviceProvider): Endpoint<EnableTenantCommand, Result>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    
    public override async Task<Result> HandleAsync(EnableTenantCommand request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}

[HttpGet(UserRoutes.Tenants.GetAll)]
[Tags("Tenants")]
public class GetTenantsEndpoint(IServiceProvider serviceProvider): EndpointWithoutRequest<TResult<IEnumerable<TenantResult>>>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    
    public override async Task<TResult<IEnumerable<TenantResult>>> HandleAsync(CancellationToken cancellationToken)
    {
        var request = new GetTenantsQuery();
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}

[HttpGet(UserRoutes.Tenants.GetById)]
[Tags("Tenants")]
public class GetTenantEndpoint(IServiceProvider serviceProvider): Endpoint<GetTenantQuery, TResult<TenantResult>>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    
    public override async Task<TResult<TenantResult>> HandleAsync(GetTenantQuery request,CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}

[HttpGet(UserRoutes.Tenants.GetUsers)]
[Tags("Tenants")]
public class GetUsersTenantEndpoint(IServiceProvider serviceProvider): Endpoint<GetUsersTenantQuery, TResult<IEnumerable<UserResult>>>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    
    public override async Task<TResult<IEnumerable<UserResult>>> HandleAsync(GetUsersTenantQuery request,CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}

[HttpGet(UserRoutes.Tenants.GetRoles)]
[Tags("Tenants")]
public class GetRolesTenantEndpoint(IServiceProvider serviceProvider): Endpoint<GetRolesTenantQuery, TResult<IEnumerable<RoleResult>>>
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    
    public override async Task<TResult<IEnumerable<RoleResult>>> HandleAsync(GetRolesTenantQuery request,CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        await SendAsync(result, cancellation: cancellationToken);
        return result;
    }
}