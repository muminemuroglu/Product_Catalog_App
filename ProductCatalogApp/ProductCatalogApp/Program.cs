using System;
using System.Runtime.CompilerServices;
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
                Console.WriteLine("------------------------------------");
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
                    // Kategori Ekleme
                    Console.WriteLine("Kategori Adını Giriniz:");
                    string? categoryName = Console.ReadLine();
                    if (string.IsNullOrEmpty(categoryName))
                    {
                        Console.WriteLine("Kategori adı boş olamaz.");
                        return;
                    }
                    Category newCategory = new Category
                    {
                        CategoryName = categoryName
                    };
                    int result = categoryService.AddCategory(newCategory);
                    if (result > 0)
                    {
                        Console.WriteLine("Kategori başarıyla eklendi.");
                    }
                    else
                    {
                        Console.WriteLine("Kategori eklenirken hata oluştu.");
                    }
                    break;
                case "2":
                    // Kategori Güncelleme
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
                    Category updatedCategory = new Category
                    {
                        Id = id,
                        CategoryName = newCategoryName
                    };
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
                    // Kategori Silme
                    Console.WriteLine("Silmek istediğiniz Kategori ID'sini giriniz:");
                    string? deleteID = Console.ReadLine();
                    if (string.IsNullOrEmpty(deleteID))
                    {
                        Console.WriteLine("Kategori ID'si boş olamaz.");
                        return;
                    }
                    bool deleteResult = categoryService.DeleteCategory(deleteID);
                    if (deleteResult)
                    {
                        Console.WriteLine($"ID'si {deleteID} olan kategori ve o kategoriye ait ürünler başarıyla silindi.");
                    }
                    else
                    {
                        Console.WriteLine("Kategori silinemedi. Lütfen geçerli bir ID giriniz.");
                    }
                    break;
                case "4":
                    // Tüm Kategorileri Getir
                    List<Category> categories = categoryService.GetAllCategories();
                    Console.WriteLine("Tüm Kategoriler:");
                    foreach (var cat in categories)
                    {
                        Console.WriteLine($"ID: {cat.Id}, Kategori Adı: {cat.CategoryName}");
                    }
                    break;
                case "5":
                    // Kategori ID'sine Göre Getir
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
            Console.WriteLine("ÜRÜN İŞLEMLERİ:");
            Console.WriteLine("1. Ürün Ekle");
            Console.WriteLine("2. Ürün Güncelle");
            Console.WriteLine("3. Ürün Sil");
            Console.WriteLine("4. Ürün ID'ye Göre Getir");
            Console.WriteLine("5. Ürünleri Sayfalandırarak Getir");
            Console.WriteLine("6. Ürün Arama");
            Console.WriteLine("7. Fiyat Aralığı ve Kategoriye Göre Filtreleme");
            Console.WriteLine("8. Ana Menüye Dön..");

            string? choice = Console.ReadLine();

            if (string.IsNullOrEmpty(choice))
            {
                Console.WriteLine("Geçersiz seçim, lütfen tekrar deneyin.");
                return;
            }

            switch (choice)
            {
                case "1":
                    // Ürün Ekleme
                    Console.WriteLine("Ürün Adını Giriniz:");
                    string? productName = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(productName))
                    {
                        Console.WriteLine("Ürün adı boş olamaz.");
                        return;
                    }
                    Console.WriteLine("Ürün Fiyatını Giriniz:");
                    string? priceInput = Console.ReadLine();
                    if (string.IsNullOrEmpty(priceInput) || !decimal.TryParse(priceInput, out decimal price))
                    {
                        Console.WriteLine("Geçersiz fiyat.");
                        return;
                    }
                    Console.WriteLine("Ürün Açıklamasını Giriniz:");
                    string? definition = Console.ReadLine();

                    Console.WriteLine("Kategori ID'sini Giriniz:");
                    string? categoryId = Console.ReadLine();
                    if (string.IsNullOrEmpty(categoryId))
                    {
                        Console.WriteLine("Kategori ID'si boş olamaz.");
                        return;
                    }
                    Console.WriteLine("Ürün stok miktarını giriniz (varsayılan 0):");
                    string? stockInput = Console.ReadLine();
                    int stockQuantity = string.IsNullOrEmpty(stockInput) ? 0 : int.Parse(stockInput);
                    if (stockQuantity < 0)
                    {
                        Console.WriteLine("Stok miktarı negatif olamaz.");
                        return;
                    }
                    Product newProduct = new Product
                    {
                        Name = productName,
                        Price = price,
                        Definition = definition,
                        CategoryId = categoryId,
                        StockQuantity = stockQuantity
                    };
                    int addResult = productService.AddProduct(newProduct);
                    if (addResult > 0)
                    {
                        Console.WriteLine("Ürün başarıyla eklendi.");
                    }
                    else
                    {
                        Console.WriteLine("Ürün eklenirken hata oluştu.");
                    }
                    break;
                case "2":
                    // Ürün Güncelleme
                    Console.WriteLine("Güncellemek istediğiniz Ürün ID'sini giriniz:");
                    string? productId = Console.ReadLine();
                    if (string.IsNullOrEmpty(productId))
                    {
                        Console.WriteLine("Ürün ID'si boş olamaz.");
                        return;
                    }
                    Console.WriteLine("Yeni Ürün Adını Giriniz:");
                    string? newProductName = Console.ReadLine();
                    if (string.IsNullOrEmpty(newProductName))
                    {
                        Console.WriteLine("Yeni ürün adı boş olamaz.");
                        return;
                    }
                    Console.WriteLine("Yeni Ürün Fiyatını Giriniz:");
                    string? newPriceInput = Console.ReadLine();
                    if (string.IsNullOrEmpty(newPriceInput) || !decimal.TryParse(newPriceInput, out decimal newPrice))
                    {
                        Console.WriteLine("Geçersiz fiyat.");
                        return;
                    }
                    Console.WriteLine("Yeni Ürün Açıklamasını Giriniz:");
                    string? newDefinition = Console.ReadLine();
                    Console.WriteLine("Yeni Kategori ID'sini Giriniz:");
                    string? newCategoryId = Console.ReadLine();
                    if (string.IsNullOrEmpty(newCategoryId))
                    {
                        Console.WriteLine("Kategori ID'si boş olamaz.");
                        return;
                    }
                    Product updatedProduct = new Product
                    {
                        Id = productId,
                        Name = newProductName,
                        Price = newPrice,
                        Definition = newDefinition,
                        CategoryId = newCategoryId
                    };
                    bool updateResult = productService.UpdateProduct(productId, updatedProduct);
                    if (updateResult)
                    {
                        Console.WriteLine("Ürün başarıyla güncellendi.");
                    }
                    else
                    {
                        Console.WriteLine("Ürün güncellenirken hata oluştu.");
                    }
                    break;
                case "3":
                    // Ürün Sil
                    Console.WriteLine("Silmek istediğiniz Ürün ID'sini giriniz:");
                    string? deleteID = Console.ReadLine();
                    if (string.IsNullOrEmpty(deleteID))
                    {
                        Console.WriteLine("Ürün ID'si boş olamaz.");
                        return;
                    }
                    bool deleteResult = productService.DeleteProduct(deleteID);
                    if (deleteResult)
                    {
                        Console.WriteLine("Ürün başarıyla silindi.");
                    }
                    else
                    {
                        Console.WriteLine("Ürün silinemedi. Lütfen geçerli bir ID giriniz.");
                    }
                    break;
                case "4":
                    // Ürün ID'sine Göre Getir
                    Console.WriteLine("Getirmek istediğiniz Ürün ID'sini giriniz:");
                    string? getId = Console.ReadLine();
                    if (string.IsNullOrEmpty(getId))
                    {
                        Console.WriteLine("Ürün ID'si boş olamaz.");
                        return;
                    }
                    Product? product = productService.GetProductById(getId);
                    if (product != null)
                    {
                        Console.WriteLine($"ID:{product.Id}, Adı: {product.Name}, Fiyat: {product.Price}, Açıklama: {product.Definition}, Kategori ID: {product.CategoryId}");
                    }
                    else
                    {
                        Console.WriteLine("Ürün bulunamadı.");
                    }

                    break;
                case "5":
                    //  Ürünleri Sayfalandırarak Getir
                    Console.WriteLine("Sayfa numarasını giriniz:");
                    int page = int.Parse(Console.ReadLine() ?? "1");
                    if (page < 1)
                    {
                        Console.WriteLine("Sayfa numarası 1'den küçük olamaz.");
                        return;
                    }
                    Console.WriteLine("Sayfa başına ürün sayısını giriniz:");
                    int limit = int.Parse(Console.ReadLine() ?? "10");
                    if (limit < 1)
                    {
                        Console.WriteLine("Sayfa başına ürün sayısı 1'den küçük olamaz.");
                        return;
                    }
                    Console.WriteLine($"Sayfa {page} için ürünler:");
                    List<Product> products = productService.GetProducts(page, limit);
                    foreach (var prod in products)
                    {
                        Console.WriteLine($"ID:{prod.Id}, Adı: {prod.Name}, Fiyat: {prod.Price}, Açıklama: {prod.Definition}, Kategori ID: {prod.CategoryId}");
                    }
                    break;
                case "6":
                    // Ürün Ara
                    Console.WriteLine("Aramak istediğiniz ürün anahtar kelimesini giriniz:");
                    string? keyword = Console.ReadLine();
                    if (string.IsNullOrEmpty(keyword))
                    {
                        Console.WriteLine("Anahtar kelime boş olamaz..");
                        return;
                    }
                    List<Product> searchResults = productService.GetSearchProducts(keyword);
                    foreach (var searchProduct in searchResults)
                    {
                        Console.WriteLine($"ID:{searchProduct.Id}, Adı: {searchProduct.Name}, Fiyat: {searchProduct.Price}, Açıklama: {searchProduct.Definition}, Kategori ID: {searchProduct.CategoryId}");
                    }

                    break;
                case "7":
                    // Fiyat aralığı ve/veya kategoriye göre ürünleri filtrele
                    Console.WriteLine("Minimum fiyatı giriniz:");
                    string? minPriceInput = Console.ReadLine();
                    if (string.IsNullOrEmpty(minPriceInput) || !decimal.TryParse(minPriceInput, out decimal minPrice))
                    {
                        Console.WriteLine("Geçersiz minimum fiyat.");
                        return;
                    }
                    Console.WriteLine("Maksimum fiyatı giriniz:");
                    string? maxPriceInput = Console.ReadLine();
                    if (string.IsNullOrEmpty(maxPriceInput) || !decimal.TryParse(maxPriceInput, out decimal maxPrice))
                    {
                        Console.WriteLine("Geçersiz maksimum fiyat.");
                        return;
                    }
                    Console.WriteLine("Kategori ID'sini giriniz:");
                    string? filterCategoryId = Console.ReadLine();

                    List<Product> filteredProducts = productService.FilterMetod(minPrice, maxPrice, filterCategoryId);
                    foreach (var filteredProduct in filteredProducts)
                    {
                        Console.WriteLine($"ID:{filteredProduct.Id}, Adı: {filteredProduct.Name}, Fiyat: {filteredProduct.Price}, Açıklama: {filteredProduct.Definition}, Kategori ID: {filteredProduct.CategoryId}");
                    }
                    break;
                case "8":
                default:
                    Console.WriteLine("Geçersiz seçim, lütfen tekrar deneyin.");
                    break;
            }

        }

        static void LogOperations(LogService logService)
        {
            Console.WriteLine("LOG İŞLEMLERİ:");
            Console.WriteLine("1. Logları Oku");
            Console.WriteLine("2. Log Filtrele");
            Console.WriteLine("3. Ana Menüye Dön..");

            string? choice = Console.ReadLine();

            if (string.IsNullOrEmpty(choice))
            {
                Console.WriteLine("Geçersiz seçim, lütfen tekrar deneyin.");
                return;
            }

            switch (choice)
            {
                case "1":
                    // Logları Oku
                    List<string> logs = logService.ReadLogFile();
                    foreach (var log in logs)
                    {
                        Console.WriteLine(log);
                    }
                    break;
                case "2":
                    // Log Filtrele
                    Console.WriteLine("Hata mesajını giriniz (isteğe bağlı):");
                    string? errorMessage = Console.ReadLine();
                    Console.WriteLine("Başlangıç tarihini giriniz (yyyy-MM-dd):");
                    DateTime? startDate = DateTime.TryParse(Console.ReadLine(), out DateTime start) ? start : null;
                    Console.WriteLine("Bitiş tarihini giriniz (yyyy-MM-dd):");
                    DateTime? endDate = DateTime.TryParse(Console.ReadLine(), out DateTime end) ? end : null;

                    List<string> filteredLogs = logService.FilterLog(errorMessage, startDate, endDate);
                    foreach (var filteredLog in filteredLogs)
                    {
                        Console.WriteLine(filteredLog);
                    }
                    break;
                case "3":
                default:
                    Console.WriteLine("Geçersiz seçim, lütfen tekrar deneyin.");
                    break;
            }
        }

    }
}