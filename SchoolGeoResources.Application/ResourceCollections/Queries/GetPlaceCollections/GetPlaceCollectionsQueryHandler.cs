namespace SchoolGeoResources.Application.ResourceCollections.Queries.GetPlaceCollections;

using MediatR;
using Microsoft.EntityFrameworkCore;
using SchoolGeoResources.Application.Common.Interfaces;
using SchoolGeoResources.Application.Common.Mappings;
using SchoolGeoResources.Application.Common.Models;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public class GetPlaceCollectionsQueryHandler : IRequestHandler<GetPlaceCollectionsQuery, PaginatedList<ResourceCollectionDto>>
{
    private readonly IApplicationDbContext _context;

    public GetPlaceCollectionsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<ResourceCollectionDto>> Handle(GetPlaceCollectionsQuery request, CancellationToken cancellationToken)
    {
        var projection = _context.ResourceCollections
            .Include(c => c.Items)
            .Where(c => c.PlaceId == request.PlaceId && !c.IsArchived)
            .Select(c => new ResourceCollectionDto
            {
                Id = c.Id,
                Title = c.Title,
                PlaceId = c.PlaceId,
                Visibility = c.Visibility.ToString(),
                IsArchived = c.IsArchived,
                Items = c.Items.OrderBy(i => i.Order).Select(i => new CollectionItemDto
                {
                    ResourceId = i.ResourceId,
                    Order = i.Order
                }).ToList()
            });

        return await projection.PaginatedListAsync(request.PageNumber, request.PageSize, cancellationToken);
    }
}
