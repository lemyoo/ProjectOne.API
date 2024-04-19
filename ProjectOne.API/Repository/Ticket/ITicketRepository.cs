using ProjectOne.API.Dtos.InDtos;
using ProjectOne.API.Dtos.OutDtos;

namespace ProjectOne.API.Repository.Ticket
{
    public interface ITicketRepository
    {
        public bool UploadDataToDbAsync(List<InExcelDto> inExcelDtos);
        public Task<List<OutExcelDto>> CheckAgainstDBAndFileAsync(List<Models.Ticket> tickets);
    }
}
