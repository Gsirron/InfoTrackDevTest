using BookingApi.Context;
using BookingApi.Exceptions;
using BookingApi.Models.Dto;
using BookingApi.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApi.Tests
{
    public class BookingServiceTests
    {
        DbContextOptions<BookingContext> _options = new DbContextOptionsBuilder<BookingContext>()
            .UseInMemoryDatabase(databaseName: "BookingDb")
            .Options;

        [Fact]
        public async Task CreateBooking_WithTimeNotSet_Or_NameNotSet_ShouldReturn_BookingDetailsException()
        {
            var _bookingService = new BookingService(new BookingContext(_options));
            // Arrange
            var bookingDto = new BookingDto
            {
                Name = "Test"
            };

            // Act
            Func<Task> act = () => _bookingService.CreateBookingAsync(bookingDto);
            // Assert
            InvalidBookingDetailsException exception = await Assert.ThrowsAsync<InvalidBookingDetailsException>(act);
            Assert.Equal("Name or BookingTime cannot be empty", exception.Message);
        }
    }
}
