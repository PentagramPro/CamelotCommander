using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwoPanelBrowserAvalonia.Utils
{
    public static class FileUtils
    {
        public static string GetPermissions(FileSystemInfo info)
        {
            try
            {
                var attributes = info.Attributes;
                return attributes.HasFlag(FileAttributes.ReadOnly) ? "RO" : "RW";
            }
            catch
            {
                return "Unknown";
            }
        }
    }
}
