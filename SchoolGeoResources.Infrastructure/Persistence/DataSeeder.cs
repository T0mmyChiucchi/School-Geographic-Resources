using Microsoft.EntityFrameworkCore;
using SchoolGeoResources.Domain.Aggregates.OrganizationAggregate;
using SchoolGeoResources.Domain.Aggregates.OrganizationUserAggregate;
using SchoolGeoResources.Domain.Aggregates.PlaceAggregate;
using SchoolGeoResources.Domain.ValueObjects;
using System;
using System.Threading.Tasks;

namespace SchoolGeoResources.Infrastructure.Persistence;

public static class DataSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        await context.Database.MigrateAsync();

        if (!await context.Set<Organization>().AnyAsync())
        {
            var orgId = Guid.NewGuid();
            var userId = Guid.Parse("00000000-0000-0000-0000-000000000001");
            
            var org = Organization.Create(orgId, "Scuola Media Garibaldi", userId);
            context.Set<Organization>().Add(org);

            var adminUser = OrganizationUser.Create(userId, "admin@test.com", "Admin", "User", orgId, OrganizationRole.OrgAdmin);
            context.Set<OrganizationUser>().Add(adminUser);

            var place = Place.Create(Guid.NewGuid(), "Sede Centrale", GeoCoordinate.Create(41.8902, 12.4922), orgId);
            var address = Address.Create("Via Garibaldi 1", "Roma", "00100", "IT");
            place.SetAddress(address);
            context.Set<Place>().Add(place);

            await context.SaveChangesAsync();
        }
    }
}
