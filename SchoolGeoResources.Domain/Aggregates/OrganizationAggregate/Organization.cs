namespace SchoolGeoResources.Domain.Aggregates.OrganizationAggregate;

using System;
using System.Collections.Generic;
using System.Linq;
using SchoolGeoResources.Domain.Common;

public class Organization : AggregateRoot
{
    public string Name { get; private set; }
    public bool IsApproved { get; private set; }
    
    // In DDD, an Aggregate often manages its child entities. 
    // Here we store references to Members to enforce invariants (like "must have at least one OrgAdmin").
    private readonly List<OrganizationMember> _members = new();
    public IReadOnlyCollection<OrganizationMember> Members => _members.AsReadOnly();

    private Organization() { } // EF Core

    private Organization(Guid id, string name, Guid initialAdminUserId) : base(id)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Organization name cannot be empty.", nameof(name));

        Name = name;
        IsApproved = false; // Starts as draft/unapproved

        // Invariant: Una Organization deve avere almeno un membro con ruolo OrgAdmin.
        _members.Add(new OrganizationMember(initialAdminUserId, OrganizationRole.OrgAdmin));
    }

    public static Organization Create(Guid id, string name, Guid initialAdminUserId)
    {
        return new Organization(id, name, initialAdminUserId);
    }

    public void Approve()
    {
        IsApproved = true;
    }

    public void RevokeApproval()
    {
        // Invariant: Una Organization approvata non può tornare in stato Draft.
        // Wait, the PRD says "Una Organization approvata non può tornare in stato Draft."
        // Let's assume RevokeApproval is not allowed if it implies going back to draft.
        throw new InvalidOperationException("An approved organization cannot return to draft state.");
    }

    public void AddMember(Guid userId, OrganizationRole role)
    {
        // Invariant: Un utente non può avere due ruoli diversi nella stessa Organization.
        if (_members.Any(m => m.UserId == userId))
        {
            throw new InvalidOperationException("User is already a member of this organization.");
        }

        _members.Add(new OrganizationMember(userId, role));
    }

    public void ChangeMemberRole(Guid userId, OrganizationRole newRole)
    {
        var member = _members.SingleOrDefault(m => m.UserId == userId);
        if (member == null)
            throw new InvalidOperationException("User is not a member of this organization.");

        if (member.Role == OrganizationRole.OrgAdmin && newRole != OrganizationRole.OrgAdmin)
        {
            EnsureAtLeastOneOrgAdminRemains(userId);
        }

        member.UpdateRole(newRole);
    }

    public void RemoveMember(Guid userId)
    {
        var member = _members.SingleOrDefault(m => m.UserId == userId);
        if (member == null) return;

        if (member.Role == OrganizationRole.OrgAdmin)
        {
            EnsureAtLeastOneOrgAdminRemains(userId);
        }

        _members.Remove(member);
    }

    private void EnsureAtLeastOneOrgAdminRemains(Guid userIdToRemoveOrDemote)
    {
        // Invariant: L'OrgAdmin non può rimuovere se stesso se è l'unico OrgAdmin.
        var adminCount = _members.Count(m => m.Role == OrganizationRole.OrgAdmin && m.UserId != userIdToRemoveOrDemote);
        if (adminCount == 0)
        {
            throw new InvalidOperationException("Cannot remove or demote the last OrgAdmin of the organization.");
        }
    }
}

// Child entity for Organization to track members and enforce invariants
public class OrganizationMember : Entity
{
    public Guid UserId { get; private set; }
    public OrganizationRole Role { get; private set; }

    internal OrganizationMember(Guid userId, OrganizationRole role) : base(Guid.NewGuid())
    {
        UserId = userId;
        Role = role;
    }

    internal void UpdateRole(OrganizationRole newRole)
    {
        Role = newRole;
    }
}
