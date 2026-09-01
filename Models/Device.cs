using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Diagnostics;
using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore.Sqlite;
using Microsoft.EntityFrameworkCore.Design;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace MyICSv2._1.Models
{
    public class Device
    {
        [Key]
        public int DeviceId { get; set; }

        [Required]
        [MaxLength(200)]
        public string DeviceName { get; set; } = string.Empty;

        [Required]
        [MaxLength(12)]
        public string DeviceAddressIPv4 { get; set; } = string.Empty;
        
        [MaxLength(39)]
        public string DeviceAddressIPv6 { get; set; } = string.Empty;
        
        public bool? IsActive { get; set; } = null;

        //public string DeviceStatus => IsActive.HasValue ? (IsActive.Value ? "Active" : "Inactive") : "Unknown";
        public string DeviceStatus { get; set; } = "Unknown";
        public Device() { DeviceStatus = "Unknown"; }

        public Device(string name)
        {
            DeviceName = name;
            DeviceStatus = "Unknown";
        }
    }
}
