using BookingApi.Interfaces;
using BookingApi.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace BookingApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController (IBookingService bookingService): ControllerBase
    {
        private IBookingService _bookingService = bookingService;

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateBookingAsync([FromBody] BookingDto bookingDto)
        {
            return Ok();
        }
    }
}
