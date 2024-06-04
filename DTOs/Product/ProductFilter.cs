namespace DotnetCrud.DTOs
{
    public class ProductFilter
    {
        public string Name { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public int? CategoryId { get; set; }
        public string SortBy { get; set; } = "name";
        public string SortOrder { get; set; } = "asc";
        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = 10;
    }
}
