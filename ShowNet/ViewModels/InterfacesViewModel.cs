using System.Collections.ObjectModel;
using System.ComponentModel;
using Caliburn.Micro;
using ShowNet.Services;

namespace ShowNet.ViewModels;

public class InterfacesViewModel : Screen, INotifyPropertyChanged
{
    private readonly InterfacesFinder _interfacesFinder;

    public InterfacesViewModel(InterfacesFinder interfacesFinder)
    {
        _interfacesFinder = interfacesFinder;

        Interfaces = new ObservableCollection<Interface>(_interfacesFinder.GetInterfaces());
    }

    private ObservableCollection<Interface> _interfaces = new();

    public ObservableCollection<Interface> Interfaces
    {
        get => _interfaces;
        set
        {
            _interfaces = value;
            OnPropertyChanged(nameof(Interfaces));
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    public void OnPropertyChanged(string propertyName)
    {
        if (PropertyChanged != null)
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
    }
}
