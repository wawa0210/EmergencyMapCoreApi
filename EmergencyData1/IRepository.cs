using EmergencyData.MicroOrm.SqlGenerator;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EmergencyData
{
    public interface IRepository<TEntity> where TEntity : class
    {

        IDbConnection Connection { get; }

        TEntity Find(SqlQuery sqlQuery, IDbTransaction transaction = null);
        
        TEntity Find(Expression<Func<TEntity, bool>> expression);

        TEntity Find(Expression<Func<TEntity, bool>> expression,
            List<Expression<Func<TEntity, object>>> selectColumns);

        TEntity Find<TChild1>(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, object>> tChild1);

        TEntity Find(Expression<Func<TEntity, bool>> predicate,
            List<Expression<Func<TEntity, object>>> selectColumns, params Expression<Func<TEntity, object>>[] includes);

        IEnumerable<TEntity> FindAll();

        IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> expression,
            List<Expression<Func<TEntity, object>>> selectColumns);

        IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> expression);


        IEnumerable<TEntity> FindAll<TChild1>(Expression<Func<TEntity, object>> tChild1);

        IEnumerable<TEntity> FindAll<TChild1>(Expression<Func<TEntity, bool>> expression,
            Expression<Func<TEntity, object>> tChild1);

        IEnumerable<TEntity> FindAll(SqlQuery sqlQuery);

        Task<IEnumerable<TEntity>> FindAllAsync();

        Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> expression);

        Task<IEnumerable<TEntity>> FindAllAsync<TChild1>(Expression<Func<TEntity, object>> tChild1);

        Task<IEnumerable<TEntity>> FindAllAsync<TChild1>(Expression<Func<TEntity, bool>> expression,
            Expression<Func<TEntity, object>> tChild1);


        Task<TEntity> FindAsync<TChild1>(Expression<Func<TEntity, bool>> expression,
            Expression<Func<TEntity, object>> tChild1);

        Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> expression);


        bool Insert(TEntity instance);

        Task<bool> InsertAsync(TEntity instance);

        bool Delete(TEntity instance);

        bool Update(TEntity instance);

        //bool Update(SqlQuery sqlQuery);

        bool Update(SqlQuery sqlQuery,IDbTransaction transaction=null);

        bool Insert(TEntity instance, IDbTransaction transaction);
        bool Delete(TEntity instance, IDbTransaction transaction);
        bool Update(TEntity instance, IDbTransaction transaction);
    }
}