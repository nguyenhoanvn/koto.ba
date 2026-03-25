namespace Kotoba.Modules.Domain.Interfaces
{
    public interface ICurrentThoughtService
    {
        Task<bool> SetThoughtAsync(string userId, string content);
        Task<string?> GetThoughtAsync(string userId);
    }
}
