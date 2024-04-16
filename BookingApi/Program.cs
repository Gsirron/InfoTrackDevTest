using BookingApi.Context;
using BookingApi.Interfaces;
using BookingApi.Services;
using Microsoft.EntityFrameworkCore;

namespace BookingApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddScoped<IBookingService, BookingService>();
            builder.Services.AddDbContext<BookingContext>(options => options.UseInMemoryDatabase("BookingDB"));
            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
