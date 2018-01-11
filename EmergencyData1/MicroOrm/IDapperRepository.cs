#if COREFX
using IDbConnection = System.Data.Common.DbConnection;
#endif

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EmergencyData.MicroOrm.Enums;
using EmergencyData.MicroOrm.SqlGenerator;

namespace EmergencyData.MicroOrm
{
    public interface IDapperRepository<TEntity> where TEntity : class
    {
        IDbConnection Connection { get; }

        ISqlGenerator<TEntity> SqlGenerator { get; }

        TEntity Find(Expression<Func<TEntity, bool>> expression);

        TEntity Find(SqlQuery sqlQuery, IDbTransaction transaction = null);

        TEntity Find(Expression<Func<TEntity, bool>> expression, List<Expression<Func<TEntity, object>>> selectColumns);


        TEntity Find<TChild1>(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, object>> tChild1);

        TEntity Find(Expression<Func<TEntity, bool>> predicate, List<Expression<Func<TEntity, object>>> selectColumns, Expression<Func<TEntity, object>> orderColumn,OrderDirection direction,
        params Expression<Func<TEntity, object>>[] includes);

        IEnumerable<TEntity> FindAll();

        //IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> expression);
        IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> expression, IDbTransaction transaction = null);

        IEnumerable<TEntity> FindAll(SqlQuery sqlQuery, IDbTransaction transaction = null);

        IEnumerable<TEntity> FindAll<TChild1>(Expression<Func<TEntity, object>> tChild1);

        IEnumerable<TEntity> FindAll<TChild1>(Expression<Func<TEntity, bool>> expression,
            Expression<Func<TEntity, object>> tChild1);

        IEnumerable<TEntity> FindAll<TChild1>(SqlQuery sqlQuery, Expression<Func<TEntity, object>> tChild1);

        Task<IEnumerable<TEntity>> FindAllAsync();

        Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> expression);

        Task<IEnumerable<TEntity>> FindAllAsync(SqlQuery sqlQuery);

        Task<IEnumerable<TEntity>> FindAllAsync<TChild1>(Expression<Func<TEntity, object>> tChild1);

        Task<IEnumerable<TEntity>> FindAllAsync<TChild1>(Expression<Func<TEntity, bool>> expression,
            Expression<Func<TEntity, object>> tChild1);

        Task<IEnumerable<TEntity>> FindAllAsync<TChild1>(SqlQuery sqlQuery, Expression<Func<TEntity, object>> tChild1);

        Task<TEntity> FindAsync<TChild1>(Expression<Func<TEntity, object>> tChild1);

        Task<TEntity> FindAsync<TChild1>(Expression<Func<TEntity, bool>> expression,
            Expression<Func<TEntity, object>> tChild1);

        Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> expression);

        Task<TEntity> FindAsync();

        bool Insert(TEntity instance, IDbTransaction transaction = null);

        bool BulkInsert(IEnumerable<TEntity> instancEntities);

        Task<bool> InsertAsync(TEntity instance, IDbTransaction transaction = null);

        int Insert(SqlQuery sqlQuery, IDbTransaction transaction = null);

        bool Delete(TEntity instance, IDbTransaction transaction = null);


        bool Update(TEntity instance, IDbTransaction transaction = null);

        bool Update(SqlQuery sqlQuery, IDbTransaction transaction = null);

        bool Update<T>(TEntity instance, Expression<Func<T, dynamic>> fields, IDbTransaction transaction = null);


        IEnumerable<TEntity> FindAllBetween(object from, object to, Expression<Func<TEntity, object>> btwField);

        IEnumerable<TEntity> FindAllBetween(object from, object to, Expression<Func<TEntity, object>> btwField,
            Expression<Func<TEntity, bool>> expression);

        Task<IEnumerable<TEntity>> FindAllBetweenAsync(object from, object to,
            Expression<Func<TEntity, object>> btwField);

        Task<IEnumerable<TEntity>> FindAllBetweenAsync(object from, object to,
            Expression<Func<TEntity, object>> btwField, Expression<Func<TEntity, bool>> expression);

        IEnumerable<TEntity> FindAllBetween(DateTime from, DateTime to, Expression<Func<TEntity, object>> btwField);

        IEnumerable<TEntity> FindAllBetween(DateTime from, DateTime to, Expression<Func<TEntity, object>> btwField,
            Expression<Func<TEntity, bool>> expression);

        Task<IEnumerable<TEntity>> FindAllBetweenAsync(DateTime from, DateTime to,
            Expression<Func<TEntity, object>> btwField);

        Task<IEnumerable<TEntity>> FindAllBetweenAsync(DateTime from, DateTime to,
            Expression<Func<TEntity, object>> btwField, Expression<Func<TEntity, bool>> expression);
    }
}