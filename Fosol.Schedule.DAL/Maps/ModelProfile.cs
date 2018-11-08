using AutoMapper;
using Fosol.Core.Extensions.Principals;
using Fosol.Core.Extensions.Strings;
using Fosol.Schedule.DAL.Interfaces;
using System;
using System.Linq;

namespace Fosol.Schedule.DAL.Maps
{
    /// <summary>
    /// ModelProfile class, provides a way to map entities and models.
    /// </summary>
    public class ModelProfile : Profile
    {
        #region Properties
        protected IDataSource DataSource { get; private set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of an ModelProfile object, and initializes it with the specified arguments.
        /// </summary>
        public ModelProfile()
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
                .Include<Entities.Schedule, Models.Schedule>()
                .Include<Entities.Tag, Models.Tag>()
                .Include<Entities.OauthAccount, Models.OauthAccount>()
                .ForMember(dest => dest.RowVersion, opt => opt.MapFrom(src => Convert.ToBase64String(src.RowVersion)))
                .ReverseMap()
                .ForMember(dest => dest.AddedById, opt => opt.MapFrom(src => src.RowVersion == null ? this.DataSource.Principal.GetUser().Value.ConvertTo<int>() : src.AddedById))
                .ForMember(dest => dest.AddedOn, opt => opt.MapFrom(src => src.RowVersion == null ? DateTime.UtcNow : src.AddedOn))
                .ForMember(dest => dest.UpdatedById, opt => opt.MapFrom(src => src.RowVersion == null ? (int?)null : this.DataSource.Principal.GetUser().Value.ConvertTo<int>()))
                .ForMember(dest => dest.UpdatedOn, opt => opt.MapFrom(src => src.RowVersion == null ? (DateTime?)null : DateTime.UtcNow))
                .ForMember(dest => dest.RowVersion, opt => opt.MapFrom(src => Convert.FromBase64String(src.RowVersion)));

            CreateMap<Entities.Subscription, Models.Subscription>()
                .ReverseMap()
                .ForMember(dest => dest.Key, opt => opt.MapFrom(src => (src.Id == 0 && src.Id == 0 && src.Key != Guid.Empty) ? src.Key : Guid.NewGuid()));

            CreateMap<Entities.Schedule, Models.Schedule>()
                .ForPath(dest => dest.Events, opt => opt.MapFrom(src => src.Events.Select(a => a.Event)))
                .ReverseMap()
                .ForMember(dest => dest.Key, opt => opt.MapFrom(src => (src.Id == 0 && src.Key != Guid.Empty) ? src.Key : Guid.NewGuid()));
            CreateMap<Entities.ScheduleEvent, Models.Event>()
                .ReverseMap()
                .ConvertUsing(src =>
                {
                    var cevent = new Entities.Event() 
                    {
                        Id = src.Id,
                        Key = src.Key,
                        Name = src.Name,
                        Description = src.Description,
                        CalendarId = src.CalendarId,
                        StartOn = src.StartOn,
                        EndOn = src.EndOn,
                        State = src.State,
                        CriteriaRule = src.CriteriaRule,
                        AddedById = src.Id == 0 ? this.DataSource.Principal.GetUser().Value.ConvertTo<int>() : src.AddedById.Value,
                        AddedOn = src.Id == 0 ? DateTime.UtcNow : src.AddedOn,
                        UpdatedById = src.Id == 0 ? (int?)null : this.DataSource.Principal.GetUser().Value.ConvertTo<int>(),
                        UpdatedOn = src.Id == 0 ? (DateTime?)null : DateTime.UtcNow,
                        RowVersion = Convert.FromBase64String(src.RowVersion)
                    };
                    return new Entities.ScheduleEvent() { EventId = src.Id, Event = cevent };
                });

            CreateMap<Entities.User, Models.User>()
                .ForPath(dest => dest.Attributes, opt => opt.MapFrom(src => src.Attributes.Select(c => c.Attribute)))
                .ForPath(dest => dest.Gender, opt => opt.MapFrom(src => src.Info.Gender))
                .ForPath(dest => dest.FirstName, opt => opt.MapFrom(src => src.Info.FirstName))
                .ForPath(dest => dest.MiddleName, opt => opt.MapFrom(src => src.Info.MiddleName))
                .ForPath(dest => dest.LastName, opt => opt.MapFrom(src => src.Info.LastName))
                .ForPath(dest => dest.Birthdate, opt => opt.MapFrom(src => src.Info.Birthdate))
                .ForPath(dest => dest.HomeAddress, opt => opt.MapFrom(src => src.Info.HomeAddress))
                .ForPath(dest => dest.WorkAddress, opt => opt.MapFrom(src => src.Info.WorkAddress))
                .ForPath(dest => dest.HomePhone, opt => opt.MapFrom(src => src.Info.HomePhone))
                .ForPath(dest => dest.MobilePhone, opt => opt.MapFrom(src => src.Info.MobilePhone))
                .ForPath(dest => dest.WorkPhone, opt => opt.MapFrom(src => src.Info.WorkPhone))
                .ReverseMap()
                .ForPath(dest => dest.Info.Gender, opt => opt.MapFrom(src => src.Gender))
                .ForPath(dest => dest.Info.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForPath(dest => dest.Info.MiddleName, opt => opt.MapFrom(src => src.MiddleName))
                .ForPath(dest => dest.Info.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForPath(dest => dest.Info.Birthdate, opt => opt.MapFrom(src => src.Birthdate))
                .ForPath(dest => dest.Info.HomeAddress, opt => opt.MapFrom(src => src.HomeAddress))
                .ForPath(dest => dest.Info.WorkAddress, opt => opt.MapFrom(src => src.WorkAddress))
                .ForPath(dest => dest.Info.HomePhone, opt => opt.MapFrom(src => src.HomePhone))
                .ForPath(dest => dest.Info.MobilePhone, opt => opt.MapFrom(src => src.MobilePhone))
                .ForPath(dest => dest.Info.WorkPhone, opt => opt.MapFrom(src => src.WorkPhone))
                .ForMember(dest => dest.Key, opt => opt.MapFrom(src => (src.Id == 0 && src.Key != Guid.Empty) ? src.Key : Guid.NewGuid()));

            CreateMap<Entities.OauthAccount, Models.OauthAccount>()
                .ReverseMap();

            CreateMap<Entities.Account, Models.Account>()
                .ReverseMap()
                .ForMember(dest => dest.Key, opt => opt.MapFrom(src => (src.Id == 0 && src.Key != Guid.Empty) ? src.Key : Guid.NewGuid()));

            CreateMap<Entities.Calendar, Models.Calendar>()
                .ForPath(dest => dest.Criteria, opt => opt.MapFrom(src => src.Criteria.Select(c => c.Criteria)))
                .ForPath(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags.Select(a => a.Tag)))
                .ReverseMap()
                .ForMember(dest => dest.Key, opt => opt.MapFrom(src => (src.Id == 0 && src.Key != Guid.Empty) ? src.Key : Guid.NewGuid()));

            CreateMap<Entities.Participant, Models.Participant>()
                .ForPath(dest => dest.ContactInfo, opt => opt.MapFrom(src => src.ContactInfo.Select(a => a.ContactInfo)))
                .ForPath(dest => dest.Attributes, opt => opt.MapFrom(src => src.Attributes.Select(a => a.Attribute)))
                .ForPath(dest => dest.WorkPhone, opt => opt.MapFrom(src => src.WorkPhone))
                .ReverseMap()
                .ForMember(dest => dest.Key, opt => opt.MapFrom(src => (src.Id == 0 && src.Key != Guid.Empty) ? src.Key : Guid.NewGuid()));

            CreateMap<Entities.Event, Models.Event>()
                .ForPath(dest => dest.Criteria, opt => opt.MapFrom(src => src.Criteria.Select(c => c.Criteria)))
                .ForPath(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags.Select(a => a.Tag)))
                .ReverseMap()
                .ForMember(dest => dest.Key, opt => opt.MapFrom(src => (src.Id == 0 && src.Key != Guid.Empty) ? src.Key : Guid.NewGuid()));

            CreateMap<Entities.Activity, Models.Activity>()
                .ForPath(dest => dest.Criteria, opt => opt.MapFrom(src => src.Criteria.Select(c => c.Criteria)))
                .ForPath(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags.Select(a => a.Tag)))
                .ReverseMap()
                .ForMember(dest => dest.Key, opt => opt.MapFrom(src => (src.Id == 0 && src.Key != Guid.Empty) ? src.Key : Guid.NewGuid()));

