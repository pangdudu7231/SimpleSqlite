namespace SimpleSqlite
{
    /// <summary>
    /// 数据表信息的基类
    /// </summary>
    public abstract class BaseTable
    {
        /// <summary>
        /// 数据表的表名
        /// </summary>
        public string TableName { get; protected set; }

        /// <summary>
        /// 数据表中的列信息
        /// </summary>
        public BaseColumn[] Columns { get; protected set; }

        /// <summary>
        /// 数据表中主键列信息
        /// </summary>
        public BaseColumn PColumn { get; protected set; }

        /// <summary>
        /// 数据表中可更改的列信息（除去主键的列）
        /// </summary>
        public BaseColumn[] AlterableColumns { get; protected set; }
    }

    /// <summary>
    /// 数据表的信息类
    /// </summary>
    public sealed class SqlTable : BaseTable
    {
        
    }
}