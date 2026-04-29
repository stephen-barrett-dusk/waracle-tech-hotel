using hotel.Model;

namespace hotel.Service.Interface
{
    public interface IRoomBookingService
    {
        public Task<(RoomBookingResult, RoomBooking?)> BookRoom(int roomId, int guestCount, DateTime fromDate, DateTime toDate, CancellationToken cancellationToken = default);
        public Task<RoomBooking?> GetBooking(string reference, CancellationToken cancellationToken = default);


    }
}
