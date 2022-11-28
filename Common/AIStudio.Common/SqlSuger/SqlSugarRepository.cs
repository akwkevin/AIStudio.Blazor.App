using Microsoft.Extensions.DependencyInjection;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AIStudio.Common.SqlSuger;

/// <summary>
/// SqlSugar 仓储实现类
/// </summary>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
public partial class SqlSugarRepository<TEntity>
where TEntity : class, new()
{
    /// <summary>
    /// The update ignore columns
    /// </summary>
    private readonly string[] UpdateIgnoreColumns = new string[] { "CreateTime", "CreatorId", "CreatorName" };

    #region 属性
    /// <summary>
    /// 初始化 SqlSugar 客户端
    /// </summary>
    private readonly SqlSugarScope _db;
    /// <summary>
    /// The service provider
    /// </summary>
    private readonly IServiceProvider _serviceProvider;
    /// <summary>
    /// 数据库上下文
    /// </summary>
    /// <value>
    /// The context.
    /// </value>
    public virtual SqlSugarScope Context { get; }
    /// <summary>
    /// Gets the entity context.
    /// </summary>
    /// <value>
    /// The entity context.
    /// </value>
    public virtual SqlSugarProvider EntityContext { get; }
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="db">The database.</param>
    /// <param name="serviceProvider">The service provider.</param>
    public SqlSugarRepository(ISqlSugarClient db, IServiceProvider serviceProvider)
    {
        Context = _db = (SqlSugarScope)db;
        EntityContext = _db.GetConnectionWithAttr<TEntity>();
        Ado = EntityContext.Ado;

        _serviceProvider = serviceProvider;
    }
    /// <summary>
    /// 实体集合
    /// </summary>
    /// <value>
    /// The entities.
    /// </value>
    public virtual ISugarQueryable<TEntity> Entities => EntityContext.Queryable<TEntity>();

    /// <summary>
    /// 原生 Ado 对象
    /// </summary>
    /// <value>
    /// The ADO.
    /// </value>
    public virtual IAdo Ado { get; }
    #endregion

    #region 查询
    /// <summary>
    /// 获取总数
    /// </summary>
    /// <param name="whereExpression">The where expression.</param>
    /// <returns></returns>
    public int Count(Expression<Func<TEntity, bool>> whereExpression)
    {
        return Entities.Count(whereExpression);
    }

    /// <summary>
    /// 获取总数
    /// </summary>
    /// <param name="whereExpression">The where expression.</param>
    /// <returns></returns>
    public Task<int> CountAsync(Expression<Func<TEntity, bool>> whereExpression)
    {
        return Entities.CountAsync(whereExpression);
    }

    /// <summary>
    /// 检查是否存在
    /// </summary>
    /// <param name="whereExpression">The where expression.</param>
    /// <returns></returns>
    public bool Any(Expression<Func<TEntity, bool>> whereExpression)
    {
        return Entities.Any(whereExpression);
    }

    /// <summary>
    /// 检查是否存在
    /// </summary>
    /// <param name="whereExpression">The where expression.</param>
    /// <returns></returns>
    public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> whereExpression)
    {
        return await Entities.AnyAsync(whereExpression);
    }

    /// <summary>
    /// 通过主键获取实体
    /// </summary>
    /// <param name="Id">The identifier.</param>
    /// <returns></returns>
    public TEntity Single(dynamic Id)
    {
        return Entities.InSingle(Id);
    }

    /// <summary>
    /// 获取一个实体
    /// </summary>
    /// <param name="whereExpression">The where expression.</param>
    /// <returns></returns>
    public TEntity Single(Expression<Func<TEntity, bool>> whereExpression)
    {
        return Entities.Single(whereExpression);
    }

    /// <summary>
    /// 获取一个实体
    /// </summary>
    /// <param name="whereExpression">The where expression.</param>
    /// <returns></returns>
    public Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> whereExpression)
    {
        return Entities.SingleAsync(whereExpression);
    }

    /// <summary>
    /// 获取一个实体
    /// </summary>
    /// <param name="whereExpression">The where expression.</param>
    /// <returns></returns>
    public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> whereExpression)
    {
        return Entities.First(whereExpression);
    }

    /// <summary>
    /// 获取一个实体
    /// </summary>
    /// <param name="whereExpression">The where expression.</param>
    /// <returns></returns>
    public async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> whereExpression)
    {
        return await Entities.FirstAsync(whereExpression);
    }

    /// <summary>
    /// 获取列表
    /// </summary>
    /// <returns></returns>
    public List<TEntity> ToList()
    {
        return Entities.ToList();
    }

    /// <summary>
    /// 获取列表
    /// </summary>
    /// <param name="whereExpression">The where expression.</param>
    /// <returns></returns>
    public List<TEntity> ToList(Expression<Func<TEntity, bool>> whereExpression)
    {
        return Entities.Where(whereExpression).ToList();
    }

    /// <summary>
    /// 获取列表
    /// </summary>
    /// <param name="whereExpression">The where expression.</param>
    /// <param name="orderByExpression">The order by expression.</param>
    /// <param name="orderByType">Type of the order by.</param>
    /// <returns></returns>
    public List<TEntity> ToList(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> orderByExpression = null, OrderByType orderByType = OrderByType.Asc)
    {
        return Entities.OrderByIF(orderByExpression != null, orderByExpression, orderByType).Where(whereExpression).ToList();
    }

    /// <summary>
    /// 获取列表
    /// </summary>
    /// <returns></returns>
    public Task<List<TEntity>> ToListAsync()
    {
        return Entities.ToListAsync();
    }

    /// <summary>
    /// 获取列表
    /// </summary>
    /// <param name="whereExpression">The where expression.</param>
    /// <returns></returns>
    public Task<List<TEntity>> ToListAsync(Expression<Func<TEntity, bool>> whereExpression)
    {
        return Entities.Where(whereExpression).ToListAsync();
    }

    /// <summary>
    /// 获取列表
    /// </summary>
    /// <param name="whereExpression">The where expression.</param>
    /// <param name="orderByExpression">The order by expression.</param>
    /// <param name="orderByType">Type of the order by.</param>
    /// <returns></returns>
    public Task<List<TEntity>> ToListAsync(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> orderByExpression = null, OrderByType orderByType = OrderByType.Asc)
    {
        return Entities.OrderByIF(orderByExpression != null, orderByExpression, orderByType).Where(whereExpression).ToListAsync();
    }
    #endregion

    #region 新增
    /// <summary>
    /// Ases the insertable.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <returns></returns>
    public virtual IInsertable<TEntity> AsInsertable(TEntity entity)
    {
        return EntityContext.Insertable(entity);
    }

    /// <summary>
    /// Ases the insertable.
    /// </summary>
    /// <param name="entities">The entities.</param>
    /// <returns></returns>
    public virtual IInsertable<TEntity> AsInsertable(params TEntity[] entities)
    {
        return EntityContext.Insertable(entities);
    }

    /// <summary>
    /// 新增一条记录
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <returns></returns>
    public virtual int Insert(TEntity entity)
    {
        return EntityContext.Insertable(entity).ExecuteCommand();
    }

    /// <summary>
    /// 新增多条记录
    /// </summary>
    /// <param name="entities">The entities.</param>
    /// <returns></returns>
    public virtual int Insert(params TEntity[] entities)
    {
        return EntityContext.Insertable(entities).ExecuteCommand();
    }

    /// <summary>
    /// 新增多条记录
    /// </summary>
    /// <param name="entities">The entities.</param>
    /// <returns></returns>
    public virtual int Insert(IEnumerable<TEntity> entities)
    {
        return EntityContext.Insertable(entities.ToArray()).ExecuteCommand();
    }

    /// <summary>
    /// 新增一条记录返回自增Id
    /// </summary>
    /// <param name="insertObj">The insert object.</param>
    /// <returns></returns>
    public virtual int InsertReturnIdentity(TEntity insertObj)
    {
        return EntityContext.Insertable(insertObj).ExecuteReturnIdentity();
    }

    /// <summary>
    /// 新增一条记录返回雪花Id
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <returns></returns>
    public virtual long InsertReturnSnowflakeId(TEntity entity)
    {
        return EntityContext.Insertable(entity).ExecuteReturnSnowflakeId();
    }

    /// <summary>
    /// 新增一条记录返回实体
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <returns></returns>
    public virtual TEntity InsertReturnEntity(TEntity entity)
    {
        return EntityContext.Insertable(entity).ExecuteReturnEntity();
    }



    /// <summary>
    /// 新增一条记录
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <returns></returns>
    public virtual Task<int> InsertAsync(TEntity entity)
    {
        return EntityContext.Insertable(entity).ExecuteCommandAsync();
    }

    /// <summary>
    /// 新增多条记录
    /// </summary>
    /// <param name="entities">The entities.</param>
    /// <returns></returns>
    public virtual Task<int> InsertAsync(params TEntity[] entities)
    {
        return EntityContext.Insertable(entities).ExecuteCommandAsync();
    }

    /// <summary>
    /// 新增多条记录
    /// </summary>
    /// <param name="entities">The entities.</param>
    /// <returns></returns>
    public virtual Task<int> InsertAsync(IEnumerable<TEntity> entities)
    {
        if (entities != null && entities.Any())
        {
            return EntityContext.Insertable(entities.ToArray()).ExecuteCommandAsync();
        }
        return Task.FromResult(0);
    }

    /// <summary>
    /// 新增一条记录返回自增Id
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <returns></returns>
    public virtual async Task<long> InsertReturnIdentityAsync(TEntity entity)
    {
        return await EntityContext.Insertable(entity).ExecuteReturnBigIdentityAsync();
    }

    /// <summary>
    /// 新增一条记录返回雪花Id
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <returns></returns>
    public virtual async Task<long> InsertReturnSnowflakeIdAsync(TEntity entity)
    {
        return await EntityContext.Insertable(entity).ExecuteReturnSnowflakeIdAsync();
    }

    /// <summary>
    /// 新增一条记录返回实体
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <returns></returns>
    public virtual async Task<TEntity> InsertReturnEntityAsync(TEntity entity)
    {
        return await EntityContext.Insertable(entity).ExecuteReturnEntityAsync();
    }
    #endregion

    #region 更新

    /// <summary>
    /// 更新一条记录
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <returns></returns>
    public virtual int Update(TEntity entity)
    {
        return EntityContext.Updateable(entity).IgnoreColumns(UpdateIgnoreColumns).ExecuteCommand();
    }

    /// <summary>
    /// 更新多条记录
    /// </summary>
    /// <param name="entities">The entities.</param>
    /// <returns></returns>
    public virtual int Update(params TEntity[] entities)
    {
        return EntityContext.Updateable(entities).IgnoreColumns(UpdateIgnoreColumns).ExecuteCommand();
    }
    /// <summary>
    /// 更新多条记录
    /// </summary>
    /// <param name="entities">The entities.</param>
    /// <returns></returns>
    public virtual int Update(IEnumerable<TEntity> entities)
    {
        return EntityContext.Updateable(entities.ToArray()).IgnoreColumns(UpdateIgnoreColumns).ExecuteCommand();
    }

    /// <summary>
    /// 更新一条记录
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <returns></returns>
    public virtual async Task<int> UpdateAsync(TEntity entity)
    {
        return await EntityContext.Updateable(entity).IgnoreColumns(UpdateIgnoreColumns).ExecuteCommandAsync();
    }
    /// <summary>
    /// 更新记录
    /// </summary>
    /// <param name="predicate">更新的条件</param>
    /// <param name="content">更新的内容</param>
    /// <returns></returns>
    public virtual int Update(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TEntity>> content)
    {
        return EntityContext.Updateable(content).Where(predicate).IgnoreColumns(UpdateIgnoreColumns).ExecuteCommand();
    }

    /// <summary>
    /// 更新记录
    /// </summary>
    /// <param name="predicate">更新的条件</param>
    /// <param name="content">更新的内容</param>
    /// <returns></returns>
    public virtual async Task<int> UpdateAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TEntity>> content)
    {
        return await EntityContext.Updateable(content).Where(predicate).IgnoreColumns(UpdateIgnoreColumns).ExecuteCommandAsync();
    }

    /// <summary>
    /// 更新多条记录
    /// </summary>
    /// <param name="entities">The entities.</param>
    /// <returns></returns>
    public virtual Task<int> UpdateAsync(params TEntity[] entities)
    {
        return EntityContext.Updateable(entities).IgnoreColumns(UpdateIgnoreColumns).ExecuteCommandAsync();
    }

    /// <summary>
    /// 更新多条记录
    /// </summary>
    /// <param name="entities">The entities.</param>
    /// <returns></returns>
    public virtual Task<int> UpdateAsync(IEnumerable<TEntity> entities)
    {
        return EntityContext.Updateable(entities.ToArray()).IgnoreColumns(UpdateIgnoreColumns).ExecuteCommandAsync();
    }

    /// <summary>
    /// Ases the updateable.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <returns></returns>
    public virtual IUpdateable<TEntity> AsUpdateable(TEntity entity)
    {
        return EntityContext.Updateable(entity).IgnoreColumns(UpdateIgnoreColumns);
    }

    /// <summary>
    /// Ases the updateable.
    /// </summary>
    /// <param name="entities">The entities.</param>
    /// <returns></returns>
    public virtual IUpdateable<TEntity> AsUpdateable(IEnumerable<TEntity> entities)
    {
        return EntityContext.Updateable<TEntity>(entities).IgnoreColumns(UpdateIgnoreColumns);
    }
    #endregion

    #region 删除
    /// <summary>
    /// 删除一条记录
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <returns></returns>
    public virtual int Delete(TEntity entity)
    {
        return EntityContext.Deleteable(entity).ExecuteCommand();
    }

    /// <summary>
    /// 删除一条记录
    /// </summary>
    /// <param name="key">The key.</param>
    /// <returns></returns>
    public virtual int Delete(object key)
    {
        return EntityContext.Deleteable<TEntity>().In(key).ExecuteCommand();
    }

    /// <summary>
    /// 删除多条记录
    /// </summary>
    /// <param name="keys">The keys.</param>
    /// <returns></returns>
    public virtual int Delete(params object[] keys)
    {
        return EntityContext.Deleteable<TEntity>().In(keys).ExecuteCommand();
    }

    /// <summary>
    /// 自定义条件删除记录
    /// </summary>
    /// <param name="whereExpression">The where expression.</param>
    /// <returns></returns>
    public int Delete(Expression<Func<TEntity, bool>> whereExpression)
    {
        return EntityContext.Deleteable<TEntity>().Where(whereExpression).ExecuteCommand();
    }

    /// <summary>
    /// 删除一条记录
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <returns></returns>
    public virtual Task<int> DeleteAsync(TEntity entity)
    {
        return EntityContext.Deleteable(entity).ExecuteCommandAsync();
    }

    /// <summary>
    /// 删除一条记录
    /// </summary>
    /// <param name="key">The key.</param>
    /// <returns></returns>
    public virtual Task<int> DeleteAsync(object key)
    {
        return EntityContext.Deleteable<TEntity>().In(key).ExecuteCommandAsync();
    }

    /// <summary>
    /// 删除多条记录
    /// </summary>
    /// <param name="keys">The keys.</param>
    /// <returns></returns>
    public virtual Task<int> DeleteAsync(params object[] keys)
    {
        return EntityContext.Deleteable<TEntity>().In(keys).ExecuteCommandAsync();
    }

    /// <summary>
    /// 自定义条件删除记录
    /// </summary>
    /// <param name="whereExpression">The where expression.</param>
    /// <returns></returns>
    public async Task<int> DeleteAsync(Expression<Func<TEntity, bool>> whereExpression)
    {
        return await EntityContext.Deleteable<TEntity>().Where(whereExpression).ExecuteCommandAsync();
    }
    #endregion

    #region 其他
    /// <summary>
    /// 根据表达式查询多条记录
    /// </summary>
    /// <param name="predicate">The predicate.</param>
    /// <returns></returns>
    public virtual ISugarQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate)
    {
        return AsQueryable(predicate);
    }

    /// <summary>
    /// 根据表达式查询多条记录
    /// </summary>
    /// <param name="condition">if set to <c>true</c> [condition].</param>
    /// <param name="predicate">The predicate.</param>
    /// <returns></returns>
    public virtual ISugarQueryable<TEntity> Where(bool condition, Expression<Func<TEntity, bool>> predicate)
    {
        return AsQueryable().WhereIF(condition, predicate);
    }

    /// <summary>
    /// 构建查询分析器
    /// </summary>
    /// <returns></returns>
    public virtual ISugarQueryable<TEntity> AsQueryable()
    {
        return Entities;
    }

    /// <summary>
    /// 构建查询分析器
    /// </summary>
    /// <param name="predicate">The predicate.</param>
    /// <returns></returns>
    public virtual ISugarQueryable<TEntity> AsQueryable(Expression<Func<TEntity, bool>> predicate)
    {
        return Entities.Where(predicate);
    }

    /// <summary>
    /// 直接返回数据库结果
    /// </summary>
    /// <returns></returns>
    public virtual List<TEntity> AsEnumerable()
    {
        return AsQueryable().ToList();
    }

    /// <summary>
    /// 直接返回数据库结果
    /// </summary>
    /// <param name="predicate">The predicate.</param>
    /// <returns></returns>
    public virtual List<TEntity> AsEnumerable(Expression<Func<TEntity, bool>> predicate)
    {
        return AsQueryable(predicate).ToList();
    }

    /// <summary>
    /// 直接返回数据库结果
    /// </summary>
    /// <returns></returns>
    public virtual Task<List<TEntity>> AsAsyncEnumerable()
    {
        return AsQueryable().ToListAsync();
    }

    /// <summary>
    /// 直接返回数据库结果
    /// </summary>
    /// <param name="predicate">The predicate.</param>
    /// <returns></returns>
    public virtual Task<List<TEntity>> AsAsyncEnumerable(Expression<Func<TEntity, bool>> predicate)
    {
        return AsQueryable(predicate).ToListAsync();
    }

    /// <summary>
    /// Determines whether the specified where expression is exists.
    /// </summary>
    /// <param name="whereExpression">The where expression.</param>
    /// <returns>
    ///   <c>true</c> if the specified where expression is exists; otherwise, <c>false</c>.
    /// </returns>
    public virtual bool IsExists(Expression<Func<TEntity, bool>> whereExpression)
    {
        return Entities.Any(whereExpression);
    }

    /// <summary>
    /// Determines whether [is exists asynchronous] [the specified where expression].
    /// </summary>
    /// <param name="whereExpression">The where expression.</param>
    /// <returns></returns>
    public virtual Task<bool> IsExistsAsync(Expression<Func<TEntity, bool>> whereExpression)
    {
        return Entities.AnyAsync(whereExpression);
    }
    #endregion

    #region 仓储事务
    /// <summary>
    /// 切换仓储(注意使用环境)
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    /// <returns>
    /// 仓储
    /// </returns>
    public virtual SqlSugarRepository<T> Change<T>()
        where T : class, new()
    {
        return _serviceProvider.GetRequiredService<SqlSugarRepository<T>>();
    }
    /// <summary>
    /// 当前db
    /// </summary>
	public void CurrentBeginTran()
    {
        Ado.BeginTran();
    }
    /// <summary>
    /// 当前db
    /// </summary>
    public void CurrentCommitTran()
    {
        Ado.CommitTran();
    }
    /// <summary>
    /// 当前db
    /// </summary>
    public void CurrentRollbackTran()
    {
        Ado.RollbackTran();
    }
    /// <summary>
    /// 所有db
    /// </summary>
    public void BeginTran()
    {
        Context.BeginTran();
    }
    /// <summary>
    /// 所有db
    /// </summary>
    public void CommitTran()
    {
        Context.CommitTran();
    }
    /// <summary>
    /// 所有db
    /// </summary>
    public void RollbackTran()
    {
        Context.RollbackTran();
    }
    #endregion
}
