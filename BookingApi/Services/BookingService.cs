using BookingApi.Context;
using BookingApi.Exceptions;
using BookingApi.Interfaces;
using BookingApi.Models.Dto;
using BookingApi.Models.Entity;
using Microsoft.EntityFrameworkCore;

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

            // Validate BookingTime is a in a Valid Format
            var bookingTime = new TimeOnly();
            var ParseResult = TimeOnly.TryParse(newBooking.BookingTime, out bookingTime);

            if (!ParseResult)
            {
                throw new InvalidBookingDetailsException("BookingTime must be in the format HH:mm");
            }

            // Checks if the BookingTime is Between 09:00 to 16:00
            if (!bookingTime.IsBetween(new TimeOnly(8,59), new TimeOnly(16, 01)))
            {
                throw new InvalidBookingDetailsException("BookingTime must be between 09:00 to 16:00 hours");
            }

            return null;

        }
        /// <summary>
        /// Checks the Database for the number of bookings during a particular Timeframe
        /// Bookings are for 1 hour Spot is held from 9:00 to 9:59
        /// </summary>
        /// <param name="bookingTime"></param>
        /// <returns></returns>
        public async Task<bool> CheckIfSettlementsAreFullAsync(TimeOnly bookingTime)
        {
            var CurrentSettlements = await _context.Bookings
                .Where(b => b.BookingTime.IsBetween(bookingTime, bookingTime.AddMinutes(59)))
                .CountAsync();

            // If the Number of Bookings is greater than 3, then the Settlements are Full
            return CurrentSettlements > 3 ? true : false;
        }

    }
}
