using AutoMapper;
using Soditech.IntelPrev.Reports.Persistence.Models;
using Soditech.IntelPrev.Reports.Shared.ReportAttachments;

namespace Soditech.IntelPrev.Reports.Application.ReportAttachments;

public class ReportAttachmentProfile : Profile
{
    public ReportAttachmentProfile()
    {
        CreateMap<UpdateReportAttachmentCommand, ReportAttachment>()
            .ForAllMembers(opt => opt
                .Condition((src, dest, srcMember) => srcMember != null && !string.IsNullOrEmpty(srcMember.ToString())));

        CreateMap<ReportAttachment, ReportAttachmentResult>();
        CreateMap<CreateReportAttachmentCommand, ReportAttachment>();
        
        CreateMap<ReportAttachment, ReportAttachmentCreatedEvent>();
        CreateMap<ReportAttachment, ReportAttachmentUpdatedEvent>();
    }
    
}