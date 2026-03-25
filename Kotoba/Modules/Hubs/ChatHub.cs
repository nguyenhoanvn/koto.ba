using Kotoba.Modules.Domain.DTOs;
using Kotoba.Modules.Domain.Entities;
using Kotoba.Modules.Domain.Enums;
using Kotoba.Modules.Domain.Interfaces;
using Kotoba.Modules.Infrastructure.Data;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Identity.Client;

namespace Kotoba.Modules.Hubs
{
    public class ChatHub : Hub
    {
        private readonly KotobaDbContext _context;
        private readonly IAttachmentService _attachmentService;

        public ChatHub(KotobaDbContext context, IAttachmentService attachmentService)
        {
            _context = context;
            _attachmentService = attachmentService;
        }

        // Join room theo conversationId
        public async Task JoinConversation(string conversationId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, conversationId);
        }

        public async Task LeaveConversation(string conversationId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, conversationId);
        }

        //public async Task SendMessage(string tempId, string conversationId, string senderId, string content)
        //{
        //    // Lưu DB
        //    var message = new Message
        //    {
        //        Id = Guid.NewGuid(),
        //        ConversationId = Guid.Parse(conversationId),
        //        SenderId = senderId,
        //        Content = content,
        //        CreatedAt = DateTime.UtcNow
        //    };

        //    await _context.Messages.AddAsync(message);
        //    await _context.SaveChangesAsync();

        //    var dto = new MessageDto
        //    {
        //        TempId = tempId,
        //        MessageId = message.Id,
        //        SenderId = senderId,
        //        Content = content,
        //        ConversationId = Guid.Parse(conversationId),
        //        CreatedAt = message.CreatedAt,
        //        Status = MessageStatus.Sent
        //    };

        //    // Broadcast chỉ trong group (conversation)
        //    await Clients.Group(conversationId).SendAsync("ReceiveMessage", dto);
        //}

        //public async Task SendMessage(string tempId, string conversationId, string senderId, string content, List<string> attachmentUrls)
        //{            

        //    var message = new Message
        //    {
        //        Id = Guid.NewGuid(),
        //        ConversationId = Guid.Parse(conversationId),
        //        SenderId = senderId,
        //        Content = content,
        //        CreatedAt = DateTime.UtcNow
        //    };

        //    await _context.Messages.AddAsync(message);

        //    // ✅ Lưu attachments vào DB
        //    var attachmentDtos = new List<AttachmentDto>();
        //    foreach (var url in attachmentUrls)
        //    {
        //        var savedName = Path.GetFileName(url);
        //        var fileName = savedName; // hoặc lấy tên gốc nếu bạn truyền thêm

        //        var attachment = new Attachment
        //        {
        //            Id = Guid.NewGuid(),
        //            MessageId = message.Id,
        //            FileName = fileName,
        //            SavedName = savedName,
        //            ContentType = GetContentType(savedName),
        //            Url = url,
        //            Size = 0 // đã lưu file rồi, size có thể bỏ qua hoặc truyền thêm
        //        };

        //        _context.Attachments.Add(attachment);
        //        attachmentDtos.Add(new AttachmentDto
        //        {
        //            Id = attachment.Id,
        //            FileName = attachment.FileName,
        //            ContentType = attachment.ContentType,
        //            Url = attachment.Url,
        //            Size = attachment.Size
        //        });
        //    }

        //    await _context.SaveChangesAsync();

        //    var dto = new MessageDto
        //    {
        //        TempId = tempId,
        //        MessageId = message.Id,
        //        SenderId = senderId,
        //        Content = content,
        //        ConversationId = Guid.Parse(conversationId),
        //        CreatedAt = message.CreatedAt,
        //        Status = MessageStatus.Sent,
        //        Attachments = attachmentDtos
        //    };

        //    await Clients.Group(conversationId).SendAsync("ReceiveMessage", dto);
        //}
        public async Task SendMessage(string tempId, string conversationId, string senderId, string content, List<AttachmentDto> uploadedFiles)
        {
            var message = new Message
            {
                Id = Guid.NewGuid(),
                ConversationId = Guid.Parse(conversationId),
                SenderId = senderId,
                Content = content,
                CreatedAt = DateTime.UtcNow
            };

            await _context.Messages.AddAsync(message);

            // ✅ Lưu attachments vào DB dùng ID của Message thật
            var finalAttachmentDtos = new List<AttachmentDto>();

            if (uploadedFiles != null && uploadedFiles.Any())
            {
                foreach (var file in uploadedFiles)
                {
                    var attachment = new Attachment
                    {
                        Id = Guid.NewGuid(),
                        MessageId = message.Id, // ID thật vừa tạo
                        FileName = file.FileName,
                        SavedName = Path.GetFileName(file.Url),
                        ContentType = file.ContentType,
                        Url = file.Url,
                        Size = file.Size
                    };

                    _context.Attachments.Add(attachment);

                    finalAttachmentDtos.Add(new AttachmentDto
                    {
                        Id = attachment.Id,
                        FileName = attachment.FileName,
                        ContentType = attachment.ContentType,
                        Url = attachment.Url,
                        Size = attachment.Size
                    });
                }
            }

            // Save DB 1 lần cho cả Message và Attachments
            await _context.SaveChangesAsync();

            var dto = new MessageDto
            {
                TempId = tempId,
                MessageId = message.Id,
                SenderId = senderId,
                Content = content,
                ConversationId = Guid.Parse(conversationId),
                CreatedAt = message.CreatedAt,
                Status = MessageStatus.Sent,
                Attachments = finalAttachmentDtos // Gửi lại attachments đầy đủ ID thật để UI update
            };

            await Clients.Group(conversationId).SendAsync("ReceiveMessage", dto);
        }

        // Helper
        private string GetContentType(string fileName)
        {
            var ext = Path.GetExtension(fileName).ToLower();
            return ext switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".webp" => "image/webp",
                ".pdf" => "application/pdf",
                ".zip" => "application/zip",
                _ => "application/octet-stream"
            };
        }
    }
}
