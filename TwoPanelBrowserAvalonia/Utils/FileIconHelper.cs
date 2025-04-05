using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwoPanelBrowserAvalonia.Utils
{
    using System;
    using System.Diagnostics;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Runtime.InteropServices;

    public static class FileIconHelper
    {

      

        // Основной публичный метод
        public static Avalonia.Media.Imaging.Bitmap GetIcon(string pathToFile)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return GetWindowsIcon(pathToFile);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return GetLinuxIcon(pathToFile);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return GetMacIcon(pathToFile);
            }
            else
            {
                throw new PlatformNotSupportedException("Неизвестная ОС");
            }
        }

        // ==========================
        // Windows: Получаем иконку через SHGetFileInfo
        // ==========================
        private static Avalonia.Media.Imaging.Bitmap GetWindowsIcon(string filePath)
        {
            Bitmap bmp = GetRegisteredIcon(filePath).ToBitmap();
            using (MemoryStream memory = new MemoryStream())
            {
                bmp.Save(memory, ImageFormat.Png);
                memory.Position = 0;

                //AvIrBitmap is our new Avalonia compatible image. You can pass this to your view
                Avalonia.Media.Imaging.Bitmap avIrBitmap = new Avalonia.Media.Imaging.Bitmap(memory);
                return avIrBitmap;
            }
        }

        public static System.Drawing.Icon GetRegisteredIcon(string filePath)
        {
            var shinfo = new SHfileInfo();
            Win32.SHGetFileInfo(filePath, 0, ref shinfo, (uint)Marshal.SizeOf(shinfo), Win32.SHGFI_ICON | Win32.SHGFI_SMALLICON);
            return System.Drawing.Icon.FromHandle(shinfo.hIcon);
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SHfileInfo
        {
            public IntPtr hIcon;
            public int iIcon;
            public uint dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
        }


        internal sealed class Win32
        {
            public const uint SHGFI_ICON = 0x100;
            public const uint SHGFI_LARGEICON = 0x0; // large
            public const uint SHGFI_SMALLICON = 0x1; // small

            [System.Runtime.InteropServices.DllImport("shell32.dll")]
            public static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHfileInfo psfi, uint cbSizeFileInfo, uint uFlags);
        }
        // ==========================
        // Linux: Получаем иконку по MIME-типу
        // ==========================
        private static Avalonia.Media.Imaging.Bitmap GetLinuxIcon(string filePath)
        {
            try
            {
                string mimeType = GetLinuxMimeType(filePath);
                if (string.IsNullOrEmpty(mimeType))
                    return null;

                string iconName = mimeType.Replace("/", "-");
                string iconPath = $"/usr/share/icons/{iconName}.png";

                if (File.Exists(iconPath))
                {
                    return new Avalonia.Media.Imaging.Bitmap(iconPath);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Ошибка при получении иконки в Linux: {ex.Message}");
            }
            return null;
        }

        private static string GetLinuxMimeType(string filePath)
        {
            try
            {
                ProcessStartInfo psi = new()
                {
                    FileName = "xdg-mime",
                    Arguments = $"query filetype \"{filePath}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using Process process = Process.Start(psi);
                string output = process?.StandardOutput.ReadToEnd().Trim();
                process?.WaitForExit();
                return output;
            }
            catch
            {
                return null;
            }
        }

        // ==========================
        // macOS: Используем NSWorkspace для получения иконки
        // ==========================
        private static Avalonia.Media.Imaging.Bitmap GetMacIcon(string filePath)
        {
            try
            {
                string script = $@"tell application ""Finder"" to return POSIX path of (get icon of file ""{filePath}"" as alias)";
                ProcessStartInfo psi = new()
                {
                    FileName = "osascript",
                    Arguments = $"-e \"{script}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using Process process = Process.Start(psi);
                string iconPath = process?.StandardOutput.ReadToEnd().Trim();
                process?.WaitForExit();

                if (!string.IsNullOrEmpty(iconPath) && File.Exists(iconPath))
                {
                    return new Avalonia.Media.Imaging.Bitmap(iconPath);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Ошибка при получении иконки в macOS: {ex.Message}");
            }
            return null;
        }
    }

}
