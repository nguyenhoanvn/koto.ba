using Kotoba.Modules.Domain.DTOs;

namespace Kotoba.Modules.Domain.Interfaces;

/// <summary>
/// Service for uploading and managing file attachments.
/// </summary>
public interface IAttachmentService
{
    Task<AttachmentDto?> UploadAttachmentAsync(UploadAttachmentRequest request);
    Task<List<AttachmentDto>> GetAttachmentsAsync(Guid messageId);
}
