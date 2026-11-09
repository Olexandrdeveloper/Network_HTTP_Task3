using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Network_HTTP_Task3
{
    internal class Program
    {
        private static HttpClient client = new HttpClient();

        static async Task Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.Write("Введіть нікнейм користувача GitHub: ");
            string username = Console.ReadLine() ?? string.Empty;

            string url = $"https://api.github.com/users/{username}/repos";

            try
            {
                client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (compatible; C# App)");

                HttpResponseMessage response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Користувача не знайдено або виникла помилка (Код: {response.StatusCode})");
                    return;
                }

                string jsonResponse = await response.Content.ReadAsStringAsync();

                var repos = JsonSerializer.Deserialize<List<GitHubRepo>>(jsonResponse);

                Console.WriteLine($"\n--- Репозиторії користувача {username} ---");
                if (repos != null && repos.Count > 0)
                {
                    foreach (var repo in repos)
                    {
                        Console.WriteLine($"- {repo.name} (Зірочок: {repo.stargazers_count})");
                    }
                }
                else
                {
                    Console.WriteLine("У цього користувача немає публічних репозиторіїв.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка: {ex.Message}");
            }
            finally
            {
                client.DefaultRequestHeaders.UserAgent.Clear();
            }
        }
    }

    public class GitHubRepo
    {
        public string name { get; set; }
        public int stargazers_count { get; set; }
    }
}
