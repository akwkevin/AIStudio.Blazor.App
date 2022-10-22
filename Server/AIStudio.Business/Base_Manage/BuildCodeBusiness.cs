using AIStudio.Common.DI;
using AIStudio.Entity.Base_Manage;
using AIStudio.IBusiness.Base_Manage;
using AIStudio.Util;
using AIStudio.Util.Common;
using Microsoft.Extensions.Hosting;
using SqlSugar;
using System.Text;
using System.Linq;
using AIStudio.DbFactory.DataAccess;
using AIStudio.Entity.DTO.Base_Manage.InputDTO;

namespace AIStudio.Business.Base_Manage
{
    public class BuildCodeBusiness : BaseBusiness<Base_DbLink>, IBuildCodeBusiness, ITransientDependency
    {
        public BuildCodeBusiness(ISqlSugarClient db, IHostEnvironment evn)
            : base(db)
        {
            var projectPath = evn.ContentRootPath;
            var root = GetDirectory(projectPath);
            _solutionPath = root.FullName;
        }

        //找到工程根目录
        private DirectoryInfo GetDirectory(string projectPath)
        {
            //找到Client的目录或者Server的目录
            var direct = Directory.GetParent(projectPath);
            if (direct.Name == "Client" || direct.Name == "Server")//所有的可执行项目都在Client或者Server中
            {
                return Directory.GetParent(direct.FullName);
            }
            else
            {
                return Directory.GetParent(direct.FullName);
            }
        }

        private static readonly List<string> ignoreProperties =
            new List<string> { "Id", "CreateTime", "CreatorId", "CreatorName", "Deleted" };

        #region 外部接口

        /// <summary>
        /// 获取所有数据库连接
        /// </summary>
        /// <returns></returns>
        public List<Base_DbLink> GetAllDbLink()
        {
            return GetList();
        }

        /// <summary>
        /// 获取数据库所有表
        /// </summary>
        /// <param name="linkId">数据库连接Id</param>
        /// <returns></returns>
        public List<DbFactory.DataAccess.DbTableInfo> GetDbTableList(string linkId)
        {
            if (linkId.IsNullOrEmpty())
                return new List<DbFactory.DataAccess.DbTableInfo>();
            else
                return GetTheDbHelper(linkId).GetDbAllTables();
        }

