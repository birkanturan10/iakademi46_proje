namespace iakademi46_proje.Models
{
    public class MainPageModel
    {
        public List<Product>? SliderProducts { get; set; } //slider    (StatusID = 1)
        public List<Product>? NewProducts { get; set; } //en yeni ürünler  (Adddate kolonu)
        public Product? ProductOfDay { get; set; } //günün ürünü (StatusID =6)
        public List<Product>? SpecialProducts { get; set; } //özel (StatusID = 2)
        public List<Product>? StarProducts { get; set; } //yıldızlı (StatusID=3)
        public List<Product>? FeaturedProducts { get; set; } //fırsat (StatusID = 4)
        public List<Product>? DiscountedProducts { get; set; } //indirimli (Discount kolonu)
        public List<Product>? HighlightedProducts { get; set; } //öne çıkanlar (Highlight kolunu)
        public List<Product>? TopsellerProducts { get; set; } //çok satanlar (Topseller kolonu)
        public List<Product>? NotableProducts { get; set; } //dikkat çeken (StatusID = 5)


        public Product? ProductDetails { get; set; }
        public string? CategoryName { get; set; }
        public string? BrandName { get; set; }
        public List<Product>? RelatedProducts { get; set; }
    }
}
