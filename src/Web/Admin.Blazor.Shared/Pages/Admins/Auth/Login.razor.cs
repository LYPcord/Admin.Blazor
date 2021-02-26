using System.Threading.Tasks;
using Admin.Blazor.Shared.Data.Admin;
using Admin.Core.Service.Admin.Auth.Input;
using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Admin.Blazor.Shared.Pages.Admins.Auth
{
    public partial class Login
    {

#nullable disable
        private Message MessageElement { get; set; }
#nullable disable

        private AuthLoginInput Model = new AuthLoginInput();
        [Inject] public AuthService AuthService { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }

        /// <summary>
        /// OnInitializedAsync 方法
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }

        private async Task HandleLogin(EditContext context) 
        {
            var rm =await AuthService.LoginAsync(Model);
           
            if (rm.Code==1)
                NavigationManager.NavigateTo("/");
            else
                ShowMessage(Color.Warning, rm.Msg);
        }

        [Inject] public MessageService? MessageService { get; set; }

        private void ShowMessage(Color color,string msg)
        {
            MessageElement.SetPlacement(Placement.Top);
            MessageService?.Show(new MessageOption()
            {
                Host = MessageElement,
                Content = msg,
                Color=color
            });
        }
    }
}
