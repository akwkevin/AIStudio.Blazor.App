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
    public class TransactionalAttribute : BaseAOPAttribute
    {
        private readonly bool _crossDb;
        private readonly IsolationLevel _isolationLevel;

        public TransactionalAttribute(bool crossDb = false, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            _crossDb = crossDb;
        }

        private TransactionContainer _container;
        public override async Task Befor(IAOPContext context)
        {
            _container = context.ServiceProvider.GetService<TransactionContainer>();

            if (!_container.TransactionOpened)
            {
                _container.TransactionOpened = true;
                _container.BeginTransaction(_crossDb, _isolationLevel);
            }
        }
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

    public class TransactionContainer : IScopedDependency
    {
        public TransactionContainer(ISqlSugarClient db)
        {
            Db = db;
        }
        private readonly ISqlSugarClient Db;
        public bool TransactionOpened { get; set; }

        /// <summary>
        /// 开始事务,跨库不支持IsolationLevel
        /// </summary>
        /// <param name="crossDb">是否跨库</param>        
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
