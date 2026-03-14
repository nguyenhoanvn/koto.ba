using Kotoba.Modules.Domain.DTOs;
using Kotoba.Modules.Domain.Interfaces;

namespace Kotoba.Modules.Infrastructure.Services.Social
{
    public class AIReplyService : IAIReplyService
    {
        public Task<List<string>> GenerateSuggestionsAsync(AIReplyRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
