using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MyICSv2._1.Models;

namespace MyICSv2._1.Views
{
    public partial class AddDeviceWindow : Window
    {
        public AddDeviceWindow()
        {
            InitializeComponent();
        }

        private void OnClick_Save(object? sender, RoutedEventArgs e)
        {
            var name = DeviceNameField.Text?.Trim();

            if (string.IsNullOrWhiteSpace(name))
            {
                // simple guard � you could show a validation message instead
                return;
            }

            // Return the new Device as the dialog result
            Close(new Device(name.Trim()));
        }

        private void OnClick_Cancel(object? sender, RoutedEventArgs e)
        {
            Close(null);
        }
    }
}