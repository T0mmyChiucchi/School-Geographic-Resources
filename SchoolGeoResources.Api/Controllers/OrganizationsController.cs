namespace SchoolGeoResources.Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using MediatR;
using SchoolGeoResources.Application.Organizations.Commands.CreateOrganization;
using System;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class OrganizationsController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrganizationsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrganization([FromBody] CreateOrganizationCommand command)
    {
        var organizationId = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetOrganization), new { id = organizationId }, new { Id = organizationId });
    }

    [HttpGet("{id}")]
    public IActionResult GetOrganization(Guid id)
    {
        // Placeholder for GET query
        return Ok(new { Id = id, Name = "Sample Organization" });
    }
}
