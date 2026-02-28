using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using backend.Entities.Models;

namespace backend.Repository.Configuration
{
    public class RoleConfiguration : IEntityTypeConfiguration<ApplicationRole>
    {
        internal static readonly Guid ROLE_ADMINISTRADOR = Guid.Parse("418b0ac3-4dba-4ccf-883e-63fdd7ac62a9");
        internal static readonly Guid ROLE_FUNCIONARIO = Guid.Parse("52c71224-fe99-4ec0-9130-0f0de200eec9");
        internal static readonly Guid ROLE_CHEFE = Guid.Parse("1704848a-5252-4f1f-82ef-9c4a751683d9");

        public void Configure(EntityTypeBuilder<ApplicationRole> builder)
        {
            builder.HasData(
            new ApplicationRole
            {
                Id = ROLE_ADMINISTRADOR,
                Name = "Administrador",
                NormalizedName = "ADMINISTRADOR"
            },
            new ApplicationRole
            {
                Id = ROLE_FUNCIONARIO,
                Name = "Funcionario",
                NormalizedName = "FUNCIONARIO"
            },
            new ApplicationRole
            {
                Id = ROLE_CHEFE,
                Name = "Chefe",
                NormalizedName = "CHEFE"
            }
            );
        }
    }
}