using Microsoft.AspNetCore.Mvc;
using hotel.Service.Interface;
using hotel.Model.ClientModel;

namespace hotel.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HotelController (ILogger<HotelController> logger, IHotelService hotelService, IHotelRoomService roomService)
        : ControllerBase
    {

        [HttpGet]
        [Route("Search/{name}")]
        public async Task<IActionResult> Get(string? name)
        {
            try
            {
                var hotel = await hotelService.SearchByName(name);
                if (hotel == null)
                {
                    return NotFound();
                }
                return Ok(new HotelDetail(hotel));
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, "Failed to find hotel with search term");
                logger.LogDebug(ex, "Failed to find hotel with search term {term}", name);
                throw;
            }
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var hotels = await hotelService.Get();
                return Ok(hotels.Select(h => new HotelDetail(h)).ToList());
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, "Failed to find hotel with search term");
                throw;
            }
        }

        [HttpGet]
        [Route("Availability/{hotelId}")]
        public async Task<IActionResult> CheckAvailability(int hotelId, int guestCount, DateTime fromDate, DateTime toDate)
        {
            var hotel = await hotelService.Get(hotelId);
            if (hotel == null)
            {
                logger.LogInformation("Requested invalid hotel Id: {hotelId}", hotelId);
                return NotFound();
            }
            var rooms = await roomService.FindAvailableRooms(hotelId, guestCount, fromDate, toDate);
            
            return Ok(rooms.Select(r => new HotelRoomDetail(r)).ToList());

        }

    }
}
