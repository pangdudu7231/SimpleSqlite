using System;
using System.Linq;
using System.Reflection;

namespace SimpleSqlite
{
    /// <summary>
    ///     数据表信息的基类
    /// </summary>
    public abstract class BaseTable
    {
        /// <summary>
        ///     表的类型
        /// </summary>
        public Type TableType { get; protected set; }

        /// <summary>
        ///     数据表的表名
        /// </summary>
        public string TableName { get; protected set; }

        /// <summary>
        ///     数据表中的列信息
        /// </summary>
        public BaseColumn[] Columns { get; protected set; }

        /// <summary>
        ///     数据表中主键列信息
        /// </summary>
        public BaseColumn PKColumn { get; protected set; }

        /// <summary>
        ///     数据表中可更改的列信息（除去主键的列）
        /// </summary>
        public BaseColumn[] AlterableColumns { get; protected set; }
    }

    /// <summary>
    ///     数据表的信息类
    /// </summary>
    public sealed class SqlTable : BaseTable
    {
        #region ctor

        public SqlTable(Type tableType)
        {
            TableType = tableType;
            TableName = tableType.TryGetTableName(out var tableName) ? tableName : tableType.Name;
            var columns = GetColumns();
            Columns = columns;
            PKColumn = columns.FirstOrDefault(column => column.IsPrimaryKey);
            AlterableColumns = columns.Where(column => !column.IsPrimaryKey).ToArray();
        }

        #endregion

        #region private functions

        /// <summary>
        ///     获得
        /// </summary>
        /// <returns></returns>
        private BaseColumn[] GetColumns()
        {
            const BindingFlags bindingAttr = BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty;

            var properties = TableType.GetProperties(bindingAttr);
            return properties.Where(property => !property.HasIgnore())
                .Select(property => (BaseColumn) new SqlColumn(property))
                .ToArray();
        }

        #endregion
    }
}