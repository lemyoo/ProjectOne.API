
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
