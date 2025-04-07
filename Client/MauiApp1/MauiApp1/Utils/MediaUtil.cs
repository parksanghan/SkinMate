using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.Utils
{
    public class MediaUtil
    {
        public async Task<bool> RequestGalleryPermissionAsync()
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
        public async Task<bool> CheckCameraPermission()
        {
            var status = await Permissions.RequestAsync<Permissions.Camera>();
            return status == PermissionStatus.Granted;
        }
   
    }
}
