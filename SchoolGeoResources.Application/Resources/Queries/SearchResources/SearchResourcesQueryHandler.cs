namespace SchoolGeoResources.Application.Resources.Queries.SearchResources;

using MediatR;
using Microsoft.EntityFrameworkCore;
using SchoolGeoResources.Application.Common.Interfaces;
using SchoolGeoResources.Domain.ValueObjects;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public class SearchResourcesQueryHandler : IRequestHandler<SearchResourcesQuery, List<ResourceSearchResultDto>>
{
    private readonly IApplicationDbContext _context;

    public SearchResourcesQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<ResourceSearchResultDto>> Handle(SearchResourcesQuery request, CancellationToken cancellationToken)
    {
        // Only return published resources for public search
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

        // We join with Places to get the PlaceName
        var joinedQuery = from r in query
                          join p in _context.Places on r.PlaceId equals p.Id
                          select new { Resource = r, PlaceName = p.Name };

        // EF Core might struggle to translate TagSet properly if it's a ValueObject containing a collection depending on configuration.
        // Assuming TagSet is stored as JSON or a separated string, we can do client-side filtering or a string Contains if configured as string.
        // For now, we fetch the results and filter by tag in-memory if Tag is specified, 
        // OR if EF Core translates it, we can do it in DB. Let's do it in DB using string matching if it's serialized as string, 
        // but since we might use a complex type, we'll fetch and filter if request.Tag is present.
        
        var results = await joinedQuery.ToListAsync(cancellationToken);

        if (!string.IsNullOrWhiteSpace(request.Tag))
        {
            var tagToLower = request.Tag.ToLower();
            results = results.Where(x => x.Resource.Tags.Tags.Any(t => t.ToLower() == tagToLower)).ToList();
        }

        return results.Select(x => new ResourceSearchResultDto
        {
            Id = x.Resource.Id,
            Title = x.Resource.Title,
            Type = x.Resource.Type.ToString(),
            Visibility = x.Resource.Visibility.ToString(),
            Tags = x.Resource.Tags.Tags.ToList(),
            PlaceId = x.Resource.PlaceId,
            PlaceName = x.PlaceName
        }).ToList();
    }
}
