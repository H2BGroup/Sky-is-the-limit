using Microsoft.AspNetCore.Mvc;
using OfferService.Models;
using OfferService.Models.DTO;
using OfferService.Services;

namespace OfferService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfferController : ControllerBase
    {
        private readonly Services.OfferService _offerService;

        public OfferController(Services.OfferService offerService)
        {
            _offerService = offerService;
        }

        // GET: api/Offer
        [HttpGet]
        public async Task<ActionResult<GetOffersResponse>> GetAll()
        {
            var offers = await _offerService.GetAllOffers();
            return Ok(OfferDTOMapper.OffersToResponse(offers));
        }

        // GET: api/Offer/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<GetOfferResponse>> GetById(string id)
        {
            var offer = await _offerService.GetOfferById(id);
            if (offer == null)
                return NotFound();

            return Ok(OfferDTOMapper.OfferToResponse(offer));
        }

        // PUT: api/Offer/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] PutOfferRequest request)
        {
            var newOffer = OfferDTOMapper.RequestToOffer(id, request);
            await _offerService.CreateOffer(newOffer);

            return CreatedAtAction(nameof(GetById), new { id = newOffer.Id }, null);
        }

        // DELETE: api/Offer/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var success = await _offerService.DeleteOffer(id);
            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}