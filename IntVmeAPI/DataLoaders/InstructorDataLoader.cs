using IntVmeAPI.DTOs;
using IntVmeAPI.Services.Instructors;

namespace IntVmeAPI.DataLoaders
{
    public class InstructorDataLoader : BatchDataLoader<Guid, InstructorDTO>
    {
        private readonly InstructorsRepository _instructorsRepository;
        public InstructorDataLoader(
            IBatchScheduler batchScheduler,
            DataLoaderOptions? options = null,
            InstructorsRepository instructorsRepository = null)
            : base(batchScheduler, options)
        {
            _instructorsRepository = instructorsRepository;
        }

        protected override async Task<IReadOnlyDictionary<Guid, InstructorDTO>> LoadBatchAsync(
            IReadOnlyList<Guid> keys,
            CancellationToken cancellationToken)
        {
            IEnumerable<InstructorDTO> instructors = await _instructorsRepository.GetManyByIds(keys);
            return instructors.ToDictionary(i => i.Id);
        }
    }
}
