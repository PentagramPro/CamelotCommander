using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwoPanelBrowserAvalonia.FileSystem;

namespace TwoPanelBrowserAvalonia.Controllers
{
    public class CopyWindowController : ViewModelBase
    {
        public IFileSystemItem? SelectedFile;
        public string TargetPath { get; internal set; } = "";
        public string SourcePath => SelectedFile?.FullPath ?? "";
        public List<string> ConflictItems {get; internal set; } = new List<string>();

        public CopyWindowController()
        { }


        public async Task InitializeAsync(IFileSystemItem selectedFile, string targetPath)
        {
            SelectedFile = selectedFile;
            TargetPath = targetPath;
            RaisePropertyChanged(nameof(SourcePath));
            RaisePropertyChanged(nameof(TargetPath));

            var conflicts = await FileTools.CopyTools
                              .PrepareAsync(new List<string> { SourcePath }, TargetPath);


            ConflictItems = conflicts.AlreadyExists.Select(info => info.Name).ToList();
            RaisePropertyChanged(nameof(ConflictItems));
        }
    }
}
