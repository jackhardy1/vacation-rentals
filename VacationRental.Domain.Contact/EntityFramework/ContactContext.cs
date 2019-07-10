
namespace VacationRental.Domain.Contact.EntityFramework
{
    using Microsoft.EntityFrameworkCore;
    using VacationRental.Domain.Contact.Models;

    public class ContactContext : DbContext
    {
        public ContactContext(DbContextOptions<ContactContext> options) : base(options)
        {
        }

        public DbSet<Contact> Contacts { get; set; }
    }
}
