using System.ComponentModel.DataAnnotations;

namespace BookingApi.Models.Dto
{
    public class BookingDto
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Booking time is required")]
        public string BookingTime { get; set; }
    }
}
