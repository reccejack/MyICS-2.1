using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyICSv2._1.Models;
using MyICSv2._1.Data;

namespace MyICSv2._1.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty]
    public partial string Greeting { get; set; } = "Welcome to Avalonia!";

    public ObservableCollection<Device> Devices { get; } = new ObservableCollection<Device>();

    private readonly IDbContextFactory<AppDbContext> _dbFactory;

    public MainViewModel()
    {
        _dbFactory = App.Services.GetRequiredService<IDbContextFactory<AppDbContext>>();
    }

    public async Task LoadItemsAsync()
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        var items = await db.Devices.AsNoTracking().OrderBy(d => d.DeviceName).ToListAsync();

        Devices.Clear();
        foreach (var item in items)
            Devices.Add(item);
    }

    public async Task AddDeviceAsync(Device device)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        db.Devices.Add(device);
        await db.SaveChangesAsync();
        Devices.Add(device);
    }

    public async Task<bool> RemoveDeviceAsync(Device device)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        var rows = await db.Devices.Where(d => d.DeviceId == device.DeviceId).ExecuteDeleteAsync();
        if (rows > 0)
        {
            Devices.Remove(device);
            return true;
        }

        return false;
    }
}
