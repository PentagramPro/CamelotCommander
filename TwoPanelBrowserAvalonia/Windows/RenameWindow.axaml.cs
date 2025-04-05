using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace TwoPanelBrowserAvalonia;

public partial class RenameWindow : Window
{
    public RenameWindow()
    {
        InitializeComponent();
    }

    public void OnCancel(object sender, RoutedEventArgs e)
    {
        Close();
    }

    public void OnOk(object sender, RoutedEventArgs e)
    {
    }
}