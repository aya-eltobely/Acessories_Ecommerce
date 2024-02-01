namespace AccessoriesWebsite.Helpers
{
    public class PageResult<T> where T : class
    {

        public PageResult() { }

        public List<T> Data { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalItem { get; set; }
        public string search { get; set; }
    }
}
