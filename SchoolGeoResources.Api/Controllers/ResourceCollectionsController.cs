namespace SchoolGeoResources.Api.Controllers;

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolGeoResources.Application.ResourceCollections.Commands.CreateCollection;
using SchoolGeoResources.Application.ResourceCollections.Commands.AddResourceToCollection;
using SchoolGeoResources.Application.ResourceCollections.Queries.GetPlaceCollections;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ResourceCollectionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ResourceCollectionsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Create(CreateResourceCollectionCommand command)
    {
        var collectionId = await _mediator.Send(command);
        return Ok(collectionId);
    }

    [HttpPost("{id}/items")]
    public async Task<IActionResult> AddResource(Guid id, [FromBody] AddResourceToCollectionCommand command)
    {
        if (id != command.CollectionId)
        {
            return BadRequest("Collection ID in the URL does not match the body.");
        }

        await _mediator.Send(command);
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateCollection(Guid id, [FromBody] SchoolGeoResources.Application.ResourceCollections.Commands.UpdateCollection.UpdateResourceCollectionCommand command)
    {
        if (id != command.Id) return BadRequest("Id mismatch");
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteCollection(Guid id)
    {
        await _mediator.Send(new SchoolGeoResources.Application.ResourceCollections.Commands.DeleteCollection.DeleteResourceCollectionCommand { Id = id });
        return NoContent();
    }

    [HttpDelete("{id}/resources/{resourceId}")]
    public async Task<ActionResult> RemoveResourceFromCollection(Guid id, Guid resourceId)
    {
        var command = new SchoolGeoResources.Application.ResourceCollections.Commands.RemoveResourceFromCollection.RemoveResourceFromCollectionCommand 
        { 
            CollectionId = id, 
            ResourceId = resourceId 
        };
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<SchoolGeoResources.Application.Common.Models.PaginatedList<ResourceCollectionDto>>> GetPlaceCollections([FromQuery] Guid placeId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
    {
        var query = new GetPlaceCollectionsQuery { PlaceId = placeId, PageNumber = pageNumber, PageSize = pageSize };
        var collections = await _mediator.Send(query);
        return Ok(collections);
    }
}
