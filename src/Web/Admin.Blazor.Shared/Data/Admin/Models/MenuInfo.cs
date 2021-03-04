using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Blazor.Shared.Data.Admin.Models
{
    public class MenuInfo
    {
        public List<Menu> Menus { get; set; }
        public List<Api> Apis { get; set; }
    }

    public class Menu 
    {
        public long Id { get; set; }
        public long ParentId { get; set; }
        public string Label { get; set; }
    }

    public class Api 
    {
        public long Id { get; set; }
        public string Label { get; set; }
    }
}
