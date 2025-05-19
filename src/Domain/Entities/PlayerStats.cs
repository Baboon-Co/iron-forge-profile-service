using Domain.Abstractions;

namespace Domain.Entities;

public class PlayerStats : IHasTimestamps
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public int ProfileId { get; set; }
    public Profile? Profile { get; set; }

    public int Rating { get; set; }
    public int Wins { get; set; }
    public int Losses { get; set; }
}