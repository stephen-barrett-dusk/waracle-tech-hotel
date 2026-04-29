namespace hotel.Service.Interface
{
    public interface ISeedingService
    {

        public Task BookingReset(CancellationToken cancellationToken = default);


        public Task FullReset(CancellationToken cancellationToken = default);

        public Task Seed(CancellationToken cancellationToken = default);
    }
}
