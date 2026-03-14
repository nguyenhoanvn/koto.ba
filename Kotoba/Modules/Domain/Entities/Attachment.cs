using Kotoba.Modules.Domain.Enums;

namespace Kotoba.Modules.Domain.Entities
{
    public class Attachment
    {
        public Guid Id { get; set; }
        public Guid MessageId { get; set; }
        public string FileName { get; set; } = string.Empty;
        public FileType FileType { get; set; }
        public string FileUrl { get; set; } = string.Empty;
        public long FileSizeBytes { get; set; }
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual Message Message { get; set; } = null!;
    }
}
