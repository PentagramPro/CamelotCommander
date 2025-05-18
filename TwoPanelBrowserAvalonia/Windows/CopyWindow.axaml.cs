using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System;
using TwoPanelBrowserAvalonia.Controllers;
using TwoPanelBrowserAvalonia.FileSystem;
using TwoPanelBrowserAvalonia.Utils;

namespace TwoPanelBrowserAvalonia;

public partial class CopyWindow : Window
{
    CopyWindowController _controller;
    public CopyWindow(IFileSystemItem selectedFile, string targetPath)
    {
        InitializeComponent();
        var controller = new CopyWindowController();
        DataContext = controller;

        Opened += async (_, __) =>
        {
            await controller.InitializeAsync(selectedFile, targetPath);
        };

    }


    public void OnSkipIfExists(object sender, RoutedEventArgs e)
    {

    }

    public void OnOverwriteIfExists(object sender, RoutedEventArgs e)
    {

    }


    public void OnCancel(object sender, RoutedEventArgs e)
    {

    }
}
