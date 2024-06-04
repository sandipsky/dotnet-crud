namespace DotnetCrud.DTOs
{
    public class PagedResponse<T>
    {
        public IEnumerable<T> Items { get; set; }
        public int TotalElements { get; set; }
    }
}