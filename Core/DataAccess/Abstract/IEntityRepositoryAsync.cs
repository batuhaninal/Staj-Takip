﻿using Core.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.DataAccess.Abstract
{
    public interface IEntityRepositoryAsync<T> : IEntityRepositoryBase<T>
        where T : class, IEntity, new()
    {
        /// <summary>
        /// Tüm verileri paramatreye ve bağıntılı nesnelere göre asenkron şekilde getirir.
        /// </summary>
        /// <param name="predicates">
        /// Filtre koşulları verilebilir.
        /// </param>
        /// <param name="includes">
        /// Bağıntılı nesneler paramatre olarak verilebilir.
        /// </param>
        /// <returns></returns>
        Task<IList<T>> GetAllAsync(IList<Expression<Func<T, bool>>> predicates, IList<Expression<Func<T, object>>> includes = null);

        /// <summary>
        /// Tüm verileri paramatreye ve bağıntılı nesnelere göre asenkron şekilde getirir.
        /// </summary>
        /// <param name="predicate">
        /// Filtre koşulu verilebilir.
        /// </param>
        /// <param name="includes">
        /// Bağıntılı nesneler paramatre olarak verilebilir.
        /// </param>
        /// <returns></returns>
        Task<IList<T>> GetAllAsync(Expression<Func<T, bool>> predicate = null, params Expression<Func<T, object>>[] includes);


        /// <summary>
        /// Tek veri getirmek için kullanılabilir. Verilen paramatre ve bağıntılı nesnelere göre getirir.
        /// </summary>
        /// <param name="predicate">
        /// Filtre koşulu verilebilir.
        /// </param>
        /// <param name="includes">
        /// Bağıntılı nesneler paramatre olarak verilebilir.
        /// </param>
        /// <returns></returns>
        Task<T> GetAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);

        /// <summary>
        /// Tek veri getirmek için kullanılabilir. Verilen paramatrelere ve bağıntılı nesnelere göre getirir.
        /// </summary>
        /// <param name="predicates">
        /// Filtre koşulu verilebilir.
        /// </param>
        /// <param name="includes">
        /// Bağıntılı nesneler paramatre olarak verilebilir.
        /// </param>
        /// <returns></returns>
        Task<T> GetAsync(IList<Expression<Func<T, bool>>> predicates, IList<Expression<Func<T, object>>> includes = null);
        Task<T> AddAsync(T entity);
        Task DeleteAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
        Task<int> CountAsync(Expression<Func<T, bool>> predicate = null);
        Task<IList<T>> SearchAsync(IList<Expression<Func<T, bool>>> predicates, IList<Expression<Func<T, object>>> includes = null);
    }
}
