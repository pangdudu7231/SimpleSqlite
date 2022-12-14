using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Mono.Data.Sqlite;

namespace SimpleSqlite
{
    /// <summary>
    ///     sqlite的扩展类
    /// </summary>
    public static class SqliteExtension
    {
        #region SqliteColumn

        /// <summary>
        ///     获得列的声明语句
        /// </summary>
        /// <returns></returns>
        public static string GetDeclaration(this BaseColumn column)
        {
            var decl = $"{column.ColumnName} {SqliteUtility.GetSqliteType(column.ColumnType)} ";
            if (column.IsPrimaryKey) decl += "primary key ";
            if (column.IsAutoIncrement) decl += "autoincrement ";
            if (column.IsNotNull) decl += "not null ";
            if (column.IsUnique) decl += "unique ";
            if (!string.IsNullOrEmpty(column.Collate)) decl += $"collate {column.Collate} ";
            if (column.Default != null) decl += $"default {column.Default} ";

            return decl;
        }

        #endregion

        #region SqliteAttribute

        /// <summary>
        ///     尝试获取表名的值
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static bool TryGetTableName(this ICustomAttributeProvider provider, out string tableName)
        {
            var attribute = provider.GetCustomAttributes(typeof(TableAttribute), true).FirstOrDefault();
            tableName = (attribute as TableAttribute)?.TableName;
            return attribute != null;
        }

        /// <summary>
        ///     尝试获取列名的值
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public static bool TryGetColumnName(this ICustomAttributeProvider provider, out string columnName)
        {
            var attribute = provider.GetCustomAttributes(typeof(ColumnAttribute), true).FirstOrDefault();
            columnName = (attribute as ColumnAttribute)?.ColumnName;
            return columnName != null;
        }

        /// <summary>
        ///     是否带有主键的属性
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public static bool HasPrimaryKey(this ICustomAttributeProvider provider)
        {
            var attribute = provider.GetCustomAttributes(typeof(PrimaryKeyAttribute), true).FirstOrDefault();
            return attribute != null;
        }

        /// <summary>
        ///     是否带有自增的属性
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public static bool HasAutoIncrement(this ICustomAttributeProvider provider)
        {
            var attribute = provider.GetCustomAttributes(typeof(AutoIncrementAttribute), true).FirstOrDefault();
            return attribute != null;
        }

        /// <summary>
        ///     是否带有不为空的属性
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public static bool HasNotNull(this ICustomAttributeProvider provider)
        {
            var attribute = provider.GetCustomAttributes(typeof(NotNullAttribute), true).FirstOrDefault();
            return attribute != null;
        }

        /// <summary>
        ///     是否带有唯一性的属性
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public static bool HasUnique(this ICustomAttributeProvider provider)
        {
            var attribute = provider.GetCustomAttributes(typeof(UniqueAttribute), true).FirstOrDefault();
            return attribute != null;
        }

        /// <summary>
        ///     尝试获取默认的值
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool TryGetDefault(this ICustomAttributeProvider provider, out object value)
        {
            var attribute = provider.GetCustomAttributes(typeof(DefaultAttribute), true).FirstOrDefault();
            value = (attribute as DefaultAttribute)?.Value;
            return attribute != null;
        }

        /// <summary>
        ///     尝试获取排序规则的值
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="collate"></param>
        /// <returns></returns>
        public static bool TryGetCollation(this ICustomAttributeProvider provider, out string collate)
        {
            var attribute = provider.GetCustomAttributes(typeof(CollationAttribute), true).FirstOrDefault();
            collate = (attribute as CollationAttribute)?.Collate;
            return collate != null;
        }

        /// <summary>
        ///     是否带有忽略的属性
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public static bool HasIgnore(this ICustomAttributeProvider provider)
        {
            var attribute = provider.GetCustomAttributes(typeof(IgnoreAttribute), true).FirstOrDefault();
            return attribute != null;
        }

        #endregion

        #region 表的处理扩展方法

        #region 同步处理方法

        /// <summary>
        ///     检查数据表是否存在
        /// </summary>
        /// <param name="connection"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool TableIsExists<T>(this SqliteConnection connection) where T : IDbTable, new()
        {
            var tableName = GetTableName<T>();
            return connection.TableIsExists(tableName);
        }

