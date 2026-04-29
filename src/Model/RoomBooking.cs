namespace hotel.Model
{
    public class RoomBooking
    {
        public int Id { get; set; }
        public DateTime Arrival { get; set; }
        public DateTime Departure { get; set; }
        public HotelRoom Room { get; set; } = null!;
        public string? BookingReference { get; set; }
        public int Guests { get; set; }

    }
}
