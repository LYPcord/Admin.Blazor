using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;

using Admin.Blazor.Shared.Data.Admin;
using Admin.Core;
using Admin.Core.Common.Input;
using Admin.Core.Model.Admin;

using BootstrapBlazor.Components;


namespace Admin.Blazor.Shared.Pages.Admins.User
{
    public partial class User : ComponentBase
    {

        PageInput<UserEntity> pageInput = new PageInput<UserEntity>();
        private long TotalCount { get; set; }

        [Inject] public UserService UserService { get; set; }
        [Inject] public RoleService RoleService { get; set; }

        [NotNull]
        private Modal? Modal { get; set; }

        /// <summary>
        /// 
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            pageInput.CurrentPage = 1;
            pageInput.PageSize = PageItemsSource?.First() ?? 0;
            Items =await GetUsers(pageInput);
            //await GetAllRoles();
        }


        /// <summary>
        /// 查询
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

        private async Task<bool> OnDeleteAsync(IEnumerable<BindItem> items)
        {          
            var ids = items.Select(s => s.Id).ToArray();
            var response = await UserService.UserBatchSoftDeleteAsync(ids);

            if (response.Code == 1)
                Items.RemoveAll(item => items.Contains(item));

            return await Task.FromResult(response.Code == 1);
        }

        private Task<bool> OnSaveAsync(BindItem item)
        {
            Items.Add(item);
            return Task.FromResult(true);
        }

        /// <summary>
        /// 编辑用户信息
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        private async Task ShowEditDialog(IEnumerable<BindItem> items)
        {
            await GetAllRoles();//获取全部角色信息

            var id = items.FirstOrDefault().Id;
            var response = await UserService.GetUserByIdAsync(id);

            if (response.Code == 1) 
                Model = response.Data.ChangeType<EditModel>();

            await Modal.Toggle();
        }

        private async Task OnConfirm()
        {
            //_confirm = true;
            await Modal.Toggle();
        }

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
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
        /// 获得 默认数据集合
        /// </summary>
        private IEnumerable<SelectedItem> RoleSelectedItems;

        /// <summary>
        /// 获取所有角色
        /// </summary>
        /// <summary>
        private async Task GetAllRoles()
        {
            var input =new PageInput<RoleEntity>();
            input.PageSize = int.MaxValue;

            var roles = await RoleService.GetPageRoleListAsync(input);

            if (roles.Code==1)
            {
                RoleSelectedItems = roles.Data.List.Select(s => new SelectedItem 
                {
                     Value=s.Id.ToString(),
                     Text=s.Name                             
                }).ToList();
            }
        }

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
            [Display(Name = "用户名")]
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
            /// 创建时间
            /// </summary>
            [AutoGenerateColumn(Order = 40, FormatString = "yyyy-MM-dd HH;mm;ss", Width = 180, Sortable = true, Filterable = true, Searchable = true)]
            [Display(Name = "创建时间")]
            public DateTime? CreatedTime { get; set; }

            /// <summary>
            /// 状态
            /// </summary>
            [Display(Name = "状态")]
            [AutoGenerateColumn(Order = 50, Sortable = true, Filterable = true, Searchable = true)]
            public int Status { get; set; }
        }

        public EditModel Model { get; set; } = new EditModel();

        public class EditModel
        {
            /// <summary>
            /// 主键Id
            /// </summary>
            public int Id { get; set; }

            /// <summary>
            /// 账号
            /// </summary>
            [Required(ErrorMessage = "请输入账号")]
            public string UserName { get; set; }

            /// <summary>
            /// 昵称
            /// </summary>
            public string NickName { get; set; }

            /// <summary>
            /// 状态
            /// </summary>
            public int Status { get; set; }

            /// <summary>
            /// 备注
            /// </summary>
            public string Remark { get; set; }

            /// <summary>
            /// 角色
            /// </summary>
            public long[] RoleIds { get; set; }

            /// <summary>
            /// 角色名
            /// </summary>
            public string RoleNames { get; set; } 

            /// <summary>
            /// 版本
            /// </summary>
            public long Version { get; set; }
        }
    }
}
