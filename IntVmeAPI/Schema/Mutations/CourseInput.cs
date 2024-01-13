using IntVmeAPI.Models;

namespace IntVmeAPI.Schema.Mutations
{
    public class CourseInput
    {
        public required string Name { get; set; }
        public SubjectEnum Subject { get; set; }
        public Guid InstructorId { get; set; }
    }
}
