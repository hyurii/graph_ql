namespace IntVmeAPI.Schema.Queries
{
    public class Instructor : PersonType
    {
        [GraphQLName("gpa")]
        public double Salary { get; set; }
    }
}
