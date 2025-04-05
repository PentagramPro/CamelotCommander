using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System;
using System.IO;
using TwoPanelBrowserAvalonia.Controllers;
using TwoPanelBrowserAvalonia.FileSystem;
using TwoPanelBrowserAvalonia.Utils;

namespace TwoPanelBrowserAvalonia;

public partial class RenameWindow : Window
{
    private IFileSystemItem? _targetFile;
    private RenameWindowController? _controller;
    public RenameWindow()
    {
        InitializeComponent();
        DataContextChanged += OnDataContextChanged;
    }

    private void OnDataContextChanged(object? sender, EventArgs e)
    {
        _controller = ContextUtils.FromContext<RenameWindowController>(DataContext);
        _targetFile = _controller.TargetFile;
        if (_targetFile != null)
        {
            FileNameTextBox.Text = _targetFile.Name;
            FileNameTextBox.Focus();
            FileNameTextBox.SelectAll();
        }
    }

    public void OnCancel(object sender, RoutedEventArgs e)
    {
        Close();
    }

    public void OnOk(object sender, RoutedEventArgs e)
    {
        if (_targetFile == null)
            return;

        string newName = FileNameTextBox.Text ?? "";
        _controller?.Rename(newName);
    }
}