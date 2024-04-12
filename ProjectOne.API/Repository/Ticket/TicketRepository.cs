
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProjectOne.API.Dtos.InDtos;
using ProjectOne.API.Dtos.OutDtos;
using System.Collections.Generic;

namespace ProjectOne.API.Repository.Ticket
{
    public class TicketRepository : ITicketRepository
    {
        private readonly TicketContext ticketRepository;
        private readonly IMapper mapper;
        public TicketRepository(TicketContext ticketRepository, IMapper mapper)
        {
            this.ticketRepository = ticketRepository;
            this.mapper = mapper;
        }

        private async Task<bool> CheckIfTicketExistsAsync(string ticketCode)
        {
            var exists = await ticketRepository.Tickets.AnyAsync(t => t.TicketCode == ticketCode);

            if (exists)
            {
                return true;
            }
            else { return false; }
        }
        /*public string AddTicket(Models.Ticket ticket)
        {
            var contstraint = CheckIfTicketExistsAsync(ticket.TicketCode).Result;
            if (contstraint)
            {
                return $"This ticket code {ticket.TicketCode}  already exists";
            }
            ticketRepository.Tickets.Add(ticket);
            ticketRepository.SaveChanges();
            return $"Successful";
        }*/

        /*public List<OutExcelDto> SendAllTicketsForVerifiactionAsync(List<Models.Ticket> tickets)
        {
            List<OutExcelDto> outExcelDtos = new List<OutExcelDto>();

            foreach (var ticket in tickets)
            {
                OutExcelDto outExcelDto = new OutExcelDto();

                outExcelDto.TicketCode = ticket.TicketCode;
                outExcelDto.DateCreated = ticket.DateCreated;
                outExcelDto.TicketType = ticket.TicketType;
                outExcelDto.DuplicateFromDb = false;
                outExcelDto.DuplicateFromExcelSheet = false;

                var exists = ticketRepository.Tickets.AnyAsync(t => t.TicketCode == ticket.TicketCode).Result;
                if(exists)
                {
                    outExcelDto.DuplicateFromDb = true;
                }

                outExcelDtos.Add(outExcelDto);
            }

            //var duplicatesFromFile = outExcelDtos.GroupBy(t => t.TicketCode).Where(group => group.Count() > 1).Select(group => group);
        
            return outExcelDtos ;
        }*/

        public async Task<List<OutExcelDto>> CheckAgainstDBAndFileAsync(List<Models.Ticket> tickets)
        {
            var outExcelDtos = new List<OutExcelDto>();
            foreach (var ticket in tickets)
            {
                var outExcelDto = new OutExcelDto();
                outExcelDto.Id = ticket.Id;
                outExcelDto.DateCreated = ticket.DateCreated;
                outExcelDto.DuplicateFromDb = await CheckIfTicketExistsAsync(ticket.TicketCode);
                outExcelDto.TicketCode = ticket.TicketCode;
                outExcelDto.DuplicateFromExcelSheet = false;
                outExcelDto.TicketType = ticket.TicketType;

                outExcelDtos.Add(outExcelDto);
            }

            CheckAgainstFile(outExcelDtos);

            return outExcelDtos;
        }

        private List<OutExcelDto> CheckAgainstFile(List<OutExcelDto> outExcelDtos)
        {
            var duplicates = outExcelDtos.GroupBy(s => s.TicketCode)
                             .Where(g => g.Count() > 1)
                             .Select(g => g.Key);

            foreach (var dupe in duplicates)
            {
                foreach (var outExcelDto in outExcelDtos)
                {
                    if (outExcelDto.TicketCode == dupe)
                    {
                        outExcelDto.DuplicateFromExcelSheet = true;
                    }
                }
            }
            return outExcelDtos;
        }

        public List<Models.Ticket> UploadDataToDbAsync(List<InExcelDto> inExcelDtos)
        {
            var tickets = mapper.Map<List<Models.Ticket>>(inExcelDtos);
            foreach(var ticket in tickets)
            {
                ticketRepository.Add(ticket);
                ticketRepository.SaveChanges();
            }
            
            return tickets;
        }
    }
}
