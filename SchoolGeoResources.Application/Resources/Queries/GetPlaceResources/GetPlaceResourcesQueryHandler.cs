namespace SchoolGeoResources.Application.Resources.Queries.GetPlaceResources;

using MediatR;
using Microsoft.EntityFrameworkCore;
using SchoolGeoResources.Application.Common.Interfaces;
using SchoolGeoResources.Domain.Aggregates.ResourceAggregate;
using SchoolGeoResources.Domain.ValueObjects;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public class GetPlaceResourcesQueryHandler : IRequestHandler<GetPlaceResourcesQuery, List<ResourceDto>>
{
    private readonly IApplicationDbContext _context;

    public GetPlaceResourcesQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<ResourceDto>> Handle(GetPlaceResourcesQuery request, CancellationToken cancellationToken)
    {
        return await _context.Resources
            .Where(r => r.PlaceId == request.PlaceId && r.State == PublicationState.Published)
            .Select(r => new ResourceDto
            {
                Id = r.Id,
                Title = r.Title,
                Type = r.Type,
                State = r.State,
                MediaUrl = r.MediaUrl,
                PlaceId = r.PlaceId
            })
            .ToListAsync(cancellationToken);
    }
}
