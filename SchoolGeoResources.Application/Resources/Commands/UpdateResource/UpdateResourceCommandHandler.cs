namespace SchoolGeoResources.Application.Resources.Commands.UpdateResource;

using MediatR;
using SchoolGeoResources.Application.Common.Interfaces;
using SchoolGeoResources.Domain.ValueObjects;
using System;
using System.Threading;
using System.Threading.Tasks;

public class UpdateResourceCommandHandler : IRequestHandler<UpdateResourceCommand, Unit>
{
    private readonly IRepository<SchoolGeoResources.Domain.Aggregates.ResourceAggregate.Resource> _repository;
    private readonly IUserContextService _userContextService;

    public UpdateResourceCommandHandler(
        IRepository<SchoolGeoResources.Domain.Aggregates.ResourceAggregate.Resource> repository,
        IUserContextService userContextService)
    {
        _repository = repository;
        _userContextService = userContextService;
    }

    public async Task<Unit> Handle(UpdateResourceCommand request, CancellationToken cancellationToken)
    {
        var resource = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (resource == null)
            throw new Exception("Resource not found");

        await _userContextService.EnsureUserCanModifyOrganizationAsync(resource.OrganizationId, cancellationToken);

        resource.UpdateBasicInfo(request.Title, request.Type);
        resource.SetVisibility(request.Visibility);
        
        if (request.MediaUrl != null)
        {
            resource.SetMediaUrl(request.MediaUrl);
        }

        if (request.Tags != null)
        {
            resource.SetTags(TagSet.Create(request.Tags));
        }

        await _repository.UpdateAsync(resource, cancellationToken);

        return Unit.Value;
    }
}
