using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using backend.Entities.Models;

namespace backend.Repository.Configuration;

public class UserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<Guid>>
{
    public void Configure(EntityTypeBuilder<IdentityUserRole<Guid>> builder)
    {
        builder.HasData(
            new IdentityUserRole<Guid>
            {
                UserId = UserConfiguration.ADMIN,
                RoleId = RoleConfiguration.ROLE_ADMINISTRADOR,
            },
            new IdentityUserRole<Guid>
            {
                UserId = UserConfiguration.ALICE_PARKER,
                RoleId = RoleConfiguration.ROLE_FUNCIONARIO
            },
            new IdentityUserRole<Guid>
            {
                UserId = UserConfiguration.ANNA_COLLINS,
                RoleId = RoleConfiguration.ROLE_FUNCIONARIO
            },
            new IdentityUserRole<Guid>
            {
                UserId = UserConfiguration.CAROL_GREEN,
                RoleId = RoleConfiguration.ROLE_FUNCIONARIO
            },
            new IdentityUserRole<Guid>
            {
                UserId = UserConfiguration.PHILIP_DAVIS,
                RoleId = RoleConfiguration.ROLE_FUNCIONARIO
            },
            new IdentityUserRole<Guid>
            {
                UserId = UserConfiguration.FLORENCE_REED,
                RoleId = RoleConfiguration.ROLE_FUNCIONARIO
            },
            new IdentityUserRole<Guid>
            {
                UserId = UserConfiguration.JOHN_FOSTER,
                RoleId = RoleConfiguration.ROLE_FUNCIONARIO
            },
            new IdentityUserRole<Guid>
            {
                UserId = UserConfiguration.MARY_ALLEN,
                RoleId = RoleConfiguration.ROLE_FUNCIONARIO
            },
            new IdentityUserRole<Guid>
            {
                UserId = UserConfiguration.SANDRA_QUINN,
                RoleId = RoleConfiguration.ROLE_FUNCIONARIO
            },
            new IdentityUserRole<Guid>
            {
                UserId = UserConfiguration.LOUIS_NEWTON,
                RoleId = RoleConfiguration.ROLE_FUNCIONARIO
            }
        );
    }
}
