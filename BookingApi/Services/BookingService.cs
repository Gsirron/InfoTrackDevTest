using BookingApi.Context;
using BookingApi.Exceptions;
using BookingApi.Interfaces;
using BookingApi.Models.Dto;
using BookingApi.Models.Entity;

namespace BookingApi.Services
{
    public class BookingService:IBookingService
    {
        private readonly BookingContext _context;

        public BookingService(BookingContext context)
        {
            _context = context;
        }

        public async Task<Booking> CreateBookingAsync(BookingDto newBooking)
        {
            if (string.IsNullOrEmpty(newBooking.Name) || string.IsNullOrEmpty(newBooking.BookingTime))
            {
                throw new InvalidBookingDetailsException("Name or BookingTime cannot be empty");
            }

            var bookingTime = new TimeOnly();
            var ParseResult = TimeOnly.TryParse(newBooking.BookingTime, out bookingTime);

            if (!ParseResult)
            {
                throw new InvalidBookingDetailsException("BookingTime must be in the format HH:mm");
            }

            if (!bookingTime.IsBetween(new TimeOnly(8, 59), new TimeOnly(16, 01)))
            {
                throw new InvalidBookingDetailsException("BookingTime must be between 09:00 to 16:00 hours");
            }

            return null;

        }
    }
}
