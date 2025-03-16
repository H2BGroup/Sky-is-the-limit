using Microsoft.AspNetCore.Mvc;
using reservation_service.Models;
using reservation_service.Models.DTO;
using reservation_service.Services;

namespace reservation_service.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class ReservationController : ControllerBase
{
    private readonly IReservationService _reservationService;

    public ReservationController(IReservationService reservationService)
    {
        _reservationService = reservationService;
    }

    [HttpGet]
    public GetReservationsResponse GetReservations()
    {
        return ReservationDTOMapper.ReservationsToResponse(_reservationService.GetReservations());
    }

    [HttpGet("{id}")]
    public ActionResult<GetReservationResponse> GetReservation(string id)
    {
        Reservation? reservation = _reservationService.GetReservation(id);
        if (reservation == null)
        {
            return NotFound();
        }
        return ReservationDTOMapper.ReservationToResponse(reservation);
    }

    [HttpPut("{id}")]
    public IActionResult Create(string id, PutReservationRequest reservation)
    {
        try
        {
            _reservationService.Create(ReservationDTOMapper.RequestToReservation(id, reservation));
        } 
        catch(ArgumentException e)
        {
            return BadRequest(e.Message);
        }
        return CreatedAtAction(nameof(GetReservation), new { id = id }, reservation);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(string id)
    {
        _reservationService.Delete(id);
        return NoContent();
    }
}