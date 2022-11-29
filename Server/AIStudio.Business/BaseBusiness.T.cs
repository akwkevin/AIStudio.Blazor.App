﻿using AIStudio.Common.Types;
using AIStudio.Entity;
using AIStudio.Entity.Base_Manage;
using AIStudio.Entity.DTO.Base_Manage.InputDTO;
using AIStudio.IBusiness;
using AIStudio.Util;
using AIStudio.Util.Common;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using Quartz.Util;
using SqlSugar;
using StackExchange.Redis;
using System.Data;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace AIStudio.Business
{
    /// <summary>
    /// 描述：业务处理基类
    /// </summary>
    /// <typeparam name="T">泛型约束（数据库实体）</typeparam>
    /// <seealso cref="AIStudio.IBusiness.IBaseBusiness&lt;T&gt;" />
    public abstract class BaseBusiness<T> : IBaseBusiness<T> where T : class, new()
    {
        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="db">注入数据库</param>
        public BaseBusiness(ISqlSugarClient db)
        {
            Db = db;
            EntityName = typeof(T).Name;
            LogicDelete = !typeof(T).GetProperty(GlobalConst.Deleted).IsNullOrEmpty();
        }

        #endregion

        #region 私有成员

        /// <summary>
        /// Gets the value field.
        /// </summary>
        /// <value>
        /// The value field.
        /// </value>
        protected virtual string _valueField { get; } = "Id";
        /// <summary>
        /// Gets the text field.
        /// </summary>
        /// <value>
        /// The text field.
        /// </value>
        /// <exception cref="System.Exception">请在子类重写</exception>
        protected virtual string _textField { get => throw new Exception("请在子类重写"); }

        #endregion

        #region 外部属性

        /// <summary>
        /// Gets the database.
        /// </summary>
        /// <value>
        /// The database.
        /// </value>
        public ISqlSugarClient Db { get; }

        /// <summary>
        /// Gets the name of the entity.
        /// </summary>
        /// <value>
        /// The name of the entity.
        /// </value>
        public string EntityName { get; }

        /// <summary>
        /// Gets a value indicating whether [logic delete].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [logic delete]; otherwise, <c>false</c>.
        /// </value>
        public bool LogicDelete { get; }
        #endregion

        #region 事物提交

        /// <summary>
        /// Runs the transaction.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="isolationLevel">The isolation level.</param>
        /// <returns></returns>
        public (bool Success, Exception ex) RunTransaction(Action action, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            bool success = true;
            Exception resEx = null;
            try
            {
                Db.Ado.BeginTran(isolationLevel);

                action();

                Db.Ado.CommitTran();
            }
            catch (Exception ex)
            {
                success = false;
                resEx = ex;
                Db.Ado.RollbackTran();
            }
            finally
            {

            }

            return (success, resEx);
        }
        /// <summary>
        /// Runs the transaction asynchronous.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="isolationLevel">The isolation level.</param>
        /// <returns></returns>
        public async Task<(bool Success, Exception ex)> RunTransactionAsync(Func<Task> action, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            bool success = true;
            Exception resEx = null;
            try
            {
                Db.Ado.BeginTran(isolationLevel);

                await action();

                Db.Ado.CommitTran();
            }
            catch (Exception ex)
            {
                success = false;
                resEx = ex;
                Db.Ado.RollbackTran();
            }
            finally
            {

            }

            return (success, resEx);
        }
        #endregion

        #region 增加数据

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public int Insert(T entity)
        {
            return Db.Insertable(entity).ExecuteCommand();
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public async Task<int> InsertAsync(T entity)
        {
            return await Db.Insertable(entity).ExecuteCommandAsync();
        }

        /// <summary>
        /// 添加多条数据
        /// </summary>
        /// <param name="entities">实体对象集合</param>
        /// <returns></returns>
        public int Insert(List<T> entities)
        {
            return Db.Insertable(entities).ExecuteCommand();
        }

        /// <summary>
        /// 添加多条数据
        /// </summary>
        /// <param name="entities">实体对象集合</param>
        /// <returns></returns>
        public async Task<int> InsertAsync(List<T> entities)
        {
            return await Db.Insertable(entities).ExecuteCommandAsync();
        }

        /// <summary>
        /// 添加多条数据
        /// </summary>
        /// <param name="entities">实体对象集合</param>
        /// <returns></returns>
        public async Task<int> InsertAsync(List<object> entities)
        {
            return await Db.Insertable(entities.OfType<T>().ToList()).ExecuteCommandAsync();
        }

        /// <summary>
        /// 批量添加数据,速度快
        /// </summary>
        /// <param name="entities"></param>
        public void BulkInsert(List<T> entities)
        {
            Db.Fastest<T>().BulkCopy(entities);
        }

        /// <summary>
        /// 批量添加数据,速度快
        /// </summary>
        /// <param name="entities">The entities.</param>
        public async Task BulkInsertAsync(List<T> entities)
        {
            await Db.Fastest<T>().BulkCopyAsync(entities);
        }
        #endregion

        #region 删除数据

        /// <summary>
        /// 删除所有数据,这个好像没有软删除
        /// </summary>
        /// <returns></returns>
        public int DeleteAll()
        {
            return Db.Deleteable<T>().ExecuteCommand();
        }

        /// <summary>
        /// 删除所有数据,这个好像没有软删除
        /// </summary>
        /// <returns></returns>
        public async Task<int> DeleteAllAsync()
        {
            return await Db.Deleteable<T>().ExecuteCommandAsync();
        }

        /// <summary>
        /// 删除指定主键数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public int Delete(string key)
        {
            if (LogicDelete)//软删除
            {
                return Db.Deleteable<T>().In(key).IsLogic().ExecuteCommand(GlobalConst.Deleted);
            }
            else
            { 
                return Db.Deleteable<T>().In(key).ExecuteCommand();
            }
        }

        /// <summary>
        /// 删除指定主键数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(string key)
        {
            if (LogicDelete)//软删除
            {
                return await Db.Deleteable<T>().In(key).IsLogic().ExecuteCommandAsync(GlobalConst.Deleted);
            }
            else
            {
                return await Db.Deleteable<T>().In(key).ExecuteCommandAsync();
            }
        }

        /// <summary>
        /// 通过主键删除多条数据
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public int Delete(List<string> keys)
        {
            if (LogicDelete)//软删除
            {
                return Db.Deleteable<T>().In(keys).IsLogic().ExecuteCommand(GlobalConst.Deleted);
            }
            else
            {
                return Db.Deleteable<T>().In(keys).ExecuteCommand();
            }
        }

        /// <summary>
        /// 通过主键删除多条数据
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(List<string> keys)
        {
            if (LogicDelete)//软删除
            {
                return await Db.Deleteable<T>().In(keys).IsLogic().ExecuteCommandAsync(GlobalConst.Deleted);
            }
            else
            {
                return await Db.Deleteable<T>().In(keys).ExecuteCommandAsync();
            }
        }

        /// <summary>
        /// 删除单条数据
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public int Delete(T entity)
        {
            if (LogicDelete)//软删除
            {
                return Db.Deleteable(entity).IsLogic().ExecuteCommand(GlobalConst.Deleted);
            }
            else
            {
                return Db.Deleteable(entity).ExecuteCommand();
            }
        }

        /// <summary>
        /// 删除单条数据
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(T entity)
        {
            if (LogicDelete)//软删除
            {
                return await Db.Deleteable(entity).IsLogic().ExecuteCommandAsync(GlobalConst.Deleted);
            }
            else
            {
                return await Db.Deleteable(entity).ExecuteCommandAsync();
            }
        }

        /// <summary>
        /// 删除多条数据
        /// </summary>
        /// <param name="entities">实体对象集合</param>
        /// <returns></returns>
        public int Delete(List<T> entities)
        {
            if (LogicDelete)//软删除
            {
                return Db.Deleteable(entities).IsLogic().ExecuteCommand(GlobalConst.Deleted);
            }
            else
            {
                return Db.Deleteable(entities).ExecuteCommand();
            }
        }

        /// <summary>
        /// 删除多条数据
        /// </summary>
        /// <param name="entities">实体对象集合</param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(List<T> entities)
        {
            if (LogicDelete)//软删除
            {
                return await Db.Deleteable(entities).IsLogic().ExecuteCommandAsync(GlobalConst.Deleted);
            }
            else
            {
                return await Db.Deleteable(entities).ExecuteCommandAsync();
            }
        }

        /// <summary>
        /// 删除指定条件数据
        /// </summary>
        /// <param name="condition">筛选条件</param>
        /// <returns></returns>
        public int Delete(Expression<Func<T, bool>> condition)
        {
            if (LogicDelete)//软删除
            {
                return Db.Deleteable(condition).IsLogic().ExecuteCommand(GlobalConst.Deleted);
            }
            else
            {
                return Db.Deleteable(condition).ExecuteCommand();
            }
        }

        /// <summary>
        /// 删除指定条件数据
        /// </summary>
        /// <param name="condition">筛选条件</param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(Expression<Func<T, bool>> condition)
        {
            if (LogicDelete)//软删除
            {
                return await Db.Deleteable(condition).IsLogic().ExecuteCommandAsync(GlobalConst.Deleted);
            }
            else
            {
                return await Db.Deleteable(condition).ExecuteCommandAsync();
            }
        }

        /// <summary>
        /// 使用SQL语句按照条件删除数据
        /// 用法:Delete_Sql"Base_User"(x=&gt;x.Id == "Admin")
        /// 注：生成的SQL类似于DELETE FROM [Base_User] WHERE [Name] = 'xxx' WHERE [Id] = 'Admin'
        /// </summary>
        /// <param name="where">条件</param>
        /// <returns>
        /// 影响条数
        /// </returns>
        public int DeleteSql(Expression<Func<T, bool>> where)
        {
            if (LogicDelete)//软删除
            {
                return Db.Deleteable(where).IsLogic().ExecuteCommand(GlobalConst.Deleted);
            }
            else
            {
                return Db.Deleteable(where).ExecuteCommand();
            }
        }

        /// <summary>
        /// 使用SQL语句按照条件删除数据
        /// 用法:Delete_Sql"Base_User"(x=&gt;x.Id == "Admin")
        /// 注：生成的SQL类似于DELETE FROM [Base_User] WHERE [Name] = 'xxx' WHERE [Id] = 'Admin'
        /// </summary>
        /// <param name="where">条件</param>
        /// <returns>
        /// 影响条数
        /// </returns>
        public async Task<int> DeleteSqlAsync(Expression<Func<T, bool>> where)
        {
            if (LogicDelete)//软删除
            {
                return await Db.Deleteable(where).IsLogic().ExecuteCommandAsync(GlobalConst.Deleted);
            }
            else
            {
                return await Db.Deleteable(where).ExecuteCommandAsync();
            }
        }

        #endregion

        #region 更新数据

        /// <summary>
        /// 更新一条数据
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public int Update(T entity)
        {
            return Db.Updateable(entity).ExecuteCommand();
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(T entity)
        {
            return await Db.Updateable(entity).ExecuteCommandAsync();
        }

        /// <summary>
        /// 更新多条数据
        /// </summary>
        /// <param name="entities">数据列表</param>
        /// <returns></returns>
        public int Update(List<T> entities)
        {
            return Db.Updateable(entities).ExecuteCommand();
        }

        /// <summary>
        /// 更新多条数据
        /// </summary>
        /// <param name="entities">数据列表</param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(List<T> entities)
        {
            return await Db.Updateable(entities).ExecuteCommandAsync();
        }

        /// <summary>
        /// 更新多条数据
        /// </summary>
        /// <param name="entities">数据列表</param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(List<object> entities)
        {
            return await Db.Updateable(entities.OfType<T>().ToList()).ExecuteCommandAsync();
        }

        /// <summary>
        /// 指定条件更新
        /// </summary>
        /// <param name="whereExpre">筛选表达式</param>
        /// <param name="set">更改属性回调</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">暂未实现</exception>
        public int Update(Expression<Func<T, bool>> whereExpre, Action<T> set)
        {
            throw new Exception("暂未实现");
        }

        /// <summary>
        /// 指定条件更新
        /// </summary>
        /// <param name="whereExpre">筛选表达式</param>
        /// <param name="set">更改属性回调</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">暂未实现</exception>
        public async Task<int> UpdateAsync(Expression<Func<T, bool>> whereExpre, Action<T> set)
        {
            throw new Exception("暂未实现");
        }
        #endregion

        #region 查询数据
        /// <summary>
        /// 根据lambda表达式条件获取单个实体
        /// </summary>
        /// <param name="predicate">lambda表达式条件</param>
        /// <returns></returns>
        public T FirstOrDefault(Expression<Func<T, bool>> predicate)
        {
            return Db.Queryable<T>().First(predicate);
        }

        /// <summary>
        /// 根据lambda表达式条件获取单个实体
        /// </summary>
        /// <param name="predicate">lambda表达式条件</param>
        /// <returns></returns>
        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await Db.Queryable<T>().FirstAsync(predicate);
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        public T GetEntity(params object[] keyValue)
        {
            return Db.Queryable<T>().In(keyValue).First();
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        public async Task<T> GetEntityAsync(params object[] keyValue)
        {
            return await Db.Queryable<T>().In(keyValue).FirstAsync();
        }

        /// <summary>
        /// 获取表的所有数据，当数据量很大时不要使用！
        /// </summary>
        /// <returns></returns>
        public List<T> GetList()
        {
            return Db.Queryable<T>().ToList();
        }

        /// <summary>
        /// 获取表的所有数据，当数据量很大时不要使用！
        /// </summary>
        /// <returns></returns>
        public async Task<List<T>> GetListAsync()
        {
            return await Db.Queryable<T>().ToListAsync();
        }

        /// <summary>
        /// 获取实体对应的表，延迟加载，主要用于支持Linq查询操作
        /// </summary>
        /// <returns></returns>
        public virtual ISugarQueryable<T> GetIQueryable()
        {
            return Db.Queryable<T>().WithCache();
        }

        /// <summary>
        /// Gets the i queryable dynamic.
        /// </summary>
        /// <returns></returns>
        public virtual ISugarQueryable<dynamic> GetIQueryableDynamic()
        {
            return Db.Queryable<dynamic>().AS($"{EntityName}").WithCache();
        }

        /// <summary>
        /// Gets the i queryable.
        /// </summary>
        /// <param name="searchKeyValues">The search key values.</param>
        /// <returns></returns>
        public virtual ISugarQueryable<T> GetIQueryable(Dictionary<string, object> searchKeyValues)
        {
            var q = GetIQueryable();
            //按字典筛选
            if (searchKeyValues != null)
            {
                foreach (var keyValuePair in searchKeyValues.Where(p => !string.IsNullOrEmpty(p.Key) && !string.IsNullOrEmpty(p.Value?.ToString())))
                {
                    var newWhere = DynamicExpressionParser.ParseLambda<T, bool>(
                        ParsingConfig.Default, false, $@"{keyValuePair.Key}.Contains(@0)", keyValuePair.Value);
                    q = q.Where(newWhere);
                }
            }

            return q;
        }
        #endregion

        #region 执行Sql语句

        #endregion

        #region 其它操作

        #endregion

        #region 常规操作
        /// <summary>
        /// 全部数据
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public async Task<List<T>> GetDataListAsync(SearchInput input)
        {
            var q = GetIQueryable(input.SearchKeyValues);

            return await q.OrderBy($@"{input.SortField} {input.SortType} ").ToListAsync();
        }

        /// <summary>
        /// 分页数据
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public virtual async Task<PageResult<T>> GetDataListAsync(PageInput input)
        {
            var q = GetIQueryable(input.SearchKeyValues);

            return await q.GetPageResultAsync(input);
        }

        /// <summary>
        /// Gets the data asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public virtual async Task<T> GetTheDataAsync(string id)
        {
            return await GetIQueryable().In(id).FirstAsync();
        }

        /// <summary>
        /// Gets the data dto asynchronous.
        /// </summary>
        /// <typeparam name="DTO">The type of to.</typeparam>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public virtual async Task<DTO> GetTheDataDTOAsync<DTO>(string id) where DTO : T
        {
            return await GetIQueryable().In(id).Select<DTO>().FirstAsync();
        }

        /// <summary>
        /// Adds the data asynchronous.
        /// </summary>
        /// <param name="newData">The new data.</param>
        public virtual async Task AddDataAsync(T newData)
        {
            await Db.Insertable(newData).ExecuteCommandAsync();
        }

        /// <summary>
        /// Updates the data asynchronous.
        /// </summary>
        /// <param name="theData">The data.</param>
        public virtual async Task UpdateDataAsync(T theData)
        {
            await Db.Updateable(theData).ExecuteCommandAsync();
        }

        /// <summary>
        /// Saves the data asynchronous.
        /// </summary>
        /// <param name="theData">The data.</param>
        public virtual async Task SaveDataAsync(T theData)
        {
            if (theData.GetPropertyValue(GlobalConst.Id).IsNullOrEmpty())
            {
                await AddDataAsync(theData);
            }
            else
            {
                await UpdateDataAsync(theData);
            }
        }

        /// <summary>
        /// Deletes the data asynchronous.
        /// </summary>
        /// <param name="ids">The ids.</param>
        public virtual async Task DeleteDataAsync(List<string> ids)
        {
            await Db.Deleteable<T>().In(ids).ExecuteCommandAsync();
        }
        #endregion

        #region 选项
        /// <summary>
        /// 构建前端Select远程搜索数据
        /// </summary>
        /// <param name="input">查询参数</param>
        /// <returns></returns>
        public async Task<List<SelectOption>> GetOptionListAsync(PageInput input)
        {
            return await GetOptionListAsync(input, _textField, _valueField, null);
        }

        /// <summary>
        /// 构建前端Select远程搜索数据
        /// </summary>
        /// <param name="input">查询参数</param>
        /// <param name="textFiled">文本字段</param>
        /// <param name="valueField">值字段</param>
        /// <param name="source">指定数据源</param>
        /// <returns></returns>
        public async Task<List<SelectOption>> GetOptionListAsync(PageInput input, string textFiled, string valueField, ISugarQueryable<T> source = null)
        {
            var q = GetNewQ();
            //按字典筛选
            if (input.SearchKeyValues != null)
            {
                foreach (var keyValuePair in input.SearchKeyValues.Where(p => !string.IsNullOrEmpty(p.Key) && !string.IsNullOrEmpty(p.Value?.ToString())))
                {
                    var newWhere = DynamicExpressionParser.ParseLambda<T, bool>(
                        ParsingConfig.Default, false, $@"{keyValuePair.Key}.Contains(@0)", keyValuePair.Value);
                    q = q.Where(newWhere);
                }
            }

            RefAsync<int> total = 0;
            var resList = (await q.ToPageListAsync(input.PageIndex, input.PageRows, total)).Select(x => new SelectOption()
            {
                Value = x.GetPropertyValue(valueField)?.ToString(),
                Text = x.GetPropertyValue(textFiled)?.ToString()
            }).ToList();

            return resList;

            ISugarQueryable<T> GetNewQ()
            {
                return source ?? GetIQueryable();
            }
        }
        #endregion

        #region 历史数据查询
        /// <summary>
        /// Gets the data list asynchronous.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="dateField">The date field.</param>
        /// <returns></returns>
        public async Task<List<T>> GetDataListAsync(SearchInput<HistorySearch> input, string dateField = GlobalConst.CreateTime)
        {
            var q = GetIQueryable(input.SearchKeyValues);

            if (input.Search.StartTime != null && input.Search.EndTime != null)
            {
                var newWhere = DynamicExpressionParser.ParseLambda<T, bool>(
                      ParsingConfig.Default, false, $@"{dateField} > @0 && {dateField} < @1", new object[] { input.Search.StartTime.Value, input.Search.EndTime.Value });
                q = q.Where(newWhere);
            }
            else if (input.Search.StartTime == null && input.Search.EndTime != null)
            {
                var newWhere = DynamicExpressionParser.ParseLambda<T, bool>(
                    ParsingConfig.Default, false, $@"{dateField} < @0", new object[] { input.Search.EndTime.Value });
                q = q.Where(newWhere);
            }
            else if (input.Search.StartTime != null && input.Search.EndTime == null)
            {
                var newWhere = DynamicExpressionParser.ParseLambda<T, bool>(
                    ParsingConfig.Default, false, $@"{dateField} > @0", new object[] { input.Search.StartTime.Value });
                q = q.Where(newWhere);
            }

            return await q.OrderBy($@"{input.SortField} {input.SortType} ").ToListAsync();
        }
        /// <summary>
        /// Gets the data list asynchronous.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="dateField">The date field.</param>
        /// <returns></returns>
        public async Task<PageResult<T>> GetDataListAsync(PageInput<HistorySearch> input, string dateField = GlobalConst.CreateTime)
        {
            input.SortField = dateField;
            input.SortType = "desc";

            var q = GetIQueryable(input.SearchKeyValues);

            if (input.Search.StartTime != null && input.Search.EndTime != null)
            {
                var newWhere = DynamicExpressionParser.ParseLambda<T, bool>(
                      ParsingConfig.Default, false, $@"{dateField} > @0 && {dateField} < @1", new object[] { input.Search.StartTime.Value, input.Search.EndTime.Value });
                q = q.Where(newWhere);
            }
            else if (input.Search.StartTime == null && input.Search.EndTime != null)
            {
                var newWhere = DynamicExpressionParser.ParseLambda<T, bool>(
                    ParsingConfig.Default, false, $@"{dateField} < @0", new object[] { input.Search.EndTime.Value });
                q = q.Where(newWhere);
            }
            else if (input.Search.StartTime != null && input.Search.EndTime == null)
            {
                var newWhere = DynamicExpressionParser.ParseLambda<T, bool>(
                    ParsingConfig.Default, false, $@"{dateField} > @0", new object[] { input.Search.StartTime.Value });
                q = q.Where(newWhere);
            }

            return await q.GetPageResultAsync(input);
        }

        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public static class BaseBusinessExtension
    {
        /// <summary>
        /// Gets the page result.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static async Task<PageResult<T>> GetPageResult<T>(this ISugarQueryable<T> source, PageInput input)
        {
            int total = 0;
            var list = source.OrderBy($@"{input.SortField} {input.SortType} ").ToPageList(input.PageIndex, input.PageRows, ref total);
            return new PageResult<T> { Data = list, Total = total };
        }
        /// <summary>
        /// Gets the page result asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static async Task<PageResult<T>> GetPageResultAsync<T>(this ISugarQueryable<T> source, PageInput input)
        {
            RefAsync<int> total = 0;
            var list = await source.OrderBy($@"{input.SortField} {input.SortType} ").ToPageListAsync(input.PageIndex, input.PageRows, total);
            return new PageResult<T> { Data = list, Total = total };
        }
    }
}
