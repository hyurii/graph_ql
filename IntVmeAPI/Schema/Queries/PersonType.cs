namespace IntVmeAPI.Schema.Queries
{
    public abstract class PersonType : ISearchResult
    {
        public Guid Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
    }
}
