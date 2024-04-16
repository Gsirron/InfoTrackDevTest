namespace BookingApi.Exceptions
{
    public class InvalidBookingDetailsException:Exception
    {
        public InvalidBookingDetailsException() { }
        public InvalidBookingDetailsException(string message) : base(message) { }
        public InvalidBookingDetailsException(string message, Exception inner) : base(message, inner) { }

    }
}
