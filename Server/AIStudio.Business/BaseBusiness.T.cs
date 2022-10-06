using AIStudio.Entity.Base_Manage;
using AIStudio.Entity.DTO.Base_Manage.InputDTO;
using AIStudio.IBusiness;
using AIStudio.Util;
using AIStudio.Util.Common;
using SqlSugar;
using System.Data;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace AIStudio.Business
{
    /// <summary>
    /// 描述：业务处理基类
    /// </summary>
    /// <typeparam name="T">泛型约束（数据库实体）</typeparam>
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
        }

        #endregion

        #region 私有成员

        protected virtual string _valueField { get; } = "Id";
        protected virtual string _textField { get => throw new Exception("请在子类重写"); }

        #endregion

        #region 外部属性

        /// <summary>
        /// 业务仓储接口(支持软删除),支持联表操作
        /// 注：若需要访问逻辑删除的数据,请使用IDbAccessor.FullRepository
        /// 注：仅支持单线程操作
        /// </summary>
        public ISqlSugarClient Db { get; }

        public string EntityName { get; }

        #endregion

        #region 事物提交

        public (bool Success, Exception ex) RunTransaction(Action action, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            throw new Exception("暂未实现");
        }
        public async Task<(bool Success, Exception ex)> RunTransactionAsync(Func<Task> action, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            throw new Exception("暂未实现");
        }

        #endregion

        #region 增加数据

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="entity">实体对象</param>
        public int Insert(T entity)
        {
            return Db.Insertable(entity).ExecuteReturnIdentity();
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="entity">实体对象</param>
        public async Task<int> InsertAsync(T entity)
        {
            return await Db.Insertable(entity).ExecuteReturnIdentityAsync();
        }

        /// <summary>
        /// 添加多条数据
        /// </summary>
        /// <param name="entities">实体对象集合</param>
        public int Insert(List<T> entities)
        {
            return Db.Insertable(entities).ExecuteCommand();
        }

        /// <summary>
        /// 添加多条数据
        /// </summary>
        /// <param name="entities">实体对象集合</param>
        public async Task<int> InsertAsync(List<T> entities)
        {
            return await Db.Insertable(entities).ExecuteCommandAsync();
        }

        /// <summary>
        /// 添加多条数据
        /// </summary>
        /// <param name="entities">实体对象集合</param>
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
        /// <param name="entities"></param>
        public async Task BulkInsertAsync(List<T> entities)
        {
            await Db.Fastest<T>().BulkCopyAsync(entities);
        }
        #endregion

        #region 删除数据

        /// <summary>
        /// 删除所有数据
        /// </summary>
        public int DeleteAll()
        {
            return Db.Deleteable<T>().ExecuteCommand();
        }

        /// <summary>
        /// 删除所有数据
        /// </summary>
        public async Task<int> DeleteAllAsync()
        {
            return await Db.Deleteable<T>().ExecuteCommandAsync();
        }

        /// <summary>
        /// 删除指定主键数据
        /// </summary>
        /// <param name="key"></param>
        public int Delete(string key)
        {
            return Db.Deleteable<T>().In(key).ExecuteCommand();
        }

        /// <summary>
        /// 删除指定主键数据
        /// </summary>
        /// <param name="key"></param>
        public async Task<int> DeleteAsync(string key)
        {
            return await Db.Deleteable<T>().In(key).ExecuteCommandAsync();
        }

        /// <summary>
        /// 通过主键删除多条数据
        /// </summary>
        /// <param name="keys"></param>
        public int Delete(List<string> keys)
        {
            return Db.Deleteable<T>().In(keys).ExecuteCommand();
        }

        /// <summary>
        /// 通过主键删除多条数据
        /// </summary>
        /// <param name="keys"></param>
        public async Task<int> DeleteAsync(List<string> keys)
        {
            return await Db.Deleteable<T>().In(keys).ExecuteCommandAsync();
        }

        /// <summary>
        /// 删除单条数据
        /// </summary>
        /// <param name="entity">实体对象</param>
        public int Delete(T entity)
        {
            return Db.Deleteable(entity).ExecuteCommand();
        }

        /// <summary>
        /// 删除单条数据
        /// </summary>
        /// <param name="entity">实体对象</param>
        public async Task<int> DeleteAsync(T entity)
        {
            return await Db.Deleteable(entity).ExecuteCommandAsync();
        }

        /// <summary>
        /// 删除多条数据
        /// </summary>
        /// <param name="entities">实体对象集合</param>
        public int Delete(List<T> entities)
        {
            return Db.Deleteable(entities).ExecuteCommand();
        }

        /// <summary>
        /// 删除多条数据
        /// </summary>
        /// <param name="entities">实体对象集合</param>
        public async Task<int> DeleteAsync(List<T> entities)
        {
            return await Db.Deleteable(entities).ExecuteCommandAsync();
        }

        /// <summary>
        /// 删除指定条件数据
        /// </summary>
        /// <param name="condition">筛选条件</param>
        public int Delete(Expression<Func<T, bool>> condition)
        {
            return Db.Deleteable(condition).ExecuteCommand();
        }

        /// <summary>
        /// 删除指定条件数据
        /// </summary>
        /// <param name="condition">筛选条件</param>
        public async Task<int> DeleteAsync(Expression<Func<T, bool>> condition)
        {
            return await Db.Deleteable(condition).ExecuteCommandAsync();
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
            return Db.Deleteable(where).ExecuteCommand();
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
            return await Db.Deleteable(where).ExecuteCommandAsync();
        }

        #endregion

        #region 更新数据

        /// <summary>
        /// 更新一条数据
        /// </summary>
        /// <param name="entity">实体对象</param>
        public int Update(T entity)
        {
            return Db.Updateable(entity).ExecuteCommand();
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        /// <param name="entity">实体对象</param>
        public async Task<int> UpdateAsync(T entity)
        {
            return await Db.Updateable(entity).ExecuteCommandAsync();
        }

        /// <summary>
        /// 更新多条数据
        /// </summary>
        /// <param name="entities">数据列表</param>
        public int Update(List<T> entities)
        {
            return Db.Updateable(entities).ExecuteCommand();
        }

        /// <summary>
        /// 更新多条数据
        /// </summary>
        /// <param name="entities">数据列表</param>
        public async Task<int> UpdateAsync(List<T> entities)
        {
            return await Db.Updateable(entities).ExecuteCommandAsync();
        }

        /// <summary>
        /// 更新多条数据
        /// </summary>
        /// <param name="entities">数据列表</param>
        public async Task<int> UpdateAsync(List<object> entities)
        {
            return await Db.Updateable(entities.OfType<T>().ToList()).ExecuteCommandAsync();
        }

        /// <summary>
        /// 指定条件更新
        /// </summary>
        /// <param name="whereExpre">筛选表达式</param>
        /// <param name="set">更改属性回调</param>
        public int Update(Expression<Func<T, bool>> whereExpre, Action<T> set)
        {
            throw new Exception("暂未实现");
        }

        /// <summary>
        /// 指定条件更新
        /// </summary>
        /// <param name="whereExpre">筛选表达式</param>
        /// <param name="set">更改属性回调</param>
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
            return Db.Queryable<T>();
        }

        public virtual ISugarQueryable<dynamic> GetIQueryableDynamic()
        {
            return Db.Queryable<dynamic>().AS($"{EntityName}");
        }

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

        #region 业务返回

        /// <summary>
        /// 返回成功
        /// </summary>
        /// <returns></returns>
        public AjaxResult Success()
        {
            AjaxResult res = new AjaxResult
            {
                Success = true,
                Msg = "请求成功！",
            };

            return res;
        }

        /// <summary>
        /// 返回成功
        /// </summary>
        /// <param name="data">返回数据</param>
        /// <returns></returns>
        public AjaxResult<U> Success<U>(U data)
        {
            AjaxResult<U> res = new AjaxResult<U>
            {
                Success = true,
                Msg = "操作成功",
                Data = data
            };

            return res;
        }

        /// <summary>
        /// 返回成功
        /// </summary>
        /// <param name="data">返回数据</param>
        /// <param name="msg">返回消息</param>
        /// <returns></returns>
        public AjaxResult<U> Success<U>(U data, string msg)
        {
            AjaxResult<U> res = new AjaxResult<U>
            {
                Success = true,
                Msg = msg,
                Data = data
            };

            return res;
        }

        /// <summary>
        /// 返回错误
        /// </summary>
        /// <returns></returns>
        public AjaxResult Error()
        {
            AjaxResult res = new AjaxResult
            {
                Success = false,
                Msg = "请求失败！",
            };

            return res;
        }

        /// <summary>
        /// 返回错误
        /// </summary>
        /// <param name="msg">错误提示</param>
        /// <returns></returns>
        public AjaxResult Error(string msg)
        {
            AjaxResult res = new AjaxResult
            {
                Success = false,
                Msg = msg,
            };

            return res;
        }

        #endregion

        #region 其它操作

        #endregion

        #region 常规操作
        /// <summary>
        /// 全部数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<List<T>> GetDataListAsync(SearchInput input)
        {
            var q = GetIQueryable(input.SearchKeyValues);

            return await q.OrderBy($@"{input.SortField} {input.SortType} ").ToListAsync();
        }

        /// <summary>
        /// 分页数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual async Task<PageResult<T>> GetDataListAsync(PageInput input)
        {
            var q = GetIQueryable(input.SearchKeyValues);          

            return await q.GetPageResultAsync(input);
        }

        public virtual async Task<T> GetTheDataAsync(string id)
        {
            return await GetIQueryable().In(id).FirstAsync();
        }

        public virtual async Task<DTO> GetTheDataDTOAsync<DTO>(string id) where DTO : T
        {
            return await GetIQueryable().In(id).Select<DTO>().FirstAsync();
        }

        public virtual async Task AddDataAsync(T newData)
        {
            await Db.Insertable(newData).ExecuteCommandAsync();
        }

        public virtual async Task UpdateDataAsync(T theData)
        {
            await Db.Updateable(theData).ExecuteCommandAsync();
        }

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
        public IQueryable<T> GetHistoryDataQueryable(Expression<Func<T, bool>> expression, DateTime? start, DateTime? end, string dateField = "CreateTime")
        {
            throw new Exception("暂未实现");
        }
        public async Task<int> GetHistoryDataCount(Expression<Func<T, bool>> expression, DateTime? start, DateTime? end, string dateField = "CreateTime")
        {
            throw new Exception("暂未实现");
        }

        public async Task<List<T>> GetHistoryDataList(Expression<Func<T, bool>> expression, DateTime? start, DateTime? end, string dateField = "CreateTime")
        {
            throw new Exception("暂未实现");
        }

        public async Task<PageResult<T>> GetPageHistoryDataList(PageInput input, Expression<Func<T, bool>> expression, DateTime? start, DateTime? end, string dateField = "CreateTime")
        {
            throw new Exception("暂未实现");
        }

        #endregion
    }

    public static class BaseBusinessExtension
    {
        public static async Task<PageResult<T>> GetPageResult<T>(this ISugarQueryable<T> source, PageInput input)
        {
            int total = 0;
            var list = source.OrderBy($@"{input.SortField} {input.SortType} ").ToPageList(input.PageIndex, input.PageRows, ref total);
            return new PageResult<T> { Data = list, Total = total };
        }
        public static async Task<PageResult<T>> GetPageResultAsync<T>(this ISugarQueryable<T> source, PageInput input)
        {
            RefAsync<int> total = 0;
            var list = await source.OrderBy($@"{input.SortField} {input.SortType} ").ToPageListAsync(input.PageIndex, input.PageRows, total);
            return new PageResult<T> { Data = list, Total = total };
        }
    }
}
