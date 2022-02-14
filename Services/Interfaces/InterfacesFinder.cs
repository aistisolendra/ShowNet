using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;

namespace ShowNet.Services.Interfaces;

public interface IInterfacesFinder
{
    List<Interface> GetInterfaces();
    void UpdateInterfaces();
}

public class InterfacesFinder : IInterfacesFinder
{
    private List<Interface> _interfaces;

    public InterfacesFinder()
    {
        _interfaces = new List<Interface>();

        UpdateInterfaces();
    }

    public List<Interface> GetInterfaces()
    {
        return _interfaces;
    }

    public void UpdateInterfaces()
    {
        _interfaces = NetworkInterface
            .GetAllNetworkInterfaces()
            .Select(i => new Interface(i, GetIpVersions(i)))
            .ToList();
    }

    private string GetIpVersions(NetworkInterface inter)
    {
        string versionResult =
            inter.Supports(NetworkInterfaceComponent.IPv4)
                ? "IPv4"
                : "IPv4 not supported";

        versionResult +=
            inter.Supports(NetworkInterfaceComponent.IPv6)
                ? "IPv6"
                : "IPv6 not supported";

        return versionResult;
    }
}

public class Interface : INotifyPropertyChanged
{
    public Interface(
        string name,
        string description,
        string interfaceType,
        string opStatus,
        string dnsEnabled,
        string ipVersion)
    {
        Name = name;
        Description = description;
        InterfaceType = interfaceType;
        OpStatus = opStatus;
        DnsEnabled = dnsEnabled;
        IpVersion = ipVersion;
    }

    public Interface(NetworkInterface inter, string ipVersion)
    {
        Name = inter.Name;
        Description = inter.Description;
        InterfaceType = inter.NetworkInterfaceType.ToString();
        DnsEnabled = inter.GetIPProperties().IsDnsEnabled.ToString();
        IpVersion = ipVersion;
        OpStatus = inter.OperationalStatus.ToString();
    }

    private string _name = string.Empty;

    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            OnPropertyChanged(nameof(Name));
        }
    }

    private string _description = string.Empty;

    public string Description
    {
        get => _description;
        set
        {
            _description = value;
            OnPropertyChanged(nameof(Description));
        }
    }

    private string _interfaceType = string.Empty;

    public string InterfaceType
    {
        get => _interfaceType;
        set
        {
            _interfaceType = value;
            OnPropertyChanged(nameof(InterfaceType));
        }
    }

    private string _dnsEnabled = string.Empty;

    public string DnsEnabled
    {
        get => _dnsEnabled;
        set
        {
            _dnsEnabled = value;
            OnPropertyChanged(nameof(DnsEnabled));
        }
    }

    private string _ipVersion = string.Empty;

    public string IpVersion
    {
        get => _ipVersion;
        set
        {
            _ipVersion = value;
            OnPropertyChanged(IpVersion);
        }
    }

    private string _opStatus = string.Empty;

    public string OpStatus
    {
        get => _opStatus;
        set
        {
            _opStatus = value;
            OnPropertyChanged(OpStatus);
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public void OnPropertyChanged([CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
