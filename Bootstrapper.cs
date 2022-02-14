using System;
using System.Collections.Generic;
using System.Windows;
using Caliburn.Micro;
using ShowNet.Services;
using ShowNet.ViewModels;

namespace ShowNet;

public class Bootstrapper : BootstrapperBase
{
    private readonly SimpleContainer _container = new();

    public Bootstrapper()
    {
        Initialize();
    }

    protected override void OnStartup(object sender, StartupEventArgs e)
    {
        DisplayRootViewFor<ShellViewModel>();
    }

    protected override object GetInstance(Type serviceType, string key)
    {
        return _container.GetInstance(serviceType, key);
    }

    protected override IEnumerable<object> GetAllInstances(Type serviceType)
    {
        return _container.GetAllInstances(serviceType);
    }

    protected override void BuildUp(object instance)
    {
        _container.BuildUp(instance);
    }

    protected override void Configure()
    {
        RegisterNecessary();
        RegisterViewModels();
        RegisterServices();
    }

    private void RegisterViewModels()
    {
        _container.PerRequest<ShellViewModel, ShellViewModel>();
        _container.PerRequest<InterfacesViewModel>();
        _container.PerRequest<MainViewModel>();
    }

    private void RegisterNecessary()
    {
        _container.Singleton<IWindowManager, WindowManager>();
        _container.Singleton<IEventAggregator, EventAggregator>();
    }

    private void RegisterServices()
    {
        _container.PerRequest<IInterfacesFinder, InterfacesFinder>();
    }
}