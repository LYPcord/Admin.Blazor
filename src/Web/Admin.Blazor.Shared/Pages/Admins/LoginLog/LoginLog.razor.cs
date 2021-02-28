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

        /// <summary>
        /// OnInitializedAsync 方法
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            var bindItems = await GetLoginLogs();
            Items = bindItems.Data.List.Select(s => new BindItem
            {
                Id = s.Id,
                NickName = s.NickName,
                CreatedTime = s.CreatedTime,
                IP = s.IP,
                ElapsedMilliseconds = s.ElapsedMilliseconds,
                Status = s.Status,
                Msg = s.Msg,
                Browser = s.Browser,
                Os = s.Os
            }).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        private Task<QueryData<BindItem>> OnQueryAsync(QueryPageOptions options)
        {
            var items = Items;

            var isSearched = false;
            if (options.Searchs.Any())
            {
                // 针对 SearchText 进行模糊查询
                items = items.Where(options.Searchs.GetFilterFunc<BindItem>(FilterLogic.Or)).ToList();
                isSearched = true;
            }

            // 过滤
            var isFiltered = false;
            if (options.Filters.Any())
            {
                items = items.Where(options.Filters.GetFilterFunc<BindItem>()).ToList();

                // 通知内部已经过滤数据了
                isFiltered = true;
            }

            // 排序
            var isSorted = false;
            if (!string.IsNullOrEmpty(options.SortName))
            {
                var invoker = items.GetSortLambda().Compile();
                items = invoker(items, options.SortName, options.SortOrder).ToList();

                // 通知内部已经过滤数据了
                isSorted = true;
            }

            // 设置记录总数
            var total = items.Count();

            // 内存分页
            //items = items.Skip((options.PageIndex - 1) * options.PageItems).Take(options.PageItems).ToList();


            return Task.FromResult(new QueryData<BindItem>()
            {
                Items = items,
                TotalCount = total,
                IsSorted = isSorted,
                IsFiltered = isFiltered,
                IsSearch = isSearched
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

        private async Task<ResponseModel<PageOutput<LoginLogListOutput>>> GetLoginLogs()
        {
            pageInput.CurrentPage = 1;
            pageInput.PageSize = 10;
            return await LoginLogService.GetLoginLogsAsync(pageInput);
        }

        /// <summary>
        /// 
        /// </summary>
        private List<BindItem> Items { get; set; }

        /// <summary>
        /// 
        /// </summary>
        private IEnumerable<int> PageItemsSource => new int[] { 4, 10, 20 };

        /// <summary>
        ///
        /// </summary>
        public class BindItem
        {
            /// <summary>
            ///
            /// </summary>
            [Display(Name = "主键")]
            [AutoGenerateColumn(Ignore = true)]
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
            [AutoGenerateColumn(Order = 1, FormatString = "yyyy-MM-dd", Width = 180, Sortable = true, Filterable = true, Searchable = true)]
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
