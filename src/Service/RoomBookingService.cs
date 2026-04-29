using hotel.Configuration;
using hotel.Model;
using hotel.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace hotel.Service
{
    public class RoomBookingService(ILogger<RoomBookingService> logger, HotelContext dbContext, IHotelRoomService searchService)
        : IRoomBookingService
    {
        public async Task<(RoomBookingResult, RoomBooking?)> BookRoom(int roomId, int guestCount, DateTime arrival, DateTime departure, CancellationToken cancellationToken = default)
        {

            if(departure <= arrival)
            {
                logger.LogWarning("Room booking request with invalid dates: Arriving {arrival}, Departing {departure}", arrival, departure);
                return (RoomBookingResult.InvalidDates, null);
            }

            var room = await dbContext.HotelRoom
                .Include(r => r.RoomType)
                .Include(r => r.Hotel)
                .FirstOrDefaultAsync(room => room.Id == roomId);

            if (room == null)
            {
                logger.LogWarning("Room booking request with invalid room Id: {roomId}", roomId);
                return (RoomBookingResult.RoomNotFound, null);
            }
            if (room.Capacity < guestCount)
            {
                logger.LogWarning("Room booking request for more guests than room capacity. Room {roomId}, Guests/Capacity: {guest}/{capacity}", roomId, guestCount, room.Capacity);
                return (RoomBookingResult.RoomOccupancyError, null);
            }
            var isBooked = await searchService.IsRoomBooked(roomId, arrival, departure);

            if (isBooked)
            {
                logger.LogWarning("Room booking request for unavailable room. Room Id: {roomId}, Dates {from}-{to}", roomId, arrival, departure);
                return (RoomBookingResult.RoomNotAvailable, null);
            }


            //TODO: Convert booking reference to base32 for customer convenience
            var booking = new RoomBooking()
            {
                Room = room,
                Arrival = arrival,
                Departure = departure,
                Guests = guestCount,
                BookingReference = Guid.NewGuid().ToString()
            };
            await dbContext.RoomBooking.AddAsync(booking);
            await dbContext.SaveChangesAsync();
            return (RoomBookingResult.OK, booking);
        }


        public async Task<RoomBooking?> GetBooking(string reference, CancellationToken cancellationToken = default)
        {
            return await dbContext.RoomBooking
                .Include(b => b.Room)
                .ThenInclude(r=> r.RoomType)
                .Include(b => b.Room)
                .ThenInclude(r => r.Hotel)
                .FirstOrDefaultAsync(booking => booking.BookingReference == reference, cancellationToken);
        }

    }
}
