namespace ProductAPI.Controllers.DTO
{

        public class ProductDTO
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public double Price { get; set; }
            public List<string> Images { get; set; }
        }
}
