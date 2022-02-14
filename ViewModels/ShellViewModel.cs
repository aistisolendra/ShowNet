using Caliburn.Micro;

namespace ShowNet.ViewModels;

public class ShellViewModel : Conductor<Screen>
{
    public ShellViewModel(MainViewModel mainViewModel, InterfacesViewModel interfacesViewModel)
    {
        ChangeView(interfacesViewModel);
    }

    private async void ChangeView(Screen screen)
    {
        await ActivateItemAsync(screen)
            .ConfigureAwait(false);
    }
}