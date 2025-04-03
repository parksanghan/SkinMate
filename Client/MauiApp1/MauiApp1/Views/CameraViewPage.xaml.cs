using CommunityToolkit.Maui.Core.Primitives;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;

namespace MauiApp1.Views;

public partial class CameraViewPage : ContentPage
{
    public CameraInfo? SelectedCamera { get; set; }
    public CameraViewPage()
    {
        InitializeComponent();
        BindingContext = this;

    }
    // 권한 설정
    private async Task<bool> RequestGalleryPermissionAsync()
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
    private async Task<bool> CheckCameraPermission()
    {
        var status = await Permissions.RequestAsync<Permissions.Camera>();
        return status == PermissionStatus.Granted;
    }
    //  전면 카메라 변경
    private async void FrontBtnClicked(object sender, EventArgs e)
    {
        try
        {
#if WINDOWS
            bool result = await CheckCameraPermission();
            if (!result) return;
#endif
            var cameras = await cameraView.GetAvailableCameras(CancellationToken.None);
            if (cameras == null || cameras.Count == 0)
            {
                await DisplayAlert("경고", "사용 가능한 카메라가 없습니다", "확인");
                return;
            }

            SelectedCamera = cameras.FirstOrDefault(c => c.Position == CameraPosition.Front);
            if (SelectedCamera == null)
            {
                await DisplayAlert("경고", "전면 카메라를 찾을 수 없습니다", "확인");
                return;
            }

            cameraView.SelectedCamera = SelectedCamera;
            await cameraView.StartCameraPreview(CancellationToken.None);
        }
        catch (Exception ex)
        {
            await DisplayAlert("에러", ex.Message, "확인");
        }

    }
    // 후면카메라 전환
    private async void RearBtnClicked(object sender, EventArgs e)
    {
        try
        {
#if WINDOWS
            bool result = await CheckCameraPermission();
            if (!result) return;
#endif
            var cameras = await cameraView.GetAvailableCameras(CancellationToken.None);

            if (cameras == null || cameras.Count == 0)
            {
                await DisplayAlert("경고", "사용 가능한 카메라가 없습니다", "확인");
                return;
            }

            SelectedCamera = cameras.FirstOrDefault(c => c.Position == CameraPosition.Rear);
            if (SelectedCamera == null)
            {
                await DisplayAlert("경고", "후면 카메라를 찾을 수 없습니다", "확인");
                return;
            }

            cameraView.SelectedCamera = SelectedCamera;
            await cameraView.StartCameraPreview(CancellationToken.None);
        }
        catch (Exception ex)
        {
            await DisplayAlert("에러", ex.Message, "확인");
        }
    }
    // 진단 버튼
    private async void SendBtnClicked(object sender, EventArgs e)
    {
       
    }
    // 캡처 버튼
    private async void CaptureBtnClicked(object sender, EventArgs e)
    {
        try
        {
            if (cameraView.SelectedCamera == null)
            {
                await DisplayAlert("경고", "카메라가 선택되지 않았습니다", "확인");
                return;
            }

            await Task.Delay(1000);
            await cameraView.CaptureImage(CancellationToken.None);
        }
        catch (Exception ex)
        {
            await DisplayAlert("예외 발생", $"메시지: {ex.Message}\n내부 예외: {ex.InnerException?.Message}", "확인");
        }

    }
    // 
    private async void CameraView_MediaCaptured(object sender, MediaCapturedEventArgs e)
    {
        Console.WriteLine("📸 MediaCaptured 이벤트 호출됨");

 

        try
        {
            using var memoryStream = new MemoryStream();
            await e.Media.CopyToAsync(memoryStream);
            var imageBytes = memoryStream.ToArray();

            Console.WriteLine($"[DEBUG] 이미지 바이트 크기: {imageBytes.Length}");

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
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await DisplayAlert("저장 완료", $"이미지가 저장되었습니다.\n{filePath}", "확인");
            });
#endif
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ 예외 발생: {ex}");

            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await DisplayAlert("예외 발생", $"메시지: {ex.Message}\n스택: {ex.StackTrace}", "확인");
            });
        }

    }
    // 파일 가져오기 버튼
    private async void FileBtnClicked(object sender, EventArgs e)
    {
        try
        {
            if (!await RequestGalleryPermissionAsync())
            {
                await DisplayAlert("권한 거부", "갤러리 접근 권한이 필요합니다.", "확인");
                return;
            }

            var results = await FilePicker.PickMultipleAsync(new PickOptions
            {
                PickerTitle = "이미지 선택",
                FileTypes = FilePickerFileType.Images
            });

            if (results == null || !results.Any())
                return;

            using var form = new MultipartFormDataContent();
            int i = 0;
            foreach (var file in results)
            {
                var stream = await file.OpenReadAsync();

                // 바이트 크기 측정용 메모리 스트림 복사
                using var memory = new MemoryStream();
                await stream.CopyToAsync(memory);
                var imageBytes = memory.ToArray();

                var byteContent = new ByteArrayContent(imageBytes);
                byteContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg");

                // 디버깅 출력
                Console.WriteLine($"{i}+[DEBUG] 파일 이름: {file.FileName}");
                Console.WriteLine($"{i}[DEBUG] Content-Type: {byteContent.Headers.ContentType}");
                Console.WriteLine($"{i}[DEBUG] Byte 크기: {imageBytes.Length}");

                form.Add(byteContent, "files", file.FileName);
                i++;
            }
          
        }
        catch (Exception ex)
        {
            await DisplayAlert("예외", ex.Message, "확인");
        }
    }
 
}