using IntVmeAPI.Models;

namespace IntVmeAPI.Schema.Mutations
{
    public class CourseResult
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public SubjectEnum Subject { get; set; }
        public Guid InstructorId { get; set; }

    }
}
