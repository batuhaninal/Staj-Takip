using Core.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.DataAccess.Abstract
{
    public interface IEntityRepository<T> : IEntityRepositoryBase<T>
        where T : class, IEntity, new()
    {
        /// <summary>
        /// Tüm verileri paramatrelere ve bağıntılı nesnelere göre getirir.
        /// </summary>
        /// <param name="predicates">
        /// Filtre koşulları verilebilir.
        /// </param>
        /// <param name="includes">
        /// Bağıntılı nesneler paramatre olarak verilebilir.
        /// </param>
        /// <returns></returns>
        IList<T> GetAll(IList<Expression<Func<T, bool>>> predicates, IList<Expression<Func<T, object>>> includes = null);

        /// <summary>
        /// Tüm verileri paramatreye ve bağıntılı nesnelere göre getirir.
        /// </summary>
        /// <param name="predicate">
        /// Filtre koşulu verilebilir.
        /// </param>
        /// <param name="includes">
        /// Bağıntılı nesneler paramatre olarak verilebilir.
        /// </param>
        /// <returns></returns>
        IList<T> GetAll(Expression<Func<T, bool>> predicate = null, IList<Expression<Func<T, object>>> includes = null);

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
        T Get(Expression<Func<T, bool>> predicate, IList<Expression<Func<T, object>>> includes = null);


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
        T Get(IList<Expression<Func<T, bool>>> predicates, IList<Expression<Func<T, object>>> includes = null);
        T Add(T entity);
        void Delete(T entity);
        T Update(T entity);
        bool Any(Expression<Func<T, bool>> predicate);
        int Count(Expression<Func<T, bool>> predicate = null);
    }
}
