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
            {
                //KategoriId kontrolü
                var category = _categories.Find(c => c.Id == product.CategoryId).FirstOrDefault();
                if (category == null)
                {
                    string errorMsg = $"Category with ID {product.CategoryId} does not exist.";
                    _logService.LogError(errorMsg, nameof(AddProduct));
                    return 0;
                }

                _productCollection.InsertOne(product);
                return 1;
            }
            catch (Exception ex)
            {
                string errorMsg = $"Error adding product:{ex.Message}";
                _logService.LogError(errorMsg, nameof(AddProduct));
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
                _logService.LogError(errorMsg, nameof(UpdateProduct));
                return false;
            }
        }


        //Id'ye göre ürün silme

        public bool DeleteProduct(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id) || id.Length != 24)
                {
                    _logService.LogError("Geçersiz ürün ID formatı.", nameof(DeleteProduct));
                    return false;
                }

                var product = _productCollection.Find(p => p.Id == id).FirstOrDefault();
                if (product == null)
                {
                    _logService.LogError("Silinmek istenen ürün bulunamadı.", nameof(DeleteProduct));
                    return false;
                }

                var result = _productCollection.DeleteOne(p => p.Id == id);
                return result.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                string errorMsg = $"Error deleting product: {ex.Message}";
                _logService.LogError(errorMsg, nameof(DeleteProduct));
                return false;
            }
        }

        // Id' sine göre ürün bilgilerini getirme.
        public Product? GetProductById(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    string errorMsg = $"GetProductById failed: ID was provided as null or empty.";
                    _logService.LogError(errorMsg, nameof(GetProductById));
                    return null;
                }
                return _productCollection
                    .Find(product => product.Id == id)
                    .FirstOrDefault();
            }
            catch (Exception ex)
            {
                string errorMsg = $"Error getting product by id: {ex.Message}";
                _logService.LogError(errorMsg, nameof(GetProductById));
                return null;
            }
        }


        //Tüm ürünleri sayfalandırarak listeyen ve son eklenenleri ilk sırada getiren metod.

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
                _logService.LogError(errorMsg, nameof(GetProducts));
                return new List<Product>();
            }
        }




        // Ürün adında veya açıklamasında anahtar kelimeyi içeren ürünleri listeler.
        public List<Product> GetSearchProducts(string keyword)
        {
            try
            {
                var filter = Builders<Product>.Filter.Or(
                    Builders<Product>.Filter.Regex(p => p.Name, new BsonRegularExpression(keyword, "i")), // "i" ile büyük/küçük harf duyarsızlığı sağlanıyor
                    Builders<Product>.Filter.Regex(p => p.Definition, new BsonRegularExpression(keyword, "i"))
                );
                return _productCollection.Find(filter).ToList();
            }
            catch (Exception ex)
            {
                string errorMsg = $"Error searching products: {ex.Message}";
                _logService.LogError(errorMsg, nameof(GetSearchProducts));
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
                _logService.LogError(errorMsg, nameof(FilterMetod));
                return new List<Product>();
            }
        }
    }
}