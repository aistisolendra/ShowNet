using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
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

        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
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
        RegisterServices();
        RegisterViewModels();
    }

    protected override void OnExit(object sender, EventArgs e)
    {
        NLog.LogManager.Shutdown();
        base.OnExit(sender, e);
    }

    private void RegisterViewModels()
    {
        _container.Singleton<ShellViewModel, ShellViewModel>();
        _container.Singleton<InterfacesViewModel>();
        _container.Singleton<MainViewModel>();
    }

    private void RegisterNecessary()
    {
        _container.Singleton<IWindowManager, WindowManager>();
        _container.Singleton<IEventAggregator, EventAggregator>();
    }

    private void RegisterServices()
    {
        _container.PerRequest<InterfacesFinder>();
    }
}
