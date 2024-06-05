public class ProductForm
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }
    public bool IsFeatured { get; set; }
    public int CategoryId { get; set; }
    public int BrandId { get; set; }
    public IFormFile Image { get; set; }
}