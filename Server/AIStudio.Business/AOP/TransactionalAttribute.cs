using AIStudio.Common.DI;
using AIStudio.Common.DI.AOP;
using Microsoft.Extensions.DependencyInjection;
using Quartz.Impl.AdoJobStore;
using SqlSugar;
using System.Data;

namespace AIStudio.Business.AOP
{
    /// <summary>
    /// 使用事务包裹
    /// </summary>
    /// <seealso cref="AIStudio.Common.DI.AOP.BaseAOPAttribute" />
    public class TransactionalAttribute : BaseAOPAttribute
    {
        /// <summary>
        /// The cross database
        /// </summary>
        private readonly bool _crossDb;
        /// <summary>
        /// The isolation level
        /// </summary>
        private readonly IsolationLevel _isolationLevel;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionalAttribute"/> class.
        /// </summary>
        /// <param name="crossDb">if set to <c>true</c> [cross database].</param>
        /// <param name="isolationLevel">The isolation level.</param>
        public TransactionalAttribute(bool crossDb = false, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            _crossDb = crossDb;
        }

        /// <summary>
        /// The container
        /// </summary>
        private TransactionContainer _container;
        /// <summary>
        /// Befors the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        public override async Task Befor(IAOPContext context)
        {
            _container = context.ServiceProvider.GetService<TransactionContainer>();

            if (!_container.TransactionOpened)
            {
                _container.TransactionOpened = true;
                _container.BeginTransaction(_crossDb, _isolationLevel);
            }
        }
        /// <summary>
        /// Afters the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <exception cref="System.Exception">系统异常</exception>
        public override async Task After(IAOPContext context)
        {
            _container = context.ServiceProvider.GetService<TransactionContainer>();

            try
            {
                if (_container.TransactionOpened)
                {
                    _container.CommitTransaction();
                }
            }
            catch (Exception ex)
            {
                _container.RollbackTransaction();
                throw new Exception("系统异常", ex);
            }

            if (_container.TransactionOpened)
            {
                _container.TransactionOpened = false;
            }

            await Task.CompletedTask;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="AIStudio.Common.DI.IScopedDependency" />
    public class TransactionContainer : IScopedDependency
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionContainer"/> class.
        /// </summary>
        /// <param name="db">The database.</param>
        public TransactionContainer(ISqlSugarClient db)
        {
            Db = db;
        }
        /// <summary>
        /// The database
        /// </summary>
        private readonly ISqlSugarClient Db;
        /// <summary>
        /// Gets or sets a value indicating whether [transaction opened].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [transaction opened]; otherwise, <c>false</c>.
        /// </value>
        public bool TransactionOpened { get; set; }

        /// <summary>
        /// 开始事务,跨库不支持IsolationLevel
        /// </summary>
        /// <param name="crossDb">是否跨库</param>
        /// <param name="isolationLevel">The isolation level.</param>
        public void BeginTransaction(bool crossDb = false, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            if (crossDb)
            {
                (Db as SqlSugarScope).BeginTran();
            }
            else
            {
                Db.Ado.BeginTran();
            }
        }

        /// <summary>
        /// 提交事务
        /// </summary>
        /// <param name="crossDb">是否跨库</param>
        public void CommitTransaction(bool crossDb = false)
        {
            if (crossDb)
            {
                (Db as SqlSugarScope).CommitTran();
            }
            else
            {
                Db.Ado.CommitTran();
            }
        }

        /// <summary>
        /// 回退事务
        /// </summary>
        /// <param name="crossDb">是否跨库</param>
        public void RollbackTransaction(bool crossDb = false)
        {
            if (crossDb)
            {
                (Db as SqlSugarScope).RollbackTran();
            }
            else
            {
                Db.Ado.RollbackTran();
            }
        }
    }
}
