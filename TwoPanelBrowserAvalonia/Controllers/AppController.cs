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
        public void SetActiveBrowser(FileBrowserController fileBrowser)
        {
            _activeFileBrowser = fileBrowser;
        }
        public FileBrowserController CreateFileBrowser()
        {
            var fileBrowser = new FileBrowserController(this,Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));
            _fileBrowsers.Add(fileBrowser);
            return fileBrowser;
        }
        public FunctionalToolbarController CreateFunctionalToolbar()
        {
            _functionalToolbar = new FunctionalToolbarController(this);
            return _functionalToolbar;
        }

        
    }
}
