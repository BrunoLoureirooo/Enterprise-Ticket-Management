using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using backend.Entities.Models;

namespace backend.Repository.Configuration;

public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    internal static readonly Guid ALICE_PARKER    = Guid.Parse("e3a5e7de-975c-4d0f-ad17-cd563e62789a");
    internal static readonly Guid ANNA_COLLINS    = Guid.Parse("251e0a52-d589-4845-ada8-daeb08be76ea");
    internal static readonly Guid CAROL_GREEN     = Guid.Parse("024e414c-d9d9-477a-bcb5-c6777bc7d3f9");
    internal static readonly Guid PHILIP_DAVIS    = Guid.Parse("0eae7ece-6671-4f83-9caa-fc2d8eadd9ae");
    internal static readonly Guid FLORENCE_REED   = Guid.Parse("db21f4f6-0eb4-4905-a604-ae5ab05696a6");
    internal static readonly Guid JOHN_FOSTER     = Guid.Parse("f1cc0ba3-1050-43f7-86bd-98d4b7f7e216");
    internal static readonly Guid MARY_ALLEN      = Guid.Parse("804eef0f-2ec6-4b19-8b54-3373388f122b");
    internal static readonly Guid SANDRA_QUINN    = Guid.Parse("faf84edb-dd0b-4de9-90f9-3671335d01c7");
    internal static readonly Guid LOUIS_NEWTON    = Guid.Parse("4b93b0ab-aed4-4eb1-a40b-900229359841");
    internal static readonly Guid ADMIN           = Guid.Parse("fb874eba-9c48-4340-a690-bebb2195d379");

    internal static readonly string DEV_PASSWORD = "a";

    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        PasswordHasher<ApplicationUser> hasher = new PasswordHasher<ApplicationUser>();

        ApplicationUser aliceParker = new ApplicationUser()
        {
            Id = ALICE_PARKER,
            Email = "alice.parker@company.com",
            NormalizedEmail = "ALICE.PARKER@COMPANY.COM",
            Nome = "Alice Parker",
            EmailConfirmed = true,
            ConcurrencyStamp = Guid.Parse("e0981140-1b7c-4f33-a556-8ce519876d66").ToString(),
            SecurityStamp = Guid.Parse("82a360c3-37f9-42da-9dd2-e8af93b864fc").ToString()
        };
        aliceParker.PasswordHash = hasher.HashPassword(aliceParker, DEV_PASSWORD);
        builder.HasData(aliceParker);

        ApplicationUser annaCollins = new ApplicationUser()
        {
            Id = ANNA_COLLINS,
            Email = "anna.collins@company.com",
            NormalizedEmail = "ANNA.COLLINS@COMPANY.COM",
            Nome = "Anna Collins",
            EmailConfirmed = true,
            ConcurrencyStamp = Guid.Parse("1635a002-94d3-44ef-86bb-57748d6b5654").ToString(),
            SecurityStamp = Guid.Parse("9dce8fcf-555e-475b-a676-0336b4a2886e").ToString()
        };
        annaCollins.PasswordHash = hasher.HashPassword(annaCollins, DEV_PASSWORD);
        builder.HasData(annaCollins);

        ApplicationUser carolGreen = new ApplicationUser()
        {
            Id = CAROL_GREEN,
            Email = "carol.green@company.com",
            NormalizedEmail = "CAROL.GREEN@COMPANY.COM",
            Nome = "Carol Green",
            EmailConfirmed = true,
            ConcurrencyStamp = Guid.Parse("5701278b-d7ec-40ba-9cd6-708c44563c16").ToString(),
            SecurityStamp = Guid.Parse("75da7095-038e-4aca-970a-23122ef0e9c7").ToString()
        };
        carolGreen.PasswordHash = hasher.HashPassword(carolGreen, DEV_PASSWORD);
        builder.HasData(carolGreen);

        ApplicationUser philipDavis = new ApplicationUser()
        {
            Id = PHILIP_DAVIS,
            Email = "philip.davis@company.com",
            NormalizedEmail = "PHILIP.DAVIS@COMPANY.COM",
            Nome = "Philip Davis",
            EmailConfirmed = true,
            ConcurrencyStamp = Guid.Parse("e5607436-cab6-4d49-bf0a-2a3f595fd1f7").ToString(),
            SecurityStamp = Guid.Parse("bf601b92-1997-477c-a5b3-8958adc58474").ToString()
        };
        philipDavis.PasswordHash = hasher.HashPassword(philipDavis, DEV_PASSWORD);
        builder.HasData(philipDavis);

        ApplicationUser florenceReed = new ApplicationUser()
        {
            Id = FLORENCE_REED,
            Email = "florence.reed@company.com",
            NormalizedEmail = "FLORENCE.REED@COMPANY.COM",
            Nome = "Florence Reed",
            EmailConfirmed = true,
            ConcurrencyStamp = Guid.Parse("85ce73c8-afd1-4679-bd8b-8cf37c891da0").ToString(),
            SecurityStamp = Guid.Parse("6db4e253-246d-4ed7-b833-e0d896805c07").ToString()
        };
        florenceReed.PasswordHash = hasher.HashPassword(florenceReed, DEV_PASSWORD);
        builder.HasData(florenceReed);

        ApplicationUser johnFoster = new ApplicationUser()
        {
            Id = JOHN_FOSTER,
            Email = "john.foster@company.com",
            NormalizedEmail = "JOHN.FOSTER@COMPANY.COM",
            Nome = "John Foster",
            EmailConfirmed = true,
            ConcurrencyStamp = Guid.Parse("5e9c546c-5c8b-446b-834d-89d3fb270ac9").ToString(),
            SecurityStamp = Guid.Parse("c4225b9c-3e5c-4526-a267-b55fc1dbdde8").ToString()
        };
        johnFoster.PasswordHash = hasher.HashPassword(johnFoster, DEV_PASSWORD);
        builder.HasData(johnFoster);

        ApplicationUser maryAllen = new ApplicationUser()
        {
            Id = MARY_ALLEN,
            Email = "mary.allen@company.com",
            NormalizedEmail = "MARY.ALLEN@COMPANY.COM",
            Nome = "Mary Allen",
            EmailConfirmed = true,
            ConcurrencyStamp = Guid.Parse("57635711-b231-4e09-866c-cb8970cab382").ToString(),
            SecurityStamp = Guid.Parse("214c00b2-682f-4310-84ba-8cd7f466f20d").ToString()
        };
        maryAllen.PasswordHash = hasher.HashPassword(maryAllen, DEV_PASSWORD);
        builder.HasData(maryAllen);

        ApplicationUser sandraQuinn = new ApplicationUser()
        {
            Id = SANDRA_QUINN,
            Email = "sandra.quinn@company.com",
            NormalizedEmail = "SANDRA.QUINN@COMPANY.COM",
            Nome = "Sandra Quinn",
            EmailConfirmed = true,
            ConcurrencyStamp = Guid.Parse("e4e88c59-a13a-44b2-bedf-11fcb65c5d2b").ToString(),
            SecurityStamp = Guid.Parse("1d4e98cc-c78e-4abe-8164-aa41358c89b0").ToString()
        };
        sandraQuinn.PasswordHash = hasher.HashPassword(sandraQuinn, DEV_PASSWORD);
        builder.HasData(sandraQuinn);

        ApplicationUser louisNewton = new ApplicationUser()
        {
            Id = LOUIS_NEWTON,
            Email = "louis.newton@company.com",
            NormalizedEmail = "LOUIS.NEWTON@COMPANY.COM",
            Nome = "Louis Newton",
            EmailConfirmed = true,
            ConcurrencyStamp = Guid.Parse("c02f62ed-c493-4c6b-9925-7ed27623346f").ToString(),
            SecurityStamp = Guid.Parse("fb09937d-40b0-4999-ae4b-8c551449e296").ToString()
        };
        louisNewton.PasswordHash = hasher.HashPassword(louisNewton, DEV_PASSWORD);
        builder.HasData(louisNewton);

        ApplicationUser admin = new ApplicationUser()
        {
            Id = ADMIN,
            Email = "admin@admin.com",
            NormalizedEmail = "ADMIN@ADMIN.COM",
            Nome = "Admin",
            EmailConfirmed = true,
            ConcurrencyStamp = Guid.Parse("be7e12e8-b4d4-4136-8be0-a022c5ae1b77").ToString(),
            SecurityStamp = Guid.Parse("8f15dade-0a81-4238-ae5c-cad2ca8d0aae").ToString()
        };
        admin.PasswordHash = hasher.HashPassword(admin, DEV_PASSWORD);
        builder.HasData(admin);
    }
}
