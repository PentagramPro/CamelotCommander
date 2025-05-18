using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwoPanelBrowserAvalonia.FileSystem;

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

        private IFileSystemItem? GetSelectedItem()
        {
            var browser = _appController?.ActiveFileBrowser;
            if (browser == null || browser.SelectedItem == null)
                return null;
            return browser.SelectedItem;
        }
        public void OnRename()
        {
            var selectedItem = GetSelectedItem();
            if(selectedItem == null)
                return;

            var dialog = new RenameWindow();
            dialog.DataContext = new RenameWindowController(selectedItem);
            dialog.ShowDialog(_mainWindow);
        }

        public void OnCopy()
        {
            var selectedItem = GetSelectedItem();
            if(selectedItem == null)
                return;

            var otherBrowser = _appController?.OtherFileBrowser;
            if(otherBrowser == null)
                return;

            
            var dialog = new CopyWindow(selectedItem, otherBrowser.CurrentPath);
            dialog.ShowDialog(_mainWindow);
        }

    }
}