        /// <summary>
        ///     检查数据表是否存在
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static bool TableIsExists(this SqliteConnection connection, string tableName)
        {
            try
            {
                using var command = connection.CreateCommand();
                command.CommandText = GetTableIsExistsQuery(tableName);
                using var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    var count = (long) reader["count(*)"];
                    reader.Close();
                    return count == 1;
                }

                return false;
            }
            catch (Exception)
            {
                // 数据库为空的时候，判断会发生异常
                return false;
            }
        }

        /// <summary>
        ///     创建表
        /// </summary>
        /// <param name="connection"></param>
        /// <typeparam name="T"></typeparam>
        /// <exception cref="SqliteException"></exception>
        public static void CreateTable<T>(this SqliteConnection connection) where T : IDbTable, new()
        {
            var (table, query) = GetCreateTableParams<T>();
            if (connection.TableIsExists(table.TableName)) //数据表已经存在
            {
                var msg = $"Table:'{table.TableName}' is exists already.";
                throw new SqliteException((int) SQLiteErrorCode.Error, msg);
            }

            using var command = connection.CreateCommand();
            command.CommandText = query;
            command.ExecuteNonQuery();
        }

        /// <summary>
        /// 删除一张表
        /// </summary>
        /// <param name="connection"></param>
        /// <typeparam name="T"></typeparam>
        public static void DeleteTable<T>(this SqliteConnection connection) where T : IDbTable, new()
        {
            var tableName = GetTableName<T>();
            connection.DeleteTable(tableName);
        }

        /// <summary>
        ///     删除一张表
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="tableName"></param>
        public static void DeleteTable(this SqliteConnection connection, string tableName)
        {
            if (!connection.TableIsExists(tableName)) //数据表不存在
            {
                var msg = $"Table:'{tableName}' is not exists.";
                throw new SqliteException((int) SQLiteErrorCode.Error, msg);
            }

            using var command = connection.CreateCommand();
            command.CommandText = GetDeleteTableQuery(tableName);
            command.ExecuteNonQuery();
        }

        /// <summary>
        /// 删除多张表
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="tableNames"></param>
        public static void DeleteTables(this SqliteConnection connection, params string[] tableNames)
        {
            foreach (var tableName in tableNames) connection.DeleteTable(tableName);
        }

        /// <summary>
        /// 更改表名
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="oldTableName"></param>
        /// <param name="newTableName"></param>
        public static void AlterTableName(this SqliteConnection connection, string oldTableName, string newTableName)
        {
            if (!connection.TableIsExists(oldTableName)) //旧的表不存在
            {
                var msg = $"Old table:'{oldTableName}' is not exist";
                throw new SqliteException((int) SQLiteErrorCode.Error, msg);
            }

            if (connection.TableIsExists(newTableName)) //新的表已经存在
            {
                var msg = $"New table:'{newTableName}' is exists already.";
                throw new SqliteException((int) SQLiteErrorCode.Error, msg);
            }

            using var command = connection.CreateCommand();
            command.CommandText = GetAlterTableNameQuery(oldTableName, newTableName);
            command.ExecuteNonQuery();
        }

        #endregion

        #region 异步处理方法

        /// <summary>
        ///     检查数据表是否存在
        /// </summary>
        /// <param name="connection"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static async Task<bool> TableIsExistsAsync<T>(this SqliteConnection connection) where T : IDbTable, new()
        {
            var tableName = GetTableName<T>();
            return await connection.TableIsExistsAsync(tableName);
        }

