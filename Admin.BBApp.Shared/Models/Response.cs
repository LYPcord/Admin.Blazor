namespace Admin.BBApp.Shared.Models
{
    public class Response<T>
    {
        public int Code { get; set; }
        public string Msg { get; set; }
        public T Data { get; set; }
    }
}
