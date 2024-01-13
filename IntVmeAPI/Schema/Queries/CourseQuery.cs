using HotChocolate.Authorization;
using IntVmeAPI.Domain.Courses;
using IntVmeAPI.DTOs;
using IntVmeAPI.Middlewares;
using IntVmeAPI.Schema.Filters;
using IntVmeAPI.Schema.Sorters;
using IntVmeAPI.Services;
using IntVmeAPI.Services.Courses;

namespace IntVmeAPI.Schema.Queries
{
    [ExtendObjectType(typeof(Query))]
    public class CourseQuery
    {
        private readonly CoursesRepository _coursesRepository;

        public CourseQuery(CoursesRepository coursesRepository)
        {
            _coursesRepository = coursesRepository;
        }
        public IQueryable<Course> GetCoursesDB(SchoolDBContext context)
        {
            return context.Courses.Select(c => new Course()
            {
                Id = c.Id,
                Name = c.Name,
                Subject = c.Subject,
                InstructorId = c.InstructorId,
            });
        }

        [Authorize]
        [UseUser]
        [UsePaging(IncludeTotalCount = true, DefaultPageSize = 10)]
        [UseProjection]
        [UseFiltering(typeof(CourseFilter))]
        [UseSorting(typeof(CourseSort))]
        public IEnumerable<Course> GetCourses(
            int count,
            CourseUseCases courseUseCases,
            CancellationToken cancellationToken) => courseUseCases.GetAllCourses(count, cancellationToken);



        public async Task<Course> GetCourseById(Guid id)
        {
            CourseDTO courseDTO = await _coursesRepository.GetById(id);
            return new Course()
            {
                Id = courseDTO.Id,
                Name = courseDTO.Name,
                Subject = courseDTO.Subject,
                InstructorId = courseDTO.InstructorId,
                CreatorId = courseDTO.CreatorId,
            };
        }

        [UsePaging(IncludeTotalCount = true, DefaultPageSize = 10)]
        [UseProjection]
        [UseFiltering(typeof(CourseFilter))]
        [UseSorting(typeof(CourseSort))]
        public async Task<IEnumerable<ISearchResult>> CoursesSearch(
            string term,
            CourseUseCases courseUseCases,
            CancellationToken cancellationToken)
        {
            return await courseUseCases.GetCourseSearch(term, cancellationToken);
        }
    }
}
