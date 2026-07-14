namespace SchoolGeoResources.Api.Controllers;

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolGeoResources.Application.Resources.Commands.CreateResource;
using SchoolGeoResources.Application.Resources.Commands.PublishResource;
using SchoolGeoResources.Application.Resources.Queries.GetPlaceResources;
using SchoolGeoResources.Application.Resources.Queries.SearchResources;
using SchoolGeoResources.Domain.Aggregates.ResourceAggregate;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ResourcesController : ControllerBase
{
    private readonly IMediator _mediator;

    public ResourcesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Create(CreateResourceCommand command)
    {
        var resourceId = await _mediator.Send(command);
        return Ok(resourceId);
    }

    [HttpPost("{id}/publish")]
    public async Task<IActionResult> Publish(Guid id)
    {
        var command = new PublishResourceCommand { ResourceId = id };
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<SchoolGeoResources.Application.Common.Models.PaginatedList<ResourceDto>>> GetPlaceResources([FromQuery] Guid placeId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
    {
        var query = new GetPlaceResourcesQuery { PlaceId = placeId, PageNumber = pageNumber, PageSize = pageSize };
        var resources = await _mediator.Send(query);
        return Ok(resources);
    }

    [HttpGet("search")]
    [AllowAnonymous]
    public async Task<ActionResult<SchoolGeoResources.Application.Common.Models.PaginatedList<ResourceSearchResultDto>>> SearchResources(
        [FromQuery] string? searchTerm = null,
        [FromQuery] string? tag = null,
        [FromQuery] ResourceType? type = null,
        [FromQuery] Guid? placeId = null,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20)
    {
        var query = new SearchResourcesQuery
        {
            SearchTerm = searchTerm,
            Tag = tag,
            Type = type,
            PlaceId = placeId,
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var results = await _mediator.Send(query);
        return Ok(results);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateResource(Guid id, [FromBody] SchoolGeoResources.Application.Resources.Commands.UpdateResource.UpdateResourceCommand command)
    {
        if (id != command.Id) return BadRequest("Id mismatch");
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteResource(Guid id)
    {
        await _mediator.Send(new SchoolGeoResources.Application.Resources.Commands.DeleteResource.DeleteResourceCommand { Id = id });
        return NoContent();
    }
}
