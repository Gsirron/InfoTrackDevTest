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

        /// <summary>
        /// Validates the Booking Details and then creates a new Booking if there less than 4 Bookings during that timeframe
        /// <param name="string"></param>
        /// <returns>Type of Booking</returns>
        /// <exception cref="InvalidBookingDetailsException"></exception>
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
            if (!bookingTime.IsBetween(new TimeOnly(9,00), new TimeOnly(16, 01)))
            {
                throw new InvalidBookingDetailsException("BookingTime must be between 09:00 to 16:00 hours");
            }

            if (await CheckIfSettlementsAreFullAsync(bookingTime))
            {
                throw new BookingTimesFullException($"Settlements are full for this time of {bookingTime}");
            }

            // Map to Booking Entity
            var booking = new Booking
            {
                BookingId = Guid.NewGuid(),
                Name = newBooking.Name,
                BookingTime = bookingTime
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();
            return booking;

        }
        /// <summary>
        /// Checks the Database for the number of bookings during a particular Timeframe
        /// Bookings are for 1 hour Spot is held from 9:00 to 9:59
        /// </summary>
        /// <param name="bookingTime">The Inputted Booking Time as TimeOnly</param>
        /// <returns>bool</returns>
        public async Task<bool> CheckIfSettlementsAreFullAsync(TimeOnly bookingTime)
        {
            var end = bookingTime.AddMinutes(59);
            var CurrentSettlements = await _context.Bookings
                .CountAsync(b=> b.BookingTime.IsBetween(bookingTime,end) 
                || b.BookingTime.AddMinutes(59).IsBetween(bookingTime,end));

            // If the Number of Bookings is greater than 3, then the Settlements are Full
            return CurrentSettlements > 3 ? true : false;
        }

    }
}
