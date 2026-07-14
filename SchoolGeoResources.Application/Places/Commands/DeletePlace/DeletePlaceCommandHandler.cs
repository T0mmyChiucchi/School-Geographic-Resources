namespace SchoolGeoResources.Application.Places.Commands.DeletePlace;

using MediatR;
using Microsoft.EntityFrameworkCore;
using SchoolGeoResources.Application.Common.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public class DeletePlaceCommandHandler : IRequestHandler<DeletePlaceCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly IRepository<SchoolGeoResources.Domain.Aggregates.PlaceAggregate.Place> _repository;
    private readonly IUserContextService _userContextService;

    public DeletePlaceCommandHandler(
        IApplicationDbContext context,
        IRepository<SchoolGeoResources.Domain.Aggregates.PlaceAggregate.Place> repository,
        IUserContextService userContextService)
    {
        _context = context;
        _repository = repository;
        _userContextService = userContextService;
    }

    public async Task<Unit> Handle(DeletePlaceCommand request, CancellationToken cancellationToken)
    {
        var place = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (place == null)
            throw new Exception("Place not found");

        await _userContextService.EnsureUserCanModifyOrganizationAsync(place.OrganizationId, cancellationToken);

        // Cautious approach: Block deletion if there are any resources linked to this place
        bool hasResources = await _context.Resources.AnyAsync(r => r.PlaceId == place.Id, cancellationToken);
        if (hasResources)
            throw new InvalidOperationException("Cannot delete this place because it has associated resources. Please delete or reassign the resources first.");

        await _repository.DeleteAsync(place, cancellationToken);

        return Unit.Value;
    }
}
