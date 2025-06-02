using AutoMapper;
using Soditech.IntelPrev.Reports.Persistence.Models;
using Soditech.IntelPrev.Reports.Shared.Reports;

namespace Soditech.IntelPrev.Reports.Application.Reports;

public class ReportProfile : Profile
{
    public ReportProfile()
    {
        CreateMap<UpdateReportCommand, Report>()
            .ForAllMembers(opt => opt
                .Condition((src, dest, srcMember) => srcMember != null && !string.IsNullOrEmpty(srcMember.ToString())));

        CreateMap<Report, ReportResult>();
        CreateMap<CreateReportCommand, Report>();
        
        CreateMap<Report, ReportCreatedEvent>();
        CreateMap<Report, ReportUpdatedEvent>();
    }
    
}