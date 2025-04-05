using System;
using System.Collections.Generic;
using Avalonia.Media.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TwoPanelBrowserAvalonia.FileSystem
{
    public enum EFileType
    {
        File,
        Directory
    }

    public interface IFileSystemItem
    {
        public Bitmap? Icon { get; }
        public string Name { get;  }
        public string Modified { get; }
        public string Permissions { get; }
        public string FullPath { get; }
        public EFileType FileType { get;  }

        public FileSystemInfo FileSystemInfo { get; }
    }
}
