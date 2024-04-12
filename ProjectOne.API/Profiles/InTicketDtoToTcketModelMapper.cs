using AutoMapper;
using ProjectOne.API.Dtos.InDtos;
using ProjectOne.API.Models;

namespace ProjectOne.API.Profiles
{
    public class InTicketDtoToTcketModelMapper:Profile
    {
        public InTicketDtoToTcketModelMapper()
        {
            CreateMap<InExcelDto, Ticket>();
        }
    }
}
