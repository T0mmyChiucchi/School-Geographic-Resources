using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using SchoolGeoResources.Application.Common.Interfaces;
using SchoolGeoResources.Application.Places.Commands.CreatePlace;
using SchoolGeoResources.Domain.Aggregates.OrganizationAggregate;
using SchoolGeoResources.Domain.Aggregates.OrganizationUserAggregate;
using SchoolGeoResources.Domain.Aggregates.PlaceAggregate;
using Xunit;

namespace SchoolGeoResources.Application.UnitTests.Places.Commands;

public class CreatePlaceCommandHandlerTests
{
    private readonly IPlaceRepository _placeRepository;
    private readonly IApplicationDbContext _context;
    private readonly IUserContextService _userContextService;
    private readonly CreatePlaceCommandHandler _handler;

    public CreatePlaceCommandHandlerTests()
    {
        _placeRepository = Substitute.For<IPlaceRepository>();
        _context = Substitute.For<IApplicationDbContext>();
        _userContextService = Substitute.For<IUserContextService>();
        
        _handler = new CreatePlaceCommandHandler(_placeRepository, _context, _userContextService);
    }

    [Fact]
    public async Task Handle_ShouldCreatePlaceAndSaveChanges()
    {
        // Arrange
        var command = new CreatePlaceCommand
        {
            Name = "Colosseo",
            Latitude = 41.8902,
            Longitude = 12.4922,
            Street = "Piazza del Colosseo",
            City = "Rome",
            PostalCode = "00184",
            CountryCode = "IT"
        };

        var orgUser = OrganizationUser.Create(Guid.NewGuid(), "test@test.com", "Test", "User", Guid.NewGuid(), OrganizationRole.OrgAdmin);
        _userContextService.GetCurrentUserAsync(Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(orgUser));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeEmpty();

        await _placeRepository.Received(1).AddAsync(Arg.Is<Place>(p => 
            p.Name == "Colosseo" && 
            p.OrganizationId == orgUser.OrganizationId), 
            Arg.Any<CancellationToken>());

        await _context.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
