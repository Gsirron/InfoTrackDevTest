using BookingApi.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace BookingApi.Context
{
    public class BookingContext: DbContext
    {
        public BookingContext(DbContextOptions<BookingContext> options) : base(options)
        {
        }

        public DbSet<Booking> Bookings { get; set; }
    }
}
