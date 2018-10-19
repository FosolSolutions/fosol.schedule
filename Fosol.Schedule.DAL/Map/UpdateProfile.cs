﻿using AutoMapper;
using Fosol.Core.Extensions.Principals;
using Fosol.Core.Extensions.Strings;
using Fosol.Schedule.DAL.Interfaces;
using System;

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
                .ReverseMap()
                .ForMember(dest => dest.Key, opt => opt.MapFrom(src => (src.Key != Guid.Empty) ? src.Key : Guid.NewGuid()));
            CreateMap<Entities.Account, Models.Account>()
                .ReverseMap()
                .ForMember(dest => dest.Key, opt => opt.MapFrom(src => (src.Key != Guid.Empty) ? src.Key : Guid.NewGuid()));
            CreateMap<Entities.Calendar, Models.Calendar>()
                .ReverseMap()
                .ForMember(dest => dest.Key, opt => opt.MapFrom(src => (src.Key != Guid.Empty) ? src.Key : Guid.NewGuid()));
            CreateMap<Entities.Participant, Models.Participant>()
                .ReverseMap()
                .ForMember(dest => dest.Key, opt => opt.MapFrom(src => (src.Key != Guid.Empty) ? src.Key : Guid.NewGuid()));
            CreateMap<Entities.Event, Models.Event>()
                .ReverseMap()
                .ForMember(dest => dest.Key, opt => opt.MapFrom(src => (src.Key != Guid.Empty) ? src.Key : Guid.NewGuid()));
            CreateMap<Entities.Activity, Models.Activity>()
                .ReverseMap()
                .ForMember(dest => dest.Key, opt => opt.MapFrom(src => (src.Key != Guid.Empty) ? src.Key : Guid.NewGuid()));
            CreateMap<Entities.Opening, Models.Opening>()
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