using ProfileAnalyzer.Domain.Entities;
using ProfileAnalyzer.Domain.Interfaces;

namespace ProfileAnalyzer.Infrastructure.Repositories
{
    public class Repository : IRepository
    {
        private readonly string _userFilePath = $"data/users.json";
        private readonly string _socialAccountsFilePath = $"data/social_accounts.json";
        private readonly IFileReader _fileReader;

        private readonly List<User> _users;
        private readonly List<SocialAccount> _socialAccounts;

        public Repository(IFileReader fileReader)
        {
            _fileReader = fileReader ?? throw new ArgumentNullException(nameof(fileReader));
            _users = _fileReader.ReadJsonFileAsync<List<User>>(_userFilePath).Result ?? [];
            _socialAccounts =
                _fileReader.ReadJsonFileAsync<List<SocialAccount>>(_socialAccountsFilePath).Result
                ?? [];
        }

        public IQueryable<User> Users => _users.AsQueryable();

        public IQueryable<SocialAccount> SocialAccounts => _socialAccounts.AsQueryable();

        public async Task<int> CreateUser(User user)
        {
            // Generate a random user ID for demonstration purposes.
            int userId = Random.Shared.Next(1, 1000);

            // Ensure the user has a unique ID and set IDs for social accounts
            user.UserId = userId;
            user.SocialAccounts.ForEach(account =>
            {
                account.UserId = userId;
                account.SocialAccountId = Random.Shared.Next(1, 1000);
            });

            // Add the user and their social accounts to the in-memory lists
            _users.Add(user);
            _socialAccounts.AddRange(user.SocialAccounts);

            // Write both files in parallel and wait for both to complete
            await _fileReader.WriteJsonFileAsync(_userFilePath, _users);

            return userId;
        }
    }
}
