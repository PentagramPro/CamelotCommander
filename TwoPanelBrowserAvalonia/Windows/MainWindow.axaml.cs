using Avalonia.Controls;
using Avalonia.Input;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
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
        AppController? appController = serviceProvider.GetService<AppController>();
        PanelLeft.DataContext = appController?.CreateFileBrowser();
        PanelRight.DataContext = appController?.CreateFileBrowser();
        FunctionalToolbar.DataContext = appController?.CreateFunctionalToolbar(this);

    }

}