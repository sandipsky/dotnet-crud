namespace DotnetCrud.DTOs
{
    public class UserFilter
    {
        public string Name { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string SortBy { get; set; } = "name";
        public string SortOrder { get; set; } = "asc";
        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = 10;
    }
}
