using System;
using MongoDB.Bson;
using MongoDB.Driver;
using ProductCatalogApp.Models;
using ProductCatalogApp.Services;

namespace ProductCatalogApp
{
    class Program
    {
        static void Main(string[] args)
        {
            LogService logService = new();
            CategoryService categoryService = new(logService);
            ProductService productService = new(logService);

            while (true)
            {
                Console.WriteLine("____ÜRÜN KATALOG UYGULAMASI____");
                Console.WriteLine("1. Kategori İşlemleri");
                Console.WriteLine("2. Ürün İşlemleri");
                Console.WriteLine("3. Log İşlemleri");
                Console.WriteLine("4. Çıkış");
                Console.WriteLine(" Lütfen bir seçim yapınız:");

                string? choice = Console.ReadLine();

                if (string.IsNullOrEmpty(choice))
                {
                    Console.WriteLine("Geçersiz seçim, lütfen tekrar deneyin.");
                    continue;
                }
                switch (choice)
                {
                    case "1":
                        CategoryOperations(categoryService);
                        break;
                    case "2":
                        ProductOperations(productService);
                        break;
                    case "3":
                        LogOperations(logService);
                        break;
                    case "4":
                        Console.WriteLine("Uygulamadan çıkılıyor...");
                        return;
                    default:
                        Console.WriteLine("Geçersiz seçim, lütfen tekrar deneyin.");
                        break;
                }

            }
        }

        static void CategoryOperations(CategoryService categoryService)
        {
            Console.WriteLine("KATEGORİ İŞLEMLERİ:");
            Console.WriteLine("1. Kategori Ekle");
            Console.WriteLine("2. Kategori Güncelle");
            Console.WriteLine("3. Kategori Sil");
            Console.WriteLine("4. Tüm Kategorileri Getir");
            Console.WriteLine("5. Kategori ID'ye Göre Getir");
            Console.WriteLine("6. Ana Menüye Dön..");

            string? choice = Console.ReadLine();

            if (string.IsNullOrEmpty(choice))
            {
                Console.WriteLine("Geçersiz seçim, lütfen tekrar deneyin.");
                return;
            }

            switch (choice)
            {
                case "1":
                    // Add Category
                    Console.WriteLine("Kategori Adını Giriniz:");
                    string? categoryName = Console.ReadLine();
                    if (string.IsNullOrEmpty(categoryName))
                    {
                        Console.WriteLine("Kategori adı boş olamaz.");
                        return;
                    }
                    Category newCategory = new Category(categoryName);
                    int result = categoryService.AddCategory(newCategory);
                    break;
                case "2":
                    // Update Category
                    Console.WriteLine("Güncellemek istediğiniz Kategori ID'sini giriniz:");
                    string? id = Console.ReadLine();

                    if (string.IsNullOrEmpty(id))
                    {
                        Console.WriteLine("Kategori ID'si boş olamaz.");
                        return;
                    }
                    Console.WriteLine("Yeni Kategori Adını Giriniz:");
                    string? newCategoryName = Console.ReadLine();
                    if (string.IsNullOrEmpty(newCategoryName))
                    {
                        Console.WriteLine("Yeni kategori adı boş olamaz.");
                        return;
                    }
                    Category updatedCategory = new Category(newCategoryName);
                    bool updateResult = categoryService.UpdateCategory(id, updatedCategory);
                    if (updateResult)
                    {
                        Console.WriteLine("Kategori başarıyla güncellendi.");
                    }
                    else
                    {
                        Console.WriteLine("Kategori güncellenirken hata oluştu.");
                    }
                    break;
                case "3":
                    // Delete Category
                    Console.WriteLine("Silmek istediğiniz Kategori ID'sini giriniz:");
                    string? deleteID = Console.ReadLine();
                    if (string.IsNullOrEmpty(deleteID))
                    {
                        Console.WriteLine("Kategori ID'si boş olamaz.");
                        return;
                    }
                    categoryService.DeleteCategory(deleteID);
                    break;
                case "4":
                    // Get All Categories
                    List<Category> categories = categoryService.GetAllCategories();
                    Console.WriteLine("Tüm Kategoriler:");
                    foreach (var cat in categories)
                    {
                        Console.WriteLine($"ID: {cat.Id}, Kategori Adı: {cat.CategoryName}");
                    }
                    break;
                case "5":
                    // Get Category By ID
                    Console.WriteLine("Getirmek istediğiniz Kategori ID'sini giriniz:");
                    string? getId = Console.ReadLine();
                    if (string.IsNullOrEmpty(getId))
                    {
                        Console.WriteLine("Kategori ID'si boş olamaz.");
                        return;
                    }
                    Category? category = categoryService.GetCategoryById(getId);
                    if (category != null)
                    {
                        Console.WriteLine($"ID:{category.Id},Kategori Adı:{category.CategoryName}");
                    }
                    else
                    {
                        Console.WriteLine("Kategori bulunamadı.");
                    }

                    break;
                case "6":
                default:
                    Console.WriteLine("Geçersiz seçim, lütfen tekrar deneyin.");
                    break;
            }
        }

        static void ProductOperations(ProductService productService)
        {



        }

        static void LogOperations(LogService logService)
        {

        }

    }
}
