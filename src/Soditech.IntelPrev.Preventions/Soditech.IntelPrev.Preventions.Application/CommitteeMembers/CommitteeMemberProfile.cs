using AutoMapper;
using Soditech.IntelPrev.Preventions.Persistence.Models;
using Soditech.IntelPrev.Preventions.Shared.CommitteeMembers;

namespace Soditech.IntelPrev.Preventions.Application.CommitteeMembers;

public class CommitteeMemberProfile : Profile
{
    public CommitteeMemberProfile()
    {
        CreateMap<UpdateCommitteeMemberCommand, CommitteeMember>()
            .ForAllMembers(opt => opt
                .Condition((src, dest, srcMember) => srcMember != null && !string.IsNullOrEmpty(srcMember.ToString())));

        CreateMap<CommitteeMember, CommitteeMemberResult>();
        CreateMap<CreateCommitteeMemberCommand, CommitteeMember>();
        
        CreateMap<CommitteeMember, CommitteeMemberCreatedEvent>();
        CreateMap<CommitteeMember, CommitteeMemberUpdatedEvent>();
    }
    
}