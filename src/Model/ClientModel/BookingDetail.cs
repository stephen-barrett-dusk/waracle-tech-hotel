namespace hotel.Model.ClientModel
{
    public class BookingDetail
    {

        public BookingDetail(RoomBooking source)
        {
            Room = new HotelRoomDetail(source.Room);
            Hotel = new HotelDetail(source.Room.Hotel);
            BookingReference = source.BookingReference;
            Guests = source.Guests;
            StartDate = source.Arrival;
            EndDate = source.Departure;
        }

        public string BookingReference { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public HotelRoomDetail Room { get; set; } = null!;
        public HotelDetail Hotel { get; set; } = null;
        public int Guests { get; set; }
    }
}
