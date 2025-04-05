using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using System;
using Tmds.DBus.Protocol;
using TwoPanelBrowserAvalonia.Controllers;
using TwoPanelBrowserAvalonia.Utils;

namespace TwoPanelBrowserAvalonia.Components;

public partial class FunctionalToolbar : UserControl
{
    private FunctionalToolbarController? _controller;
    public FunctionalToolbar()
    {
        InitializeComponent();
        
        Loaded += OnLoaded;
        DataContextChanged += OnDataContextChanged;
    }

    private void OnDataContextChanged(object? sender, EventArgs e)
    {
        _controller = ContextUtils.FromContext<FunctionalToolbarController>(DataContext);
    }

    private void OnLoaded(object? sender, RoutedEventArgs e)
    {
        TopLevel.GetTopLevel(this).KeyDown += OnKeyDown;
    }

    private void OnKeyDown(object? sender, KeyEventArgs e)
    {
        switch (e.Key)
        {
            case Key.F2:
                _controller?.OnRename();
                break;
        }
        
        
    }

    public void OnRename(object sender, RoutedEventArgs e)
    {
       _controller?.OnRename();
    }

    public void OnView(object sender, RoutedEventArgs e)
    {
        // Method implementation here
    }

    public void OnEdit(object sender, RoutedEventArgs e)
    {
        // Method implementation here
    }

    public void OnCopy(object sender, RoutedEventArgs e)
    {
        // Method implementation here
    }

    public void OnMove(object sender, RoutedEventArgs e)
    {
        // Method implementation here
    }

    public void OnNewDir(object sender, RoutedEventArgs e)
    {
        // Method implementation here
    }

    public void OnDelete(object sender, RoutedEventArgs e)
    {
        // Method implementation here
    }
}