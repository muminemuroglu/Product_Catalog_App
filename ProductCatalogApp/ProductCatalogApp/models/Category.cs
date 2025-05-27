using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace ProductCatalogApp.Models
{
    public class Category
    {


        public Category(string categoryName)
        {

            CategoryName = categoryName;
        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("category_name")]
        [Required(ErrorMessage = "Category name is required.")]
        public string CategoryName { get; set; }
    }
}