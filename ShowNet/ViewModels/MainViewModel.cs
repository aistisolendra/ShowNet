using Caliburn.Micro;

namespace ShowNet.ViewModels;

public class MainViewModel : Conductor<Screen>
{
    private readonly Screen _interfacesScreen;

    public MainViewModel(InterfacesViewModel interfacesViewModel)
    {
        _interfacesScreen = interfacesViewModel;

        ChangeView(_interfacesScreen);
    }

    private async void ChangeView(Screen screen)
    {
        await ActivateItemAsync(screen)
            .ConfigureAwait(false);
    }
}
