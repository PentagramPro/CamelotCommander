using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwoPanelBrowserAvalonia.FileSystem;

namespace TwoPanelBrowserAvalonia.Controllers
{
    public class RenameWindowController
    {
        public IFileSystemItem TargetFile { get; internal set; }
        public RenameWindowController(IFileSystemItem targetFile)
        {
            TargetFile = targetFile;
        }

        public void Rename(string newName)
        {
    
            string directoryName = Path.GetDirectoryName(TargetFile.FullPath) ?? "";
            if (string.IsNullOrWhiteSpace(newName) || string.IsNullOrWhiteSpace(directoryName))
                return;


            string newPath = Path.Combine(directoryName, newName);
            if (File.Exists(newPath))
                return;

            File.Move(TargetFile.FullPath, newPath);
        }
    }
}
