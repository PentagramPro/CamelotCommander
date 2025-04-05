using Avalonia.Media.Imaging;
using Avalonia.Platform;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TwoPanelBrowserAvalonia.Controllers.FileSystem;
using TwoPanelBrowserAvalonia.Utils;

namespace TwoPanelBrowserAvalonia.Controllers
{
    class FileBrowserController
    {
        public ObservableCollection<IFileSystemItem> Files { get; } = new();
        public ICommand EnterItem { get; }

        public string CurrentPath { get; internal set; }

        public FileBrowserController()
        {
            EnterItem = ReactiveCommand.Create((IFileSystemItem selectedItem) => OnEnterItem(selectedItem));
        }

        private void OnEnterItem(IFileSystemItem selectedItem)
        {  
            if(selectedItem.FileType == EFileType.Directory)
            {
                LoadFiles(selectedItem.FullPath);
            }
            
        }

        public void LoadFiles(string path)
        {
            Files.Clear();
            CurrentPath = path;
            Bitmap dirIcon = BitmapHelper.LoadBitmap("avares://TwoPanelBrowserAvalonia/Assets/folder.png");
            try
            {
                DirectoryInfo? parentDir = Directory.GetParent(path);
                if (parentDir != null)
                {
                    Files.Add(new FileSystemItem_DirectoryUp(parentDir, dirIcon));
                }
               
                foreach (var dir in Directory.GetDirectories(path))
                {
                    var info = new DirectoryInfo(dir);
                    Files.Add(new FileSystemItem_Directory(info, dirIcon));
                }

                foreach (var file in Directory.GetFiles(path))
                {
                    var info = new FileInfo(file);
                    Files.Add(new FileSystemItem_File(info, FileIconHelper.GetIcon(file)));
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading files: {ex.Message}");
            }
        }

        


       


    }
}
