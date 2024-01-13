using IntVmeAPI.DTOs;
using Microsoft.EntityFrameworkCore;

namespace IntVmeAPI.Services
{
    public class SchoolDBContext : DbContext
    {
        public SchoolDBContext(DbContextOptions<SchoolDBContext> options) : base(options) { }
        public DbSet<CourseDTO> Courses { get; set; }
        public DbSet<InstructorDTO> Instructors { get; set; }
        public DbSet<StudentDTO> Students { get; set; }
    }
}
