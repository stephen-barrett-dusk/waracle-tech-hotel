using hotel.Model;

namespace hotel.Service.Interface
{
    public interface IHotelService
    {
        public Task<Hotel?> SearchByName(string? term, CancellationToken cancellationToken = default);
        public Task<IEnumerable<Hotel>> Get(CancellationToken cancellationToken = default);
        public Task<Hotel?> Get(int id, CancellationToken cancellationToken = default);
    }
}
