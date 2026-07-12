namespace SchoolGeoResources.Domain.Aggregates.OrganizationUserAggregate;

using System;
using SchoolGeoResources.Domain.Common;
using SchoolGeoResources.Domain.Aggregates.OrganizationAggregate;

public class OrganizationUser : AggregateRoot
{
    public string Email { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public Guid OrganizationId { get; private set; }
    public OrganizationRole Role { get; private set; }

    private OrganizationUser() { } // EF Core

    private OrganizationUser(Guid id, string email, string firstName, string lastName, Guid organizationId, OrganizationRole role) 
        : base(id)
    {
        Email = email ?? throw new ArgumentNullException(nameof(email));
        FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
        LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
        OrganizationId = organizationId;
        Role = role;
    }

    public static OrganizationUser Create(Guid id, string email, string firstName, string lastName, Guid organizationId, OrganizationRole role)
    {
        return new OrganizationUser(id, email, firstName, lastName, organizationId, role);
    }

    public void ChangeRole(OrganizationRole newRole)
    {
        Role = newRole;
    }
}
