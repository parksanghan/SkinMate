﻿ 
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
        private const string BaseUrl = "http://172.30.1.98:8080";
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
        private HttpService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<string> LoginAsync(LoginRequest request)
        {
            var url = $"{BaseUrl}/login"; // 실제 서버 IP로 변경
            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var res = await _httpClient.PostAsync(url, content);
            var jsonRes = await res.Content.ReadAsStringAsync();
            Console.WriteLine($"[DEBUG] 응답 본문: {jsonRes}");
            if (res.IsSuccessStatusCode)
            {

                var obj = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonRes);
                string result = obj["status"].ToLower();
                Console.WriteLine($"[DEBUG] 로그인 성공: {result}");
                return result;
            }
            else
            {
                throw new Exception($"서버 오류: {jsonRes}");
            }
        }
        public async Task<String> RegisterAsync(RegisterRequest request)
        {

            var url = $"{BaseUrl}/register"; // 실제 서버 IP로 변경
            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var res = await _httpClient.PostAsync(url, content);
            var jsonRes = await res.Content.ReadAsStringAsync();
            Console.WriteLine($"[DEBUG] 응답 본문: {jsonRes}");

            if (res.IsSuccessStatusCode)
            {
                var obj = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonRes);
                string result = obj["status"].ToLower();
                Console.WriteLine($"[DEBUG] 가입 성공: {result}");
                return result;
            }
            else
            {
                throw new Exception($"서버 오류: {jsonRes}");
            }

        }
        public async Task<string> UploadFilesAsync(MultipartFormDataContent data)
        {
            try
            {
                var url = $"{BaseUrl}/upload";
                var res = await _httpClient.PostAsync(url, data);
                var jsonRes = await res.Content.ReadAsStringAsync();
                Console.WriteLine($"[DEBUG] 응답 본문: {jsonRes}");
                if (res.IsSuccessStatusCode)
                {
                    var obj = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonRes);
                    string result = obj["status"].ToLower();
                    Console.WriteLine($"[DEBUG] 업로드 성공: {result}");
                    return result;
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
        public async Task<string> SendChatMessageAsync(string message)
        {
            var url = $"{BaseUrl}/chat";
            var content = new FormUrlEncodedContent(new[]
            {
            new KeyValuePair<string, string>("message", message)
        });

            var res = await _httpClient.PostAsync(url, content);
            var jsonRes = await res.Content.ReadAsStringAsync();
            Console.WriteLine($"[DEBUG] 응답 본문: {jsonRes}");

            if (res.IsSuccessStatusCode)
            {
                try
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    var obj = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonRes, options);
                    if (obj != null && obj.TryGetValue("status", out var status) && status == "ok")
                    {
                        return obj.TryGetValue("msg", out var msg) ? msg : "";
                    }
                    return "";
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[DEBUG] JSON 파싱 오류: {ex.Message}");
                    return "";
                }
            }
            else
            {
                throw new Exception($"채팅 실패: {jsonRes}");
            }
        }
    }
}
