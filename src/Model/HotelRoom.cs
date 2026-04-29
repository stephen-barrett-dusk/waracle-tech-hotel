namespace hotel.Model
{
    public class HotelRoom
    {
        public int Id { get; set; }
        public string? RoomNumber { get; set; }
        public Hotel Hotel { get; set; } = null!;
        public RoomType RoomType { get; set; } = null!;
        public int Capacity { get; set; }
    }
}
