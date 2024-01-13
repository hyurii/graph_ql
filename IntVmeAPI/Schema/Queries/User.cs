namespace IntVmeAPI.Schema.Queries
{
    public class User
    {
        public string Id { get; set; }
        public required string Username { get; set; }
        public string? PhotoUrl { get; set; }

    }
}
