namespace SchoolGeoResources.Application.Places.Commands.UpdatePlaceState;

using MediatR;
using SchoolGeoResources.Application.Common.Interfaces;
using SchoolGeoResources.Domain.Aggregates.PlaceAggregate;
using System;
using System.Threading;
using System.Threading.Tasks;

public class UpdatePlaceStateCommandHandler : IRequestHandler<UpdatePlaceStateCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdatePlaceStateCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdatePlaceStateCommand request, CancellationToken cancellationToken)
    {
        var place = await _context.Places.FindAsync(new object[] { request.Id }, cancellationToken);

        if (place == null)
        {
            throw new Exception("Place not found");
        }

        place.ChangeState(request.NewState);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
