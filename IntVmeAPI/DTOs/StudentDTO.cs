namespace IntVmeAPI.DTOs
{
    public class StudentDTO
    {
        public Guid Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public double GPA { get; set; }
        public IEnumerable<CourseDTO>? Courses { get; set; }
    }
}
