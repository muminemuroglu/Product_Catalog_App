using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace ProductCatalogApp.Models
{
    public class Product
    {


        public Product()
        {
            DateAdded = DateTime.Now;

        }
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("name")]
        [Required(ErrorMessage = "Name is required.")]
        public string? Name { get; set; }

        [BsonElement("definition")]
        public string? Definition { get; set; }

        [BsonElement("price")]
        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public decimal Price { get; set; }

        [BsonElement("stock_quantity")]
        [Required(ErrorMessage = "Stock quantity is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "Stock quantity must be a positive number.")]
        public int StockQuantity { get; set; }

        [Required]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("category_id")]
        public string? CategoryId { get; set; }

        [BsonElement("date_added")]
        public DateTime DateAdded { get; set; }
    }
}