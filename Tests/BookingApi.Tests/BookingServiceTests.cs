using BookingApi.Context;
using BookingApi.Exceptions;
using BookingApi.Models.Dto;
using BookingApi.Models.Entity;
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
        public async Task CreateBooking_WithTimeNotSet_ShouldReturn_BookingDetailsException()
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

        [Fact]
        public async Task CreateBooking_WithNameNotSet_ShouldReturn_BookingDetailsException()
        {
            var _bookingService = new BookingService(new BookingContext(_options));
            // Arrange
            var bookingDto = new BookingDto
            {
                BookingTime = "13:00"
            };

            // Act
            Func<Task> act = () => _bookingService.CreateBookingAsync(bookingDto);
            // Assert
            InvalidBookingDetailsException exception = await Assert.ThrowsAsync<InvalidBookingDetailsException>(act);
            Assert.Equal("Name or BookingTime cannot be empty", exception.Message);
        }
        [Fact]
        public async Task CreateBooking_WithInvalidTime_ShouldReturn_BookingDetailsException()
        {
            var _bookingService = new BookingService(new BookingContext(_options));
            // Arrange
            var bookingDto = new BookingDto
            {   
                Name = "Test",
                BookingTime = "13:00A"
            };

            // Act
            Func<Task> act = () => _bookingService.CreateBookingAsync(bookingDto);
            // Assert
            InvalidBookingDetailsException exception = await Assert.ThrowsAsync<InvalidBookingDetailsException>(act);
            Assert.Equal("BookingTime must be in the format HH:mm", exception.Message);
        }

        [Theory]
        [InlineData("08:59")]
        [InlineData("16:01")]
        public async Task CreateBooking_WithTimeOutsideOfBuinessHours_ShouldReturn_BookingDetailsException(string timeString)
        {
            var _bookingService = new BookingService(new BookingContext(_options));
            // Arrange
            var bookingDto = new BookingDto
            {
                Name = "Test",
                BookingTime = timeString
            };

            // Act
            Func<Task> act = () => _bookingService.CreateBookingAsync(bookingDto);
            // Assert
            InvalidBookingDetailsException exception = await Assert.ThrowsAsync<InvalidBookingDetailsException>(act);
            Assert.Equal("BookingTime must be between 09:00 to 16:00 hours", exception.Message);
        }

        [Theory]
        [InlineData(10)]
        public async Task CheckIfSettlementsAreFull_Will_Return_True_If_SettlementsGreaterThan_3(int intToTime)
        {
            // Arrange
            using var context = new BookingContext(_options);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            await context.AddRangeAsync(
                new Booking { BookingId = Guid.NewGuid(), BookingTime = new TimeOnly(10, 0), Name = "Test" },
                new Booking { BookingId = Guid.NewGuid(), BookingTime = new TimeOnly(10, 30), Name = "Test1" },
                new Booking { BookingId = Guid.NewGuid(), BookingTime = new TimeOnly(10, 0), Name = "Test2" },
                new Booking { BookingId = Guid.NewGuid(), BookingTime = new TimeOnly(10, 45), Name = "Test3" }
                );
            await context.SaveChangesAsync();

            var _bookingService = new BookingService(context);
            var TimeToCheck = new TimeOnly(intToTime, 0);

            // Act
            var result = await _bookingService.CheckIfSettlementsAreFullAsync(TimeToCheck);
            // Assert 
            Assert.True(result);

            context.Database.EnsureDeleted();

        }
    }
}
