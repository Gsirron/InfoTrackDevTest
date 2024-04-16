using BookingApi.Context;
using BookingApi.Controllers;
using BookingApi.Models.Dto;
using BookingApi.Models.Entity;
using BookingApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory.Query.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApi.Tests
{
    public class BookingControllerTests
    {
        DbContextOptions<BookingContext> _options = new DbContextOptionsBuilder<BookingContext>()
            .UseInMemoryDatabase(databaseName: "BookingDb")
            .Options;

        [Fact]
        public async Task CreateBookingWIth_InvalidData_ShouldReturn_BadRequestObjectResult()
        {
            // Arrange
            var _bookingService = new BookingService(new BookingContext(_options));
            var controller = new BookingController(_bookingService);

            var bookingDto = new BookingDto
            {
                Name = "Test"
            };

            // Act
            var result = await controller.CreateBookingAsync(bookingDto);
            //Assert
            Assert.IsType<BadRequestObjectResult>(result);

        }

        [Fact]
        public async Task CreateBookingWIth_ValidData_ShouldReturn_OkObjectResult_WithGuid()
        {
            // Arrange
            var _bookingService = new BookingService(new BookingContext(_options));
            var controller = new BookingController(_bookingService);

            var bookingDto = new BookingDto
            {
                Name = "Test",
                BookingTime = "13:00"
            };

            // Act
            var result = await controller.CreateBookingAsync(bookingDto);
            //Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            // result.Value is the object that was returned from the controller in a Anonymous type
            var config = okResult.Value;
            // Check if the result has a bookingId
            var id = config.GetType().GetProperty("bookingId").GetValue(config, null);
            Assert.IsType<Guid>(id);
        }

        [Fact]
        public async Task WhenAllSettlements_AreBooked_Return_ConflictStatus()
        {
            // Arrange
            using var context = new BookingContext(_options);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            await context.AddRangeAsync(
                new Booking { BookingId = Guid.NewGuid(), BookingTime = new TimeOnly(10, 15), Name = "Test" },
                new Booking { BookingId = Guid.NewGuid(), BookingTime = new TimeOnly(10, 30), Name = "Test1" },
                new Booking { BookingId = Guid.NewGuid(), BookingTime = new TimeOnly(10, 0), Name = "Test2" },
                new Booking { BookingId = Guid.NewGuid(), BookingTime = new TimeOnly(10, 45), Name = "Test3" }
                );
            await context.SaveChangesAsync();

            var _bookingService = new BookingService(context);
            var controller = new BookingController(_bookingService);

            var bookingDto = new BookingDto
            {
                Name = "Test",
                BookingTime = "10:00"
            };

            // Act

            var result = await controller.CreateBookingAsync(bookingDto);
            //Assert
            Assert.IsType<ConflictObjectResult>(result);

        }
    }
}
