namespace ProjectOne.API.Dtos.InDtos
{
    public class InExcelDto
    {
        public string TicketCode { get; set; }
        public DateTime DateCreated { get; set; }
        public string TicketType { get; set; }
        public bool DuplicateFromExcelSheet { get; set; }
        public bool DuplicateFromDb { get; set; }
    }
}
