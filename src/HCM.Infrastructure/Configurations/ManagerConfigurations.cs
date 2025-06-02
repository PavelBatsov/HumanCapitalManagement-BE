using HCM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HCM.Infrastructure.Configurations
{
    public class ManagerConfigurations : IEntityTypeConfiguration<ManagerEntity>
    {
        public void Configure(EntityTypeBuilder<ManagerEntity> builder)
        {
            builder
                .HasMany(x => x.Employees)
                .WithOne(x => x.Manager)
                .HasForeignKey(x => x.ManagerId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
