using Microsoft.AspNetCore.Mvc;
using hotel.Service.Interface;
using hotel.Model;
using hotel.Model.ClientModel;

namespace hotel.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookingController (ILogger<BookingController> logger, IRoomBookingService roomBookingService, IHotelRoomService roomService)
        : ControllerBase
    {

        private async Task<string> OccupancyError(int roomId)
        {
            var room = await roomService.Get(roomId);
            var roomName = room?.RoomNumber ?? $"roomId";
            var capacity = room?.Capacity ?? 0;
            return $"Too many guests for room {roomName}. Maximum capacity is {capacity}";

        }

        [HttpPost]
        [Route("Book")]
        public async Task<IActionResult> Post(int roomId, int guestCount, DateTime arrival, DateTime departure)
        {
            try
            {
                var (result, booking) = await roomBookingService.BookRoom(roomId, guestCount, arrival, departure);

                switch (result)
                {
                    case RoomBookingResult.RoomNotFound:
                        return NotFound();
                    case RoomBookingResult.RoomOccupancyError:
                        return BadRequest(await OccupancyError(roomId));
                    case RoomBookingResult.RoomNotAvailable:
                        return BadRequest("The requested room is not available for those dates.");
                    case RoomBookingResult.InvalidDates:
                        return BadRequest("Selected Dates are invalid. Departure date must be after arrival date.");
                    case RoomBookingResult.OK:
                        return Ok(new BookingDetail(booking));
                    default:
                        return BadRequest("Unable to fulfil booking request.");
                }
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, "Failed to remove all bookings");
                throw;
            }
        }

        [HttpGet]
        [Route("{reference}")]
        public async Task<IActionResult> Get(string reference)
        {
            var booking = await roomBookingService.GetBooking(reference);
            if(booking== null)
            {
                return NotFound();
            }

            return Ok(new BookingDetail(booking));
        }

    }
}
