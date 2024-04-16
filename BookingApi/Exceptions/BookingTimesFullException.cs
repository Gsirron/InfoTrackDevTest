namespace BookingApi.Exceptions
{
    public class BookingTimesFullException : Exception
    {
        public BookingTimesFullException(string message) : base(message) { }

        public BookingTimesFullException(string message, Exception innerException) : base(message, innerException){ }
        public BookingTimesFullException(){ }

    }
}
