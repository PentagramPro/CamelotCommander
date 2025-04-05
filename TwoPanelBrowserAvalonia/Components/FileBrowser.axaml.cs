using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;
using TwoPanelBrowserAvalonia.Controllers;

namespace TwoPanelBrowserAvalonia.Components;

public partial class FileBrowser : UserControl
{

    private readonly FileBrowserController _controller;
 
    public FileBrowser()
    {
        InitializeComponent();
        _controller = new FileBrowserController();

        // Загрузка файлов текущей директории
        _controller.LoadFiles(Directory.GetCurrentDirectory());
        DataContext = _controller;

        
        
        FilesDataGrid.KeyDown += FilesDataGrid_KeyDown;
    }



    private void FilesDataGrid_KeyDown(object? sender, KeyEventArgs e)
    {
        switch(e.Key)
        {
            case Key.Enter:
                var item = FilesDataGrid.SelectedItem;
                break;
            case Key.Tab:
                
                break;
        }
    }
}