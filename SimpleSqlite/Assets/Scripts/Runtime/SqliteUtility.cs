using System;
using System.Threading.Tasks;
using Mono.Data.Sqlite;

namespace SimpleSqlite
{
    /// <summary>
    ///     sqlite工具类
    /// </summary>
    public static class SqliteUtility
    {
        #region public static functions

        /// <summary>
        ///     获得数据对应的数据类型在Sqlite中的类型字符串
        ///     Sqlite中的数据类型：https://www.sqlite.org/datatype3.html
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetSqliteType(Type type)
        {
            //对应sqlite中的integer类型
            if (type == typeof(sbyte) || type == typeof(byte) || type == typeof(short) || type == typeof(ushort) ||
                type == typeof(int) || type == typeof(uint) || type == typeof(long) || type == typeof(ulong) ||
                type == typeof(bool) || type.IsEnum) return "integer";
            //对应sqlite中的real类型
            if (type == typeof(float) || type == typeof(double) || type == typeof(decimal)) return "real";
            //对应sqlite中的text类型
            if (type == typeof(string)) return "text";
            //对应sqlite中的numeric类型
            if (type == typeof(DateTime)) return "numeric";
            //对应sqlite中的blob类型
            if (type == typeof(byte[])) return "blob";

            throw new NotSupportedException($"The type {type.Name} can not to sqlite type.");
        }

        /// <summary>
        ///     同步打开数据库连接
        /// </summary>
        /// <param name="dbPath"></param>
        /// <returns></returns>
        public static SqliteConnection OpenDb(string dbPath)
        {
            var connection = new SqliteConnection($"Data Source={dbPath}");
            connection.Open();
            return connection;
        }

        /// <summary>
        ///     异步打开数据库连接
        /// </summary>
        /// <param name="dbPath"></param>
        /// <returns></returns>
        public static async Task<SqliteConnection> OpenDbAsync(string dbPath)
        {
            var connection = new SqliteConnection($"Data Source={dbPath}");
            await connection.OpenAsync();
            return connection;
        }

        /// <summary>
        ///     同步创建并且打开数据库
        /// </summary>
        /// <param name="dbPath"></param>
        /// <returns></returns>
        public static SqliteConnection CreateAndOpenDb(string dbPath)
        {
            SqliteConnection.CreateFile(dbPath);
            return OpenDb(dbPath);
        }

        /// <summary>
        ///     异步创建并且打开数据库
        /// </summary>
        /// <param name="dbPath"></param>
        /// <returns></returns>
        public static async Task<SqliteConnection> CreateAndOpenDbAsync(string dbPath)
        {
            SqliteConnection.CreateFile(dbPath);
            return await OpenDbAsync(dbPath);
        }

        #endregion
    }
}