            CreateMap<Entities.Opening, Models.Opening>()
                .ForPath(dest => dest.Criteria, opt => opt.MapFrom(src => src.Criteria.Select(c => c.Criteria)))
                .ForPath(dest => dest.Applications, opt => opt.MapFrom(src => src.Applications.Select(a => a.Participant)))
                .ForPath(dest => dest.Participants, opt => opt.MapFrom(src => src.Participants.Select(a => a.Participant)))
                .ForPath(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags.Select(a => a.Tag)))
                .ReverseMap()
                .ForMember(dest => dest.Key, opt => opt.MapFrom(src => (src.Id == 0 && src.Key != Guid.Empty) ? src.Key : Guid.NewGuid()));

            CreateMap<Entities.OpeningParticipant, Models.Participant>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ParticipantId))
                .ReverseMap()
                .ForMember(dest => dest.ParticipantId, opt => opt.MapFrom(src => src.Id));

            CreateMap<Entities.CriteriaObject, Models.Criteria>()
                .ForMember(dest => dest.Criterion, opt => opt.MapFrom(src => src.ToString()))
                .ReverseMap()
                .ForMember(dest => dest.Criteria, opt => opt.MapFrom(src => src.Criterion));

            // Tags
            CreateMap<Entities.Tag, Models.Tag>()
                .ReverseMap();
            CreateMap<Entities.CalendarTag, Models.Tag>()
                .ReverseMap()
                .ConvertUsing(src =>
                {
                    var tag = new Entities.Tag()
                    {
                        Key = src.Key,
                        Value = src.Value,
                        AddedById = src.RowVersion == null ? this.DataSource.Principal.GetUser().Value.ConvertTo<int>() : src.AddedById.Value,
                        AddedOn = src.RowVersion == null ? DateTime.UtcNow : src.AddedOn,
                        UpdatedById = src.RowVersion == null ? (int?)null : this.DataSource.Principal.GetUser().Value.ConvertTo<int>(),
                        UpdatedOn = src.RowVersion == null ? (DateTime?)null : DateTime.UtcNow,
                        RowVersion = Convert.FromBase64String(src.RowVersion)
                    };
                    return new Entities.CalendarTag() { TagKey = src.Key, TagValue = src.Value, Tag = tag };
                });
            CreateMap<Entities.EventTag, Models.Tag>()
                .ReverseMap()
                .ConvertUsing(src =>
                {
                    var tag = new Entities.Tag()
                    {
                        Key = src.Key,
                        Value = src.Value,
                        AddedById = src.RowVersion == null ? this.DataSource.Principal.GetUser().Value.ConvertTo<int>() : src.AddedById.Value,
                        AddedOn = src.RowVersion == null ? DateTime.UtcNow : src.AddedOn,
                        UpdatedById = src.RowVersion == null ? (int?)null : this.DataSource.Principal.GetUser().Value.ConvertTo<int>(),
                        UpdatedOn = src.RowVersion == null ? (DateTime?)null : DateTime.UtcNow,
                        RowVersion = Convert.FromBase64String(src.RowVersion)
                    };
                    return new Entities.EventTag() { TagKey = src.Key, TagValue = src.Value, Tag = tag };
                });
            CreateMap<Entities.ActivityTag, Models.Tag>()
                .ReverseMap()
                .ConvertUsing(src =>
                {
                    var tag = new Entities.Tag()
                    {
                        Key = src.Key,
                        Value = src.Value,
                        AddedById = src.RowVersion == null ? this.DataSource.Principal.GetUser().Value.ConvertTo<int>() : src.AddedById.Value,
                        AddedOn = src.RowVersion == null ? DateTime.UtcNow : src.AddedOn,
                        UpdatedById = src.RowVersion == null ? (int?)null : this.DataSource.Principal.GetUser().Value.ConvertTo<int>(),
                        UpdatedOn = src.RowVersion == null ? (DateTime?)null : DateTime.UtcNow,
                        RowVersion = Convert.FromBase64String(src.RowVersion)
                    };
                    return new Entities.ActivityTag() { TagKey = src.Key, TagValue = src.Value, Tag = tag };
                });
            CreateMap<Entities.OpeningTag, Models.Tag>()
                .ReverseMap()
                .ConvertUsing(src =>
                {
                    var tag = new Entities.Tag()
                    {
                        Key = src.Key,
                        Value = src.Value,
                        AddedById = src.RowVersion == null ? this.DataSource.Principal.GetUser().Value.ConvertTo<int>() : src.AddedById.Value,
                        AddedOn = src.RowVersion == null ? DateTime.UtcNow : src.AddedOn,
                        UpdatedById = src.RowVersion == null ? (int?)null : this.DataSource.Principal.GetUser().Value.ConvertTo<int>(),
                        UpdatedOn = src.RowVersion == null ? (DateTime?)null : DateTime.UtcNow,
                        RowVersion = Convert.FromBase64String(src.RowVersion)
                    };
                    return new Entities.OpeningTag() { TagKey = src.Key, TagValue = src.Value, Tag = tag };
                });

            // Attributes
            CreateMap<Entities.Attribute, Models.Attribute>().ReverseMap();
            CreateMap<Entities.ParticipantAttribute, Models.Attribute>()
                .ReverseMap()
                .ConvertUsing(src =>
                {
                    var type = String.IsNullOrWhiteSpace(src.ValueType) ? typeof(string) : Type.GetType(src.ValueType) ?? typeof(string);
                    var attribute = src.Id == 0 ? new Entities.Attribute(src.Key, src.Value, type)
                    {
                        AddedById = src.Id == 0 ? this.DataSource.Principal.GetUser().Value.ConvertTo<int>() : src.AddedById.Value,
                        AddedOn = src.Id == 0 ? DateTime.UtcNow : src.AddedOn,
                        UpdatedById = src.Id == 0 ? (int?)null : this.DataSource.Principal.GetUser().Value.ConvertTo<int>(),
                        UpdatedOn = src.Id == 0 ? (DateTime?)null : DateTime.UtcNow,
                        RowVersion = Convert.FromBase64String(src.RowVersion)
                    } : null;
                    return new Entities.ParticipantAttribute() { AttributeId = src.Id, Attribute = attribute };
                });
        }
        #endregion

        #region Methods
        public void BindDataSource(IDataSource datasource)
        {
            this.DataSource = datasource ?? throw new ArgumentNullException(nameof(datasource));
        }
        #endregion
    }
}
