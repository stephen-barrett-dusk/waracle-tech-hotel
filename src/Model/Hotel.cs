namespace hotel.Model
{
    public class Hotel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public IEnumerable<HotelRoom> Rooms { get; set; } = new List<HotelRoom>();
    }
}
