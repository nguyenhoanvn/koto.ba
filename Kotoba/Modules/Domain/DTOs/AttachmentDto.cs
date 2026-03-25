using Kotoba.Modules.Domain.Enums;

namespace Kotoba.Modules.Domain.DTOs;

public class AttachmentDto
{
    public Guid Id { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public Guid MessageId { get; set; }
    public string Url { get; set; } = string.Empty;
    public long Size { get; set; }
    public bool IsImage => ContentType.StartsWith("image/");
}