        public void Build(BuildInputDTO input)
        {
            string linkId = input.linkId;
            string areaName = input.areaName;
            List<string> tables = input.tables;
            var buildTypes = input.buildTypes;
            _areaName = areaName;
            //内部成员初始化
            _dbHelper = GetTheDbHelper(linkId);
            GetDbTableList(linkId).ForEach(aTable =>
            {
                _dbTableInfoDic.Add(aTable.TableName, aTable);
            });

            tables.ForEach(aTable =>
            {
                var tableFieldInfo = _dbHelper.GetDbTableInfo(aTable);

                //实体名
                string entityName = aTable;
                //业务逻辑参数名
                string busName = $"{entityName.ToFirstLowerStr()}Bus";
                //业务逻辑变量名
                string _busName = $"_{busName}";
                List<string> selectOptionsList = new List<string>();
                List<string> listColumnsList = new List<string>();
                List<string> formColumnsList = new List<string>();
                tableFieldInfo.Where(x => !ignoreProperties.Contains(x.Name)).ToList().ForEach(aField =>
                {
                    if (_dbHelper.DbTypeStr_To_CsharpType(aField.Type) == typeof(string))
                        selectOptionsList.Add(
$"                <a-select-option key=\"{aField.Name}\">{aField.Description}</a-select-option>");
                    listColumnsList.Add(
$"  {{ title: '{aField.Description}', dataIndex: '{aField.Name}', width: '10%' }},");
                    formColumnsList.Add(
$@"        <a-form-model-item label=""{aField.Description}"" prop=""{aField.Name}"">
          <a-input v-model=""entity.{aField.Name}"" autocomplete=""off"" />
        </a-form-model-item>");
                    Dictionary<string, string> renderParamters = new Dictionary<string, string>
                    {
                        {$"%{nameof(areaName)}%",areaName },
                        {$"%{nameof(entityName)}%",entityName },
                        {$"%{nameof(busName)}%",busName },
                        {$"%{nameof(_busName)}%",_busName },
                        {$"%selectOptions%",string.Join("\r\n",selectOptionsList) },
                        {$"%listColumns%",string.Join("\r\n",listColumnsList) },
                        {$"%formColumns%",string.Join("\r\n",formColumnsList) }
                    };

                    //buildTypes,实体层=0,业务层=1,接口层=2,页面层=3
                    //实体层
                    if (buildTypes.Contains("0"))
                    {
                        BuildEntity(tableFieldInfo, aTable);
                    }
                    string tmpFileName = string.Empty;
                    string savePath = string.Empty;
                    //业务层
                    if (buildTypes.Contains("1"))
                    {
                        //接口
                        tmpFileName = "IBusiness.txt";
                        savePath = Path.Combine(
                            _solutionPath,
                            "Coldairarrow.IBusiness",
                            areaName,
                            $"I{entityName}Business.cs");
                        WriteCode(renderParamters, tmpFileName, savePath);

                        //实现
                        tmpFileName = "Business.txt";
                        savePath = Path.Combine(
                            _solutionPath,
                            "Coldairarrow.Business",
                            areaName,
                            $"{entityName}Business.cs");
                        WriteCode(renderParamters, tmpFileName, savePath);
                    }
                    //接口层
                    if (buildTypes.Contains("2"))
                    {
                        tmpFileName = "Controller.txt";
                        savePath = Path.Combine(
                            _solutionPath,
                            "Coldairarrow.Api",
                            "Controllers",
                            areaName,
                            $"{entityName}Controller.cs");
                        WriteCode(renderParamters, tmpFileName, savePath);
                    }
                    //页面层
                    if (buildTypes.Contains("3"))
                    {
                        //表格页
                        tmpFileName = "List.txt";
                        savePath = Path.Combine(
                            _solutionPath,
                            "Coldairarrow.Web",
                            "src",
                            "views",
                            areaName,
                            entityName,
                            "List.vue");
                        WriteCode(renderParamters, tmpFileName, savePath);

                        //表单页
                        tmpFileName = "EditForm.txt";
                        savePath = Path.Combine(
                            _solutionPath,
                            "Coldairarrow.Web",
                            "src",
                            "views",
                            areaName,
                            entityName,
                            "EditForm.vue");
                        WriteCode(renderParamters, tmpFileName, savePath);
                    }
                });
            });
        }

        public Dictionary<string, List<TableInfo>> GetDbTableInfo(BuildInputDTO input)
        {
            Dictionary<string, List<TableInfo>> tabInfoDictionary = new Dictionary<string, List<TableInfo>>();
            string linkId = input.linkId;
            List<string> tables = input.tables;

            //内部成员初始化
            _dbHelper = GetTheDbHelper(linkId);

            tables.ForEach(aTable =>
            {
                List<TableInfo> tableFieldInfo = _dbHelper.GetDbTableInfo(aTable);
                tabInfoDictionary.Add(aTable, tableFieldInfo);
            });

            return tabInfoDictionary;
        }

        #endregion

        #region 私有成员

        readonly string _solutionPath;
        private string _areaName { get; set; }
        private void BuildEntity(List<TableInfo> tableInfo, string tableName)
        {
            string nameSpace = $@"AIStudio.Entity.{_areaName}";
            string filePath = Path.Combine(_solutionPath, "Common", "AIStudio.Entity", _areaName, $"{tableName}.cs");

            _dbHelper.SaveEntityToFile(tableInfo, tableName, _dbTableInfoDic[tableName].Description, filePath, nameSpace);
        }
        private DbHelper GetTheDbHelper(string linkId)
        {
            var theLink = GetTheLink(linkId);
            DbHelper dbHelper = DbHelperFactory.GetDbHelper(theLink.DbType.ToEnum<DatabaseType>(), theLink.ConnectionStr);

            return dbHelper;
        }
        private Base_DbLink GetTheLink(string linkId)
        {
            Base_DbLink resObj = new Base_DbLink();
            var theModule = GetIQueryable().Where(x => x.Id == linkId).First();
            resObj = theModule ?? resObj;

            return resObj;
        }
        private DbHelper _dbHelper { get; set; }
        private Dictionary<string, DbFactory.DataAccess.DbTableInfo> _dbTableInfoDic { get; set; } = new Dictionary<string, DbFactory.DataAccess.DbTableInfo>();
        private void WriteCode(Dictionary<string, string> paramters, string templateFileName, string savePath)
        {
            string content = File.ReadAllText(Path.Combine(_solutionPath,"Server", "AIStudio.Api", "BuildCodeTemplate", templateFileName));
            paramters.ForEach(aParamter =>
            {
                content = content.Replace(aParamter.Key, aParamter.Value);
            });
            var dir = Path.GetDirectoryName(savePath);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            File.WriteAllText(savePath, content, Encoding.UTF8);
        }

        #endregion

        #region 数据模型

        #endregion
    }
}
