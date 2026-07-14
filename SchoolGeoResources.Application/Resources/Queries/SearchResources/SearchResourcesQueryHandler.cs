namespace SchoolGeoResources.Application.Resources.Queries.SearchResources;

using MediatR;
using Microsoft.EntityFrameworkCore;
using SchoolGeoResources.Application.Common.Interfaces;
using SchoolGeoResources.Application.Common.Mappings;
using SchoolGeoResources.Application.Common.Models;
using SchoolGeoResources.Domain.ValueObjects;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public class SearchResourcesQueryHandler : IRequestHandler<SearchResourcesQuery, PaginatedList<ResourceSearchResultDto>>
{
    private readonly IApplicationDbContext _context;

    public SearchResourcesQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<ResourceSearchResultDto>> Handle(SearchResourcesQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Resources.Where(r => r.State == PublicationState.Published);

        if (request.PlaceId.HasValue)
        {
            query = query.Where(r => r.PlaceId == request.PlaceId.Value);
        }

        if (request.Type.HasValue)
        {
            query = query.Where(r => r.Type == request.Type.Value);
        }

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var searchTerm = request.SearchTerm.ToLower();
            query = query.Where(r => r.Title.ToLower().Contains(searchTerm));
        }

        var joinedQuery = from r in query
                          join p in _context.Places on r.PlaceId equals p.Id
                          select new { Resource = r, PlaceName = p.Name };

        if (!string.IsNullOrWhiteSpace(request.Tag))
        {
            var results = await joinedQuery.ToListAsync(cancellationToken);
            var tagToLower = request.Tag.ToLower();
            var filtered = results.Where(x => x.Resource.Tags.Tags.Any(t => t.ToLower() == tagToLower));
            
            var projected = filtered.Select(x => new ResourceSearchResultDto
            {
                Id = x.Resource.Id,
                Title = x.Resource.Title,
                Type = x.Resource.Type.ToString(),
                Visibility = x.Resource.Visibility.ToString(),
                Tags = x.Resource.Tags.Tags.ToList(),
                PlaceId = x.Resource.PlaceId,
                PlaceName = x.PlaceName
            });

            return projected.PaginatedList(request.PageNumber, request.PageSize);
        }
        else
        {
            var projected = joinedQuery.Select(x => new ResourceSearchResultDto
            {
                Id = x.Resource.Id,
                Title = x.Resource.Title,
                Type = x.Resource.Type.ToString(),
                Visibility = x.Resource.Visibility.ToString(),
                Tags = x.Resource.Tags.Tags.ToList(),
                PlaceId = x.Resource.PlaceId,
                PlaceName = x.PlaceName
            });

            return await projected.PaginatedListAsync(request.PageNumber, request.PageSize, cancellationToken);
        }
    }
}
