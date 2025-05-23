using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;
using ProductCatalogApp.Models;
using ProductCatalogApp.utils;

namespace ProductCatalogApp.Services
{
    public class CategoryService
    {
        private readonly IMongoCollection<Category> _categoryCollection;
        private readonly IMongoCollection<Product> _productCollection;
        private readonly LogService _logService;

        public CategoryService(LogService logService)
        {
            DBMongo dBMongo = new();
            _categoryCollection = dBMongo.GetCollection<Category>("categories");
            _productCollection = dBMongo.GetCollection<Product>("products");
            _logService = logService;

        }


        //Kategori Ekleme
        public int AddCategory(Category category)
        {
            try
            {
                _categoryCollection.InsertOne(category);
                Console.WriteLine($"CategoryID:{category.Id}added successfully. ");
                return 1;

            }
            catch (Exception ex)
            {
                string errorMsg = $"Error adding category: {ex.Message}";
                Console.WriteLine(errorMsg);
                _logService.LogError(ex, nameof(AddCategory));
                return 0;
            }

        }

        //Kategori güncelleme
        public bool UpdateCategory(string id, Category category)
        {
            try
            {
                if (!ObjectId.TryParse(id, out ObjectId objectId))
                {
                    string errorMsg = "Geçersiz kategori ID.";
                    Console.WriteLine(errorMsg);
                    _logService.LogError(new Exception(errorMsg), nameof(UpdateCategory));
                    return false;
                }


                var filter = Builders<Category>.Filter.Eq(item => item.Id, category.Id);
                ReplaceOneResult replaceOneResult = _categoryCollection.ReplaceOne(filter, category);
                return replaceOneResult.MatchedCount > 0;
            }
            catch (Exception ex)
            {
                string errorMsg = $"Error updating category: {ex.Message}";
                Console.WriteLine(errorMsg);
                _logService.LogError(ex, nameof(UpdateCategory));
                return false;
            }
        }




        //Girilen id' ye göre kategori silme

        public void DeleteCategory(string id)
        {
            try
            {
                if (!ObjectId.TryParse(id, out ObjectId objectId))
                {
                    string errorMsg = "Geçersiz kategori ID.";
                    Console.WriteLine(errorMsg);
                    _logService.LogError(new Exception(errorMsg), nameof(DeleteCategory));
                    return;
                }

                var relatedProductCount = _productCollection.CountDocuments(p => p.CategoryId == objectId);
                if (relatedProductCount > 0)
                {
                    Console.WriteLine("Bu kategoriye bağlı ürünler var. Kategori silinemez.");
                    return;
                }

                var deleteResult = _categoryCollection.DeleteOne(x => x.Id == objectId);

                if (deleteResult.DeletedCount > 0)
                    Console.WriteLine("Kategori başarıyla silindi.");
                else
                    Console.WriteLine("Kategori bulunamadı.");
            }
            catch (Exception ex)
            {
                string errorMsg = $"Error deleting category: {ex.Message}";
                Console.WriteLine(errorMsg);
                _logService.LogError(ex, nameof(DeleteCategory));
            }
        }


        //Belirtilen id ye göre kategori getirme

        public Category? GetCategoryById(string id)
        {
            try
            {
                if (!ObjectId.TryParse(id, out ObjectId objectId))
                {
                    string errorMsg = "Geçersiz kategori ID.";
                    Console.WriteLine(errorMsg);
                    _logService.LogError(new Exception(errorMsg), nameof(GetCategoryById));
                    return null;
                }

                return _categoryCollection.Find(category => category.Id == objectId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                string errorMsg = $"Beklenmeyen hata oluştu: {ex.Message}";
                Console.WriteLine(errorMsg);
                _logService.LogError(ex, nameof(GetCategoryById));
                return null;
            }
        }


        //Tüm kategorileri listeleyen metod

        public List<Category> GetAllCategories()
        {
            try
            {
                List<Category> list = _categoryCollection.Find(_ => true).ToList();
                return list;
            }
            catch (Exception ex)
            {
                string errorMsg = $"Error getting all categories: {ex.Message}";
                Console.WriteLine(errorMsg);
                _logService.LogError(ex, nameof(GetAllCategories));
                return new List<Category>();
            }

        }
    }
}