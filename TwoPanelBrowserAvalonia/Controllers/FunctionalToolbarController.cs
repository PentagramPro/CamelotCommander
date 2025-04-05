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
            var browser = _appController?.ActiveFileBrowser;
            if (browser == null || browser.SelectedItem == null)
                return;

            
        }

    }
}
