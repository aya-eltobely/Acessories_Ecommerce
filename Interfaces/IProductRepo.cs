using AccessoriesWebsite.Helpers;
using AccessoriesWebsite.Models;
using System.Numerics;

namespace AccessoriesWebsite.Interfaces
{
    public interface IProductRepo : IBaseRepo<Product>
    {
        PageResult<Product> GetAll(int pagenumber, int pagesize, string includeProperties, string search);
        List<Product> GetAll(string includeProperties);

        Product GetById(int id , string includeProperties);

    }
}
