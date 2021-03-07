using Admin.Blazor.Shared.Data;
using Admin.Blazor.Shared.Data.Admin;
using Admin.Core.Common.Input;
using Admin.Core.Common.Output;
using Admin.Core.Model.Admin;
using Admin.Core.Service.Admin.LoginLog.Output;
using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Admin.Core;

namespace Admin.Blazor.Shared.Pages.Admins.LoginLog
{
    public partial class LoginLog : ComponentBase
    {
        [Inject] public LoginLogService LoginLogService { get; set; }

        PageInput<LoginLogEntity> pageInput = new PageInput<LoginLogEntity>();
        private long TotalCount { get; set; }

        /// <summary>
        /// OnInitializedAsync 方法
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            pageInput.CurrentPage = 1;
            pageInput.PageSize = PageItemsSource?.First() ?? 0;
            Items= await GetLoginLogs(pageInput);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        private async Task<QueryData<BindItem>> OnQueryAsync(QueryPageOptions options)
        {
            var items = Items;

            // 数据请求
            pageInput.CurrentPage = options.PageIndex;
            pageInput.PageSize = options.PageItems;
            items = await GetLoginLogs(pageInput);

            return await Task.FromResult(new QueryData<BindItem>()
            {
                Items = items,
                TotalCount = TotalCount
            });
        }

        private Task<bool> OnDeleteAsync(IEnumerable<BindItem> items)
        {
            Items.RemoveAll(item => items.Contains(item));
            return Task.FromResult(true);
        }

        private Task<bool> OnSaveAsync(BindItem item)
        {
            Items.Add(item);
            return Task.FromResult(true);
        }

        private async Task<List<BindItem>> GetLoginLogs(PageInput<LoginLogEntity> page)
        {
            var logs = await LoginLogService.GetLoginLogsAsync(page);
            TotalCount = logs.Data.Total;
            return logs.Data.List.Select(s => new BindItem
            {
                Id = s.Id,
                NickName = s.NickName,
                CreatedTime = s.CreatedTime,
                IP = s.IP,
                ElapsedMilliseconds = s.ElapsedMilliseconds,
                Status = s.Status,
                Msg = s.Msg ?? "",
                Browser = s.Browser,
                Os = s.Os
            }).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        private List<BindItem> Items { get; set; }

        /// <summary>
        /// 
        /// </summary>
        private IEnumerable<int> PageItemsSource => new int[] { 4, 20, 50 };

        /// <summary>
        ///
        /// </summary>
        public class BindItem
        {
            /// <summary>
            ///
            /// </summary>
            [Display(Name = "主键")]
            [AutoGenerateColumn(Order = 0, Ignore = true, Readonly = true)]
            public long Id { get; set; }

            /// <summary>
            ///操作账号
            /// </summary>
            //[Required(ErrorMessage = "{0}不能为空")]
            [AutoGenerateColumn(Order = 10, Sortable = true, Filterable = true, Searchable = true)]
            [Display(Name = "操作账号")]
            public string NickName { get; set; }

            /// <summary>
            /// 登录时间
            /// </summary>
            [AutoGenerateColumn(Order = 1, FormatString = "yyyy-MM-dd HH:mm:ss", Width = 180, Sortable = true, Filterable = true, Searchable = true)]
            [Display(Name = "登录时间")]
            public DateTime? CreatedTime { get; set; }

            /// <summary>
            /// IP地址
            /// </summary>
            [Display(Name = "IP地址")]
            [AutoGenerateColumn(Order = 20, Sortable = true, Filterable = true, Searchable = true)]
            public string IP { get; set; }

            /// <summary>
            /// 耗时(毫秒)
            /// </summary>
            [Display(Name = "耗时(毫秒)")]
            [AutoGenerateColumn(Order = 40, Sortable = true, Filterable = true, Searchable = true)]
            public long ElapsedMilliseconds { get; set; }

            /// <summary>
            /// 登录状态
            /// </summary>
            [Display(Name = "登录状态")]
            [AutoGenerateColumn(Order = 50, Sortable = true, Filterable = true)]
            public bool Status { get; set; }

            /// <summary>
            ///登录信息
            /// </summary>
            [AutoGenerateColumn(Order = 10, Sortable = true, Filterable = true, Searchable = true)]
            [Display(Name = "登录信息")]
            public string Msg { get; set; }

            /// <summary>
            ///浏览器
            /// </summary>
            [AutoGenerateColumn(Order = 10, Sortable = true, Filterable = true, Searchable = true)]
            [Display(Name = "浏览器")]
            public string Browser { get; set; }

            /// <summary>
            ///操作系统
            /// </summary>
            [AutoGenerateColumn(Order = 10, Sortable = true, Filterable = true, Searchable = true)]
            [Display(Name = "操作系统")]
            public string Os { get; set; }
        }
    }
}
