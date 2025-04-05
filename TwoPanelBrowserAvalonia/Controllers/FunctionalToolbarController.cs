using Avalonia.Controls;
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
        private Window _mainWindow;
        public FunctionalToolbarController(AppController appController, Window mainWindow)
        {
            _appController = appController;
            _mainWindow = mainWindow;
        }

        public void OnRename()
        {
            var browser = _appController?.ActiveFileBrowser;
            if (browser == null || browser.SelectedItem == null)
                return;

            var dialog = new RenameWindow();
            dialog.ShowDialog(_mainWindow);
        }

    }
}
