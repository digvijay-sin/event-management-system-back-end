using AutoMapper;
using EventManagement.Core.RequestDTO;
using EventManagement.Core.ResponseDTO;
using EventManagement.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagement.Data.MappingService
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            // Event
            CreateMap<CreateEventRequestDTO, Event>();
            CreateMap<UpdateEventRequestDTO, Event>();
            CreateMap<Event, EventResponseDTO>();
            CreateMap<List<Event>, List<EventWithRsvpsDTO>>();

            // Rsvp 
            CreateMap<CreateRsvpRequestDTO, Rsvp>();
            CreateMap<Rsvp, RsvpResponseDTO>();
            CreateMap<UpdateEventRequestDTO, Rsvp>();

            // Login
            CreateMap<LoginRequestDTO, User>();
            CreateMap<User, UserResponseDTO>();

            //User
            CreateMap<RegistrationRequestDTO, User>();
            CreateMap<User, UserResponseDTO>();
            CreateMap<UpdateUserDTO, User>();

            //Role
            CreateMap<RoleRequestDTO, Role>();
            CreateMap<Role, RoleResponseDTO>();

        }   
    }
}
