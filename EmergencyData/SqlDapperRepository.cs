using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EmergencyData.MicroOrm.SqlGenerator;

namespace EmergencyData
{
    public class SqlDapperRepository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {

        private MicroOrm.DapperRepository<TEntity> _internalRepository;

        public SqlDapperRepository(string webConfigConnectionStr)
        {
            _internalRepository = new MicroOrm.DapperRepository<TEntity>(new SqlConnection(webConfigConnectionStr));
        }


        public SqlDapperRepository(IDbConnection connection)
        {
            _internalRepository = new MicroOrm.DapperRepository<TEntity>(connection);
            System.Diagnostics.Debug.WriteLine(connection.GetHashCode());
        }


        public IDbConnection Connection
        {
            get
            {
                return _internalRepository.Connection;
            }
        }

        private T ExecuteWithTryCatch<T>(Func<T> action, bool useTransaction = false)
        {
            try
            {
                if (Connection.State == ConnectionState.Closed)
                    Connection.Open();

                return action();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (!useTransaction && Connection != null && Connection.State != ConnectionState.Closed)
                {
                    Connection.Close();
                }
            }
        }

        #region Find

        public TEntity Find(Expression<Func<TEntity, bool>> expression)
        {
            return ExecuteWithTryCatch(() => _internalRepository.Find(expression));
        }

        public TEntity Find(Expression<Func<TEntity, bool>> expression,
           List<Expression<Func<TEntity, object>>> selectColumns)
        {
            return ExecuteWithTryCatch(() => _internalRepository.Find(expression, selectColumns));
        }

        public TEntity Find<TChild1>(
           Expression<Func<TEntity, bool>> expression,
           Expression<Func<TEntity, object>> tChild1)
        {
            return ExecuteWithTryCatch(() => _internalRepository.Find<TChild1>(expression, tChild1));
        }

        public TEntity Find(SqlQuery sqlQuery, IDbTransaction transaction = null)
        {
            return ExecuteWithTryCatch(() => _internalRepository.Find(sqlQuery, transaction));
        }

        public TEntity Find(Expression<Func<TEntity, bool>> predicate,
            List<Expression<Func<TEntity, object>>> selectColumns, params Expression<Func<TEntity, object>>[] includes)
        {
            return ExecuteWithTryCatch(() => _internalRepository.Find(predicate, selectColumns, includes));

        }
        #endregion

        #region FindAll

        public IEnumerable<TEntity> FindAll()
        {
            return ExecuteWithTryCatch(() => _internalRepository.FindAll());
        }

        public IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> expression,
            List<Expression<Func<TEntity, object>>> selectColumns)
        {
            return ExecuteWithTryCatch(() => _internalRepository.FindAll(expression, selectColumns));
        }

        public IEnumerable<TEntity> FindAll(
           Expression<Func<TEntity, bool>> expression)
        {
            return ExecuteWithTryCatch(() => _internalRepository.FindAll(expression));
        }

        public IEnumerable<TEntity> FindAll<TChild1>(
           Expression<Func<TEntity, object>> tChild1)
        {
            return ExecuteWithTryCatch(() => _internalRepository.FindAll<TChild1>(tChild1));
        }

        public IEnumerable<TEntity> FindAll<TChild1>(
           Expression<Func<TEntity, bool>> expression,
           Expression<Func<TEntity, object>> tChild1)
        {
            return ExecuteWithTryCatch(() => _internalRepository.FindAll<TChild1>(expression, tChild1));
        }

        public IEnumerable<TEntity> FindAll(SqlQuery sqlQuery)
        {
            return ExecuteWithTryCatch(() => _internalRepository.FindAll(sqlQuery));
        }

        public IEnumerable<TEntity> FindAll(SqlQuery sqlQuery, IDbTransaction transaction = null)
        {
            return ExecuteWithTryCatch(() => _internalRepository.FindAll(sqlQuery, transaction));
        }

        public IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> expression, IDbTransaction transaction = null)
        {
            return ExecuteWithTryCatch(() => _internalRepository.FindAll(expression, transaction));
        }


        public Task<IEnumerable<TEntity>> FindAllAsync()
        {
            return ExecuteWithTryCatch(() => _internalRepository.FindAllAsync());
        }

        public Task<IEnumerable<TEntity>> FindAllAsync(
            Expression<Func<TEntity, bool>> expression)
        {
            return ExecuteWithTryCatch(() => _internalRepository.FindAllAsync(expression));
        }

        public Task<IEnumerable<TEntity>> FindAllAsync
            <TChild1>(Expression<Func<TEntity, object>> tChild1)
        {
            return ExecuteWithTryCatch(() => _internalRepository.FindAllAsync<TChild1>(tChild1));
        }

        Task<IEnumerable<TEntity>> IRepository<TEntity>.FindAllAsync
           <TChild1>(Expression<Func<TEntity, bool>> expression,
               Expression<Func<TEntity, object>> tChild1)
        {
            return ExecuteWithTryCatch(() => _internalRepository.FindAllAsync<TChild1>(expression, tChild1));
        }

        Task<TEntity> IRepository<TEntity>.FindAsync<TChild1>(
           Expression<Func<TEntity, bool>> expression,
           Expression<Func<TEntity, object>> tChild1)
        {
            return ExecuteWithTryCatch(() => _internalRepository.FindAsync<TChild1>(expression, tChild1));
        }

        Task<TEntity> IRepository<TEntity>.FindAsync(
           Expression<Func<TEntity, bool>> expression)
        {
            return ExecuteWithTryCatch(() => _internalRepository.FindAsync(expression));
        }
        #endregion

        #region Delete
        public bool Delete(TEntity instance, IDbTransaction transaction)
        {
            return ExecuteWithTryCatch(() => _internalRepository.Delete(instance, transaction));
        }
        public bool Delete(TEntity instance)
        {
            return ExecuteWithTryCatch(() => _internalRepository.Delete(instance));
        }
        #endregion

        #region Update
        public bool Update(TEntity instance)
        {
            return ExecuteWithTryCatch(() => _internalRepository.Update(instance));
        }

        public bool Update(TEntity instance, IDbTransaction transaction = null)
        {
            return ExecuteWithTryCatch(() => _internalRepository.Update(instance, transaction), transaction != null);
        }

        public bool Update(SqlQuery sqlQuery, IDbTransaction transaction = null)
        {
            return ExecuteWithTryCatch(() => _internalRepository.Update(sqlQuery, transaction), transaction != null);
        }

        //public bool Update(SqlQuery sqlQuery)
        //{
        //    return ExecuteWithTryCatch(() => _internalRepository.Update(sqlQuery));
        //}


        public bool Update<T>(TEntity instance, Expression<Func<T, dynamic>> fields, IDbTransaction transaction = null)
        {
            return ExecuteWithTryCatch(() => _internalRepository.Update(instance, fields, transaction));
        }

        #endregion

        #region Insert

        public bool Insert(TEntity instance, IDbTransaction transaction = null)
        {
            return ExecuteWithTryCatch(() => _internalRepository.Insert(instance, transaction), transaction != null);
        }

        public int Insert(SqlQuery sqlQuery, IDbTransaction transaction = null)
        {
            return ExecuteWithTryCatch(() => _internalRepository.Insert(sqlQuery, transaction));
        }

        bool IRepository<TEntity>.Insert(TEntity instance)
        {
            return ExecuteWithTryCatch(() => _internalRepository.Insert(instance));
        }

        Task<bool> IRepository<TEntity>.InsertAsync(TEntity instance)
        {
            return ExecuteWithTryCatch(() => _internalRepository.InsertAsync(instance));
        }

        #endregion Insert

    }
}