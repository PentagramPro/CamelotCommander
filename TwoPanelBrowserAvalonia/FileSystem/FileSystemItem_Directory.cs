using Avalonia.Media.Imaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwoPanelBrowserAvalonia.Utils;

namespace TwoPanelBrowserAvalonia.FileSystem
{
    public class FileSystemItem_Directory : IFileSystemItem
    {
        public virtual string Name => _directoryInfo.Name;
        public string Modified => _directoryInfo.LastWriteTime.ToString("yyyy-MM-dd HH:mm");
        public string Permissions => FileUtils.GetPermissions(_directoryInfo);
        public string FullPath => _directoryInfo.FullName;
        public EFileType FileType => EFileType.Directory;
        public Bitmap? Icon { get; internal set; }
        private DirectoryInfo _directoryInfo;
        public FileSystemItem_Directory(DirectoryInfo directoryInfo, Bitmap icon)
        {
            _directoryInfo = directoryInfo;
            Icon = icon;
        }
    }

    public class FileSystemItem_DirectoryUp : FileSystemItem_Directory
    {
        public override string Name => "..";
        public FileSystemItem_DirectoryUp(DirectoryInfo directoryInfo, Bitmap icon) : base(directoryInfo, icon)
        {
   
        }
    }
}
