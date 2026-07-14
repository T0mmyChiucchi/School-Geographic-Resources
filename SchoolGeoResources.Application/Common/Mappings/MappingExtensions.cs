namespace SchoolGeoResources.Application.Common.Mappings;

using Microsoft.EntityFrameworkCore;
using SchoolGeoResources.Application.Common.Models;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public static class MappingExtensions
{
    public static async Task<PaginatedList<TDestination>> PaginatedListAsync<TDestination>(this IQueryable<TDestination> queryable, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        var count = await queryable.CountAsync(cancellationToken);
        var items = await queryable.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);
        return new PaginatedList<TDestination>(items, count, pageNumber, pageSize);
    }

    public static PaginatedList<TDestination> PaginatedList<TDestination>(this System.Collections.Generic.IEnumerable<TDestination> enumerable, int pageNumber, int pageSize)
    {
        var count = enumerable.Count();
        var items = enumerable.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        return new PaginatedList<TDestination>(items, count, pageNumber, pageSize);
    }
}
