using Avalonia;
using Avalonia.Controls;
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
    public FunctionalToolbar()
    {
        InitializeComponent();
    }

    public void OnRename(object sender, RoutedEventArgs e)
    {
        ContextUtils.FromContext<FunctionalToolbarController>(DataContext).OnRename();
        
        // Method implementation here
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