using Microsoft.AspNetCore.Mvc;
using reservation_service.Models;
using reservation_service.Models.DTO;
using reservation_service.Services;
using reservation_service.Events;
using shared.Events;

namespace reservation_service.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        private readonly Publisher _publisher;

        public BookingController(IBookingService bookingService, Publisher publisher)
        {
            _bookingService = bookingService;
            _publisher = publisher;
        }

        [HttpGet]
        public GetBookingsResponse GetBookings()
        {
            return BookingDTOMapper.BookingsToResponse(_bookingService.GetBookings());
        }

        [HttpGet("{id}")]
        public ActionResult<GetBookingResponse> GetBooking(string id)
        {
            Booking? booking = _bookingService.GetBooking(id);
            if (booking == null)
            {
                return NotFound();
            }
            return BookingDTOMapper.BookingToResponse(booking);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Create(string id, PutBookingRequest booking)
        {
            try
            {
                _bookingService.Create(BookingDTOMapper.RequestToBooking(id, booking));
                await _publisher.Publish(new BookingCreatedEvent{
                    Id = id,
                    OfferId = booking.OfferId,
                    FirstClassSeats = booking.FirstClassSeats,
                    SecondClassSeats = booking.SecondClassSeats,
                    Price = booking.Price,
                });
            } 
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
            return CreatedAtAction(nameof(GetBooking), new { id = id }, booking);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            Booking? booking = _bookingService.GetBooking(id);
            if (booking == null || booking.Status != BookingStatus.Confirmed)
            {
                return NotFound();
            }
            booking.Status = BookingStatus.Cancelled;
            booking.StatusTime = DateTime.UtcNow;
            _bookingService.Update(booking);
            await _publisher.Publish(new BookingCancelledEvent{
                Id = id,
                OfferId = booking.OfferId,
                FirstClassSeats = booking.FirstClassSeats,
                SecondClassSeats = booking.SecondClassSeats,
            });
            return NoContent();
        }
    }
}