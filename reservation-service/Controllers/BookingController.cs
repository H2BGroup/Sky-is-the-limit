using Microsoft.AspNetCore.Mvc;
using reservation_service.Models;
using reservation_service.Models.DTO;
using reservation_service.Services;

namespace reservation_service.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class BookingController : ControllerBase
{
    private readonly IBookingService _bookingService;

    public BookingController(IBookingService bookingService)
    {
        _bookingService = bookingService;
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
    public IActionResult Create(string id, PutBookingRequest booking)
    {
        try
        {
            _bookingService.Create(BookingDTOMapper.RequestToBooking(id, booking));
        } 
        catch(ArgumentException e)
        {
            return BadRequest(e.Message);
        }
        return CreatedAtAction(nameof(GetBooking), new { id = id }, booking);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(string id)
    {
        _bookingService.Delete(id);
        return NoContent();
    }
}