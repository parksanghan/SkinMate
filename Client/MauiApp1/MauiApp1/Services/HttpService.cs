
 
using MauiApp1.Data;
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
        private const string BaseUrl = "http://10.101.139.199:8080";
        private string? MyId { get; set; }
        private List< LogEntry?> _diaLogEntry; // 진단결과 로그
        private List<LogEntry?> _chatLgoEntry; // 채팅 로그 
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
                this.MyId = request.UserId; // 여기서 자체 id 할당
            
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
                var url = $"{BaseUrl}/{this.MyId}/upload";
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
            var url = $"{BaseUrl}/{this.MyId}/chat";
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
        public async Task<List<LogEntry>> GetLogMessageAsync()
        {
            try
            {
                /*var url = $"{BaseUrl}/{this.MyId}/logs";*/
                var url = $"{BaseUrl}/getlog";
                var res = await _httpClient.GetAsync(url);
                var jsonRes = await res.Content.ReadAsStringAsync();
                if (res.IsSuccessStatusCode)
                {
                    var opt = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    var logs =  JsonSerializer.Deserialize<List<LogEntry>>
                        (jsonRes, opt)?? new List<LogEntry> { new LogEntry() };
                    return logs;
                }
                else
                {
                    throw new Exception($"로그 조회 실패: {jsonRes}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DEBUG] GetLogMessageAsync 예외: {ex.Message}");
                throw;// 그대로 던짐
            }
        }
        // 앱이 시작될 때 UserContext 초기화 

        // app.cs에서 초기화 
        public async Task ContextInit()
        {
            try
            {
                var logs = await GetLogMessageAsync();
                _diaLogEntry = logs.Where(log => log.log_type == "진단분석").ToList();
                _chatLgoEntry = logs.Where(log => log.log_type == "질의응답").ToList();
                Console.WriteLine($"[DEBUG] 진단 로그 수: {_diaLogEntry.Count}");
                Console.WriteLine($"[DEBUG] 채팅 로그 수: {_chatLgoEntry.Count}");
                // 아래부터 삭제예정
                var json = JsonSerializer.Serialize(_diaLogEntry, new JsonSerializerOptions { WriteIndented = true });
                Console.WriteLine(json);
                var json2 = JsonSerializer.Serialize(_chatLgoEntry, new JsonSerializerOptions { WriteIndented = true });         
                Console.WriteLine(json2);
            }
            catch
            {
                throw new Exception($"Context Init 실패");
            }
        }
    }
}
