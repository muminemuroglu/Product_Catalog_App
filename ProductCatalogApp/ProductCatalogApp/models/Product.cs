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
        public ObjectId Id { get; set; }

        [BsonElement("name")]
        [BsonRequired]
        public string Name { get; set; } // benzersiz ve boş olmamasını servis sınıflarında denetleyeceğiz.

        [BsonElement("definition")]
        public string? Definition { get; set; }

        [BsonElement("price")]
        [BsonRequired]
        [BsonRepresentation(BsonType.Decimal128)]
        public decimal Price { get; set; }

        [BsonElement("stock_quantity")]
        [BsonRequired]
        public int Stock_quantity { get; set; } //= 0 ve pozitif olmasını servis sınıfında  denetleyeceğiz.

        [BsonElement("category_id")]
        public ObjectId CategoryId { get; set; }

        [BsonElement("date_added")]
        public DateTime Date_Added { get; set; }
    }
}