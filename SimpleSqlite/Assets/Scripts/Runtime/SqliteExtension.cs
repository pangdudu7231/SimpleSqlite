using System.Linq;
using System.Reflection;

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
        public static string GetDeclaration(BaseColumn column)
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
    }
}