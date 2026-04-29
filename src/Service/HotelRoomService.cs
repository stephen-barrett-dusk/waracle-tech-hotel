using hotel.Configuration;
using hotel.Model;
using hotel.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace hotel.Service
{
    public class HotelRoomService(HotelContext dbContext)
        : IHotelRoomService
    {

        public async Task<IEnumerable<HotelRoom>> FindAvailableRooms(int hotelId, int guestCount, DateTime arrival, DateTime departure, CancellationToken cancellationToken = default)
        {

            if(departure <= arrival) return new List<HotelRoom>(); //short-cut invalid dates.

            var bookings = dbContext.RoomBooking.Where(booking => arrival < booking.Departure && departure > booking.Arrival);

            return await dbContext.HotelRoom
                .Include(r => r.RoomType)
                .Where(room => room.Hotel.Id == hotelId)
                .Where(room => room.Capacity >= guestCount)
                .LeftJoin(bookings, room => room, booking => booking.Room, (room, booking) => new { Room = room, Booking = booking })
                .Where(bookedRoom => bookedRoom.Booking == null)
                .Select(bookedRoom => bookedRoom.Room)
                .ToListAsync(cancellationToken)
                ;

        }

        public async Task<bool> IsRoomBooked(int roomId, DateTime arrival, DateTime departure, CancellationToken cancellationToken = default)
        {
            return await dbContext.RoomBooking
                .Where(booking => booking.Room.Id == roomId)
                .Where(booking => arrival < booking.Departure && departure > booking.Arrival)
                .AnyAsync(cancellationToken);

        }

        public async Task<HotelRoom?> Get(int id, CancellationToken cancellationToken = default)
        {
            return await dbContext.HotelRoom.FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
        }
    }
}
