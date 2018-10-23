using AutoMapper;
using Fosol.Core.Extensions.Principals;
using Fosol.Core.Extensions.Strings;
using Fosol.Schedule.DAL.Interfaces;
using System;
using System.Linq;

namespace Fosol.Schedule.DAL.Map
{
    /// <summary>
    /// UpdateProfile class, provides a way to map entities and models for update operations.
    /// </summary>
    public class UpdateProfile : Profile
    {
        /// <summary>
        /// Creates a new instance of an UpdateProfile object, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="datasource"></param>
        public UpdateProfile(IDataSource datasource)
        {
            CreateMap<Entities.BaseEntity, Models.BaseModel>()
                .Include<Entities.Subscription, Models.Subscription>()
                .Include<Entities.User, Models.User>()
                .Include<Entities.Account, Models.Account>()
                .Include<Entities.Calendar, Models.Calendar>()
                .Include<Entities.Participant, Models.Participant>()
                .Include<Entities.Event, Models.Event>()
                .Include<Entities.Activity, Models.Activity>()
                .Include<Entities.Opening, Models.Opening>()
                .Include<Entities.Attribute, Models.Attribute>()
                .ForMember(dest => dest.RowVersion, opt => opt.MapFrom(src => Convert.ToBase64String(src.RowVersion)))
                .ReverseMap()
                .ForMember(dest => dest.RowVersion, opt => opt.MapFrom(src => Convert.FromBase64String(src.RowVersion)))
                .ForMember(dest => dest.UpdatedById, opt => opt.MapFrom(src => datasource.Principal.GetNameIdentifier().Value.ConvertTo<int>()))
                .ForMember(dest => dest.UpdatedOn, opt => opt.UseValue(DateTime.UtcNow));

            CreateMap<Entities.Subscription, Models.Subscription>()
                .ReverseMap()
                .ForMember(dest => dest.Key, opt => opt.MapFrom(src => (src.Key != Guid.Empty) ? src.Key : Guid.NewGuid()));
            CreateMap<Entities.User, Models.User>()
                .ForPath(dest => dest.Gender, opt => opt.MapFrom(src => src.Info.Gender))
                .ForPath(dest => dest.FirstName, opt => opt.MapFrom(src => src.Info.FirstName))
                .ForPath(dest => dest.MiddleName, opt => opt.MapFrom(src => src.Info.MiddleName))
                .ForPath(dest => dest.LastName, opt => opt.MapFrom(src => src.Info.LastName))
                .ForPath(dest => dest.Birthdate, opt => opt.MapFrom(src => src.Info.Birthdate))
                .ReverseMap()
                .ForPath(dest => dest.Info.Gender, opt => opt.MapFrom(src => src.Gender))
                .ForPath(dest => dest.Info.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForPath(dest => dest.Info.MiddleName, opt => opt.MapFrom(src => src.MiddleName))
                .ForPath(dest => dest.Info.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForPath(dest => dest.Info.Birthdate, opt => opt.MapFrom(src => src.Birthdate))
                .ForMember(dest => dest.Key, opt => opt.MapFrom(src => (src.Key != Guid.Empty) ? src.Key : Guid.NewGuid()));
            CreateMap<Entities.Account, Models.Account>()
                .ReverseMap()
                .ForMember(dest => dest.Key, opt => opt.MapFrom(src => (src.Key != Guid.Empty) ? src.Key : Guid.NewGuid()));
            CreateMap<Entities.Calendar, Models.Calendar>()
                .ForPath(dest => dest.Criteria, opt => opt.MapFrom(src => src.Criteria.Select(c => c.Criteria)))
                .ReverseMap()
                .ForMember(dest => dest.Key, opt => opt.MapFrom(src => (src.Key != Guid.Empty) ? src.Key : Guid.NewGuid()));
            CreateMap<Entities.Participant, Models.Participant>()
                .ForPath(dest => dest.ContactInfo, opt => opt.MapFrom(src => src.ContactInfo.Select(a => a.ContactInfo)))
                .ReverseMap()
                .ForMember(dest => dest.Key, opt => opt.MapFrom(src => (src.Key != Guid.Empty) ? src.Key : Guid.NewGuid()));
            CreateMap<Entities.OpeningParticipant, Models.Participant>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ParticipantId))
                .ReverseMap()
                .ForMember(dest => dest.ParticipantId, opt => opt.MapFrom(src => src.Id));
            CreateMap<Entities.Event, Models.Event>()
                .ForPath(dest => dest.Criteria, opt => opt.MapFrom(src => src.Criteria.Select(c => c.Criteria)))
                .ForPath(dest => dest.Criteria, opt => opt.MapFrom(src => src.Criteria.Select(c => c.Criteria)))
                .ReverseMap()
                .ForMember(dest => dest.Key, opt => opt.MapFrom(src => (src.Key != Guid.Empty) ? src.Key : Guid.NewGuid()));
            CreateMap<Entities.Activity, Models.Activity>()
                .ForPath(dest => dest.Criteria, opt => opt.MapFrom(src => src.Criteria.Select(c => c.Criteria)))
                .ForPath(dest => dest.Criteria, opt => opt.MapFrom(src => src.Criteria.Select(c => c.Criteria)))
                .ReverseMap()
                .ForMember(dest => dest.Key, opt => opt.MapFrom(src => (src.Key != Guid.Empty) ? src.Key : Guid.NewGuid()));
            CreateMap<Entities.Opening, Models.Opening>()
                .ForPath(dest => dest.Criteria, opt => opt.MapFrom(src => src.Criteria.Select(c => c.Criteria)))
                .ForPath(dest => dest.Applications, opt => opt.MapFrom(src => src.Applications.Select(a => a.Participant)))
                .ForPath(dest => dest.Participants, opt => opt.MapFrom(src => src.Participants.Select(a => a.Participant)))
                .ReverseMap()
                .ForMember(dest => dest.Key, opt => opt.MapFrom(src => (src.Key != Guid.Empty) ? src.Key : Guid.NewGuid()));
            CreateMap<Entities.CriteriaObject, Models.Criteria>()
                .ForMember(dest => dest.Criterion, opt => opt.MapFrom(src => src.ToString()))
                .ReverseMap()
                .ForMember(dest => dest.Criteria, opt => opt.MapFrom(src => src.Criterion));
            CreateMap<Entities.Attribute, Models.Attribute>().ReverseMap();
        }
    }
}
