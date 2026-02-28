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
                UserId = UserConfiguration.ANA_PINTO,
                RoleId = RoleConfiguration.ROLE_FUNCIONARIO
            },
            new IdentityUserRole<Guid>
            {
                UserId = UserConfiguration.ANA_CASACO,
                RoleId = RoleConfiguration.ROLE_FUNCIONARIO
            },
            new IdentityUserRole<Guid>
            {
                UserId = UserConfiguration.CARLA_AZEVEDO,
                RoleId = RoleConfiguration.ROLE_FUNCIONARIO
            },
            new IdentityUserRole<Guid>
            {
                UserId = UserConfiguration.FILIPE_DIAS,
                RoleId = RoleConfiguration.ROLE_FUNCIONARIO
            },
            new IdentityUserRole<Guid>
            {
                UserId = UserConfiguration.FILOMENA_REBELO,
                RoleId = RoleConfiguration.ROLE_FUNCIONARIO
            },
            new IdentityUserRole<Guid>
            {
                UserId = UserConfiguration.JOAO_FONSECA,
                RoleId = RoleConfiguration.ROLE_FUNCIONARIO
            },
            new IdentityUserRole<Guid>
            {
                UserId = UserConfiguration.MARIA_ALVES,
                RoleId = RoleConfiguration.ROLE_FUNCIONARIO
            },
            new IdentityUserRole<Guid>
            {
                UserId = UserConfiguration.SANDRA_CONCEICAO,
                RoleId = RoleConfiguration.ROLE_FUNCIONARIO
            },
            new IdentityUserRole<Guid>
            {
                UserId = UserConfiguration.LUIS_NEVES,
                RoleId = RoleConfiguration.ROLE_FUNCIONARIO
            }

        );
    }
}