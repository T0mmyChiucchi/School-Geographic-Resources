namespace SchoolGeoResources.Application.ResourceCollections.Commands.AddResourceToCollection;

using FluentValidation;
using MediatR;
using System;

public class AddResourceToCollectionCommand : IRequest
{
    public Guid CollectionId { get; set; }
    public Guid ResourceId { get; set; }
}

public class AddResourceToCollectionCommandValidator : AbstractValidator<AddResourceToCollectionCommand>
{
    public AddResourceToCollectionCommandValidator()
    {
        RuleFor(v => v.CollectionId).NotEmpty();
        RuleFor(v => v.ResourceId).NotEmpty();
    }
}
