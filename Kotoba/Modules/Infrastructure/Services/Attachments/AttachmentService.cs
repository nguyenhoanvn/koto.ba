using Kotoba.Modules.Domain.DTOs;
using Kotoba.Modules.Domain.Enums;
using Kotoba.Modules.Domain.Interfaces;
using Kotoba.Modules.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Kotoba.Modules.Domain.Entities;

namespace Kotoba.Modules.Infrastructure.Services.Attachments
{
    public class AttachmentService : IAttachmentService
    {
        private readonly string _uploadPath;

        public AttachmentService(IConfiguration config)
        {
            _uploadPath = config["Attachments:UploadPath"] ?? "uploads";
            if (!Directory.Exists(_uploadPath))
            {
                Directory.CreateDirectory(_uploadPath);
            }
        }

        // Đổi return type sang AttachmentDto, không truyền messageId ảo vào nữa
        public async Task<AttachmentDto> SaveFilePhysicalAsync(Stream stream, string fileName, string contentType)
        {
            var ext = Path.GetExtension(fileName);
            var savedName = $"{Guid.NewGuid()}{ext}";
            var filePath = Path.Combine(_uploadPath, savedName);

            await using var fs = File.Create(filePath);
            await stream.CopyToAsync(fs);
            var size = fs.Length;

            // Chỉ trả về thông tin file đã lưu, KHÔNG lưu DB ở đây
            return new AttachmentDto
            {
                FileName = fileName,
                ContentType = contentType,
                Url = $"/uploads/{savedName}", // Thư mục public ảo của bạn
                Size = size
            };
        }
    }
}
