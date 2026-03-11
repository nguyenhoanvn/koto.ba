using Kotoba.Core.Interfaces;
using Kotoba.Shared.DTOs;
using Kotoba.Server.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using Kotoba.Infrastructure.Services.Reactions;
using Kotoba.Domain.Enums;
using Kotoba.Infrastructure.Services.Attachments;

namespace Kotoba.Server.Controllers;

[ApiController]
[Route("api/conversations")]
public class ConversationsController : ControllerBase
{
    private readonly IConversationService _conversationService;
    private readonly IMessageService _messageService;
    private readonly IHubContext<ChatHub> _hubContext;
    private readonly IReactionService _reactionService;
    private readonly IAttachmentService _attachmentService;

    public ConversationsController(
        IConversationService conversationService,
        IMessageService messageService,
        IHubContext<ChatHub> hubContext,
        IReactionService reactionService,
        IAttachmentService attachmentService)
    {
        _conversationService = conversationService;
        _messageService = messageService;
        _hubContext = hubContext;
        _reactionService = reactionService;
        _attachmentService = attachmentService;

    }

    [HttpGet("{userId}")]
    public async Task<ActionResult<List<ConversationDto>>> GetUserConversations(string userId)
    {
        var conversations = await _conversationService.GetUserConversationsAsync(userId);
        return Ok(conversations);
    }

    [HttpGet("{conversationId:guid}/messages")]
    public async Task<ActionResult<List<MessageDto>>> GetMessages(
        Guid conversationId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var messages = await _messageService.GetMessagesAsync(
            conversationId,
            new PagingRequest { Page = page, PageSize = pageSize });
        return Ok(messages);
    }

    [HttpPost("{conversationId:guid}/messages")]
    public async Task<ActionResult<MessageDto>> SendMessage(
    Guid conversationId,
    [FromBody] SendMessageRequest request)
    {
        request.ConversationId = conversationId;
        request.SenderId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? request.SenderId;

        var message = await _messageService.SendMessageAsync(request);
        if (message == null)
            return BadRequest("Failed to send message — check conversation access.");


        await _hubContext.Clients
            .Group(conversationId.ToString())
            .SendAsync("MessageReceived", message);

        return Ok(message);
    }

    [HttpGet("{conversationId:guid}/messages/{messageId:guid}/reactions")]
    public async Task<ActionResult<List<ReactionDto>>> GetReactions(
    Guid conversationId,
    Guid messageId)
    {
        var reactions = await _reactionService.GetReactionsAsync(messageId);
        return Ok(reactions);
    }

    [HttpPost("{conversationId:guid}/messages/{messageId:guid}/reactions")]
    public async Task<ActionResult<ReactionDto>> AddOrUpdateReaction(
           Guid conversationId,
           Guid messageId,
           [FromBody] AddReactionRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        var reaction = await _reactionService.AddOrUpdateReactionAsync(userId, messageId, request.ReactionType);
        if (reaction == null)
            return NotFound("Message not found.");

        await _hubContext.Clients
            .Group(conversationId.ToString())
            .SendAsync("ReactionUpdated", reaction);

        return Ok(reaction);
    }

        [HttpDelete("{conversationId:guid}/messages/{messageId:guid}/reactions")]
    public async Task<IActionResult> RemoveReaction(Guid conversationId, Guid messageId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        var removed = await _reactionService.RemoveReactionAsync(userId, messageId);
        if (!removed) return NotFound("Reaction not found.");

        await _hubContext.Clients
            .Group(conversationId.ToString())
            .SendAsync("ReactionRemoved", new { messageId, userId });

        return NoContent();
    }

    [HttpGet("{conversationId:guid}/messages/{messageId:guid}/attachments")]
    public async Task<ActionResult<List<AttachmentDto>>> GetAttachments(
    Guid conversationId,
    Guid messageId)
    {
        var attachments = await _attachmentService.GetAttachmentsAsync(messageId);
        return Ok(attachments);
    }

    [HttpPost("{conversationId:guid}/messages/{messageId:guid}/attachments")]
    [RequestSizeLimit(50 * 1024 * 1024)] 
    public async Task<ActionResult<AttachmentDto>> UploadAttachment(
     Guid conversationId,
     Guid messageId,
     IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file provided.");

        var request = new UploadAttachmentRequest
        {
            MessageId = messageId,
            FileName = file.FileName,
            ContentType = file.ContentType,
            FileStream = file.OpenReadStream()
        };

        var attachment = await _attachmentService.UploadAttachmentAsync(request);
        if (attachment == null)
            return NotFound("Message not found.");

        return Ok(attachment);
    }



}
public sealed class AddReactionRequest
{
    public ReactionType ReactionType { get; set; }
}
