using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace ProductCatalogApp.Models
{
    public class Category
    {

        public ObjectId Id { get; set; }

        [BsonElement("category_name")]
        [BsonRequired]
        public string Category_name { get; set; }
    }
}