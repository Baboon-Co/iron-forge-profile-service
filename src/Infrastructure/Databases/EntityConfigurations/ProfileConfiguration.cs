using Domain.Entities;
using Infrastructure.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Databases.EntityConfigurations;

public class ProfileConfiguration : IEntityTypeConfiguration<Profile>
{
    public void Configure(EntityTypeBuilder<Profile> builder)
    {
        builder.ToTable(b => b.HasCheckConstraint("CK_Profile_Nickname_ValidCharacters",
            $"Nickname ~ '{ValidationPatterns.NicknamePattern}'"));
        builder.Property(p => p.Nickname)
            .HasMaxLength(30);
        builder.HasIndex(p => p.Nickname)
            .IsUnique();

        builder.HasIndex(p => p.UserId)
            .IsUnique();
    }
}