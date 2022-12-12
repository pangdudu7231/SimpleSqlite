namespace SimpleSqlite
{
    /// <summary>
    ///     sqlite数据库操作的返回码
    ///     https://www.sqlite.org/c3ref/c_abort.html
    /// </summary>
    public enum SqliteResult
    {
        /// <summary>
        ///     执行成功的返回
        /// </summary>
        OK = 0,

        /// <summary>
        ///     一般错误
        /// </summary>
        ERROR = 1,

        /// <summary>
        ///     SQLite的内部错误
        /// </summary>
        INTERNAL = 2,

        /// <summary>
        ///     拒绝访问
        /// </summary>
        PERM = 3,

        /// <summary>
        ///     返回请求终止
        /// </summary>
        ABORT = 4,

        /// <summary>
        ///     数据库文件被锁
        /// </summary>
        BUSY = 5,

        /// <summary>
        ///     数据库中的表被锁
        /// </summary>
        LOCKED = 6,

        /// <summary>
        ///     内存分配失败
        /// </summary>
        NOMEM = 7,

        /// <summary>
        ///     试图写入数据到一个只读数据库中
        /// </summary>
        READONLY = 8,

        /// <summary>
        ///     操作被sqlite3_interrupt() 终止
        /// </summary>
        INTERRUPT = 9,

        /// <summary>
        ///     IO错误
        /// </summary>
        IOERR = 10,

        /// <summary>
        ///     数据库磁盘映像格式不正确
        /// </summary>
        CORRUPT = 11,

        /// <summary>
        ///     sqlite3_file_control()中未知的操作码
        /// </summary>
        NOTFOUND = 12,

        /// <summary>
        ///     数据库已满，插入数据失败
        /// </summary>
        FULL = 13,

        /// <summary>
        ///     不能打开数据库文件
        /// </summary>
        CANTOPEN = 14,

        /// <summary>
        ///     数据库锁定协议错误
        /// </summary>
        PROTOCOL = 15,

        /// <summary>
        ///     仅内部使用
        /// </summary>
        EMPTY = 16,

        /// <summary>
        ///     数据库架构已更改
        /// </summary>
        SCHEMA = 17,

        /// <summary>
        ///     string或者blob超出长度限制
        /// </summary>
        TOOBIG = 18,

        /// <summary>
        ///     由于违反约束而中止
        /// </summary>
        CONSTRAINT = 19,

        /// <summary>
        ///     数据类型不匹配
        /// </summary>
        MISMATCH = 20,

        /// <summary>
        ///     使用主机上不支持的操作系统功能
        /// </summary>
        NOLFS = 22,

        /// <summary>
        ///     授权被拒绝
        /// </summary>
        AUTH = 23,

        /// <summary>
        ///     格式错误(未使用)
        /// </summary>
        FORMAT = 24,

        /// <summary>
        ///     sqlite3_bind的第二个参数超出范围
        /// </summary>
        RANGE = 25,

        /// <summary>
        ///     打开的文件不是数据库文件
        /// </summary>
        NOTADB = 26,

        /// <summary>
        ///     sqlite3_log()的通知
        /// </summary>
        NOTICE = 27,

        /// <summary>
        ///     sqlite3_log()的警告
        /// </summary>
        WARNING = 28,

        /// <summary>
        ///     sqlite3_step()已经存在另一行
        /// </summary>
        ROW = 100,

        /// <summary>
        ///     sqlite3_step()已经完成执行
        /// </summary>
        DONE = 101
    }
}