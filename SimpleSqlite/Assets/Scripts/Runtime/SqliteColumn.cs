using System;
using System.Reflection;

namespace SimpleSqlite
{
    /// <summary>
    ///     数据表列信息的基类
    /// </summary>
    public abstract class BaseColumn
    {
        /// <summary>
        ///     列的数据类型
        /// </summary>
        public Type ColumnType { get; protected set; }
        
        /// <summary>
        ///     列名
        /// </summary>
        public string ColumnName { get; protected set; }

        /// <summary>
        ///     是否为主键字段
        /// </summary>
        public bool IsPrimaryKey { get; protected set; }

        /// <summary>
        ///     是否为自增字段
        /// </summary>
        public bool IsAutoIncrement { get; protected set; }

        /// <summary>
        ///     是否为不为空的字段
        /// </summary>
        public bool IsNotNull { get; protected set; }

        /// <summary>
        ///     是否为唯一字段
        /// </summary>
        public bool IsUnique { get; protected set; }

        /// <summary>
        ///     字段的默认值
        /// </summary>
        public object Default { get; protected set; }

        /// <summary>
        ///     字段的排序规则
        /// </summary>
        public string Collate { get; protected set; }

        /// <summary>
        ///     向数据表实例中设置当前列的值
        /// </summary>
        /// <param name="dbTable"></param>
        /// <param name="val"></param>
        public abstract void SetValueToDbTable(IDbTable dbTable, object val);

        /// <summary>
        ///     从数据表实例中获取当前列的值
        /// </summary>
        /// <param name="dbTable"></param>
        /// <returns></returns>
        public abstract object GetValueFromDbTable(IDbTable dbTable);
    }

    /// <summary>
    /// sql中列的信息
    /// </summary>
    public sealed class SqlColumn : BaseColumn
    {
        #region private fields

        private readonly PropertyInfo _propertyInfo;

        #endregion

        #region ctor

        public SqlColumn(PropertyInfo propertyInfo)
        {
            _propertyInfo = propertyInfo;
            //给字段赋值
            ColumnType = propertyInfo.PropertyType;
            ColumnName = propertyInfo.TryGetColumnName(out var columnName) ? columnName : propertyInfo.Name;
            IsPrimaryKey = propertyInfo.HasPrimaryKey();
            IsAutoIncrement = propertyInfo.HasAutoIncrement();
            IsNotNull = propertyInfo.HasNotNull();
            IsUnique = propertyInfo.HasUnique();
            if (propertyInfo.TryGetDefault(out var value)) Default = value;
            if (propertyInfo.TryGetCollation(out var collate)) Collate = collate;
        }

        #endregion
        
        #region public functions

        public override void SetValueToDbTable(IDbTable dbTable, object val)
        {
            _propertyInfo.SetValue(dbTable, Convert.ChangeType(val, ColumnType));
        }

        public override object GetValueFromDbTable(IDbTable dbTable)
        {
            return _propertyInfo.GetValue(dbTable);
        }

        #endregion
    }
}