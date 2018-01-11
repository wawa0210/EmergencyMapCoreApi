using EmergencyData.MicroOrm;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Data.SqlClient;

namespace EmergencyBaseService
{
    /// <summary>
    /// AppService基类
    /// </summary>
    public class BaseAppService
    {

        private IDbConnection _connection;
        public BaseAppService()
        {

        }

        /// <summary>
        /// 根据类型获取Repository对象
        /// </summary>
        /// <typeparam name="T">Repository对象类型</typeparam>
        /// <returns></returns>
        protected virtual DapperRepository<T> GetRepositoryInstance<T>(string connStr = null) where T : class, new()
        {
            //获取程序集名+类名，作为CallContext的key
            var type = typeof(T);
            var typeName = type.Assembly.FullName + type.FullName;

            //保证在同一个HTTP请求下，对象是单例的,优先从CallContext中取
            //var repository = CallContext.GetData(typeName) as DapperRepository<T>;
            //if (repository == null)
            //{
            if (_connection == null)
            {
                if (string.IsNullOrEmpty(connStr))
                    connStr = "Database=EmergencyMap;Data Source=118.31.43.50;User Id=root;Password=ZX199302;CharSet=utf8;port=3306";
                _connection = new MySqlConnection(connStr);
            }
            var repository = new DapperRepository<T>(_connection);
            //CallContext.SetData(typeName, repository);
            //}
            return repository;
        }
    }
}
