using Kotoba.Modules.Domain.DTOs;
using Kotoba.Modules.Domain.Interfaces;

namespace Kotoba.Modules.Infrastructure.Services.Attachments
{
    public class AttachmentService : IAttachmentService
    {
        public Task<List<AttachmentDto>> GetAttachmentsAsync(Guid messageId)
        {
            throw new NotImplementedException();
        }

        public Task<AttachmentDto?> UploadAttachmentAsync(UploadAttachmentRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
