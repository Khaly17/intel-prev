using AutoMapper;
using Soditech.IntelPrev.Reports.Persistence.Models;
using Soditech.IntelPrev.Reports.Shared.ReportDatas;

namespace Soditech.IntelPrev.Reports.Application.ReportDatas;

public class ReportDataProfile : Profile
{
    public ReportDataProfile()
    {
        CreateMap<UpdateReportDataCommand, ReportData>()
            .ForAllMembers(opt => opt
                .Condition((src, dest, srcMember) => srcMember != null && !string.IsNullOrEmpty(srcMember.ToString())));

        CreateMap<ReportData, ReportDataResult>();
        CreateMap<CreateReportDataCommand, ReportData>();
        
        CreateMap<ReportData, ReportDataCreatedEvent>();
        CreateMap<ReportData, ReportDataUpdatedEvent>();
    }
    
}