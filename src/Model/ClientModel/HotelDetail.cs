namespace hotel.Model.ClientModel
{
    public class HotelDetail
    {
        public HotelDetail(Hotel source)
        {
            HotelId = source.Id;
            Name = source.Name;
            Rooms = source.Rooms.Select(r => new HotelRoomDetail(r)).ToList();
        }

        public int HotelId { get; set; }
        public string Name { get; set; }
        public IEnumerable<HotelRoomDetail> Rooms { get; set; }

    }
}
