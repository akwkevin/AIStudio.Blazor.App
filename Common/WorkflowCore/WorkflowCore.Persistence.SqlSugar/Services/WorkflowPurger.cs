using SqlSugar;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WorkflowCore.Interface;
using WorkflowCore.Models;
using WorkflowCore.Persistence.SqlSugar.Models;

namespace WorkflowCore.Persistence.SqlSugar.Services
{
    public class WorkflowPurger : IWorkflowPurger
    {
        private readonly ISqlSugarClient _db;

        public WorkflowPurger(ISqlSugarClient db)
        {
            _db = db;
        }

        public async Task PurgeWorkflows(WorkflowStatus status, DateTime olderThan, CancellationToken cancellationToken = default)
        {
            var olderThanUtc = olderThan.ToUniversalTime();

            var workflows = await _db.Queryable<PersistedWorkflow>().Where(x => x.Status == status && x.CompleteTime < olderThanUtc).ToListAsync();

            try
            {
                _db.Ado.BeginTran();
                foreach (var wf in workflows)
                {
                    foreach (var pointer in wf.ExecutionPointers)
                    {
                        foreach (var extAttr in pointer.ExtensionAttributes)
                        {
                            await _db.Deleteable(extAttr).ExecuteCommandAsync();
                        }

                        await _db.Deleteable(pointer).ExecuteCommandAsync();
                    }
                    await _db.Deleteable(wf).ExecuteCommandAsync();
                }
                _db.Ado.CommitTran();
            }
            catch (Exception ex)
            {
                _db.Ado.RollbackTran();
            }
            finally
            {

            }
        }
    }
}