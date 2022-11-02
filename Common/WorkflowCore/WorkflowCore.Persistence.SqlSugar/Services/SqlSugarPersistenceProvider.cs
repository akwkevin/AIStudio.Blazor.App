using SqlSugar;
using WorkflowCore.Interface;
using WorkflowCore.Models;
using WorkflowCore.Persistence.SqlSugar.Models;

namespace WorkflowCore.Persistence.SqlSugar.Services
{
    public class SqlSugarPersistenceProvider : IPersistenceProvider
    {
        private readonly bool _canCreateDB;
        private readonly bool _canMigrateDB;
        private readonly ISqlSugarClient _db;

        public bool SupportsScheduledCommands => true;

        public SqlSugarPersistenceProvider(ISqlSugarClient db, bool canCreateDB, bool canMigrateDB)
        {
            _db = db;
            _canCreateDB = canCreateDB;
            _canMigrateDB = canMigrateDB;
        }

        public async Task<string> CreateEventSubscription(EventSubscription subscription, CancellationToken cancellationToken = default)
        {
            subscription.Id = Guid.NewGuid().ToString();
            var persistable = subscription.ToPersistable();
            var result = _db.Insertable(subscription).ExecuteReturnEntity();
            return subscription.Id;
        }

        public async Task<string> CreateNewWorkflow(WorkflowInstance workflow, CancellationToken cancellationToken = default)
        {
            workflow.Id = Guid.NewGuid().ToString();
            var persistable = workflow.ToPersistable();
            var result = _db.Insertable(persistable).ExecuteReturnEntity();
            return workflow.Id;
        }

        public async Task<IEnumerable<string>> GetRunnableInstances(DateTime asAt, CancellationToken cancellationToken = default)
        {
            var now = asAt.ToUniversalTime().Ticks;
            var raw = await _db.Queryable<PersistedWorkflow>()
                .Where(x => x.NextExecution.HasValue && (x.NextExecution <= now) && (x.Status == WorkflowStatus.Runnable))
                .Select(x => x.InstanceId)
                .ToListAsync();

            return raw.Select(s => s.ToString()).ToList();
        }

        public async Task<IEnumerable<WorkflowInstance>> GetWorkflowInstances(WorkflowStatus? status, string type, DateTime? createdFrom, DateTime? createdTo, int skip, int take)
        {
            var query = _db.Queryable<PersistedWorkflow>();

            if (status.HasValue)
                query = query.Where(x => x.Status == status.Value);

            if (!String.IsNullOrEmpty(type))
                query = query.Where(x => x.WorkflowDefinitionId == type);

            if (createdFrom.HasValue)
                query = query.Where(x => x.CreateTime >= createdFrom.Value);

            if (createdTo.HasValue)
                query = query.Where(x => x.CreateTime <= createdTo.Value);

            var rawResult = await query.Skip(skip).Take(take).ToListAsync();

            //第一层
            _db.ThenMapper(rawResult, item =>
            {
                _db.Queryable<PersistedExecutionPointer>().SetContext(x => x.WorkflowId, () => item.PersistenceId, item).ToList().ForEach(pointer => item.ExecutionPointers.Add(pointer));
            });
            // 第二层
            _db.ThenMapper(rawResult.SelectMany(it => it.ExecutionPointers), item =>
            {
                item.ExtensionAttributes = _db.Queryable<PersistedExtensionAttribute>().SetContext(x => x.ExecutionPointerId, () => item.PersistenceId, item).ToList();
            });

            List<WorkflowInstance> result = new List<WorkflowInstance>();

            foreach (var item in rawResult)
                result.Add(item.ToWorkflowInstance());

            return result;
        }

