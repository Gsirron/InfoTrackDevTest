using BookingApi.Exceptions;
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
            try
            {
                var result = await _bookingService.CreateBookingAsync(bookingDto);
                return Ok(new { bookingId = result.BookingId });
            }
            catch (InvalidBookingDetailsException e)
            {
                return BadRequest(new { Error = e.Message });
            }
            catch (BookingTimesFullException e)
            {
                return Conflict(new { Error = e.Message });
            }
            catch (Exception e)
            {
                return StatusCode(500, new { Error = e.Message });
            }
        }
    }
}
