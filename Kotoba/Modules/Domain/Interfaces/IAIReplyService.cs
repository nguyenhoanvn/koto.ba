using Kotoba.Modules.Domain.DTOs;

namespace Kotoba.Modules.Domain.Interfaces;

/// <summary>
/// Service for generating AI reply suggestions.
/// </summary>
public interface IAIReplyService
{
    Task<List<string>> GenerateSuggestionsAsync(AIReplyRequest request);
}
