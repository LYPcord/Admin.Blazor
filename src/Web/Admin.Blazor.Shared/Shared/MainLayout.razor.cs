using Admin.Blazor.Shared.Data.Admin;
using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Admin.Blazor.Shared.Shared
{
    /// <summary>
    /// 
    /// </summary>
    public sealed partial class MainLayout
    {
        [Inject] public AuthService AuthService { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        private bool UseTabSet { get; set; } = true;

        private string Theme { get; set; } = "";

        private bool IsOpen { get; set; }

        private bool IsFixedHeader { get; set; } = true;

        private bool IsFixedFooter { get; set; } = true;

        private bool IsFullSide { get; set; } = true;

        private bool ShowFooter { get; set; } = true;

        private List<MenuItem> Menus { get; set; }

        private string UserName { get; set; }
        private string NickName { get; set; }

        /// <summary>
        /// 异步初始化时 方法
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            // TODO: 菜单获取可以通过数据库获取，此处为示例直接拼装的菜单集合
            Menus = GetIconSideMenuItems();

            //await GetUserInfoAsync();
        }

        private static List<MenuItem> GetIconSideMenuItems()
        {
            var menus = new List<MenuItem>
            {
                new MenuItem() { Text = "返回组件库", Icon = "fa fa-fw fa-home", Url = "https://www.blazor.zone/components" },
                new MenuItem() { Text = "Index", Icon = "fa fa-fw fa-fa", Url = "" },
                new MenuItem() { Text = "Counter", Icon = "fa fa-fw fa-check-square-o", Url = "counter" },
                new MenuItem() { Text = "FetchData", Icon = "fa fa-fw fa-database", Url = "fetchdata" },
                new MenuItem() { Text = "Table", Icon = "fa fa-fw fa-table", Url = "table" },
                new MenuItem() { Text = "登录日志", Icon = "fa fa-fw fa-table", Url = "admin/loginlog" },
                new MenuItem() { Text = "操作日志", Icon = "fa fa-fw fa-table", Url = "admin/oprationlog" },
                new MenuItem() { Text = "用户管理", Icon = "fa fa-fw fa-table", Url = "admin/user" }
            };

            return menus;
        }

        private async Task GetUserInfoAsync()
        {
            var rm = await AuthService.GetUserInfoAsync();
            if (rm.Code == 1)
            {
                UserName = rm.Data.User.UserName;
                NickName = rm.Data.User.NickName;
                Menus = rm.Data.Menus.Select(s => new MenuItem 
                {
                    Text=s.Label,
                    Icon= "fa fa-fw fa-fa",
                    Url=s.Path
                }).ToList();
            }
        }
    }
}
