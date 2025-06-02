using AutoMapper;
using Soditech.IntelPrev.Reports.Persistence.Models;
using Soditech.IntelPrev.Reports.Shared.Alerts;

namespace Soditech.IntelPrev.Reports.Application.Alerts;

public class AlertProfile : Profile
{
    public AlertProfile()
    {
        CreateMap<UpdateAlertCommand, Alert>()
            .ForAllMembers(opt => opt
                .Condition((src, dest, srcMember) => srcMember != null && !string.IsNullOrEmpty(srcMember.ToString())));

        CreateMap<Alert, AlertResult>();
        CreateMap<CreateAlertCommand, Alert>();
        
        CreateMap<Alert, AlertCreatedEvent>();
        CreateMap<Alert, AlertUpdatedEvent>();
    }
    
}