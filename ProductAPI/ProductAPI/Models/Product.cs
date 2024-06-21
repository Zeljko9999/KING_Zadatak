using ProductAPI.Models;

namespace ProductAPI.Models
{
    public class Product
    {
        public int id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string category { get; set; }
        public double price { get; set; }
        public double discountPercentage { get; set; }
        public double rating { get; set; }
        public int stock { get; set; }
        public List<string> tags { get; set; }
        public string brand { get; set; }
        public string sku { get; set; }
        public int weight { get; set; }
        public Dimensions dimensions { get; set; }
        public string warrantyInformation { get; set; }
        public string shippingInformation { get; set; }
        public string availabilityStatus { get; set; }
        public List<Review> reviews { get; set; }
        public string returnPolicy { get; set; }
        public int minimumOrderQuantity { get; set; }
        public Meta meta { get; set; }
        public List<string> images { get; set; }
        public string thumbnail { get; set; }
    }
}
