using AutoMapper;
using Soditech.IntelPrev.Reports.Persistence.Models;
using Soditech.IntelPrev.Reports.Shared.ReportComments;

namespace Soditech.IntelPrev.Reports.Application.ReportComments;

public class ReportCommentProfile : Profile
{
    public ReportCommentProfile()
    {
        CreateMap<UpdateReportCommentCommand, ReportComment>()
            .ForAllMembers(opt => opt
                .Condition((src, dest, srcMember) => srcMember != null && !string.IsNullOrEmpty(srcMember.ToString())));

        CreateMap<ReportComment, ReportCommentResult>();
        CreateMap<CreateReportCommentCommand, ReportComment>();
        
        CreateMap<ReportComment, ReportCommentCreatedEvent>();
        CreateMap<ReportComment, ReportCommentUpdatedEvent>();
    }
    
}