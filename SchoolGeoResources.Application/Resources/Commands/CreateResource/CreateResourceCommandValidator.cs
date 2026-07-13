namespace SchoolGeoResources.Application.Resources.Commands.CreateResource;

using FluentValidation;
using SchoolGeoResources.Domain.Aggregates.ResourceAggregate;

public class CreateResourceCommandValidator : AbstractValidator<CreateResourceCommand>
{
    public CreateResourceCommandValidator()
    {
        RuleFor(v => v.Title).NotEmpty().MaximumLength(200);
        RuleFor(v => v.PlaceId).NotEmpty();
        
        When(v => v.Type == ResourceType.Document, () =>
        {
            RuleFor(v => v.FileName).NotEmpty();
            RuleFor(v => v.ContentType).NotEmpty();
            RuleFor(v => v.SizeBytes).NotNull().GreaterThan(0);
        });

        When(v => v.Type != ResourceType.Document, () =>
        {
            RuleFor(v => v.MediaUrl).NotEmpty();
        });
    }
}