        public async Task<WorkflowInstance> GetWorkflowInstance(string Id, CancellationToken cancellationToken = default)
        {
            var uid = new Guid(Id);
            var raw = await _db.Queryable<PersistedWorkflow>()
                .FirstAsync(x => x.InstanceId == uid);

            //第一层
            _db.ThenMapper(new List<PersistedWorkflow> { raw }, item =>
            {
                _db.Queryable<PersistedExecutionPointer>().SetContext(x => x.WorkflowId, () => item.PersistenceId, item).ToList().ForEach(pointer => item.ExecutionPointers.Add(pointer));
            });
            // 第二层
            _db.ThenMapper(raw.ExecutionPointers, item =>
            {
                item.ExtensionAttributes = _db.Queryable<PersistedExtensionAttribute>().SetContext(x => x.ExecutionPointerId, () => item.PersistenceId, item).ToList();
            });

            if (raw == null)
                return null;

            return raw.ToWorkflowInstance();

        }

        public async Task<IEnumerable<WorkflowInstance>> GetWorkflowInstances(IEnumerable<string> ids, CancellationToken cancellationToken = default)
        {
            if (ids == null)
            {
                return new List<WorkflowInstance>();
            }

            var uids = ids.Select(i => new Guid(i));
            var query = _db.Queryable<PersistedWorkflow>()
                .Where(x => uids.Contains(x.InstanceId));

            var rawResult = await query.ToListAsync();
            //第一层
            _db.ThenMapper(rawResult, item =>
            {
                _db.Queryable<PersistedExecutionPointer>().SetContext(x => x.WorkflowId, () => item.PersistenceId, item).ToList().ForEach(pointer => item.ExecutionPointers.Add(pointer));
            });
            // 第二层
            _db.ThenMapper(rawResult.SelectMany(it => it.ExecutionPointers), item =>
            {
                item.ExtensionAttributes = _db.Queryable<PersistedExtensionAttribute>().SetContext(x => x.ExecutionPointerId, () => item.PersistenceId, item).ToList();
            });

            List<WorkflowInstance> result = new List<WorkflowInstance>();

            foreach (var item in rawResult)
                result.Add(item.ToWorkflowInstance());

            return result;
        }

