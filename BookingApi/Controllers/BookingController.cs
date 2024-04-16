using BookingApi.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace BookingApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : ControllerBase
    {
        public async Task<IActionResult> CreateBookingAsync([FromBody] BookingDto bookingDto)
        {

            return Ok();
        }
    }
}
