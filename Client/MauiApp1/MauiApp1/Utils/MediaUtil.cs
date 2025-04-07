using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.Utils
{
    public class MediaUtil
    {
        public static async Task<bool> RequestGalleryPermissionAsync()
        {

            if (DeviceInfo.Version.Major >= 13)

            {
                var status = await Permissions.RequestAsync<Permissions.Media>();
                return status == PermissionStatus.Granted;
            }
            else
            {
                var status = await Permissions.RequestAsync<Permissions.StorageRead>();
                return status == PermissionStatus.Granted;
            }
            return true;
        }
        public static async Task<bool> CheckCameraPermission()
        {
            var status = await Permissions.RequestAsync<Permissions.Camera>();
            return status == PermissionStatus.Granted;
        }
        public static async Task<string> SaveCapturedImageAsync(byte[] imageBytes)
            {
                var path = string.Empty;
                try {
    #if ANDROID
                    var dcimDir = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDcim);
                    var cameraDir = new Java.IO.File(dcimDir, "Camera");
                    if (!cameraDir.Exists()) cameraDir.Mkdir();
                    var filePath = System.IO.Path.Combine(cameraDir.AbsolutePath, $"captured_{DateTime.Now:yyyyMMdd_HHmmss}.jpg");
                    await File.WriteAllBytesAsync(filePath, imageBytes);
                    Console.WriteLine($"✅ 이미지 저장됨: {filePath}");
                     // ✅ 저장 후 MediaScanner 호출
                    Android.Content.Context context = Android.App.Application.Context;
                    Android.Content.Intent mediaScanIntent = new(Android.Content.Intent.ActionMediaScannerScanFile);
                    var contentUri = Android.Net.Uri.FromFile(new Java.IO.File(filePath));
                    mediaScanIntent.SetData(contentUri);
                    context.SendBroadcast(mediaScanIntent);
                    return path = filePath;
    #else
                    await Task.CompletedTask; // ⚠️ CS1998 제거용 dummy await
                    return path;
    #endif

                }
                catch (Exception ex)
            {
                    Console.WriteLine($"❌ 예외 발생: {ex}"); return path;
            }
    }
    }
}
