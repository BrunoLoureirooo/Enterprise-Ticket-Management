using teams.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace teams.Repository.Configuration
{
    public class SyncedUserConfiguration : IEntityTypeConfiguration<SyncedUser>
    {
        public void Configure(EntityTypeBuilder<SyncedUser> builder)
        {
            builder.HasKey(u => u.UserId);
            builder.Property(u => u.Name).HasMaxLength(300);
            builder.Property(u => u.Email).HasMaxLength(300);

            builder.HasData(
                new SyncedUser { UserId = Guid.Parse("e3a5e7de-975c-4d0f-ad17-cd563e62789a"), Name = "Alice Parker",  Email = "alice.parker@company.com"  },
                new SyncedUser { UserId = Guid.Parse("251e0a52-d589-4845-ada8-daeb08be76ea"), Name = "Anna Collins",  Email = "anna.collins@company.com"  },
                new SyncedUser { UserId = Guid.Parse("024e414c-d9d9-477a-bcb5-c6777bc7d3f9"), Name = "Carol Green",   Email = "carol.green@company.com"   },
                new SyncedUser { UserId = Guid.Parse("0eae7ece-6671-4f83-9caa-fc2d8eadd9ae"), Name = "Philip Davis",  Email = "philip.davis@company.com"  },
                new SyncedUser { UserId = Guid.Parse("db21f4f6-0eb4-4905-a604-ae5ab05696a6"), Name = "Florence Reed", Email = "florence.reed@company.com" },
                new SyncedUser { UserId = Guid.Parse("f1cc0ba3-1050-43f7-86bd-98d4b7f7e216"), Name = "John Foster",   Email = "john.foster@company.com"   },
                new SyncedUser { UserId = Guid.Parse("804eef0f-2ec6-4b19-8b54-3373388f122b"), Name = "Mary Allen",    Email = "mary.allen@company.com"    },
                new SyncedUser { UserId = Guid.Parse("faf84edb-dd0b-4de9-90f9-3671335d01c7"), Name = "Sandra Quinn",  Email = "sandra.quinn@company.com"  },
                new SyncedUser { UserId = Guid.Parse("4b93b0ab-aed4-4eb1-a40b-900229359841"), Name = "Louis Newton",  Email = "louis.newton@company.com"  },
                new SyncedUser { UserId = Guid.Parse("fb874eba-9c48-4340-a690-bebb2195d379"), Name = "Admin",         Email = "admin@admin.com"           }
            );
        }
    }
}
