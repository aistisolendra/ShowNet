using Caliburn.Micro;

namespace ShowNet.ViewModels;

public class ShellViewModel : Conductor<Screen>
{
    public ShellViewModel(MainViewModel mainViewModel)
    {
        ChangeView(mainViewModel);
    }

    private async void ChangeView(Screen screen)
    {
        await ActivateItemAsync(screen)
            .ConfigureAwait(false);
    }
}
