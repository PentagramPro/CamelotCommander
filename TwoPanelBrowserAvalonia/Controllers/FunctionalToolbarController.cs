using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwoPanelBrowserAvalonia.Controllers
{
    public class FunctionalToolbarController
    {
        private AppController _appController;
        public FunctionalToolbarController(AppController appController)
        {
            _appController = appController;
        }

        public void OnRename()
        {
            _appController?.ActiveFileBrowser?.RenameSelectedItem();
        }

    }
}
