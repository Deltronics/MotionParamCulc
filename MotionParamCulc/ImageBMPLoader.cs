using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace MotionParamCulc
{
    class ImageBMPLoader
    {
        // <summary>
        // Загрузка изображения из ресурса
        // </summary>
        // <param name="uri">Путь к ресурсу</param>
        // <returns></returns>
        public static BitmapImage LoadImageFromResource(Uri uri, int width)
        {
            BitmapImage bitmapImage = new BitmapImage();

            try
            {
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = Application.GetResourceStream(uri).Stream;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.DecodePixelWidth = width;
                bitmapImage.EndInit();
                bitmapImage.Freeze();
            }
            catch (Exception)
            {
                bitmapImage = null;
            }

            return bitmapImage;
        }





    }
}
