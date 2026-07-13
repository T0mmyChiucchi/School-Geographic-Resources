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
    public async Task<ActionResult<List<PlaceDto>>> GetPlaces([FromQuery] GetPlacesQuery query)
    {
        var places = await _mediator.Send(query);
        return Ok(places);
    }
}
