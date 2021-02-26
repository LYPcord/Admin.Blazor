using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Blazor.Shared.Data.Admin.Models
{
    public class UserInfo
    {
        public User User { get; set; }
        public Menus[] Menus { get; set; }
        public string[] Permissions { get; set; }
    }
    public class User 
    {
        public string NickName { get; set; }
        public string UserName { get; set; }
        public string Avatar { get; set; }
    }
    public class Menus 
    {
        public long Id { get; set; }
        public long ParentId { get; set; }
        public string Path { get; set; }
        public string ViewPath { get; set; }
        public string Label { get; set; }
        public string Icon { get; set; }
        public bool? Opened { get; set; }
        public bool? Closable { get; set; }
        public bool Hidden { get; set; }
        public bool? NewWindow { get; set; }
        public bool? External { get; set; }
    }
}
