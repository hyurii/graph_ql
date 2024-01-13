using IntVmeAPI.DTOs;
using Microsoft.EntityFrameworkCore;

namespace IntVmeAPI.Services.Courses
{
    public class CoursesRepository
    {
        private readonly IDbContextFactory<SchoolDBContext> _dbContext;

        public CoursesRepository(IDbContextFactory<SchoolDBContext> dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<CourseDTO>> GetAll()
        {
            using (SchoolDBContext context = _dbContext.CreateDbContext())
            {
                return await context.Courses
                    .ToListAsync();
            }
        }

        public async Task<CourseDTO> GetById(Guid courseId)
        {
            using (SchoolDBContext context = _dbContext.CreateDbContext())
            {
                return await context.Courses.FirstOrDefaultAsync(c => c.Id == courseId);
            }
        }

        public async Task<CourseDTO> Create(CourseDTO course)
        {
            using (SchoolDBContext context = _dbContext.CreateDbContext())
            {
                context.Courses.Add(course);
                await context.SaveChangesAsync();

                return course;
            }
        }

        public async Task<CourseDTO> Update(CourseDTO course)
        {
            using (SchoolDBContext context = _dbContext.CreateDbContext())
            {
                context.Courses.Update(course);
                await context.SaveChangesAsync();

                return course;
            }
        }
        public async Task<bool> Delete(Guid id)
        {
            using (SchoolDBContext context = _dbContext.CreateDbContext())
            {
                CourseDTO course = new CourseDTO()
                {
                    Id = id
                };
                context.Courses.Remove(course);
                return await context.SaveChangesAsync() > 0;

            }
        }

        public async Task<IEnumerable<CourseDTO>> GetAllPaginatedRepo(int count, CancellationToken cancellationToken = default)
        {
            using (SchoolDBContext context = _dbContext.CreateDbContext())
            {
                return await context.Courses
                    .Take(count)
                    .ToListAsync(cancellationToken: cancellationToken);
            }
        }

        public async Task<IEnumerable<CourseDTO>> GetCoursesSearch(string term, CancellationToken cancellationToken = default)
        {
            using (SchoolDBContext context = _dbContext.CreateDbContext())
            {
                return await context.Courses
                    .Where(c => c.Name.Contains(term))
                    .ToListAsync(cancellationToken: cancellationToken);
            }
        }

        public async Task<IEnumerable<InstructorDTO>> GetInstructorsSearch(string term, CancellationToken cancellationToken = default)
        {
            using (SchoolDBContext context = _dbContext.CreateDbContext())
            {
                return await context.Instructors
                    .Where(c => c.FirstName.Contains(term) || c.LastName.Contains(term))
                    .ToListAsync(cancellationToken: cancellationToken);
            }
        }
    }
}
