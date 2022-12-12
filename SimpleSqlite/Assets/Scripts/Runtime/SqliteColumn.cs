namespace SimpleSqlite
{
    /// <summary>
    ///     数据表列信息的基类
    /// </summary>
    public abstract class BaseColumn
    {
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
        ///     字段的最大长度
        /// </summary>
        public int MaxLength { get; protected set; }

        /// <summary>
        ///     字段的排序规则
        /// </summary>
        public string Collation { get; protected set; }

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
}