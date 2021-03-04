using Admin.Core.Model.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Blazor.Shared.Data.Admin.Models
{
    public class DocumentInfo
    {
        public List<Plain> Plains { get; set; }
    }

    public class Plain
    {
        public long Id { get; set; }
        public long ParentId { get; set; }
        public string Label { get; set; }
        public DocumentType Type { get; set; }
        public bool? Opened { get; set; }
    }
}
