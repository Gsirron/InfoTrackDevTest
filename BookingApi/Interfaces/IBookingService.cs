using BookingApi.Models.Dto;
using BookingApi.Models.Entity;

namespace BookingApi.Interfaces
{
    public interface IBookingService
    {
        Task<Booking> CreateBookingAsync(BookingDto newBooking);
        Task<bool> CheckIfSettlementsAreFullAsync(TimeOnly bookingTime);
    }
}
