using AccessoriesWebsite.Helpers;
using AccessoriesWebsite.Interfaces;
using AccessoriesWebsite.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Numerics;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace AccessoriesWebsite.Implementation
{
    public class ProductRepo : BaseRepo<Product>, IProductRepo
    {
        public ProductRepo(ApplicationDBContext Context) : base(Context)
        {
        }


        public PageResult<Product> GetAll(int pagenumber, int pagesize, string? includeProperties, string? search)
        {
            List<Product> data = new List<Product>();
            //var data = userManager.GetUsersInRoleAsync(WebSiteRoles.SiteDoctor).GetAwaiter().GetResult();

            int totalCount;
            try
            {
                var query = Set.AsQueryable();

                var ExcludedData = (pagenumber * pagesize) - pagesize;

                if (!string.IsNullOrWhiteSpace(includeProperties))
                {
                    foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        query = query.Include(includeProp); //.Skip(ExcludedData).Take(pagesize);
                    }
                }
                data = query.ToList();

                if (!string.IsNullOrWhiteSpace(search))
                {
                    data = data.Where(d => d.Name.ToLower().Contains(search.ToLower())).ToList();
                }

                data = data.Skip(ExcludedData).Take(pagesize).ToList();



                totalCount = Set.ToList().Count;
            }

            catch (Exception)
            {

                throw;
            }

            return new PageResult<Product>()
            {
                Data = data,
                PageNumber = pagenumber,
                PageSize = pagesize,
                TotalItem = totalCount
            };
        }

        public Product GetById(int id, string includeProperties)
        {
            var product = Context.products.AsQueryable();
            foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                product = product.Include(includeProp);
            }
            return product.Where(d => d.Id == id).FirstOrDefault();
        }

        public List<Product> GetAll(string includeProperties)
        {
            var products = Context.products.AsQueryable();
            foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                products = products.Include(includeProp);
            }
            return products.ToList();
        }
        
    }
}
