using ProjectOne.API.Repository;
using System.ComponentModel.DataAnnotations;

namespace ProjectOne.API
{
    public class UniqueCodeAttribute:ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var context = (TicketContext)validationContext.GetServices(typeof(TicketContext));
            var entity = context.Tickets.SingleOrDefault(t => t.TicketCode == value.ToString());
            
            if (entity != null) 
            {
                return new ValidationResult(GetErrorMessage(value.ToString()));
            }
            return ValidationResult.Success;
        }

        public string GetErrorMessage(object? ticketCode)
        {
            return $"Ticket {ticketCode} is already in use"; 
        }
    }
}
