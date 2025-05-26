using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;

namespace DatingApp.Components
{
    public class BlazorBase : ComponentBase
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        [Inject]
        internal NavigationManager NavigationManager { get; set; }

        [Inject]
        internal ISnackbar Snackbar { get; set; }


        [Inject]
        internal IConfiguration Configuration { get; set; }

        [Inject]
        internal HttpClient Client { get; set; }

        [Inject]
        internal NavigationManager Navigation { get; set; }

        [Inject]
        internal IJSRuntime JSRuntime { get; set; }

        [Inject]
        internal IDialogService MudDialog { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    }
}
