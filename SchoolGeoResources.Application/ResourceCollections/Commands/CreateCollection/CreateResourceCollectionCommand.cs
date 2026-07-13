namespace SchoolGeoResources.Application.ResourceCollections.Commands.CreateCollection;

using FluentValidation;
using MediatR;
using SchoolGeoResources.Domain.ValueObjects;
using System;

public class CreateResourceCollectionCommand : IRequest<Guid>
{
    public string Title { get; set; } = string.Empty;
    public Guid PlaceId { get; set; }
    public Visibility Visibility { get; set; }
}

public class CreateResourceCollectionCommandValidator : AbstractValidator<CreateResourceCollectionCommand>
{
    public CreateResourceCollectionCommandValidator()
    {
        RuleFor(v => v.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters.");

        RuleFor(v => v.PlaceId)
            .NotEmpty().WithMessage("PlaceId is required.");

        RuleFor(v => v.Visibility)
            .IsInEnum().WithMessage("Visibility must be a valid enum value.");
    }
}
