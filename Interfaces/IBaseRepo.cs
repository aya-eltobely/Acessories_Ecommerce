using System.Linq.Expressions;

namespace AccessoriesWebsite.Interfaces
{
    public interface IBaseRepo<T> where T : class
    {
        // C R U D

        //public IQueryable<T> GetAll();
        //public IQueryable<T> GetAll(Func<T> Expression);

        IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = ""
            );


        public T Create(T entity);
        public List<T> CreateRange(List<T> entity);
        public List<T> UpdateRange(List<T> entity);

        public bool Delete(T entity);
        public bool Update(T entity);
        public T GetById(int id);
    }
}
