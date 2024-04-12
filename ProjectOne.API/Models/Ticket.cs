
namespace ProjectOne.API.Models
{
    public class Ticket
    {
        public int Id {  get; set; }
        public string TicketCode { get; set; }
        public DateTime DateCreated { get; set; }
        public string TicketType { get; set; }
    }
}
