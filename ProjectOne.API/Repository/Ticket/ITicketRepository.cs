using ProjectOne.API.Dtos.InDtos;
using ProjectOne.API.Dtos.OutDtos;

namespace ProjectOne.API.Repository.Ticket
{
    public interface ITicketRepository
    {
        //public List<OutExcelDto> SendAllTicketsForVerifiactionAsync(List<Models.Ticket> tickets);
        public List<Models.Ticket> UploadDataToDbAsync(List<InExcelDto> inExcelDtos);
        public Task<List<OutExcelDto>> CheckAgainstDBAndFileAsync(List<Models.Ticket> tickets);
    }
}
