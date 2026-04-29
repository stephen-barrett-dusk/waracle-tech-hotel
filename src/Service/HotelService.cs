using hotel.Configuration;
using hotel.Model;
using hotel.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace hotel.Service
{
    public class HotelService(HotelContext dbContext)
        : IHotelService
    {
        protected IQueryable<Hotel> BaseQuery => dbContext.Hotel
                .Include(h => h.Rooms)
                .ThenInclude(r => r.RoomType);

        public async Task<Hotel?> SearchByName(string? term, CancellationToken cancellationToken = default) =>
            string.IsNullOrWhiteSpace(term)
            ? null
            : await BaseQuery.FirstOrDefaultAsync(hotel => hotel.Name == term, cancellationToken);


        public async Task<IEnumerable<Hotel>> Get(CancellationToken cancellationToken = default) =>
            await BaseQuery.ToListAsync();

        public async Task<Hotel?> Get(int id, CancellationToken cancellationToken = default) =>
            await BaseQuery.FirstOrDefaultAsync(h => h.Id == id);
    }
}
