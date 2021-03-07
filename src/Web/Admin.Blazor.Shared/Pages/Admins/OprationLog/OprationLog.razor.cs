using Admin.Blazor.Shared.Data.Admin;
using Admin.Core.Common.Input;
using Admin.Core.Model.Admin;
using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Blazor.Shared.Pages.Admins.OprationLog
{
    public partial class OprationLog : ComponentBase
    {
        [Inject] public OprationLogService OprationLogService { get; set; }

        PageInput<OprationLogEntity> pageInput = new PageInput<OprationLogEntity>();
        private long TotalCount { get; set; }

        /// <summary>
        /// OnInitializedAsync 方法
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            pageInput.CurrentPage = 1;
            pageInput.PageSize = PageItemsSource?.First() ?? 0;
            Items = await GetOprationLogs(pageInput);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        private async Task<QueryData<BindItem>> OnQueryAsync(QueryPageOptions options)
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

            // 数据请求
            pageInput.CurrentPage = options.PageIndex;
            pageInput.PageSize = options.PageItems;
            items = await GetOprationLogs(pageInput);


            return await Task.FromResult(new QueryData<BindItem>()
            {
                Items = items,
                TotalCount = TotalCount,
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

        private async Task<List<BindItem>> GetOprationLogs(PageInput<OprationLogEntity> page)
        {
            var logs = await OprationLogService.GetOprationLogsAsync(page);
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
                Os = s.Os,
                ApiLabel = s.ApiLabel,
                ApiPath = s.ApiPath
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
            /// 编号
            /// </summary>
            [Display(Name = "编号")]
            [AutoGenerateColumn(Order = 10, Readonly = true)]
            public long Id { get; set; }

            /// <summary>
            ///操作账号
            /// </summary>
            //[Required(ErrorMessage = "{0}不能为空")]
            [AutoGenerateColumn(Order = 20, Sortable = true, Filterable = true, Searchable = true)]
            [Display(Name = "操作账号")]
            public string NickName { get; set; }


            /// <summary>
            /// IP地址
            /// </summary>
            [Display(Name = "IP地址")]
            [AutoGenerateColumn(Order = 30, Sortable = true, Filterable = true, Searchable = true)]
            public string IP { get; set; }

            /// <summary>
            /// 操作名称
            /// </summary>
            [Display(Name = "操作名称")]
            [AutoGenerateColumn(Order = 40, Sortable = true, Filterable = true, Searchable = true)]
            public string ApiLabel { get; set; }

            /// <summary>
            /// 操作接口
            /// </summary>
            [Display(Name = "操作接口")]
            [AutoGenerateColumn(Order = 50, Sortable = true, Filterable = true, Searchable = true)]
            public string ApiPath { get; set; }

            /// <summary>
            /// 耗时(毫秒)
            /// </summary>
            [Display(Name = "耗时(毫秒)")]
            [AutoGenerateColumn(Order = 60, Sortable = true, Filterable = true, Searchable = true)]
            public long ElapsedMilliseconds { get; set; }

            /// <summary>
            /// 操作状态
            /// </summary>
            [Display(Name = "操作状态")]
            [AutoGenerateColumn(Order = 70, Sortable = true, Filterable = true)]
            public bool Status { get; set; }

            /// <summary>
            ///操作消息
            /// </summary>
            [AutoGenerateColumn(Order = 80, Sortable = true, Filterable = true, Searchable = true)]
            [Display(Name = "操作消息")]
            public string Msg { get; set; }


            /// <summary>
            /// 操作时间
            /// </summary>
            [AutoGenerateColumn(Order = 90, FormatString = "yyyy-MM-dd HH:mm:ss", Width = 180, Sortable = true, Filterable = true, Searchable = true)]
            [Display(Name = "操作时间")]
            public DateTime? CreatedTime { get; set; }

            /// <summary>
            ///浏览器
            /// </summary>
            [AutoGenerateColumn(Ignore = true, Order = 100, Sortable = true, Filterable = true, Searchable = true)]
            [Display(Name = "浏览器")]
            public string Browser { get; set; }

            /// <summary>
            ///操作系统
            /// </summary>
            [AutoGenerateColumn(Ignore = true, Order = 110, Sortable = true, Filterable = true, Searchable = true)]
            [Display(Name = "操作系统")]
            public string Os { get; set; }
        }
    }
}
