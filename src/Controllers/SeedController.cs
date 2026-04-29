using Microsoft.AspNetCore.Mvc;
using hotel.Service.Interface;

namespace hotel.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SeedController (ILogger<HotelController> logger, ISeedingService seedingService)
        : ControllerBase
    {

        [HttpPost]
        [Route("ResetBookings")]
        public async Task<IActionResult> Post()
        {
            try
            {
                await seedingService.BookingReset();
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, "Failed to remove all bookings");
                throw;
            }
        }

        [HttpPost]
        [Route("ResetAllData")]
        public async Task<IActionResult> ResetAllData()
        {
            try
            {
                await seedingService.FullReset();
                await seedingService.Seed();
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, "Failed to remove and reseed all data.");
                throw;
            }
        }

    }
}
