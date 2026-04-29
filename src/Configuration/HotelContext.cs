using hotel.Model;
using Microsoft.EntityFrameworkCore;

namespace hotel.Configuration
{
    public class HotelContext(DbContextOptions options)  : DbContext(options)
    {
        public DbSet<Hotel> Hotel { get; set; }
        public DbSet<HotelRoom> HotelRoom { get; set; }
        public DbSet<RoomType> RoomType { get; set; }
        public DbSet<RoomBooking> RoomBooking { get; set; }

    }
}
