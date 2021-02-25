using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Blazor.Shared.Data
{
    public class BaseResponseModel
    {
        public int Code { get; set; }
        public string Msg { get; set; }
    }

    public class ResponseModel : BaseResponseModel
    {
        public object Data { get; set; } = new object();
    }

    public class ResponseModel<T>: BaseResponseModel
    {
        public T Data { get; set; }
    }
}
