using Avalonia.Media.Imaging;
using Avalonia.Platform;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwoPanelBrowserAvalonia.Utils
{
    public static class BitmapHelper
    {

        private static Dictionary<string, Bitmap> _cache = new();
        public static Bitmap LoadBitmap(string path)
        {
            Bitmap? result;
            if (!_cache.TryGetValue(path, out result))
            {

                _cache[path] = result = new Bitmap(AssetLoader.Open(new Uri(path)));
            }

            return result;
        }


    }
}
