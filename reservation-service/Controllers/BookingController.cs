using Microsoft.AspNetCore.Mvc;
using reservation_service.Events;
using reservation_service.Models;
using reservation_service.Models.DTO;
using reservation_service.Services;

namespace reservation_service.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class BookingController : ControllerBase
{
    private readonly IBookingService _bookingService;
    private readonly IEventProducer _eventProducer;

    public BookingController(IBookingService bookingService, IEventProducer eventProducer)
    {
        _bookingService = bookingService;
        _eventProducer = eventProducer;
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
            _eventProducer.Publish("BookingCreated", new BookingCreated
            {
                Id = id,
                OfferId = booking.OfferId,
                FirstClassSeats = booking.FirstClassSeats,
                SecondClassSeats = booking.SecondClassSeats,
                Price = booking.Price
            });
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
        Booking? booking = _bookingService.GetBooking(id);
        if (booking == null)
        {
            return NotFound();
        }
        _bookingService.Delete(id);
        _eventProducer.Publish("BookingCancelled", new BookingCancelled
        {
            Id = id,
            OfferId = booking.OfferId,
            FirstClassSeats = booking.FirstClassSeats,
            SecondClassSeats = booking.SecondClassSeats
        });
        return NoContent();
    }
}