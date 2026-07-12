namespace SchoolGeoResources.Application.Common.Interfaces;

using System;
using System.Threading;
using System.Threading.Tasks;
using SchoolGeoResources.Domain.Common;

public interface IRepository<T> where T : AggregateRoot
{
    Task<T> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(T entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(T entity, CancellationToken cancellationToken = default);
}
