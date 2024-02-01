using AccessoriesWebsite.Interfaces;
using AccessoriesWebsite.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AccessoriesWebsite.Implementation
{
    public class BaseRepo<T> : IBaseRepo<T> where T : class
    {
        public ApplicationDBContext Context;
        public DbSet<T> Set;

        public BaseRepo(ApplicationDBContext context)
        {
            Context = context;
            Set = Context.Set<T>();
        }


        public IQueryable<T> GetAll()
        {
            return Set;
        }

        //get all with filter where 
        public IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "")
        {
            IQueryable<T> query = Set;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProp);
            }
            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }

        public T GetById(int id)
        {
            return Set.Find(id);
        }


        public T Create(T entity)
        {
            Set.Add(entity);
            return Context.SaveChanges() > 0 ? entity : null;
        }

        public List<T> CreateRange(List<T> entity)
        {
            Set.AddRange(entity);
            return Context.SaveChanges() > 0 ? entity : null;
        }

        public bool Delete(T entity)
        {
            Set.Remove(entity);
            return Context.SaveChanges() > 0;
        }

        public bool Update(T entity)
        {
            Set.Update(entity);
            return Context.SaveChanges() > 0;
        }

        public List<T> UpdateRange(List<T> entity)
        {
            Set.UpdateRange(entity);
            return Context.SaveChanges() > 0 ? entity : null;
        }
    }
}
