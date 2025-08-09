using ProfileAnalyzer.Domain.Entities;

namespace ProfileAnalyzer.Domain.Interfaces
{
    public interface IRepository
    {
        int CreateUser(User user);

        IQueryable<User> Users { get; }

        IQueryable<SocialAccount> SocialAccounts { get; }
    }
}
