namespace SchoolGeoResources.Application.Organizations.Commands.CreateOrganization;

using System;
using MediatR;

public class CreateOrganizationCommand : IRequest<Guid>
{
    public string Name { get; set; }
    public Guid InitialAdminUserId { get; set; }
}
