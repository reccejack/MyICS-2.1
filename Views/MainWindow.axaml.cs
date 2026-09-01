using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MyICSv2._1.Models;
using MyICSv2._1.Data;

namespace MyICSv2._1.Views;

public partial class MainWindow : Window
{
    public ObservableCollection<Device> Devices { get; } = new ObservableCollection<Device>();

    private readonly IDbContextFactory<AppDbContext> _dbContextFactory;

    public MainWindow()
    {
        InitializeComponent();
        InputListBox.ItemsSource = Devices;

        _dbContextFactory = App.Services.GetRequiredService<IDbContextFactory<AppDbContext>>();

        Loaded += async (_, _) => await LoadItemsAsync();
    }

    private async Task LoadItemsAsync()
    {
        await using var db = await _dbContextFactory.CreateDbContextAsync();
        var items = await db.Devices.AsNoTracking().OrderBy(i => i.DeviceName).ToListAsync();

        Devices.Clear();
        foreach (var item in items)
        {
            Devices.Add(item);
        }
    }

    private async void OnClick_AddDevice(object? sender, RoutedEventArgs e)
    {
        var dialog = new AddDeviceWindow();

        // ShowDialog<Item?> waits for the dialog to close and gets back
        // whatever we passed into Close(...) in AddDeviceWindow
        var result = await dialog.ShowDialog<Device?>(this);

        if (result is not null)
        {
            await using var db = await _dbContextFactory.CreateDbContextAsync();
            db.Devices.Add(result);
            await db.SaveChangesAsync();
            Devices.Add(result);
        }
    }

    private async void OnClick_Remove(object? sender, RoutedEventArgs e)
    {
        if (InputListBox.SelectedItem is not Device selectedItem)
        {
            return;
        }

        await using var db = await _dbContextFactory.CreateDbContextAsync();

        // Delete by Id rather than trusting the in-memory object's full state
        var rowsAffected = await db.Devices
            .Where(i => i.DeviceId == selectedItem.DeviceId)
            .ExecuteDeleteAsync();

        if (rowsAffected > 0)
        {
            Devices.Remove(selectedItem);
        }
        // If rowsAffected == 0, someone/something else already deleted this row
        // (e.g. another client). You may want to show a message and call
        // LoadItemsAsync() again to resync the list here.
    }
}