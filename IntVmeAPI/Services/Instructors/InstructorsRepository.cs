using IntVmeAPI.DTOs;
using Microsoft.EntityFrameworkCore;

namespace IntVmeAPI.Services.Instructors
{
    public class InstructorsRepository
    {
        private readonly IDbContextFactory<SchoolDBContext> _dbContext;

        public InstructorsRepository(IDbContextFactory<SchoolDBContext> dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<InstructorDTO> GetById(Guid instructorId)
        {
            using (SchoolDBContext context = _dbContext.CreateDbContext())
            {
                return await context.Instructors.FirstOrDefaultAsync(c => c.Id == instructorId);
            }
        }

        internal async Task<IEnumerable<InstructorDTO>> GetManyByIds(IReadOnlyList<Guid> instructorIds)
        {
            using (SchoolDBContext context = _dbContext.CreateDbContext())
            {
                return await context.Instructors.Where(i => instructorIds.Contains(i.Id)).ToListAsync();
            }
        }
    }
}
