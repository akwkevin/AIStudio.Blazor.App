﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
namespace SqlSugar
{
    public class ConnectionConfig
    {
        /// <summary>
        ///Connection unique code
        /// </summary>
        public dynamic ConfigId { get; set; }
        /// <summary>
        ///DbType.SqlServer Or Other
        /// </summary>
        public DbType DbType { get; set; }
        /// <summary>
        ///Database Connection string
        /// </summary>
        public string ConnectionString { get; set; }
        /// <summary>
        /// true does not need to close the connection
        /// </summary>
        public bool IsAutoCloseConnection { get; set; }
        /// <summary>
        /// Default Attribute 
        /// </summary>
        public InitKeyType InitKeyType = InitKeyType.Attribute;
        /// <summary>
        /// Exception prompt language  
        /// </summary>
        public LanguageType  LanguageType { get=>ErrorMessage.SugarLanguageType; set=>ErrorMessage.SugarLanguageType=value; }
        /// <summary>
        ///If true, there is only one connection instance in the same thread within the same connection string
        //[Obsolete("use  SqlSugar.Ioc")]
        ///// </summary>
        //public bool IsShardSameThread { get; set; }
        /// <summary>
        /// Configure External Services replace default services,For example, Redis storage
        /// </summary>
        [JsonIgnore]
        public ConfigureExternalServices ConfigureExternalServices = new ConfigureExternalServices();
        /// <summary>
        /// If SlaveConnectionStrings has value,ConnectionString is write operation, SlaveConnectionStrings is read operation.
        /// All operations within a transaction is ConnectionString
        /// </summary>
        public List<SlaveConnectionConfig> SlaveConnectionConfigs { get; set; }
        /// <summary>
        /// More Gobal Settings
        /// </summary>
        public ConnMoreSettings MoreSettings { get; set; }
        ///// <summary>
        ///// Used for debugging errors or BUG,Used for debugging, which has an impact on Performance
        ///// </summary>
        //public SugarDebugger Debugger { get; set; }

        public string IndexSuffix { get; set; }

        [JsonIgnore]
        public AopEvents AopEvents { get;set; }
        /// <summary>
        /// 
        /// </summary>
        public SqlMiddle SqlMiddle { get;  set; }
    }
    public class SqlMiddle
    {
        public bool? IsSqlMiddle { get; set; }
        public Func<string, SugarParameter[], object> GetScalar { get; set; } = (s,p) => throw new Exception("SqlMiddle.GetScalar is null");
        public Func<string, SugarParameter[],int> ExecuteCommand { get; set; } = (s, p) => throw new Exception("SqlMiddle.ExecuteCommand is null");
        public Func<string, SugarParameter[],IDataReader> GetDataReader { get; set; } = (s, p) => throw new Exception("SqlMiddle.GetDataReader is null");
        public Func<string, SugarParameter[],DataSet> GetDataSetAll { get; set; } = (s, p) => throw new Exception("SqlMiddle.GetDataSetAll is null");
        public Func<string, SugarParameter[], Task<object>> GetScalarAsync { get; set; } = (s, p) => throw new Exception("SqlMiddle.GetScalarAsync is null");
        public Func<string, SugarParameter[], Task<int>> ExecuteCommandAsync { get; set; } = (s, p) => throw new Exception("SqlMiddle.ExecuteCommandAsync is null");
        public Func<string, SugarParameter[], Task<IDataReader>> GetDataReaderAsync { get; set; } = (s, p) => throw new Exception("SqlMiddle.GetDataReaderAsync is null");
        public Func<string, SugarParameter[], Task<DataSet>> GetDataSetAllAsync { get; set; } = (s, p) => throw new Exception("SqlMiddle.GetDataSetAllAsync is null");

    }
    public class AopEvents
    {
        public Action<DiffLogModel> OnDiffLogEvent { get; set; }
        public Action<SqlSugarException> OnError { get; set; }
        public Action<string, SugarParameter[]> OnLogExecuting { get; set; }
        public Action<string, SugarParameter[]> OnLogExecuted { get; set; }
        public Func<string, SugarParameter[], KeyValuePair<string, SugarParameter[]>> OnExecutingChangeSql { get; set; }
        public  Action<object, DataFilterModel> DataExecuting { get; set; }
        public Action<object, DataAfterModel> DataExecuted { get;  set; }
    }
    public class ConfigureExternalServices
    {

        private ISerializeService _SerializeService;
        private ICacheService _ReflectionInoCache;
        private ICacheService _DataInfoCache;
        private IRazorService _RazorService;
        public ISplitTableService SplitTableService { get; set; }
        public IRazorService RazorService
        {
            get
            {
                if (_RazorService == null)
                    return _RazorService;
                else
                    return _RazorService;
            }
            set { _RazorService = value; }
        }

        public ISerializeService SerializeService
        {
            get
            {
                if (_SerializeService == null)
                    return DefaultServices.Serialize;
                else
                    return _SerializeService;
            }
            set{ _SerializeService = value;}
        }

        public ICacheService ReflectionInoCacheService
        {
            get
            {
                if (_ReflectionInoCache == null)
                    return DefaultServices.ReflectionInoCache;
                else
                    return _ReflectionInoCache;
            }
            set{_ReflectionInoCache = value;}
        }

        public ICacheService DataInfoCacheService
        {
            get
            {
                if (_DataInfoCache == null)
                    return DefaultServices.DataInoCache;
                else
                    return _DataInfoCache;
            }
            set { _DataInfoCache = value; }
        }

        public List<SqlFuncExternal> SqlFuncServices { get; set; }
        public List<KeyValuePair<string, CSharpDataType>> AppendDataReaderTypeMappings { get;  set; }


        public Action<PropertyInfo, EntityColumnInfo> EntityService{ get; set; }
        public Action<Type,EntityInfo> EntityNameService { get; set; }
    }
}
