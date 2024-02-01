namespace AccessoriesWebsite.Helpers
{
    public class ProductPageResult<T>:PageResult<T> where T : class
    {
        public int? catId { get; set; }
    }
}
