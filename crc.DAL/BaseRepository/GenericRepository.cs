using System;
using System.Data.Entity;
using System.Linq;

namespace crc.DAL.BaseRepository
{
    public class GenericRepository<C, T> : IGenericRepository<T>
        where T : class
        where C : DbContext, new()
    {
        public C Context { get; set; }

        protected GenericRepository()
        {
            Context = new C();
        }

        public virtual IQueryable<T> GetAll()
        {
            IQueryable<T> query = Context.Set<T>();
            return query;
        }

        public IQueryable<T> FindBy(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            IQueryable<T> query = Context.Set<T>().Where(predicate);
            return query;
        }

        public virtual void Insert(T entity)
        {
            Context.Set<T>().Add(entity);
        }

        public virtual void Delete(T entity)
        {
            if (Context.Entry(entity).State == EntityState.Detached)
                Context.Set<T>().Attach(entity);

            Context.Set<T>().Remove(entity);
        }

        public virtual void Update(T entity)
        {
            if (Context.Entry(entity).State == EntityState.Detached)
                Context.Set<T>().Attach(entity);

            Context.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Save()
        {
            Context.SaveChanges();
        }

        public void Dispose()
        {
            if (Context != null)
                Context.Dispose();

            GC.SuppressFinalize(this);
        }

        public virtual T FindOne(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            T query = Context.Set<T>().AsNoTracking().FirstOrDefault(predicate);
            return query;
        }
    }
}