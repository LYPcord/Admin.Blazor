using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Admin.Blazor.Shared.Data.Admin;
using Admin.Core.Service.Admin.Auth.Input;
using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Logging;

namespace Admin.Blazor.Shared.Pages.Admins.Auth
{
    public partial class Login
    {
        private AuthLoginInput Model = new AuthLoginInput();
        [Inject] public AuthService _authService { get; set; }

        /// <summary>
        /// OnInitialized 方法
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();
        }

        private Task OnInvalidSubmit1(EditContext context)
        {
            return Task.CompletedTask;
        }

        private Task OnValidSubmit(EditContext context)
        {
            return Task.CompletedTask;
        }

        private Task OnInvalidSubmit(EditContext context)
        {
            return Task.CompletedTask;
        }

        private async Task<string> OnSubmit(EditContext context) 
        {

            var token =await _authService.LoginAsync(Model);
            ShowMessage(Color.Success,"66");
            return "";
        }
        private Message MessageElement { get; set; }

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
