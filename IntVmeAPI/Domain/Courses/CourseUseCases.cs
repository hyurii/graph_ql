using IntVmeAPI.DTOs;
using IntVmeAPI.Schema.Queries;
using IntVmeAPI.Services.Courses;

namespace IntVmeAPI.Domain.Courses
{
    public class CourseUseCases
    {
        private readonly CoursesRepository _repository;
        public CourseUseCases(CoursesRepository repository)
        {
            _repository = repository;
        }
        public IEnumerable<Course> GetAllCourses(int count, CancellationToken cancellationToken)
        {
            IEnumerable<CourseDTO> courseDTOs = _repository.GetAllPaginatedRepo(count, cancellationToken).GetAwaiter().GetResult();
            return courseDTOs.Select(c => new Course()
            {
                Id = c.Id,
                Name = c.Name,
                Subject = c.Subject,
                InstructorId = c.InstructorId,
                CreatorId = c.CreatorId,
            });
        }

        public async Task<IEnumerable<ISearchResult>> GetCourseSearch(string term, CancellationToken cancellationToken)
        {
            IEnumerable<CourseDTO> courseDTOs = await _repository.GetCoursesSearch(term, cancellationToken);
            var courses = courseDTOs.Select(c => new Course()
            {
                Id = c.Id,
                Name = c.Name,
                Subject = c.Subject,
                InstructorId = c.InstructorId,
                CreatorId = c.CreatorId,
            });

            IEnumerable<InstructorDTO> instructorDTOs = await _repository.GetInstructorsSearch(term, cancellationToken);
            var instructors = instructorDTOs.Select(c => new Instructor()
            {
                Id = c.Id,
                FirstName = c.FirstName,
                LastName = c.LastName,
                Salary = c.Salary,
            });

            return new List<ISearchResult>().Concat(courses).Concat(instructors);
        }
    }
}
