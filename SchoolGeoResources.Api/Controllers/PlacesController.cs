namespace SchoolGeoResources.Api.Controllers;

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolGeoResources.Application.Places.Commands.CreatePlace;
using SchoolGeoResources.Application.Places.Queries.GetPlaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PlacesController : ControllerBase
{
    private readonly IMediator _mediator;

    public PlacesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Create(CreatePlaceCommand command)
    {
        var placeId = await _mediator.Send(command);
        return Ok(placeId);
    }

    [HttpGet]
    [AllowAnonymous] // Allow anyone to view places on the map
    public async Task<ActionResult<List<PlaceDto>>> GetPlaces(
        [FromQuery] double minLat, [FromQuery] double maxLat, 
        [FromQuery] double minLng, [FromQuery] double maxLng,
        [FromQuery] string? searchTerm = null)
    {
        var query = new GetPlacesQuery 
        { 
            MinLat = minLat, 
            MaxLat = maxLat, 
            MinLng = minLng, 
            MaxLng = maxLng,
            SearchTerm = searchTerm
        };

        var places = await _mediator.Send(query);
        return Ok(places);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdatePlace(Guid id, [FromBody] SchoolGeoResources.Application.Places.Commands.UpdatePlace.UpdatePlaceCommand command)
    {
        if (id != command.Id) return BadRequest("Id mismatch");
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeletePlace(Guid id)
    {
        await _mediator.Send(new SchoolGeoResources.Application.Places.Commands.DeletePlace.DeletePlaceCommand { Id = id });
        return NoContent();
    }
}
