namespace BookingApi.Models.Entity
{
    public class Booking
    {
        public Guid BookingId { get; set; }
        public string Name { get; set; }
        public TimeOnly BookingTime { get; set; }

    }
}
