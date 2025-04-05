using Avalonia.Controls;
using Avalonia.Input;
using Microsoft.Extensions.DependencyInjection;
using System;
using TwoPanelBrowserAvalonia.Controllers;

namespace TwoPanelBrowserAvalonia;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }
    public MainWindow(IServiceProvider serviceProvider)
    {
        InitializeComponent();
        var appController = serviceProvider.GetService<AppController>();
        PanelLeft.DataContext = appController;
        PanelRight.DataContext = appController;
    }

}