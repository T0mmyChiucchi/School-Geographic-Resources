namespace SchoolGeoResources.Application.Resources.Commands.DeleteResource;

using MediatR;
using Microsoft.EntityFrameworkCore;
using SchoolGeoResources.Application.Common.Interfaces;
using SchoolGeoResources.Domain.Aggregates.ResourceAggregate;
using SchoolGeoResources.Domain.ValueObjects;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public class DeleteResourceCommandHandler : IRequestHandler<DeleteResourceCommand, Unit>
{
    private readonly IRepository<Resource> _repository;
    private readonly IApplicationDbContext _context;
    private readonly IUserContextService _userContextService;

    public DeleteResourceCommandHandler(
        IRepository<Resource> repository, 
        IApplicationDbContext context,
        IUserContextService userContextService)
    {
        _repository = repository;
        _context = context;
        _userContextService = userContextService;
    }

    public async Task<Unit> Handle(DeleteResourceCommand request, CancellationToken cancellationToken)
    {
        var resource = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (resource == null)
            throw new Exception("Resource not found");

        await _userContextService.EnsureUserCanModifyOrganizationAsync(resource.OrganizationId, cancellationToken);

        if (resource.State == PublicationState.Published)
        {
            // Soft delete
            resource.Archive();
            await _repository.UpdateAsync(resource, cancellationToken);
        }
        else
        {
            // Hard delete
            
            // Clean up from collections
            var collections = await _context.ResourceCollections
                .Include(c => c.Items)
                .Where(c => c.Items.Any(i => i.ResourceId == request.Id))
                .ToListAsync(cancellationToken);
                
            foreach(var collection in collections)
            {
                collection.RemoveResource(request.Id);
            }

            await _repository.DeleteAsync(resource, cancellationToken);
        }

        return Unit.Value;
    }
}
