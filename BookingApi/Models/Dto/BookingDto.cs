using System.ComponentModel.DataAnnotations;

namespace BookingApi.Models.Dto
{
    public class BookingDto
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Booking time is required")]
        // Regular expresion to ensure format is HH:mm 24 hours time
        [RegularExpression(@"^([01]?[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "Invalid time format, must be in the format HH:mm")]
        public string BookingTime { get; set; }
    }
}
