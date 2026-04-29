using hotel.Configuration;
using hotel.Model;
using hotel.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace hotel.Service
{
    public class SeedingService(ILogger<SeedingService> logger, HotelContext dbContext)
        : ISeedingService
    {

        public async Task BookingReset(CancellationToken cancellationToken = default)
        {
            await dbContext.RoomBooking.ExecuteDeleteAsync(cancellationToken);
        }


        public async Task FullReset(CancellationToken cancellationToken = default)
        {
            await dbContext.RoomBooking.ExecuteDeleteAsync(cancellationToken);
            await dbContext.HotelRoom.ExecuteDeleteAsync(cancellationToken);
            await dbContext.Hotel.ExecuteDeleteAsync(cancellationToken);
            await dbContext.RoomType.ExecuteDeleteAsync(cancellationToken);
        }

        public async Task<RoomType> SeedRoomType(string name, string description, CancellationToken cancellationToken = default)
        {
            var entry = await dbContext.RoomType.FirstOrDefaultAsync(rt => rt.Name == name);
            if (entry == null)
            {
                entry = (
                    await dbContext.RoomType.AddAsync(new RoomType()
                    {
                        Name = name,
                        Description = description
                    }, cancellationToken)
                ).Entity;
            }
            return entry;
        }

        public async Task<Hotel> SeedHotel(string name, CancellationToken cancellationToken = default)
        {
            var hotel = await dbContext.Hotel.FirstOrDefaultAsync(rt => rt.Name == name) ??
                (
                    await dbContext.Hotel.AddAsync(new Hotel()
                    {
                        Name = name
                    }, cancellationToken)
                ).Entity;
            return hotel;
        }

        public async Task<HotelRoom> SeedHotelRoom(Hotel hotel, RoomType roomType, string roomNumber, int occupancy, CancellationToken cancellationToken = default)
        {
            var entry = await dbContext.HotelRoom.FirstOrDefaultAsync(r => r.RoomNumber == roomNumber);
            if (entry == null)
            {
                entry = (await dbContext.HotelRoom.AddAsync(new HotelRoom()
                {
                    RoomNumber = roomNumber,
                    RoomType = roomType,
                    Hotel = hotel,
                    Capacity = occupancy
                }, cancellationToken))
                .Entity;
            }
            return entry;
        }




        public async Task Seed(CancellationToken cancellationToken = default)
        {

            RoomType? singleRoom = null, doubleRoom = null, deluxeRoom = null;
            Hotel? hotel1 = null, hotel2 = null;
            try
            {
                singleRoom = await SeedRoomType("Single", "Simple, Quiet, Single Bed, en-suite shower", cancellationToken);
                doubleRoom = await SeedRoomType("Double", "Double bed as standard, en-suite shower", cancellationToken);
                deluxeRoom = await SeedRoomType("Deluxe", "King size bed with fold-out single, en-suite bath and shower.", cancellationToken);
            }
            catch (Exception exception)
            {
                logger.LogCritical(exception, "Failed to add room types");
                throw;
            }

            try
            {
                hotel1 = await SeedHotel("Overlook", cancellationToken);
                hotel2 = await SeedHotel("California", cancellationToken);
            }
            catch (Exception exception)
            {
                logger.LogCritical(exception, "Failed to add hotels");
                throw;
            }

            try
            {
                await SeedHotelRoom(hotel1, singleRoom, "216", 1, cancellationToken);
                await SeedHotelRoom(hotel1, deluxeRoom, "217", 3, cancellationToken);
                await SeedHotelRoom(hotel1, doubleRoom, "218", 2, cancellationToken);
                await SeedHotelRoom(hotel1, singleRoom, "236", 1, cancellationToken);
                await SeedHotelRoom(hotel1, deluxeRoom, "237", 3, cancellationToken);
                await SeedHotelRoom(hotel1, doubleRoom, "238", 2, cancellationToken);

                await SeedHotelRoom(hotel2, singleRoom, "13-1", 1, cancellationToken);
                await SeedHotelRoom(hotel2, deluxeRoom, "13-2", 3, cancellationToken);
                await SeedHotelRoom(hotel2, doubleRoom, "13-3", 2, cancellationToken);
                await SeedHotelRoom(hotel2, singleRoom, "13-4", 1, cancellationToken);
                await SeedHotelRoom(hotel2, deluxeRoom, "13-5", 3, cancellationToken);
                await SeedHotelRoom(hotel2, doubleRoom, "13-6", 2, cancellationToken);
            }
            catch (Exception exception)
            {
                logger.LogCritical(exception, "Failed to add rooms");
                throw;
            }

            await dbContext.SaveChangesAsync();
        }


    }
}