        public async Task PersistWorkflow(WorkflowInstance workflow, CancellationToken cancellationToken = default)
        {
            var uid = new Guid(workflow.Id);
            var existingEntity = await _db.Queryable<PersistedWorkflow>()
                .Where(x => x.InstanceId == uid)
                .FirstAsync();

            //第一层
            _db.ThenMapper(new List<PersistedWorkflow> { existingEntity }, item =>
            {
                _db.Queryable<PersistedExecutionPointer>().SetContext(x => x.WorkflowId, () => item.PersistenceId, item).ToList().ForEach(pointer => item.ExecutionPointers.Add(pointer));
            });
            // 第二层
            _db.ThenMapper(existingEntity.ExecutionPointers, item =>
            {
                item.ExtensionAttributes = _db.Queryable<PersistedExtensionAttribute>().SetContext(x => x.ExecutionPointerId, () => item.PersistenceId, item).ToList();
            });

            var persistable = workflow.ToPersistable(existingEntity);

            try
            {
                _db.Ado.BeginTran();
                await _db.Updateable(persistable).ExecuteCommandAsync();
                await _db.Updateable(persistable.ExecutionPointers).ExecuteCommandAsync();
                await _db.Updateable(persistable.ExecutionPointers.SelectMany(p => p.ExtensionAttributes).ToList()).ExecuteCommandAsync();
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

        public async Task PersistWorkflow(WorkflowInstance workflow, List<EventSubscription> subscriptions, CancellationToken cancellationToken = default)
        {
            var uid = new Guid(workflow.Id);
            var existingEntity = await _db.Queryable<PersistedWorkflow>()
                .Where(x => x.InstanceId == uid)
                .FirstAsync();

            //第一层
            _db.ThenMapper(new List<PersistedWorkflow> { existingEntity }, item =>
            {
                _db.Queryable<PersistedExecutionPointer>().SetContext(x => x.WorkflowId, () => item.PersistenceId, item).ToList().ForEach(pointer => item.ExecutionPointers.Add(pointer));
            });
            // 第二层
            _db.ThenMapper(existingEntity.ExecutionPointers, item =>
            {
                item.ExtensionAttributes = _db.Queryable<PersistedExtensionAttribute>().SetContext(x => x.ExecutionPointerId, () => item.PersistenceId, item).ToList();
            });

            var workflowPersistable = workflow.ToPersistable(existingEntity);
            try
            {
                foreach (var subscription in subscriptions)
                {
                    subscription.Id = Guid.NewGuid().ToString();
                    var subscriptionPersistable = subscription.ToPersistable();
                    await _db.Insertable(subscriptionPersistable).ExecuteCommandAsync();
                }

                _db.Ado.BeginTran();
                await _db.Updateable(workflowPersistable).ExecuteCommandAsync();
                await _db.Updateable(workflowPersistable.ExecutionPointers).ExecuteCommandAsync();
                await _db.Updateable(workflowPersistable.ExecutionPointers.SelectMany(p => p.ExtensionAttributes).ToList()).ExecuteCommandAsync();
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

        public async Task TerminateSubscription(string eventSubscriptionId, CancellationToken cancellationToken = default)
        {
            var uid = new Guid(eventSubscriptionId);
            var existing = await _db.Queryable<PersistedSubscription>().FirstAsync(x => x.SubscriptionId == uid);
            await _db.Deleteable(existing).ExecuteCommandAsync();
        }

        public virtual void EnsureStoreExists()
        {
            //using (var context = ConstructDbContext())
            //{
            //    if (_canCreateDB && !_canMigrateDB)
            //    {
            //        context.Database.EnsureCreated();
            //        return;
            //    }

            //    if (_canMigrateDB)
            //    {
            //        context.Database.Migrate();
            //        return;
            //    }
            //}
        }

        public async Task<IEnumerable<EventSubscription>> GetSubscriptions(string eventName, string eventKey, DateTime asOf, CancellationToken cancellationToken = default)
        {
            asOf = asOf.ToUniversalTime();
            var raw = await _db.Queryable<PersistedSubscription>()
                .Where(x => x.EventName == eventName && x.EventKey == eventKey && x.SubscribeAsOf <= asOf)
                .ToListAsync();

            return raw.Select(item => item.ToEventSubscription()).ToList();
        }

        public async Task<string> CreateEvent(Event newEvent, CancellationToken cancellationToken = default)
        {
            newEvent.Id = Guid.NewGuid().ToString();
            var persistable = newEvent.ToPersistable();
            var result = await _db.Insertable(persistable).ExecuteReturnEntityAsync();
            return newEvent.Id;
        }

        public async Task<Event> GetEvent(string id, CancellationToken cancellationToken = default)
        {
            Guid uid = new Guid(id);
            var raw = await _db.Queryable<PersistedEvent>()
                .FirstAsync(x => x.EventId == uid);

            if (raw == null)
                return null;

            return raw.ToEvent();
        }

        public async Task<IEnumerable<string>> GetRunnableEvents(DateTime asAt, CancellationToken cancellationToken = default)
        {
            var now = asAt.ToUniversalTime();

            asAt = asAt.ToUniversalTime();
            var raw = await _db.Queryable<PersistedEvent>()
                .Where(x => !x.IsProcessed)
                .Where(x => x.EventTime <= now)
                .Select(x => x.EventId)
                .ToListAsync();

            return raw.Select(s => s.ToString()).ToList();
        }

        public async Task MarkEventProcessed(string id, CancellationToken cancellationToken = default)
        {
            var uid = new Guid(id);
            var existingEntity = await _db.Queryable<PersistedEvent>()
                .Where(x => x.EventId == uid)
                .FirstAsync();

            existingEntity.IsProcessed = true;
            await _db.Saveable(existingEntity).ExecuteCommandAsync();
        }

        public async Task<IEnumerable<string>> GetEvents(string eventName, string eventKey, DateTime asOf, CancellationToken cancellationToken)
        {
            var raw = await _db.Queryable<PersistedEvent>()
                .Where(x => x.EventName == eventName && x.EventKey == eventKey)
                .Where(x => x.EventTime >= asOf)
                .Select(x => x.EventId)
                .ToListAsync();

            var result = new List<string>();

            foreach (var s in raw)
                result.Add(s.ToString());

            return result;
        }

        public async Task MarkEventUnprocessed(string id, CancellationToken cancellationToken = default)
        {
            var uid = new Guid(id);
            var existingEntity = await _db.Queryable<PersistedEvent>()
                .Where(x => x.EventId == uid)
                .FirstAsync();

            existingEntity.IsProcessed = false;
            await _db.Saveable(existingEntity).ExecuteCommandAsync();
        }

        public async Task PersistErrors(IEnumerable<ExecutionError> errors, CancellationToken cancellationToken = default)
        {
            var executionErrors = errors as ExecutionError[] ?? errors.ToArray();
            if (executionErrors.Any())
            {
                await _db.Saveable(executionErrors.Select(error => error.ToPersistable()).ToList()).ExecuteCommandAsync();
            }
        }

        public async Task<EventSubscription> GetSubscription(string eventSubscriptionId, CancellationToken cancellationToken = default)
        {
            var uid = new Guid(eventSubscriptionId);
            var raw = await _db.Queryable<PersistedSubscription>().FirstAsync(x => x.SubscriptionId == uid);

            return raw?.ToEventSubscription();
        }

        public async Task<EventSubscription> GetFirstOpenSubscription(string eventName, string eventKey, DateTime asOf, CancellationToken cancellationToken = default)
        {
            var raw = await _db.Queryable<PersistedSubscription>().FirstAsync(x => x.EventName == eventName && x.EventKey == eventKey && x.SubscribeAsOf <= asOf && x.ExternalToken == null);

            return raw?.ToEventSubscription();
        }

        public async Task<bool> SetSubscriptionToken(string eventSubscriptionId, string token, string workerId, DateTime expiry, CancellationToken cancellationToken = default)
        {
            var uid = new Guid(eventSubscriptionId);
            var existingEntity = await _db.Queryable<PersistedSubscription>()
                .Where(x => x.SubscriptionId == uid)
                .FirstAsync();

            existingEntity.ExternalToken = token;
            existingEntity.ExternalWorkerId = workerId;
            existingEntity.ExternalTokenExpiry = expiry;
            await _db.Saveable(existingEntity).ExecuteCommandAsync();

            return true;
        }

        public async Task ClearSubscriptionToken(string eventSubscriptionId, string token, CancellationToken cancellationToken = default)
        {
            var uid = new Guid(eventSubscriptionId);
            var existingEntity = await _db.Queryable<PersistedSubscription>()
                .Where(x => x.SubscriptionId == uid)
                .FirstAsync();

            if (existingEntity.ExternalToken != token)
                throw new InvalidOperationException();

            existingEntity.ExternalToken = null;
            existingEntity.ExternalWorkerId = null;
            existingEntity.ExternalTokenExpiry = null;
            await _db.Saveable(existingEntity).ExecuteCommandAsync();
        }

        public async Task ScheduleCommand(ScheduledCommand command)
        {
            try
            {
                var persistable = command.ToPersistable();
                var result = await _db.Insertable(persistable).ExecuteReturnEntityAsync();
            }
            catch (Exception ex)
            {
                //log
            }
        }

        public async Task ProcessCommands(DateTimeOffset asOf, Func<ScheduledCommand, Task> action, CancellationToken cancellationToken = default)
        {
            var cursor = await _db.Queryable<PersistedScheduledCommand>()
                .Where(x => x.ExecuteTime < asOf.UtcDateTime.Ticks).ToListAsync();

            foreach (var command in cursor)
            {
                try
                {
                    await action(command.ToScheduledCommand());
                    await _db.Deleteable(command).ExecuteCommandAsync();
                }
                catch (Exception)
                {
                    //TODO: add logger
                }
            }

        }
    }
}
