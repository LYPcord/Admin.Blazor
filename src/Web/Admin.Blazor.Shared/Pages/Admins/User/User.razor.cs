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

namespace Admin.Blazor.Shared.Pages.Admins.User
{
    public partial class User : ComponentBase
    {
        [Inject] public UserService UserService { get; set; }

        PageInput<UserEntity> pageInput = new PageInput<UserEntity>();
        private long TotalCount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            pageInput.CurrentPage = 1;
            pageInput.PageSize = PageItemsSource?.First() ?? 0;
            Items =await GetUsers(pageInput);
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
            items = await GetUsers(pageInput);

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

        private async Task<List<BindItem>> GetUsers(PageInput<UserEntity> page)
        {
            var users = await UserService.GetUsersAsync(page);
            System.Console.WriteLine(users.ToString());
            TotalCount = users.Data.Total;
            return users.Data.List.Select(s => new BindItem
            {
                Id = s.Id,
                NickName = s.NickName,
                CreatedTime = s.CreatedTime,
                RoleNames = string.Join(',', s.RoleNames),
                Status = s.Status,
                UserName = s.UserName
            }).ToList();
        }

        /// <summary>
        /// 数据源
        /// </summary>
        private List<BindItem> Items { get; set; }

        /// <summary>
        /// 页码
        /// </summary>
        private IEnumerable<int> PageItemsSource => new int[] { 4, 10, 20 };

        /// <summary>
        /// 绑定数据源
        /// </summary>
        public class BindItem
        {
            /// <summary>
            ///  编号
            /// </summary>
            [Display(Name = "编号")]
            [AutoGenerateColumn(Order = 10,Editable =false)]
            public long Id { get; set; }

            /// <summary>
            /// 用户名
            /// </summary>
            [AutoGenerateColumn(Order = 20, Sortable = true, Filterable = true, Searchable = true, Readonly = true)]
            [Display(Name = "姓名")]
            public string UserName { get; set; }

            /// <summary>
            /// 昵称
            /// </summary>
            [Display(Name = "昵称")]
            [AutoGenerateColumn(Order = 20, Sortable = true, Filterable = true, Searchable = true)]
            public string NickName { get; set; }

            /// <summary>
            /// 角色
            /// </summary>
            [Display(Name = "角色")]
            [AutoGenerateColumn(Order = 30, Sortable = true, Filterable = true, Searchable = true)]
            public string RoleNames { get; set; }

            /// <summary>
            /// 状态
            /// </summary>
            [Display(Name = "状态")]
            [AutoGenerateColumn(Order = 40, Sortable = true, Filterable = true, Searchable = true)]
            public int Status { get; set; }

            /// <summary>
            /// 创建时间
            /// </summary>
            [AutoGenerateColumn(Order = 50, FormatString = "yyyy-MM-dd HH;mm;ss", Width = 180, Sortable = true, Filterable = true, Searchable = true)]
            [Display(Name = "创建时间")]
            public DateTime? CreatedTime { get; set; }
        }
    }
}
