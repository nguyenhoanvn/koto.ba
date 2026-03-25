using Kotoba.Modules.Domain.DTOs;
using Kotoba.Modules.Domain.Entities;
namespace Kotoba.Modules.Domain.Interfaces;

/// <summary>
/// Service for uploading and managing file attachments.
/// </summary>
public interface IAttachmentService
{
    //Task<AttachmentDto?> UploadAttachmentAsync(UploadAttachmentRequest request);
    //Task<List<AttachmentDto>> GetAttachmentsAsync(Guid messageId);
    Task<AttachmentDto> SaveFilePhysicalAsync(Stream stream, string fileName, string contentType);

}
