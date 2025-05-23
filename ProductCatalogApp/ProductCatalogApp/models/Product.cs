using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace ProductCatalogApp.Models
{
    public class Product
    {


        public Product(string name, decimal price, int stock_quantity, ObjectId categoryId)
        {
            DateAdded = DateTime.Now;
            Name = name;
            Price = price;
            StockQuantity = stock_quantity;
            CategoryId = categoryId;
        }
        public ObjectId Id { get; set; }

        [BsonElement("name")]
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

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

        [BsonElement("category_id")]
        public ObjectId CategoryId { get; set; }

        [BsonElement("date_added")]
        public DateTime DateAdded { get; set; }
    }
}