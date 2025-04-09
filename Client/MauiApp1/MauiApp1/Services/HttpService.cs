 
using MauiApp1.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MauiApp1.Services
{
    public class HttpService
    {
        private static HttpService? _instance;
        private static readonly object _lock = new(); 
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "http://127.0.0.1:8080";
        public static HttpService Instance
        {
            get
            {
                lock (_lock)
                {
                    return _instance ??= new HttpService();
                }
            }
        }
        private HttpService() { 
            _httpClient = new HttpClient(); 
        }

        public async Task<string> LoginAsync(LoginRequest request)
        {
            var url = $"{BaseUrl}/login"; // 실제 서버 IP로 변경
            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var res = await _httpClient.PostAsync(url, content);
            if (res.IsSuccessStatusCode)
            {
                //var result = JsonSerializer.Deserialize<LoginResponse>(responseJson);
                return await res.Content.ReadAsStringAsync();
            }
            else
            {
                throw new Exception($"서버 오류: {await res.Content.ReadAsStringAsync()}");
            }
        }
        public async Task<String> RegisterAsync(RegisterRequest request)
        {
            var url = $"{BaseUrl}/register"; // 실제 서버 IP로 변경
            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var res = await _httpClient.PostAsync(url, content);
            if (res.IsSuccessStatusCode)
            {
                //var result = JsonSerializer.Deserialize<LoginResponse>(responseJson);
                return await res.Content.ReadAsStringAsync();
            }
            else
            {
                throw new Exception($"서버 오류: {await res.Content.ReadAsStringAsync()}");
            }
        }
        public async Task<string> UploadFilesAsync(MultipartFormDataContent data)
        {
            try
            {
                var url = $"{BaseUrl}/upload";
                var res = await _httpClient.PostAsync(url, data);
                if (res.IsSuccessStatusCode)
                {
                    return await res.Content.ReadAsStringAsync();
                }
                else
                {
                    throw new Exception($"업로드 실패: {await res.Content.ReadAsStringAsync()}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DEBUG] UploadFilesAsync 예외: {ex.Message}");
                throw;// 그대로 던짐
            }

        }
    }
}
