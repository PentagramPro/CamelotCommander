using System;
using System.Collections.Generic;
using Avalonia.Media.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwoPanelBrowserAvalonia.Utils;

namespace TwoPanelBrowserAvalonia.FileSystem
{
    public class FileSystemItem_File : IFileSystemItem
    {
        public string Name => _fileInfo.Name;
        public string Modified => _fileInfo.LastWriteTime.ToString("yyyy-MM-dd HH:mm");
        public string Permissions => FileUtils.GetPermissions(_fileInfo);
        public string FullPath => _fileInfo.FullName;
        public EFileType FileType => EFileType.File;
        public Bitmap? Icon { get; internal set; }

        private FileInfo _fileInfo;
        public FileSystemItem_File(FileInfo fileInfo, Bitmap icon)
        {
            _fileInfo = fileInfo;
            Icon = icon;
        }
    }
    
    
}
