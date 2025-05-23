using MongoDB.Bson;
using MongoDB.Driver;
using ProductCatalogApp.Models;
using ProductCatalogApp.utils;

namespace ProductCatalogApp.Services
{
    public class ProductService
    {
        private readonly IMongoCollection<Product> _productCollection;
        private readonly IMongoCollection<Category> _categories;
        private readonly LogService _logService;

        public ProductService(LogService logService)
        {
            DBMongo dBMongo = new();
            _productCollection = dBMongo.GetCollection<Product>("products");
            _categories = dBMongo.GetCollection<Category>("categories");
            _logService = logService;
        }

        //Ürün Ekleme

        public int AddProduct(Product product)
        {
            try
            {//Boş veya geçersiz veri kontrolü
                if (string.IsNullOrWhiteSpace(product.Name))
                    throw new ArgumentException("Product name cannot be empty.");
                if (product.Price <= 0)
                    throw new ArgumentException("Product price must be greater than zero.");
                if (product.StockQuantity < 0)
                    throw new ArgumentException("Product stock quantity cannot be negative.");

                //KategoriId kontrolü
                var category = _categories.Find(c => c.Id == product.CategoryId).FirstOrDefault();
                if (category == null)
                    throw new ArgumentException("Category not found.");

                _productCollection.InsertOne(product);
                Console.WriteLine($"ProductID:{product.Id} added successfully.");
                return 1;
            }
            catch (Exception ex)
            {
                string errorMsg = $"Error adding product:{ex.Message}";
                Console.WriteLine(errorMsg);
                _logService.LogError(ex, nameof(AddProduct));
                return 0;
            }

        }

        //Ürün Güncelleme

        public bool UpdateProduct(string id, Product product)
        {
            try
            {
                var filter = Builders<Product>.Filter.Eq(item => item.Id, product.Id);
                ReplaceOneResult replaceOneResult = _productCollection.ReplaceOne(filter, product);
                return replaceOneResult.MatchedCount > 0;
            }
            catch (Exception ex)
            {
                string errorMsg = $"Error updating product: {ex.Message}";
                Console.WriteLine(errorMsg);
                _logService.LogError(ex, nameof(UpdateProduct));
                return false;
            }
        }


        //Id ' ye göre ürün silme

        public void DeleteProduct(string Id)
        {
            try
            {
                _productCollection.DeleteOne(x => x.Id.ToString() == Id);
            }
            catch (Exception ex)
            {
                string errorMsg = $"Error deleting product: {ex.Message}";
                Console.WriteLine(errorMsg);
                _logService.LogError(ex, nameof(DeleteProduct));
            }
        }

        // Id' sine göre ürün bilgilerini getirme
        public Product? GetProductById(string id)
        {
            try
            {
                return _productCollection.Find(product => product.Id.ToString() == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                string errorMsg = $"Error getting product by id: {ex.Message}";
                Console.WriteLine(errorMsg);
                _logService.LogError(ex, nameof(GetProductById));
                return null;
            }
        }


        /* 
        public Product GetProductById(string id) 
        { 
             var filter = Builders<Product>.Filter.Eq(p => p.Id, new ObjectId(id)); 
             return _productCollection.Find(filter).FirstOrDefault(); 
        }
        */


        //Tüm ürünleri sayfalandırarak listeyen ve son eklenenleri ilk sırada getiren metod

        public List<Product> GetProducts(int page = 1, int limit = 10)
        {
            try
            {
                return _productCollection
                    .Find(_ => true)
                    .SortByDescending(p => p.Id)
                    .Skip((page - 1) * limit)
                    .Limit(limit)
                    .ToList();
            }
            catch (Exception ex)
            {
                string errorMsg = $"Error getting products: {ex.Message}";
                Console.WriteLine(errorMsg);
                _logService.LogError(ex, nameof(GetProducts));
                return new List<Product>();
            }
        }






        // Ürün adında veya açıklamasında anahtar kelimeyi içeren ürünleri listeler
        public List<Product> GetSearchProducts(string keyword)
        {
            try
            {
                var filter = Builders<Product>.Filter.Or(
                    Builders<Product>.Filter.Regex(p => p.Name, new BsonRegularExpression(keyword, "e")),
                    Builders<Product>.Filter.Regex(p => p.Definition, new BsonRegularExpression(keyword, "e"))
                );
                return _productCollection.Find(filter).ToList();
            }
            catch (Exception ex)
            {
                string errorMsg = $"Error searching products: {ex.Message}";
                Console.WriteLine(errorMsg);
                _logService.LogError(ex, nameof(GetSearchProducts));
                return new List<Product>();
            }
        }
        //Fiyat aralığına ve/veya kategoriye göre ürünleri filtreler.

        public List<Product> FilterMetod(decimal? minPrice = null, decimal? maxPrice = null, string? categoryId = null)
        {
            try
            {
                var filterBuilder = Builders<Product>.Filter;
                var filter = filterBuilder.Empty;

                if (minPrice.HasValue)
                {
                    filter &= filterBuilder.Gte(p => p.Price, minPrice.Value);
                }
                if (maxPrice.HasValue)
                {
                    filter &= filterBuilder.Lte(p => p.Price, maxPrice.Value);
                }
                if (!string.IsNullOrEmpty(categoryId))
                {
                    filter &= filterBuilder.Eq("CategoryId", categoryId);
                }

                return _productCollection.Find(filter).ToList();
            }
            catch (Exception ex)
            {
                string errorMsg = $"Error filtering products: {ex.Message}";
                Console.WriteLine(errorMsg);
                _logService.LogError(ex, nameof(FilterMetod));
                return new List<Product>();
            }
        }






    }
}