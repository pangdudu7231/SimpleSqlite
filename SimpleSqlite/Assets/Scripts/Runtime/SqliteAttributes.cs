using System;

namespace SimpleSqlite
{
    /// <summary>
    ///     表的属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class TableAttribute : Attribute
    {
        /// <summary>
        ///     数据表的表名
        /// </summary>
        public string TableName { get; }

        public TableAttribute(string tableName)
        {
            TableName = tableName;
        }
    }

    /// <summary>
    ///     列的属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class ColumnAttribute : Attribute
    {
        /// <summary>
        ///     数据表的列名
        /// </summary>
        public string ColumnName { get; }

        public ColumnAttribute(string columnName)
        {
            ColumnName = columnName;
        }
    }

    /// <summary>
    ///     数据表列字段为主键的属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class PrimaryKeyAttribute : Attribute
    {
    }

    /// <summary>
    ///     数据表列字段为自增字段的属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class AutoIncrementAttribute : Attribute
    {
    }

    /// <summary>
    ///     数据表列字段不为空的属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class NotNullAttribute : Attribute
    {
    }

    /// <summary>
    ///     数据表列字段为唯一值的属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class UniqueAttribute : Attribute
    {
    }

    /// <summary>
    ///     数据表字段默认值的属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class DefaultAttribute : Attribute
    {
        /// <summary>
        ///     数据表列的默认值
        /// </summary>
        public object Value { get; }

        public DefaultAttribute(object value)
        {
            Value = value;
        }
    }

    /// <summary>
    ///     数据表字段最大长度的属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class MaxLengthAttribute : Attribute
    {
        private const int DEFAULT_MAX_STRING_LENGTH = 140; // 默认的最大长度

        public int Value { get; }

        public MaxLengthAttribute()
        {
            Value = DEFAULT_MAX_STRING_LENGTH;
        }

        public MaxLengthAttribute(int value)
        {
            Value = value;
        }
    }

    /// <summary>
    ///     数据表字段的排序规则的属性
    ///     sqlite中的三种排序规则 BINARY、NOCASE 、RTRIM
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class CollationAttribute : Attribute
    {
        /// <summary>
        ///     字段的排序规则
        /// </summary>
        public string Collate { get; }

        public CollationAttribute(string collate)
        {
            Collate = collate;
        }
    }

    /// <summary>
    ///     字段的忽略属性，不会为标记的字段创建对应的列数据
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class IgnoreAttribute : Attribute
    {
    }
}