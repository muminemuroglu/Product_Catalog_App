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
                return 1;

            }
            catch (Exception ex)
            {
                string errorMsg = $"Error adding category: {ex.Message}";
                _logService.LogError(errorMsg, nameof(AddCategory));
                return 0;
            }

        }

        //Kategori güncelleme
        public bool UpdateCategory(string id, Category category)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    _logService.LogError("Category update failed: ID was provided as null or empty.", nameof(UpdateCategory));
                    return false;
                }
                if (category == null)
                {
                    _logService.LogError("Category update failed: category object is null.", nameof(UpdateCategory));
                    return false;
                }

                category.Id = id;
                var filter = Builders<Category>.Filter.Eq(item => item.Id, id);
                ReplaceOneResult replaceOneResult = _categoryCollection.ReplaceOne(filter, category);
                return replaceOneResult.MatchedCount > 0;
            }
            catch (Exception ex)
            {
                string errorMsg = $"Error updating category: {ex.Message}";
                _logService.LogError(errorMsg, nameof(UpdateCategory));
                return false;
            }
        }

        //Girilen id' ye göre kategori silme

        public void DeleteCategory(string id)
        {
            try
            {
                _categoryCollection.DeleteOne(category => category.Id == id);

                if (_productCollection.CountDocuments(product => product.CategoryId == id) > 0)
                {
                    _productCollection.DeleteMany(product => product.CategoryId == id);

                }

            }
            catch (Exception ex)
            {
                string errorMsg = $"Error deleting category: {ex.Message}";
                _logService.LogError(errorMsg, nameof(DeleteCategory));
            }
        }


        //Belirtilen id ye göre kategori getirme

        public Category? GetCategoryById(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    string errorMsg = "Category ID cannot be null or empty.";
                    _logService.LogError(errorMsg, nameof(GetCategoryById));
                    return null;
                }
                return _categoryCollection
                .Find(category => category.Id == id)
                .FirstOrDefault();
            }
            catch (Exception ex)
            {
                string errorMsg = $"Error getting category by ID: {ex.Message}";
                _logService.LogError(errorMsg, nameof(GetCategoryById));
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
                _logService.LogError(errorMsg, nameof(GetAllCategories));
                return new List<Category>();
            }

        }
    }
}