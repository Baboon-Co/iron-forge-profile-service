using Domain.Abstractions;

namespace Domain.Entities;

public class Profile : IHasTimestamps
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public Guid UserId { get; set; }
    public string Nickname { get; set; } = string.Empty;

    public required PlayerStats PlayerStats { get; set; }
}