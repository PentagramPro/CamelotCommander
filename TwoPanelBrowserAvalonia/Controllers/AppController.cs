using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwoPanelBrowserAvalonia.Controllers
{
    public class AppController
    {
        List<FileBrowserController> _fileBrowsers = new();
        FileBrowserController? _activeFileBrowser;
        FunctionalToolbarController? _functionalToolbar;

        public FileBrowserController? ActiveFileBrowser => _activeFileBrowser;
        public FileBrowserController? OtherFileBrowser => _fileBrowsers.FirstOrDefault(x => x != _activeFileBrowser);

        public void SetActiveBrowser(FileBrowserController fileBrowser)
        {
            _activeFileBrowser = fileBrowser;
        }
        public FileBrowserController CreateFileBrowser()
        {
            var path = "C:\\home\\dev\\playground";//Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            var fileBrowser = new FileBrowserController(this,path);
            _fileBrowsers.Add(fileBrowser);
            return fileBrowser;
        }
        public FunctionalToolbarController CreateFunctionalToolbar(Window mainWindow)
        {
            _functionalToolbar = new FunctionalToolbarController(this, mainWindow);
            return _functionalToolbar;
        }

        
    }
}
