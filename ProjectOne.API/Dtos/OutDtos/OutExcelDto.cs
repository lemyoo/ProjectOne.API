namespace ProjectOne.API.Dtos.OutDtos
{
    public class OutExcelDto
    {
        public int Id { get; set; }
        public string TicketCode { get; set; }
        public DateTime DateCreated { get; set; }
        public string TicketType { get; set; }
        public bool DuplicateFromExcelSheet {  get; set; }
        public bool DuplicateFromDb { get; set; }
    }
}
