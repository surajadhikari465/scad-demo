using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using KitBuilder.DataAccess.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace KitBuilder.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        public IUnitOfWork UnitOfWork { get; set; }
       
        public Repository(IUnitOfWork unitOfWork)
        {
            this.UnitOfWork = unitOfWork;
        }
        public IQueryable<T> GetAll()
        {
            return this.UnitOfWork.Context.Set<T>().AsQueryable();
        }

        public virtual async Task<ICollection<T>> GetAllAsync()
        {

            return await this.UnitOfWork.Context.Set<T>().ToListAsync();
        }

        public virtual T Get(int id)
        {
            return this.UnitOfWork.Context.Set<T>().Find(id);
        }

        public virtual async Task<T> GetAsync(int id)
        {
            return await this.UnitOfWork.Context.Set<T>().FindAsync(id);
        }

        public virtual T Add(T t)
        {

            this.UnitOfWork.Context.Set<T>().Add(t);
            this.UnitOfWork.Context.SaveChanges();
            return t;
        }

        public virtual async Task<T> AddAsync(T t)
        {
            this.UnitOfWork.Context.Set<T>().Add(t);
            await this.UnitOfWork.Context.SaveChangesAsync();
            return t;

        }

        public virtual T Find(Expression<Func<T, bool>> match)
        {
            return this.UnitOfWork.Context.Set<T>().SingleOrDefault(match);
        }

        public virtual async Task<T> FindAsync(Expression<Func<T, bool>> match)
        {
            return await this.UnitOfWork.Context.Set<T>().SingleOrDefaultAsync(match);
        }

        public ICollection<T> FindAll(Expression<Func<T, bool>> match)
        {
            return this.UnitOfWork.Context.Set<T>().Where(match).ToList();
        }

        public async Task<ICollection<T>> FindAllAsync(Expression<Func<T, bool>> match)
        {
            return await this.UnitOfWork.Context.Set<T>().Where(match).ToListAsync();
        }

        public virtual void Delete(T entity)
        {
            this.UnitOfWork.Context.Set<T>().Remove(entity);
            this.UnitOfWork.Context.SaveChanges();
        }

        public virtual async Task<int> DeleteAsync(T entity)
        {
            this.UnitOfWork.Context.Set<T>().Remove(entity);
            return await this.UnitOfWork.Context.SaveChangesAsync();
        }

        public virtual T Update(T t, object key)
        {
            if (t == null)
                return null;
            T exist = this.UnitOfWork.Context.Set<T>().Find(key);
            if (exist != null)
            {
                this.UnitOfWork.Context.Entry(exist).CurrentValues.SetValues(t);
                this.UnitOfWork.Context.SaveChanges();
            }
            return exist;
        }

        public virtual async Task<T> UpdateAsync(T t, object key)
        {
            if (t == null)
                return null;
            T exist = await this.UnitOfWork.Context.Set<T>().FindAsync(key);
            if (exist != null)
            {
                this.UnitOfWork.Context.Entry(exist).CurrentValues.SetValues(t);
                await this.UnitOfWork.Context.SaveChangesAsync();
            }
            return exist;
        }

        public virtual void Save()
        {
            this.UnitOfWork.Context.SaveChanges();
        }

        public async virtual Task<int> SaveAsync()
        {
            return await this.UnitOfWork.Context.SaveChangesAsync();
        }

        public virtual IQueryable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            IQueryable<T> query = this.UnitOfWork.Context.Set<T>().Where(predicate);
            return query;
        }

        public virtual async Task<ICollection<T>> FindByAsync(Expression<Func<T, bool>> predicate)
        {
            return await this.UnitOfWork.Context.Set<T>().Where(predicate).ToListAsync();
        }

        public IQueryable<T> GetAllIncluding(params Expression<Func<T, object>>[] includeProperties)
        {

            IQueryable<T> queryable = GetAll();
            foreach (Expression<Func<T, object>> includeProperty in includeProperties)
            {

                queryable = queryable.Include<T, object>(includeProperty);
            }

            return queryable;
        }

        public int ExecWithStoreProcedure(string StoredProcedureName, params object[] parameters)
        {
            return UnitOfWork.Context.Database.ExecuteSqlCommand("EXEC " + StoredProcedureName, parameters);
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this.UnitOfWork.Context.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}