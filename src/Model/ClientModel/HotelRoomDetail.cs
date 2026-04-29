namespace hotel.Model.ClientModel
{
    public class HotelRoomDetail
    {
        public HotelRoomDetail(HotelRoom source)
        {
            RoomId = source.Id;
            RoomNumber = source.RoomNumber;
            RoomType = source.RoomType.Name;
            Description = source.RoomType.Description;
            Capacity = source.Capacity;
        }

        public int RoomId { get; set; }
        public string? RoomNumber { get; set; }
        public string RoomType { get; set; } = null!;
        public int Capacity { get; set; }
        public string Description { get; set; } = null!;

    }
}
