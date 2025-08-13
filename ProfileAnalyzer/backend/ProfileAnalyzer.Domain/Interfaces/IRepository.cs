using ProfileAnalyzer.Domain.Entities;

namespace ProfileAnalyzer.Domain.Interfaces
{
    public interface IRepository
    {
        Task<int> CreateUser(User user);

        IQueryable<User> Users { get; }

        IQueryable<SocialAccount> SocialAccounts { get; }
    }
}
