using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.Utils
{
    public class FontUtil
    {
        /// <summary>
        /// microchart 는 내장된 폰트를 사용하는데 내장된 폰트를 사용하여
        /// 한글 출력시 에러
        /// </summary>
        /// <returns></returns>
        public static  async Task<SKTypeface> getNaunumFontAsync()
        {

            using var stream = await FileSystem.OpenAppPackageFileAsync("NanumGothicCoding.ttf");
            return SKTypeface.FromStream(stream);
        }
    }
}
