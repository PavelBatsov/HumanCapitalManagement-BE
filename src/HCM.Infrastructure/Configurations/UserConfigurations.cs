using HCM.Domain.Entities;
using HCM.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HCM.Infrastructure.Configurations
{
    public class UserConfigurations : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder
                .HasOne(x => x.Address)
                .WithOne(x => x.User)
                .HasForeignKey<AddressEntity>(x => x.UserId);
        }
    }
}
