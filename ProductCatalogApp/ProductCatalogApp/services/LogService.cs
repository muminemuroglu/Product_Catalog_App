using System.Text.RegularExpressions;

namespace ProductCatalogApp.Services
{
    public class LogService
    {
        private readonly string _LogPath = "app_error.txt";

        public void LogError(Exception ex, string methodName)
        {
            DateTime date = DateTime.Now;
            string logEntry = $"[{date:yyyy-MM-dd HH:mm:ss}] Method: {methodName}, Error: {ex.Message}" + Environment.NewLine;
            File.AppendAllText(_LogPath, logEntry);
        }

        public List<string> ReadLogFile()
        {
            List<string> list = new();
            if (File.Exists(_LogPath))
            {
                using var reader = new StreamReader(_LogPath);
                string? line;
                while ((line = reader.ReadLine()) != null)
                {
                    list.Add(line);
                }
            }
            return list;
        }

        //Log dosyasını okuyabilen ve belirli bir hata mesajını veya zaman aralığını arayabilen metod.


        public List<string> FilterLog(string? errorMessage = null, DateTime? startDate = null, DateTime? endDate = null)
        {
            List<string> filteredLogs = new();

            if (!File.Exists(_LogPath))
                return filteredLogs;

            using var reader = new StreamReader(_LogPath);
            string? line;

            while ((line = reader.ReadLine()) != null)
            {
                // Hata mesajı varsa ve satırda yoksa, devam etme
                if (!string.IsNullOrEmpty(errorMessage) && !line.Contains(errorMessage))
                    continue;

                // Tarih filtresi uygulanacaksa
                if (startDate.HasValue || endDate.HasValue)
                {
                    string[] parts = line.Split(']'); // ']' karakterine göre böl
                    if (parts.Length > 0)
                    {
                        string datePart = parts[0].TrimStart('['); // '[' karakterini temizle

                        if (DateTime.TryParse(datePart, out DateTime logDate))
                        {
                            // Belirtilen tarih aralığını kontrol et
                            if (startDate.HasValue && logDate < startDate.Value)
                                continue;

                            if (endDate.HasValue && logDate > endDate.Value)
                                continue;
                        }
                    }
                }

                // Filtrelenen log satırını listeye ekle
                filteredLogs.Add(line);
            }

            return filteredLogs;
        }







    }
}