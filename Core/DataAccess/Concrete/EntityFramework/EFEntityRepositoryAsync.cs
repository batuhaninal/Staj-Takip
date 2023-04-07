using Core.DataAccess.Abstract;
using Core.Entities.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.DataAccess.Concrete.EntityFramework
{
    public class EFEntityRepositoryAsync<TEntity, TContext> : IEntityRepositoryAsync<TEntity>
        where TEntity : class, IEntity, new()
        where TContext: DbContext, new()
    {

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            using (var context = new TContext())
            {
                await context.Set<TEntity>().AddAsync(entity);
                await context.SaveChangesAsync();
                return entity;
            }
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
        {
            using (var context = new TContext())
            {
                return await context.Set<TEntity>().AnyAsync(predicate);
            }
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate = null)
        {
            using(var context = new TContext())
            {
                return predicate == null
                    ? await context.Set<TEntity>().CountAsync()
                    : await context.Set<TEntity>().CountAsync(predicate);
            } 
        }

        public async Task DeleteAsync(TEntity entity)
        {
            using(var context = new TContext())
            {
                context.Set<TEntity>().Remove(entity);
                await context.SaveChangesAsync();
            }
        }

        public async Task<IQueryable<TEntity>> GetAllAsync(IEnumerable<Expression<Func<TEntity, bool>>> predicates, IEnumerable<Expression<Func<TEntity, object>>> includes = null)
        {
            using (var context = new TContext())
            {
                IQueryable<TEntity> query = context.Set<TEntity>();
                if (predicates != null && predicates.Any())
                {
                    foreach (var predicate in predicates)
                    {
                        query = query.Where(predicate);
                    }
                }
                if (includes != null && includes.Any())
                {
                    foreach (var include in includes)
                    {
                        query = query.Include(include);
                    }
                }
                // AsNoTracking(), Entity Framework'te kullanılan bir fonksiyondur ve sorgulama sonucunda elde edilen nesnelerin takibinin yapılmamasını sağlar. Bu fonksiyonun kullanımı, performansı artırabilir ve hafızada daha az yer kaplamasına yardımcı olabilir.
                return await Task.Factory.StartNew(() => query.AsNoTracking());
            }
        }

        public async Task<IQueryable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate, IEnumerable<Expression<Func<TEntity, object>>> includes = null)
        {
            using (var context = new TContext())
            {
                IQueryable<TEntity> query = context.Set<TEntity>();
                if(predicate != null)
                {
                    query = query.Where(predicate);
                }

                if(includes != null && includes.Any())
                {
                    foreach (var include in includes)
                    {
                        query = query.Include(include);
                    }
                }

                return await Task.Factory.StartNew(() =>
                {
                     return query.AsNoTracking();
                });
            }
        }


        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate, IEnumerable<Expression<Func<TEntity, object>>> includes = null)
        {
            using (var context = new TContext())
            {
                IQueryable<TEntity> query = context.Set<TEntity>();
                if(predicate != null)
                {
                    query = query.Where(predicate);
                }
                if(includes != null && includes.Any())
                {
                    foreach (var include in includes)
                    {
                        query = query.Include(include);
                    }
                }

                return await query.AsNoTracking().SingleOrDefaultAsync();
            }
        }

        public async Task<TEntity> GetAsync(IEnumerable<Expression<Func<TEntity, bool>>> predicates, IEnumerable<Expression<Func<TEntity, object>>> includes = null)
        {
            using (var context = new TContext()) 
            {
                IQueryable<TEntity> query = context.Set<TEntity>();
                if(predicates != null && predicates.Any())
                {
                    foreach (var predicate in predicates)
                    {
                        query = query.Where(predicate);
                    }
                }

                if(includes  == null && includes.Any())
                {
                    foreach (var include in includes)
                    {
                        query = query.Include(include);
                    }
                }

                return await query.AsNoTracking().SingleOrDefaultAsync();
            }
        }

        public Task<IEnumerable<TEntity>> SearchAsync(IEnumerable<Expression<Func<TEntity, bool>>> predicates, IEnumerable<Expression<Func<TEntity, object>>> includes = null)
        {
            throw new NotImplementedException();
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            using (var context = new TContext())
            {
                context.Set<TEntity>().Update(entity);
                await context.SaveChangesAsync();
                return entity;
            }
        }
    }
}
