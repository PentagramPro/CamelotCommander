using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;
using TwoPanelBrowserAvalonia.Controllers;
using TwoPanelBrowserAvalonia.Controllers.FileSystem;
using TwoPanelBrowserAvalonia.Utils;

namespace TwoPanelBrowserAvalonia.Components;

public partial class FileBrowser : UserControl
{

    private FileBrowserController? _controller;
 
    public FileBrowser()
    {
        InitializeComponent();
       
        GotFocus += OnGotFocus;
        DataContextChanged += OnDataContextChanged;
        FilesDataGrid.SelectionChanged += OnSelectionChanged;
    }

    private void OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if(_controller == null)
            return;
        _controller.SelectedItem = (IFileSystemItem)FilesDataGrid.SelectedItem;
    }

    private void OnDataContextChanged(object? sender, EventArgs e)
    {
        _controller = ContextUtils.FromContext<FileBrowserController>(DataContext);
        FilesDataGrid.SelectedIndex = 0;
    }

    private void OnGotFocus(object? sender, GotFocusEventArgs e)
    {
        _controller?.OnGotFocus();
    }
}