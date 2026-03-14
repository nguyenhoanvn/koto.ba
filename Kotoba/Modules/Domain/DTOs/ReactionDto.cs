using Kotoba.Modules.Domain.Enums;

namespace Kotoba.Modules.Domain.DTOs;

public class ReactionDto
{
    public Guid MessageId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public ReactionType Type { get; set; }
    public DateTime CreatedAt { get; set; }
}
