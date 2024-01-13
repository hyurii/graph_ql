using IntVmeAPI.DataLoaders;
using IntVmeAPI.DTOs;
using IntVmeAPI.Models;

namespace IntVmeAPI.Schema.Queries
{
    public class Course : ISearchResult
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public SubjectEnum Subject { get; set; }
        [IsProjected(true)]
        public Guid InstructorId { get; set; }
        [GraphQLNonNullType]
        public async Task<Instructor> Instructor([Service] InstructorDataLoader instructorDataLoader)
        {
            InstructorDTO instructorDTO = await instructorDataLoader.LoadAsync(InstructorId);
            return new Instructor()
            {
                Id = instructorDTO.Id,
                FirstName = instructorDTO.FirstName,
                LastName = instructorDTO.LastName,
                Salary = instructorDTO.Salary,
            };
        }
        public IEnumerable<Student>? Students { get; set; }

        [IsProjected(true)]
        public string CreatorId { get; set; }

        public async Task<User?> Creator([Service] UserDataLoader userDataLoader)
        {
            if (CreatorId == null)
            {
                return null;
            }

            try
            {
                return await userDataLoader.LoadAsync(CreatorId, CancellationToken.None);

            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
