using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Databases.EntityConfigurations;

public class PlayerStatsConfiguration : IEntityTypeConfiguration<PlayerStats>
{
    public void Configure(EntityTypeBuilder<PlayerStats> builder)
    {
        builder.ToTable(b =>
        {
            b.HasCheckConstraint("CK_PlayerStats_Rating_NonNegative", "Rating >= 0");
            b.HasCheckConstraint("CK_PlayerStats_Wins_NonNegative", "Wins >= 0");
            b.HasCheckConstraint("CK_PlayerStats_Losses_NonNegative", "Losses >= 0");
        });
    }
}