        /// <summary>
        ///     检查数据表是否存在
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static async Task<bool> TableIsExistsAsync(this SqliteConnection connection, string tableName)
        {
            try
            {
                await using var command = connection.CreateCommand();
                command.CommandText = GetTableIsExistsQuery(tableName);
                await using var reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    var count = (long) reader["count(*)"];
                    await reader.CloseAsync();
                    return count == 1;
                }

                return false;
            }
            catch (Exception)
            {
                // 数据库为空的时候，判断会发生异常
                return false;
            }
        }

        /// <summary>
        ///     创建表
        /// </summary>
        /// <param name="connection"></param>
        /// <typeparam name="T"></typeparam>
        /// <exception cref="SqliteException"></exception>
        public static async Task CreateTableAsync<T>(this SqliteConnection connection) where T : IDbTable, new()
        {
            var (table, query) = GetCreateTableParams<T>();
            if (await connection.TableIsExistsAsync(table.TableName)) //数据表已经存在
            {
                var msg = $"Table:'{table.TableName}' is exists already.";
                throw new SqliteException((int) SQLiteErrorCode.Error, msg);
            }

            await using var command = connection.CreateCommand();
            command.CommandText = query;
            await command.ExecuteNonQueryAsync();
        }

        /// <summary>
        /// 删除一张表
        /// </summary>
        /// <param name="connection"></param>
        /// <typeparam name="T"></typeparam>
        public static async Task DeleteTableAsync<T>(this SqliteConnection connection) where T : IDbTable, new()
        {
            var tableName = GetTableName<T>();
            await connection.DeleteTableAsync(tableName);
        }

        /// <summary>
        ///     删除一张表
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="tableName"></param>
        public static async Task DeleteTableAsync(this SqliteConnection connection, string tableName)
        {
            if (!await connection.TableIsExistsAsync(tableName)) //数据表不存在
            {
                var msg = $"Table:'{tableName}' is not exists.";
                throw new SqliteException((int) SQLiteErrorCode.Error, msg);
            }

            await using var command = connection.CreateCommand();
            command.CommandText = GetDeleteTableQuery(tableName);
            await command.ExecuteNonQueryAsync();
        }

        /// <summary>
        /// 删除多张表
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="tableNames"></param>
        public static async Task DeleteTablesAsync(this SqliteConnection connection, params string[] tableNames)
        {
            foreach (var tableName in tableNames) await connection.DeleteTableAsync(tableName);
        }

        /// <summary>
        /// 更改表名
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="oldTableName"></param>
        /// <param name="newTableName"></param>
        public static async Task AlterTableNameAsync(this SqliteConnection connection, string oldTableName,
            string newTableName)
        {
            if (! await connection.TableIsExistsAsync(oldTableName)) //旧的表不存在
            {
                var msg = $"Old table:'{oldTableName}' is not exist";
                throw new SqliteException((int) SQLiteErrorCode.Error, msg);
            }

            if (await connection.TableIsExistsAsync(newTableName)) //新的表已经存在
            {
                var msg = $"New table:'{newTableName}' is exists already.";
                throw new SqliteException((int) SQLiteErrorCode.Error, msg);
            }

            await using var command = connection.CreateCommand();
            command.CommandText = GetAlterTableNameQuery(oldTableName, newTableName);
            await command.ExecuteNonQueryAsync();
        }

        #endregion

        /// <summary>
        ///     获得表名
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private static string GetTableName<T>() where T : IDbTable, new()
        {
            var sqlTable = new SqlTable(typeof(T));
            return sqlTable.TableName;
        }

        /// <summary>
        ///     获得判断表是否存在的语句
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        private static string GetTableIsExistsQuery(string tableName)
        {
            return $"select count(*) from sqlite_master where type='table' and name={tableName}";
        }

        /// <summary>
        ///     获得创建表的参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private static (BaseTable table, string query) GetCreateTableParams<T>() where T : IDbTable, new()
        {
            var sqlTable = new SqlTable(typeof(T));
            var query = $"create table {sqlTable.TableName} (";
            var decls = sqlTable.Columns.Select(column => column.GetDeclaration());
            query += string.Join(",", decls);
            query += ")";
            return (sqlTable, query);
        }

        /// <summary>
        ///     获得删除表的语句
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        private static string GetDeleteTableQuery(string tableName)
        {
            return $"drop table {tableName}";
        }

        /// <summary>
        /// 获得修改表名的语句
        /// </summary>
        /// <param name="oldTableName"></param>
        /// <param name="newTableName"></param>
        /// <returns></returns>
        private static string GetAlterTableNameQuery(string oldTableName, string newTableName)
        {
            return $"alter table {oldTableName} rename to {newTableName}";
        }

        #endregion
    }
}