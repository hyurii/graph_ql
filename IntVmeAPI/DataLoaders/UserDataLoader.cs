using FirebaseAdmin;
using FirebaseAdmin.Auth;
using IntVmeAPI.Schema.Queries;

namespace IntVmeAPI.DataLoaders
{
    public class UserDataLoader : BatchDataLoader<string, User>
    {
        private const int MAX_FIREBASE_SUERS_BARCH_SIZE = 100;
        private readonly FirebaseAuth _firebaseAuth;
        public UserDataLoader(
            FirebaseApp firebaseApp,
            IBatchScheduler batchScheduler) : base(batchScheduler, new DataLoaderOptions()
            {
                MaxBatchSize = MAX_FIREBASE_SUERS_BARCH_SIZE
            })
        {
            _firebaseAuth = FirebaseAuth.GetAuth(firebaseApp);
        }

        protected override async Task<IReadOnlyDictionary<string, User>> LoadBatchAsync(
            IReadOnlyList<string> userIDs,
            CancellationToken cancellationToken)
        {
            List<UidIdentifier> identifiers = userIDs.Select(u => new UidIdentifier(u)).ToList();
            GetUsersResult results = await _firebaseAuth.GetUsersAsync(identifiers);
            return results.Users.Select(u => new User()
            {
                Id = u.Uid,
                Username = u.DisplayName,
                PhotoUrl = u.PhotoUrl,
            }).ToDictionary(u => u.Id);
        }
    }
}
