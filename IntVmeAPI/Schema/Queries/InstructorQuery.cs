using IntVmeAPI.DTOs;
using IntVmeAPI.Services;

namespace IntVmeAPI.Schema.Queries
{
    [ExtendObjectType(typeof(Query))]
    public class InstructorQuery
    {
        [UsePaging]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Instructor> GetInstructors(SchoolDBContext context)
        {
            return context.Instructors.Select(i => new Instructor
            {
                Id = i.Id,
                FirstName = i.FirstName,
                LastName = i.LastName,
                Salary = i.Salary,
            });
        }

        public async Task<Instructor?> GetInstructorById(Guid id, SchoolDBContext context)
        {
            InstructorDTO? instructorDTO = await context.Instructors.FindAsync(id);

            if (instructorDTO == null)
            {
                return null;
            }
            return new Instructor
            {
                Id = instructorDTO.Id,
                FirstName = instructorDTO.FirstName,
                LastName = instructorDTO.LastName,
                Salary = instructorDTO.Salary,
            };
        }
    }
}
