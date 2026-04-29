using hotel.Model;

namespace hotel.Service.Interface
{
    public interface IHotelRoomService
    {
        public Task<HotelRoom?> Get(int Id, CancellationToken cancellationToken = default);
        public Task<IEnumerable<HotelRoom>> FindAvailableRooms(int hotelId, int guestCount, DateTime fromDate, DateTime toDate, CancellationToken cancellationToken = default);
        public Task<bool> IsRoomBooked(int roomId, DateTime fromDate, DateTime toDate, CancellationToken cancellationToken = default);
    }
}
