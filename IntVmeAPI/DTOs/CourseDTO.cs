using IntVmeAPI.Models;

namespace IntVmeAPI.DTOs
{
    public class CourseDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public SubjectEnum Subject { get; set; }
        public string CreatorId { get; set; }
        public Guid InstructorId { get; set; }
        public InstructorDTO Instructor { get; set; }
        public IEnumerable<StudentDTO>? Students { get; set; }
    }
}
