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
    public class EFEntityRepository<TEntity, TContext> : IEntityRepository<TEntity>
        where TEntity : class, IEntity, new()
        where TContext: DbContext, new()
    {
        public TEntity Add(TEntity entity)
        {
            using (var context = new TContext())
            {
                context.Set<TEntity>().Add(entity);
                context.SaveChanges();
                return entity;
            }
        }

        public bool Any(Expression<Func<TEntity, bool>> predicate)
        {
            using (var context = new TContext()) 
            {
                return context.Set<TEntity>().Any(predicate);
            }
        }

        public int Count(Expression<Func<TEntity, bool>> predicate = null)
        {
            using (var context = new TContext())
            {
                return predicate == null
                    ? context.Set<TEntity>().Count()
                    : context.Set<TEntity>().Count(predicate);
            }
        }

        public void Delete(TEntity entity)
        {
            using (var context = new TContext())
            {
                context.Set<TEntity>().Remove(entity);
                context.SaveChanges();
            }
        }

        public TEntity Get(Expression<Func<TEntity, bool>> predicate, IEnumerable<Expression<Func<TEntity, object>>> includes = null)
        {
            using (var context = new TContext())
            {
                IQueryable<TEntity> query = context.Set<TEntity>();
                if (predicate != null)
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

                return query.AsNoTracking().SingleOrDefault();
            }
        }

        public TEntity Get(IEnumerable<Expression<Func<TEntity, bool>>> predicates, IEnumerable<Expression<Func<TEntity, object>>> includes = null)
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

                if(includes == null && includes.Any())
                {
                    foreach (var include in includes)
                    {
                        query = query.Include(include);
                    }
                }

                return query.AsNoTracking().SingleOrDefault();
            }
        }

        public IQueryable<TEntity> GetAll(IEnumerable<Expression<Func<TEntity, bool>>> predicates, IEnumerable<Expression<Func<TEntity, object>>> includes = null)
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

                return query.AsNoTracking();
            }
        }

        public IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate, IEnumerable<Expression<Func<TEntity, object>>> includes = null)
        {
            using (var context = new TContext())
            {
                IQueryable<TEntity> query = context.Set<TEntity>();
                if (predicate != null)
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

                return query.AsNoTracking();
            }
        }

        public TEntity Update(TEntity entity)
        {
            using (var context = new TContext())
            {
                context.Set<TEntity>().Update(entity);
                context.SaveChanges();
                return entity;
            }
        }
    }
}
