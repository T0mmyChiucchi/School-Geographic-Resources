namespace SchoolGeoResources.Application.Places.Commands.CreatePlace;

using FluentValidation;

public class CreatePlaceCommandValidator : AbstractValidator<CreatePlaceCommand>
{
    public CreatePlaceCommandValidator()
    {
        RuleFor(v => v.Name).NotEmpty().MaximumLength(200);
        RuleFor(v => v.Latitude).InclusiveBetween(-90, 90);
        RuleFor(v => v.Longitude).InclusiveBetween(-180, 180);
        RuleFor(v => v.Street).MaximumLength(200);
        RuleFor(v => v.City).MaximumLength(100);
        RuleFor(v => v.PostalCode).MaximumLength(20);
        RuleFor(v => v.CountryCode).Length(2);
    }
}
