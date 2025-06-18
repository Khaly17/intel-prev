using System;
using AutoMapper;
using Soditech.IntelPrev.Mediatheques.Persistence.Models;
using Soditech.IntelPrev.Mediatheques.Shared.Documents;
using Soditech.IntelPrev.Mediatheques.Shared.Enums;

namespace Soditech.IntelPrev.Mediatheques.Application.Documents;

public class DocumentProfile : Profile
{
    public DocumentProfile()
    {
        CreateMap<Document, DocumentResult>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()))
            .ForMember(dest => dest.FileType, opt => opt.MapFrom(src => src.FileType.ToString()));

        CreateMap<CreateDocumentCommand, Document>()
            .ForMember(m => m.Type,
                opt => opt.MapFrom(src 
                    => (DocumentType)Enum.Parse(typeof(DocumentType), src.Type)))
            .ForMember(m => m.FileType,
                opt => opt.MapFrom(src 
                    => (FileType)Enum.Parse(typeof(FileType), src.FileType)));
        
        CreateMap<UpdateDocumentCommand, Document>()

            .ForMember(m => m.Type,
                opt => opt.MapFrom(src
                    => (DocumentType)Enum.Parse(typeof(DocumentType), src.Type)))
            .ForMember(m => m.FileType,
                opt => opt.MapFrom(src
                    => (FileType)Enum.Parse(typeof(FileType), src.FileType)))
        
            .ForAllMembers(opts => opts
                .Condition((src, dest, srcMember) => srcMember != null && !string.IsNullOrEmpty(srcMember.ToString())));

    }
}