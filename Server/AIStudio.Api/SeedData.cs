using AIStudio.Business.Quartz_Manage;
using AIStudio.Common.AppSettings;
using AIStudio.Common.IdGenerator;
using AIStudio.Common.Quartz;
using AIStudio.Entity;
using AIStudio.Entity.Base_Manage;
using AIStudio.Entity.OA_Manage;
using AIStudio.Entity.Quartz_Manage;
using AIStudio.IBusiness.Base_Manage;
using AIStudio.Util;

namespace AIStudio.Api
{
    /// <summary>
    /// 初始化数据
    /// </summary>
    public class SeedData
    {
        /// <summary>
        /// 开始初始化
        /// </summary>
        /// <param name="provider"></param>
        public static void EnsureSeedData(IServiceProvider provider)
        {
            var logger = provider.GetRequiredService<ILogger<SeedData>>();

            var configuration = provider.GetRequiredService<IConfiguration>();

            var dbList = AppSettingsConfig.ConnectionStringsOptions.DbConfigs;

            var dbLinkBusiness = provider.GetRequiredService<IBase_DbLinkBusiness>();
            var dbLinkBusinessCount = dbLinkBusiness.GetIQueryable().Count();
            if (dbLinkBusinessCount == 0)
            {
                List<Base_DbLink> dblinks = new List<Base_DbLink>();
                foreach (var item in dbList)
                {
                    var dblink = new Base_DbLink()
                    {
                        Id = IdHelper.GetId(),
                        LinkName = item.DbName,
                        ConnectionStr = item.DbString,
                        DbType = item.DbType,
                        CreateTime = DateTime.Now,
                        CreatorId = "System",
                        CreatorName = "System",
                    };
                    dblinks.Add(dblink);                  
                }
                var result = dbLinkBusiness.Insert(dblinks);
                logger.LogTrace($"Base_DbLink created");
            }

            var roleBusiness = provider.GetRequiredService<IBase_RoleBusiness>();
            var admin = roleBusiness.FirstOrDefault(p => p.RoleName == RoleTypes.部门管理员.ToString());
            if (admin == null)
            {
                admin = new Base_Role
                {
                    Id = IdHelper.GetId(),
                    RoleName = RoleTypes.部门管理员.ToString(),
                    CreateTime = DateTime.Now,
                    CreatorId = "System",
                    CreatorName = "System",
                };
                var result = roleBusiness.Insert(admin);

                logger.LogTrace($"Base_Role:{admin.RoleName} created");
            }

            var superadmin = roleBusiness.FirstOrDefault(p => p.RoleName == RoleTypes.超级管理员.ToString());
            if (superadmin == null)
            {
                superadmin = new Base_Role
                {
                    Id = IdHelper.GetId(),
                    RoleName = RoleTypes.超级管理员.ToString(),
                    CreateTime = DateTime.Now,
                    CreatorId = "System",
                    CreatorName = "System",
                };
                var result = roleBusiness.Insert(superadmin);

                logger.LogTrace($"Base_Role:{admin.RoleName} created");
            }

            var departmentBussiness = provider.GetRequiredService<IBase_DepartmentBusiness>();
            var departmentCount = departmentBussiness.GetIQueryable().Count();
            if (departmentCount == 0)
            {
                List<Base_Department> departments = new List<Base_Department>()
                {
                    new Base_Department(){  Id="Id1", Name="Wpf控件公司", ParentId = null, CreateTime=DateTime.Now, CreatorId = "System", CreatorName = "System",},
                    new Base_Department(){  Id="Id2", Name="UI部门", ParentId="Id1", CreateTime=DateTime.Now, CreatorId = "System", CreatorName = "System",},
                    new Base_Department(){  Id="Id2_1", Name="UI子部门1", ParentId="Id2", CreateTime=DateTime.Now, CreatorId = "System", CreatorName = "System",},
                    new Base_Department(){  Id="Id2_2", Name="UI子部门2", ParentId="Id2", CreateTime=DateTime.Now, CreatorId = "System", CreatorName = "System",},
                    new Base_Department(){  Id="Id3", Name="C#部门", ParentId="Id1", CreateTime=DateTime.Now, CreatorId = "System", CreatorName = "System",},
                    new Base_Department(){  Id="Id3_1", Name="C#子部门1", ParentId="Id3", CreateTime=DateTime.Now, CreatorId = "System", CreatorName = "System",},
                    new Base_Department(){  Id="Id3_2", Name="C#子部门2", ParentId="Id3", CreateTime=DateTime.Now, CreatorId = "System", CreatorName = "System",},
                };

                var result = departmentBussiness.Insert(departments);
                logger.LogTrace("Base_Department created");
            }

            var userBusiness = provider.GetRequiredService<IBase_UserBusiness>();
            var userroleBusiness = provider.GetRequiredService<IBase_UserRoleBusiness>();

            var adminUser = userBusiness.FirstOrDefault(p => p.UserName == "Admin");
            if (adminUser == null)
            {
                adminUser = new Base_User
                {
                    Id = "Admin",
                    UserName = "Admin",
                    RealName = "Admin",
                    Password = "Admin".ToMD5String(),
                    DepartmentId = "Id1",
                    CreateTime = DateTime.Now,
                    CreatorId = "System",
                    CreatorName = "System",
                };
                var result = userBusiness.Insert(adminUser);

                var adminUserRole = new Base_UserRole
                {
                    Id = IdHelper.GetId(),
                    UserId = adminUser.Id,
                    RoleId = superadmin.Id,
                    CreateTime = DateTime.Now,
                    CreatorId = "System",
                    CreatorName = "System",
                };
                var result2 = userroleBusiness.Insert(adminUserRole);

                logger.LogTrace($"Base_User:{adminUser.UserName} created");
            }

            //alice ,123456,
            var alice = userBusiness.FirstOrDefault(p => p.UserName == "alice");
            if (alice == null)
            {
                alice = new Base_User
                {
                    Id = IdHelper.GetId(),
                    UserName = "alice",
                    RealName = "alice",
                    Password = "123456".ToMD5String(),
                    DepartmentId = "Id2",
                    CreateTime = DateTime.Now,
                    CreatorId = "System",
                    CreatorName = "System",
                };
                var result = userBusiness.Insert(alice);

                var aliceUserRole = new Base_UserRole
                {
                    Id = IdHelper.GetId(),
                    UserId = adminUser.Id,
                    RoleId = alice.Id,
                    CreateTime = DateTime.Now,
                    CreatorId = "System",
                    CreatorName = "System",
                };
                var result2 = userroleBusiness.Insert(aliceUserRole);

                logger.LogTrace($"Base_User:{alice.UserName} created");
            }

            //bob ,123456,
            var bob = userBusiness.FirstOrDefault(p => p.UserName == "bob");
            if (bob == null)
            {
                bob = new Base_User
                {
                    Id = IdHelper.GetId(),
                    UserName = "bob",
                    RealName = "bob",
                    Password = "123456".ToMD5String(),
                    DepartmentId = "Id3",
                    CreateTime = DateTime.Now,
                    CreatorId = "System",
                    CreatorName = "System",
                };
                var result = userBusiness.Insert(bob);

                logger.LogTrace($"Base_User:{bob.UserName} created");
            }

            var actionBusiness = provider.GetRequiredService<IBase_ActionBusiness>();
            var actionBusinessCount = actionBusiness.GetIQueryable().Count();
            if (actionBusinessCount == 0)
            {
                List<Base_Action> actions = new List<Base_Action>()
                {
                    new Base_Action(){ Id="Id0",Deleted = false, ParentId=null,    Type = ActionType.菜单, Name="首页",     Url=null,                               Value=null,                    NeedAction=true,    Icon="home",           Sort=0, CreateTime=DateTime.Now, CreatorId = "System", CreatorName = "System",},
                    new Base_Action(){ Id="Id0_1",Deleted = false, ParentId="Id0", Type = ActionType.页面, Name="框架介绍", Url="/Home/Introduce",                  Value=null,                    NeedAction=false,   Icon=null,             Sort=1, CreateTime=DateTime.Now, CreatorId = "System", CreatorName = "System",},
                    new Base_Action(){ Id="Id0_2",Deleted = false, ParentId="Id0", Type = ActionType.页面, Name="运营统计", Url="/Home/Statis",                     Value=null,                    NeedAction=false,   Icon=null,             Sort=2, CreateTime=DateTime.Now, CreatorId = "System", CreatorName = "System",},
               
                    new Base_Action(){ Id="Id1",Deleted = false, ParentId=null,    Type = ActionType.菜单, Name="系统管理", Url=null,                               Value=null,                    NeedAction=true,    Icon="setting",        Sort=1, CreateTime=DateTime.Now, CreatorId = "System", CreatorName = "System",},
                    new Base_Action(){ Id="Id1_1",Deleted = false, ParentId="Id1", Type = ActionType.页面, Name="权限管理", Url="/Base_Manage/Base_Action/List",    Value=null,                    NeedAction=true,    Icon="lock",           Sort=1, CreateTime=DateTime.Now, CreatorId = "System", CreatorName = "System",},
                    new Base_Action(){ Id="Id1_2",Deleted = false, ParentId="Id1", Type = ActionType.页面, Name="密钥管理", Url="/Base_Manage/Base_AppSecret/List", Value=null,                    NeedAction=true,    Icon="key",            Sort=2, CreateTime=DateTime.Now, CreatorId = "System", CreatorName = "System",},
                    new Base_Action(){ Id="Id1_3",Deleted = false, ParentId="Id1", Type = ActionType.页面, Name="用户管理", Url="/Base_Manage/Base_User/List",      Value=null,                    NeedAction=true,    Icon="user-add",       Sort=3, CreateTime=DateTime.Now, CreatorId = "System", CreatorName = "System",},
                    new Base_Action(){ Id="Id1_4",Deleted = false, ParentId="Id1", Type = ActionType.页面, Name="角色管理", Url="/Base_Manage/Base_Role/List",      Value=null,                    NeedAction=true,    Icon="safety",         Sort=4, CreateTime=DateTime.Now, CreatorId = "System", CreatorName = "System",},
                    new Base_Action(){ Id="Id1_5",Deleted = false, ParentId="Id1", Type = ActionType.页面, Name="部门管理", Url="/Base_Manage/Base_Department/List",Value=null,                    NeedAction=true,    Icon="usergroup-add",  Sort=5, CreateTime=DateTime.Now, CreatorId = "System", CreatorName = "System",},
                    new Base_Action(){ Id="Id1_6",Deleted = false, ParentId="Id1", Type = ActionType.页面, Name="字典管理", Url="/Base_Manage/Base_Dictionary/List",Value=null,                    NeedAction=true,    Icon="edit",           Sort=6, CreateTime=DateTime.Now, CreatorId = "System", CreatorName = "System",},
                    new Base_Action(){ Id="Id1_7",Deleted = false, ParentId="Id1", Type = ActionType.页面, Name="表单配置", Url="/Base_Manage/Base_CommonFormConfig/List",Value=null,              NeedAction=true,    Icon="form",           Sort=7, CreateTime=DateTime.Now, CreatorId = "System", CreatorName = "System",},

                    new Base_Action(){ Id="Id1_9",Deleted = false, ParentId="Id1", Type = ActionType.页面, Name="任务管理", Url="/Quartz_Manage/Quartz_Task/List",  Value=null,                    NeedAction=true,    Icon="calendar",       Sort=9, CreateTime=DateTime.Now, CreatorId = "System", CreatorName = "System",},
                   
                    new Base_Action(){ Id="Id1_1_1",Deleted = false, ParentId="Id1_1", Type = ActionType.权限, Name="增",   Url=null,                               Value="Base_Action.Add",       NeedAction=true,    Icon=null,             Sort=1, CreateTime=DateTime.Now, CreatorId = "System", CreatorName = "System",},
                    new Base_Action(){ Id="Id1_1_2",Deleted = false, ParentId="Id1_1", Type = ActionType.权限, Name="改",   Url=null,                               Value="Base_Action.Edit",      NeedAction=true,    Icon=null,             Sort=1, CreateTime=DateTime.Now, CreatorId = "System", CreatorName = "System",},
                    new Base_Action(){ Id="Id1_1_3",Deleted = false, ParentId="Id1_1", Type = ActionType.权限, Name="删",   Url=null,                               Value="Base_Action.Delete",    NeedAction=true,    Icon=null,             Sort=1, CreateTime=DateTime.Now, CreatorId = "System", CreatorName = "System",},
                    new Base_Action(){ Id="Id1_2_1",Deleted = false, ParentId="Id1_2", Type = ActionType.权限, Name="增",   Url=null,                               Value="Base_AppSecret.Add",    NeedAction=true,    Icon=null,             Sort=1, CreateTime=DateTime.Now, CreatorId = "System", CreatorName = "System",},
                    new Base_Action(){ Id="Id1_2_2",Deleted = false, ParentId="Id1_2", Type = ActionType.权限, Name="改",   Url=null,                               Value="Base_AppSecret.Edit",   NeedAction=true,    Icon=null,             Sort=1, CreateTime=DateTime.Now, CreatorId = "System", CreatorName = "System",},
                    new Base_Action(){ Id="Id1_2_3",Deleted = false, ParentId="Id1_2", Type = ActionType.权限, Name="删",   Url=null,                               Value="Base_AppSecret.Delete", NeedAction=true,    Icon=null,             Sort=1, CreateTime=DateTime.Now, CreatorId = "System", CreatorName = "System",},
                    new Base_Action(){ Id="Id1_3_1",Deleted = false, ParentId="Id1_3", Type = ActionType.权限, Name="增",   Url=null,                               Value="Base_User.Add",         NeedAction=true,    Icon=null,             Sort=1, CreateTime=DateTime.Now, CreatorId = "System", CreatorName = "System",},
                    new Base_Action(){ Id="Id1_3_2",Deleted = false, ParentId="Id1_3", Type = ActionType.权限, Name="改",   Url=null,                               Value="Base_User.Edit",        NeedAction=true,    Icon=null,             Sort=1, CreateTime=DateTime.Now, CreatorId = "System", CreatorName = "System",},
                    new Base_Action(){ Id="Id1_3_3",Deleted = false, ParentId="Id1_3", Type = ActionType.权限, Name="删",   Url=null,                               Value="Base_User.Delete",      NeedAction=true,    Icon=null,             Sort=1, CreateTime=DateTime.Now, CreatorId = "System", CreatorName = "System",},
                    new Base_Action(){ Id="Id1_4_1",Deleted = false, ParentId="Id1_4", Type = ActionType.权限, Name="增",   Url=null,                               Value="Base_Role.Add",         NeedAction=true,    Icon=null,             Sort=1, CreateTime=DateTime.Now, CreatorId = "System", CreatorName = "System",},
                    new Base_Action(){ Id="Id1_4_2",Deleted = false, ParentId="Id1_4", Type = ActionType.权限, Name="改",   Url=null,                               Value="Base_Role.Edit",        NeedAction=true,    Icon=null,             Sort=1, CreateTime=DateTime.Now, CreatorId = "System", CreatorName = "System",},
                    new Base_Action(){ Id="Id1_4_3",Deleted = false, ParentId="Id1_4", Type = ActionType.权限, Name="删",   Url=null,                               Value="Base_Role.Delete",      NeedAction=true,    Icon=null,             Sort=1, CreateTime=DateTime.Now, CreatorId = "System", CreatorName = "System",},
                    new Base_Action(){ Id="Id1_5_1",Deleted = false, ParentId="Id1_5", Type = ActionType.权限, Name="增",   Url=null,                               Value="Base_Department.Add",   NeedAction=true,    Icon=null,             Sort=1, CreateTime=DateTime.Now, CreatorId = "System", CreatorName = "System",},
                    new Base_Action(){ Id="Id1_5_2",Deleted = false, ParentId="Id1_5", Type = ActionType.权限, Name="改",   Url=null,                               Value="Base_Department.Edit",  NeedAction=true,    Icon=null,             Sort=1, CreateTime=DateTime.Now, CreatorId = "System", CreatorName = "System",},
                    new Base_Action(){ Id="Id1_5_3",Deleted = false, ParentId="Id1_5", Type = ActionType.权限, Name="删",   Url=null,                               Value="Base_Department.Delete",NeedAction=true,    Icon=null,             Sort=1, CreateTime=DateTime.Now, CreatorId = "System", CreatorName = "System",},
                    new Base_Action(){ Id="Id1_9_1",Deleted = false, ParentId="Id1_9", Type = ActionType.权限, Name="增",   Url=null,                               Value="Quartz_Task.Add",       NeedAction=true,    Icon=null,             Sort=1, CreateTime=DateTime.Now, CreatorId = "System", CreatorName = "System",},
                    new Base_Action(){ Id="Id1_9_2",Deleted = false, ParentId="Id1_9", Type = ActionType.权限, Name="改",   Url=null,                               Value="Quartz_Task.Edit",      NeedAction=true,    Icon=null,             Sort=1, CreateTime=DateTime.Now, CreatorId = "System", CreatorName = "System",},
                    new Base_Action(){ Id="Id1_9_3",Deleted = false, ParentId="Id1_9", Type = ActionType.权限, Name="删",   Url=null,                               Value="Quartz_Task.Delete",    NeedAction=true,    Icon=null,             Sort=1, CreateTime=DateTime.Now, CreatorId = "System", CreatorName = "System",},

                    new Base_Action(){ Id="Id2",Deleted = false, ParentId=null,    Type = ActionType.菜单, Name="日志管理", Url=null,                               Value=null,                    NeedAction=true,    Icon="file-search",    Sort=2, CreateTime=DateTime.Now, CreatorId = "System", CreatorName = "System",},
                    new Base_Action(){ Id="Id2_1",Deleted = false, ParentId="Id2", Type = ActionType.页面, Name="异常日志", Url="/Base_Manage/Base_LogException/List", Value=null,                 NeedAction=true,    Icon="file-search",    Sort=1, CreateTime=DateTime.Now, CreatorId = "System", CreatorName = "System",},
                    new Base_Action(){ Id="Id2_2",Deleted = false, ParentId="Id2", Type = ActionType.页面, Name="访问日志", Url="/Base_Manage/Base_LogVisit/List",  Value=null,                    NeedAction=true,    Icon="file-search",    Sort=2, CreateTime=DateTime.Now, CreatorId = "System", CreatorName = "System",},
                    new Base_Action(){ Id="Id2_3",Deleted = false, ParentId="Id2", Type = ActionType.页面, Name="操作日志", Url="/Base_Manage/Base_LogOperating/List", Value=null,                 NeedAction=true,    Icon="file-search",    Sort=3, CreateTime=DateTime.Now, CreatorId = "System", CreatorName = "System",},
                    new Base_Action(){ Id="Id2_4",Deleted = false, ParentId="Id2", Type = ActionType.页面, Name="系统日志", Url="/Base_Manage/Base_LogSystem/List", Value=null,                    NeedAction=true,    Icon="file-search",    Sort=3, CreateTime=DateTime.Now, CreatorId = "System", CreatorName = "System",},

                    new Base_Action(){ Id="Id3",Deleted = false, ParentId=null,    Type = ActionType.菜单, Name="流程中心", Url=null,                               Value=null,                    NeedAction=true,    Icon="clock-circle",   Sort=3, CreateTime=DateTime.Now, CreatorId = "System", CreatorName = "System",},
                    new Base_Action(){ Id="Id3_1",Deleted = false, ParentId="Id3", Type = ActionType.页面, Name="流程管理", Url="/OA_Manage/OA_DefForm/List",       Value=null,                    NeedAction=true,    Icon="interaction",    Sort=1, CreateTime=DateTime.Now, CreatorId = "System", CreatorName = "System",},
                    new Base_Action(){ Id="Id3_1_1",Deleted = false, ParentId="Id3_1", Type = ActionType.权限, Name="增",   Url=null,                               Value="OA_DefForm.Add",        NeedAction=true,    Icon=null,             Sort=1, CreateTime=DateTime.Now, CreatorId = "System", CreatorName = "System",},
                    new Base_Action(){ Id="Id3_1_2",Deleted = false, ParentId="Id3_1", Type = ActionType.权限, Name="改",   Url=null,                               Value="OA_DefForm.Edit",       NeedAction=true,    Icon=null,             Sort=1, CreateTime=DateTime.Now, CreatorId = "System", CreatorName = "System",},
                    new Base_Action(){ Id="Id3_1_3",Deleted = false, ParentId="Id3_1", Type = ActionType.权限, Name="删",   Url=null,                               Value="OA_DefForm.Delete",     NeedAction=true,    Icon=null,             Sort=1, CreateTime=DateTime.Now, CreatorId = "System", CreatorName = "System",},
                    new Base_Action(){ Id="Id3_2",Deleted = false, ParentId="Id3", Type = ActionType.页面, Name="发起流程", Url="/OA_Manage/OA_DefForm/TreeList",   Value=null,                    NeedAction=true,    Icon="file-add",       Sort=1, CreateTime=DateTime.Now, CreatorId = "System", CreatorName = "System",},
                    new Base_Action(){ Id="Id3_3",Deleted = false, ParentId="Id3", Type = ActionType.页面, Name="我的流程", Url="/OA_Manage/OA_UserForm/List",      Value=null,                    NeedAction=true,    Icon="file-done",      Sort=1, CreateTime=DateTime.Now, CreatorId = "System", CreatorName = "System",},
                    new Base_Action(){ Id="Id3_3_1",Deleted = false, ParentId="Id3_3", Type = ActionType.权限, Name="增",   Url=null,                               Value="OA_UserForm.Add",       NeedAction=true,    Icon=null,             Sort=1, CreateTime=DateTime.Now, CreatorId = "System", CreatorName = "System",},
                    new Base_Action(){ Id="Id3_3_2",Deleted = false, ParentId="Id3_3", Type = ActionType.权限, Name="改",   Url=null,                               Value="OA_UserForm.Edit",      NeedAction=true,    Icon=null,             Sort=1, CreateTime=DateTime.Now, CreatorId = "System", CreatorName = "System",},
                    new Base_Action(){ Id="Id3_3_3",Deleted = false, ParentId="Id3_3", Type = ActionType.权限, Name="删",   Url=null,                               Value="OA_UserForm.Delete",    NeedAction=true,    Icon=null,             Sort=1, CreateTime=DateTime.Now, CreatorId = "System", CreatorName = "System",},

                    new Base_Action(){ Id="Id4",Deleted = false, ParentId=null,    Type = ActionType.菜单, Name="通用查询",  Url=null,                              Value=null,                    NeedAction=true,    Icon="search",        Sort=4, CreateTime=DateTime.Now, CreatorId = "System", CreatorName = "System",},
                    new Base_Action(){ Id="Id4_1",Deleted = false, ParentId="Id4", Type = ActionType.页面, Name="用户查询",  Url="/Agile_Development/Common_FormConfigQuery/List/Base_Manage.Base_User",  Value="Base_Manage.Base_User",  NeedAction=true, Icon="user-add", Sort=1, CreateTime=DateTime.Now, CreatorId = "System", CreatorName = "System",},
                    new Base_Action(){ Id="Id4_2",Deleted = false, ParentId="Id4", Type = ActionType.页面, Name="角色查询",  Url="/Agile_Development/Common_FormConfigQuery/List/Base_Manage.Base_Role",  Value="Base_Manage.Base_Role",  NeedAction=true, Icon="safety", Sort=1, CreateTime=DateTime.Now, CreatorId = "System", CreatorName = "System",},
                    new Base_Action(){ Id="Id4_3",Deleted = false, ParentId="Id4", Type = ActionType.页面, Name="密钥查询",  Url="/Agile_Development/Common_FormConfigQuery/List/Base_Manage.Base_AppSecret",  Value="Base_Manage.Base_AppSecret",  NeedAction=true, Icon="key", Sort=1, CreateTime=DateTime.Now, CreatorId = "System", CreatorName = "System",},
                };

                var result = actionBusiness.Insert(actions);
                logger.LogTrace($"Base_Action created");
            }

            var appSecretBussiness = provider.GetRequiredService<IBase_AppSecretBusiness>();
            var appSecretCount = appSecretBussiness.GetIQueryable().Count();
            if (appSecretCount == 0)
            {
                List<Base_AppSecret> actions = new List<Base_AppSecret>()
                {
                    new Base_AppSecret(){  Id="1172497995938271232", AppId="PcAdmin", AppSecret="wtMaiTRPTT3hrf5e", AppName="后台AppId", CreateTime=DateTime.Now, CreatorId = "System", CreatorName = "System",},
                    new Base_AppSecret(){  Id="1173937877642383360", AppId="AppAdmin", AppSecret="IVh9LLSVFcoQPQ5K", AppName="APP密钥", CreateTime=DateTime.Now, CreatorId = "System", CreatorName = "System",}
                };

                var result = appSecretBussiness.Insert(actions);
                logger.LogTrace($"Base_AppSecret created");
            }

            var dictionaryBusiness = provider.GetRequiredService<IBase_DictionaryBusiness>();
            var dictionaryCount = dictionaryBusiness.GetIQueryable().Count();
            if (dictionaryCount == 0)
            {
                List<Base_Dictionary> dictionaries = new List<Base_Dictionary>()
                {
                    new Base_Dictionary(){ Id="Id1",Deleted = false, ParentId=null,  Type = DictionaryType.字典项, Category = "选项", ControlType = ControlType.ComboBox,  Text = "性别", Value="Sex", Code = "", Sort=1, CreateTime=DateTime.Now, CreatorId = "System", CreatorName = "System", },
                    new Base_Dictionary(){ Id="Id1_1",Deleted = false, ParentId="Id1", Type = DictionaryType.数据集, Category = "选项", Text = "女", Value="0", Code ="",  Sort=1, CreateTime=DateTime.Now, CreatorId = "System", CreatorName = "System", },
                    new Base_Dictionary(){ Id="Id1_2",Deleted = false, ParentId="Id1", Type = DictionaryType.数据集, Category = "选项", Text = "男", Value="1", Code ="",  Sort=2, CreateTime=DateTime.Now, CreatorId = "System", CreatorName = "System", },
                    new Base_Dictionary(){ Id="Id2",Deleted = false, ParentId=null,  Type = DictionaryType.字典项, Category = "选项", Text = "姓名", Value="UserName", Code = "", Sort=2, CreateTime=DateTime.Now, CreatorId = "System", CreatorName = "System", },
                    new Base_Dictionary(){ Id="Id3",Deleted = false, ParentId=null,  Type = DictionaryType.字典项, Category = "选项", Text = "真实姓名", Value="RealName", Code = "", Sort=3, CreateTime=DateTime.Now , CreatorId = "System", CreatorName = "System",},
                    new Base_Dictionary(){ Id="Id4",Deleted = false, ParentId=null,  Type = DictionaryType.字典项, Category = "选项", Text = "密码", Value="Password", Code = "", Sort=4, CreateTime=DateTime.Now, CreatorId = "System", CreatorName = "System", },
                    new Base_Dictionary(){ Id="Id5",Deleted = false, ParentId=null,  Type = DictionaryType.字典项, Category = "选项", Text = "生日", Value="Birthday", Code = "", Sort=5, CreateTime=DateTime.Now, CreatorId = "System", CreatorName = "System", },
                    new Base_Dictionary(){ Id="Id6",Deleted = false, ParentId=null,  Type = DictionaryType.字典项, Category = "选项", ControlType = ControlType.MultiComboBox,  Text = "角色", Value="RoleIdList", Code = "Base_Role", Sort=6, CreateTime=DateTime.Now, CreatorId = "System", CreatorName = "System", },
                    new Base_Dictionary(){ Id="Id7",Deleted = false, ParentId=null,  Type = DictionaryType.字典项, Category = "选项", ControlType = ControlType.TreeSelect,  Text = "部门", Value="DepartmentId", Code = "Base_Department", Sort=7, CreateTime=DateTime.Now, CreatorId = "System", CreatorName = "System",},
                    new Base_Dictionary(){ Id="Id8",Deleted = false, ParentId=null,  Type = DictionaryType.字典项, Category = "选项", Text = "手机号码", Value="PhoneNumber", Code = "", Sort=8, CreateTime=DateTime.Now, CreatorId = "System", CreatorName = "System",},
                    new Base_Dictionary(){ Id="Id9",Deleted = false, ParentId=null,  Type = DictionaryType.字典项, Category = "选项",  Text = "创建时间", Value="CreateTime", Code = "", Sort=9, CreateTime=DateTime.Now, CreatorId = "System", CreatorName = "System",},
                    new Base_Dictionary(){ Id="Id10",Deleted = false, ParentId=null,  Type = DictionaryType.字典项, Category = "选项", Text = "修改时间", Value="ModifyTime", Code = "", Sort=10, CreateTime=DateTime.Now, CreatorId = "System", CreatorName = "System",},
                    new Base_Dictionary(){ Id="Id11",Deleted = false, ParentId=null,  Type = DictionaryType.字典项, Category = "选项", Text = "创建者", Value="CreatorName", Code = "", Sort=11, CreateTime=DateTime.Now, CreatorId = "System", CreatorName = "System",},
                    new Base_Dictionary(){ Id="Id12",Deleted = false, ParentId=null,  Type = DictionaryType.字典项, Category = "选项", Text = "修改者", Value="ModifyName", Code = "", Sort=12, CreateTime=DateTime.Now, CreatorId = "System", CreatorName = "System",},
                    new Base_Dictionary(){ Id="Id13",Deleted = false, ParentId=null,  Type = DictionaryType.字典项, Category = "选项", ControlType = ControlType.ComboBox,  Text = "紧急程度", Value="Grade", Code = "", Sort=1, CreateTime=DateTime.Now, CreatorId = "System", CreatorName = "System",},
                    new Base_Dictionary(){ Id="Id13_1",Deleted = false, ParentId="Id13", Type = DictionaryType.数据集, Category = "选项",  Text = "正常", Value="0", Code ="",  Sort=1, CreateTime=DateTime.Now, CreatorId = "System", CreatorName = "System",},
                    new Base_Dictionary(){ Id="Id13_2",Deleted = false, ParentId="Id13", Type = DictionaryType.数据集, Category = "选项", Text = "紧急", Value="1", Code ="",  Sort=2, CreateTime=DateTime.Now, CreatorId = "System", CreatorName = "System",},
                    new Base_Dictionary(){ Id="Id13_3",Deleted = false, ParentId="Id13", Type = DictionaryType.数据集, Category = "选项", Text = "特级", Value="2", Code ="",  Sort=2, CreateTime=DateTime.Now, CreatorId = "System", CreatorName = "System",},

                    new Base_Dictionary(){ Id="Id1000",Deleted = false, ParentId=null,  Type = DictionaryType.字典项, Category = "流程", ControlType = ControlType.ComboBox,  Text = "分类", Value="Type", Code = "", Sort=1000, CreateTime=DateTime.Now, CreatorId = "System", CreatorName = "System", },
                    new Base_Dictionary(){ Id="Id1000_1",Deleted = false, ParentId="Id1000",  Type = DictionaryType.数据集, Category = "流程", Text = "请假", Value="请假", Code = "", Sort=1, CreateTime=DateTime.Now, CreatorId = "System", CreatorName = "System", },
                    new Base_Dictionary(){ Id="Id1000_2",Deleted = false, ParentId="Id1000",  Type = DictionaryType.数据集, Category = "流程", Text = "报销", Value="报销", Code = "", Sort=2, CreateTime=DateTime.Now, CreatorId = "System", CreatorName = "System", },
                    new Base_Dictionary(){ Id="Id1000_3",Deleted = false, ParentId="Id1000",  Type = DictionaryType.数据集, Category = "流程", Text = "顺序", Value="顺序", Code = "", Sort=3, CreateTime=DateTime.Now, CreatorId = "System", CreatorName = "System", },
                    new Base_Dictionary(){ Id="Id1000_4",Deleted = false, ParentId="Id1000",  Type = DictionaryType.数据集, Category = "流程", Text = "选择", Value="选择", Code = "", Sort=4, CreateTime=DateTime.Now, CreatorId = "System", CreatorName = "System", },
                    new Base_Dictionary(){ Id="Id1000_5",Deleted = false, ParentId="Id1000",  Type = DictionaryType.数据集, Category = "流程", Text = "或签", Value="或签", Code = "", Sort=5, CreateTime=DateTime.Now, CreatorId = "System", CreatorName = "System", },
                    new Base_Dictionary(){ Id="Id1000_6",Deleted = false, ParentId="Id1000",  Type = DictionaryType.数据集, Category = "流程", Text = "与签", Value="与签", Code = "", Sort=6, CreateTime=DateTime.Now, CreatorId = "System", CreatorName = "System", },

                    new Base_Dictionary(){ Id="Id1000_1_1",Deleted = false, ParentId="Id1000_1",  Type = DictionaryType.数据集, Category = "流程", Text = "病假", Value="病假", Code = "", Remark="天数", Sort=1, CreateTime=DateTime.Now, CreatorId = "System", CreatorName = "System", },
                    new Base_Dictionary(){ Id="Id1000_1_2",Deleted = false, ParentId="Id1000_1",  Type = DictionaryType.数据集, Category = "流程", Text = "事假", Value="事假", Code = "", Remark="天数", Sort=2, CreateTime=DateTime.Now, CreatorId = "System", CreatorName = "System", },
                    new Base_Dictionary(){ Id="Id1000_1_3",Deleted = false, ParentId="Id1000_1",  Type = DictionaryType.数据集, Category = "流程", Text = "调休", Value="调休", Code = "", Remark="天数", Sort=3, CreateTime=DateTime.Now, CreatorId = "System", CreatorName = "System", },
                    new Base_Dictionary(){ Id="Id1000_1_4",Deleted = false, ParentId="Id1000_1",  Type = DictionaryType.数据集, Category = "流程", Text = "年假", Value="年假", Code = "", Remark="天数", Sort=4, CreateTime=DateTime.Now, CreatorId = "System", CreatorName = "System", },

                    new Base_Dictionary(){ Id="Id1000_2_1",Deleted = false, ParentId="Id1000_2",  Type = DictionaryType.数据集, Category = "流程", Text = "差旅费用", Value="差旅费用", Code = "", Remark="费用(元)", Sort=1, CreateTime=DateTime.Now, CreatorId = "System", CreatorName = "System", },
                    new Base_Dictionary(){ Id="Id1000_2_2",Deleted = false, ParentId="Id1000_2",  Type = DictionaryType.数据集, Category = "流程", Text = "采购费用", Value="采购费用", Code = "", Remark="费用(元)", Sort=2, CreateTime=DateTime.Now, CreatorId = "System", CreatorName = "System", },
                    new Base_Dictionary(){ Id="Id1000_2_3",Deleted = false, ParentId="Id1000_2",  Type = DictionaryType.数据集, Category = "流程", Text = "活动费用", Value="活动费用", Code = "", Remark="费用(元)", Sort=3, CreateTime=DateTime.Now, CreatorId = "System", CreatorName = "System", },
                    new Base_Dictionary(){ Id="Id1000_2_4",Deleted = false, ParentId="Id1000_2",  Type = DictionaryType.数据集, Category = "流程", Text = "日常费用", Value="日常费用", Code = "", Remark="费用(元)", Sort=4, CreateTime=DateTime.Now, CreatorId = "System", CreatorName = "System", },
                };

                var result = dictionaryBusiness.Insert(dictionaries);
                logger.LogTrace($"Base_Dictionary created");
            }

            var commonFormConfigBusiness = provider.GetRequiredService<IBase_CommonFormConfigBusiness>();
            var commonFormConfigCount = commonFormConfigBusiness.GetIQueryable().Count();
            if (commonFormConfigCount == 0)
            {
                List<Base_CommonFormConfig> commonFormConfigs = new List<Base_CommonFormConfig>()
                {
                    new Base_CommonFormConfig(){ Id="Id1_1", Deleted = false, Table="Base_User", Type=0, Header="姓名", PropertyName="UserName", PropertyType="string", Visibility = 0, DisplayIndex=0, CreateTime=DateTime.Now },
                    new Base_CommonFormConfig(){ Id="Id1_2", Deleted = false, Table="Base_User", Type=0, Header="手机号码", PropertyName="PhoneNumber", PropertyType="string", Visibility = 0, DisplayIndex=1, CreateTime=DateTime.Now },

                    new Base_CommonFormConfig(){ Id="Id1_11", Deleted = false, Table="Base_User", Type=1, Header="Id", PropertyName="Id", PropertyType="string", Visibility = 2, DisplayIndex=0, CanUserSort=true, CanUserResize=true, CanUserReorder=true, CreateTime=DateTime.Now },
                    new Base_CommonFormConfig(){ Id="Id1_12", Deleted = false, Table="Base_User", Type=1, Header="姓名", PropertyName="UserName", PropertyType="string", Visibility = 0, IsRequired = true, ErrorMessage="请输入姓名", DisplayIndex=1, CanUserSort=true, CanUserResize=true, CanUserReorder=true, CreateTime=DateTime.Now },
                    new Base_CommonFormConfig(){ Id="Id1_13", Deleted = false, Table="Base_User", Type=1, Header="真实姓名", PropertyName="RealName", PropertyType="string", Visibility = 0, DisplayIndex=2, CanUserSort=true, CanUserResize=true, CanUserReorder=true, CreateTime=DateTime.Now },
                    new Base_CommonFormConfig(){ Id="Id1_14", Deleted = false, Table="Base_User", Type=1, Header="密码", PropertyName="Password", PropertyType="string", Visibility = 0, ControlType=ControlType.PasswordBox, DisplayIndex=3, CanUserSort=true, CanUserResize=true, CanUserReorder=true, CreateTime=DateTime.Now },
                    new Base_CommonFormConfig(){ Id="Id1_15", Deleted = false, Table="Base_User", Type=1, Header="性别", PropertyName="Sex", PropertyType="int", ControlType=ControlType.ComboBox, Visibility = 0, DisplayIndex=4, CanUserSort=true, CanUserResize=true, CanUserReorder=true, CreateTime=DateTime.Now },
                    new Base_CommonFormConfig(){ Id="Id1_16", Deleted = false, Table="Base_User", Type=1, Header="生日", PropertyName="Birthday", PropertyType="datetime", Visibility = 0, DisplayIndex=5, CanUserSort=true, CanUserResize=true, CanUserReorder=true, CreateTime=DateTime.Now },
                    new Base_CommonFormConfig(){ Id="Id1_17", Deleted = false, Table="Base_User", Type=1, Header="角色", PropertyName="RoleIdList", PropertyType="list", ItemSource="Base_Role", ControlType=ControlType.MultiComboBox, Visibility = 0, DisplayIndex=6, CanUserSort=true, CanUserResize=true, CanUserReorder=true, CreateTime=DateTime.Now },
                    new Base_CommonFormConfig(){ Id="Id1_18", Deleted = false, Table="Base_User", Type=1, Header="部门", PropertyName="DepartmentId",PropertyType="string", ItemSource="Base_Department",ControlType=ControlType.TreeSelect, Visibility = 0, DisplayIndex=7, CanUserSort=true, CanUserResize=true, CanUserReorder=true, CreateTime=DateTime.Now },
                    new Base_CommonFormConfig(){ Id="Id1_19", Deleted = false, Table="Base_User", Type=1, Header="手机号码", PropertyName="PhoneNumber", PropertyType="string", Visibility = 0, Regex=@"^(13[0-9]|14[01456879]|15[0-35-9]|16[2567]|17[0-8]|18[0-9]|19[0-35-9])\d{8}$", ErrorMessage="请输入正确的手机号码格式", DisplayIndex=8, CanUserSort=true, CanUserResize=true, CanUserReorder=true, CreateTime=DateTime.Now },
                    new Base_CommonFormConfig(){ Id="Id1_20", Deleted = false, Table="Base_User", Type=1, Header="创建时间", PropertyName="CreateTime", PropertyType="datetime", IsReadOnly = true, Visibility = 0, DisplayIndex=9, CanUserSort=true, CanUserResize=true, CanUserReorder=true, CreateTime=DateTime.Now },
                    new Base_CommonFormConfig(){ Id="Id1_21", Deleted = false, Table="Base_User", Type=1, Header="修改时间", PropertyName="ModifyTime", PropertyType="datetime", IsReadOnly = true, Visibility = 0, DisplayIndex=10, CanUserSort=true, CanUserResize=true, CanUserReorder=true, CreateTime=DateTime.Now },
                    new Base_CommonFormConfig(){ Id="Id1_22", Deleted = false, Table="Base_User", Type=1, Header="创建者", PropertyName="CreatorName", PropertyType="string", IsReadOnly = true, Visibility = 0, DisplayIndex=11, CanUserSort=true, CanUserResize=true, CanUserReorder=true, CreateTime=DateTime.Now },
                    new Base_CommonFormConfig(){ Id="Id1_23", Deleted = false, Table="Base_User", Type=1, Header="修改者", PropertyName="ModifyName", PropertyType="string", IsReadOnly = true, Visibility = 0, DisplayIndex=12, CanUserSort=true, CanUserResize=true, CanUserReorder=true, CreateTime=DateTime.Now },

                    new Base_CommonFormConfig(){ Id="Id2_1", Deleted = false, Table="Base_Role", Type=0, Header="角色名", PropertyName="RoleName", PropertyType="string", Visibility = 0, DisplayIndex=0, CreateTime=DateTime.Now },

                    new Base_CommonFormConfig(){ Id="Id2_11", Deleted = false, Table="Base_Role", Type=1, Header="Id", PropertyName="Id", PropertyType="string", Visibility = 2, DisplayIndex=0, CanUserSort=true, CanUserResize=true, CanUserReorder=true, CreateTime=DateTime.Now },
                    new Base_CommonFormConfig(){ Id="Id2_12", Deleted = false, Table="Base_Role", Type=1, Header="角色名", PropertyName="RoleName", PropertyType="string", Visibility = 0, IsRequired = true, ErrorMessage="请输入角色名", DisplayIndex=1, CanUserSort=true, CanUserResize=true, CanUserReorder=true, CreateTime=DateTime.Now },
                    new Base_CommonFormConfig(){ Id="Id2_13", Deleted = false, Table="Base_Role", Type=1, Header="权限", PropertyName="Actions", PropertyType="list", ItemSource="Base_Action",ControlType=ControlType.MultiTreeSelect, Visibility = 0, Width="300", DisplayIndex=2, CanUserSort=true, CanUserResize=true, CanUserReorder=true, CreateTime=DateTime.Now },
                    new Base_CommonFormConfig(){ Id="Id2_14", Deleted = false, Table="Base_Role", Type=1, Header="创建时间", PropertyName="CreateTime", PropertyType="datetime", IsReadOnly = true, Visibility = 0, DisplayIndex=3, CanUserSort=true, CanUserResize=true, CanUserReorder=true, CreateTime=DateTime.Now },
                    new Base_CommonFormConfig(){ Id="Id2_15", Deleted = false, Table="Base_Role", Type=1, Header="修改时间", PropertyName="ModifyTime", PropertyType="datetime", IsReadOnly = true, Visibility = 0, DisplayIndex=4, CanUserSort=true, CanUserResize=true, CanUserReorder=true, CreateTime=DateTime.Now },
                    new Base_CommonFormConfig(){ Id="Id2_16", Deleted = false, Table="Base_Role", Type=1, Header="创建者", PropertyName="CreatorName", PropertyType="string", IsReadOnly = true, Visibility = 0, DisplayIndex=5, CanUserSort=true, CanUserResize=true, CanUserReorder=true, CreateTime=DateTime.Now },
                    new Base_CommonFormConfig(){ Id="Id2_17", Deleted = false, Table="Base_Role", Type=1, Header="修改者", PropertyName="ModifyName", PropertyType="string", IsReadOnly = true, Visibility = 0, DisplayIndex=6, CanUserSort=true, CanUserResize=true, CanUserReorder=true, CreateTime=DateTime.Now },

                    new Base_CommonFormConfig(){ Id="Id3_1", Deleted = false, Table="Base_AppSecret", Type=0, Header="应用Id", PropertyName="AppId", PropertyType="string", Visibility = 0, DisplayIndex=0, CreateTime=DateTime.Now },
                    new Base_CommonFormConfig(){ Id="Id3_2", Deleted = false, Table="Base_AppSecret", Type=0, Header="应用名", PropertyName="AppName", PropertyType="string", Visibility = 0, DisplayIndex=1, CreateTime=DateTime.Now },

                    new Base_CommonFormConfig(){ Id="Id3_11", Deleted = false, Table="Base_AppSecret", Type=1, Header="Id", PropertyName="Id", PropertyType="string", Visibility = 2, DisplayIndex=0, CanUserSort=true, CanUserResize=true, CanUserReorder=true, CreateTime=DateTime.Now },
                    new Base_CommonFormConfig(){ Id="Id3_12", Deleted = false, Table="Base_AppSecret", Type=1, Header="应用Id", PropertyName="AppId", PropertyType="string", Visibility = 0, IsRequired = true, ErrorMessage="请输入应用Id", DisplayIndex=1, CanUserSort=true, CanUserResize=true, CanUserReorder=true, CreateTime=DateTime.Now },
                    new Base_CommonFormConfig(){ Id="Id3_13", Deleted = false, Table="Base_AppSecret", Type=1, Header="密钥", PropertyName="AppSecret", PropertyType="string", Visibility = 0, IsRequired = true, ErrorMessage="请输入密钥", DisplayIndex=2, CanUserSort=true, CanUserResize=true, CanUserReorder=true, CreateTime=DateTime.Now },
                    new Base_CommonFormConfig(){ Id="Id3_14", Deleted = false, Table="Base_AppSecret", Type=1, Header="应用名", PropertyName="AppName", PropertyType="string", Visibility = 0, IsRequired = true, ErrorMessage="请输入应用名", DisplayIndex=3, CanUserSort=true, CanUserResize=true, CanUserReorder=true, CreateTime=DateTime.Now },
                    new Base_CommonFormConfig(){ Id="Id3_15", Deleted = false, Table="Base_AppSecret", Type=1, Header="创建时间", PropertyName="CreateTime", PropertyType="datetime", IsReadOnly = true, Visibility = 0, DisplayIndex=4, CanUserSort=true, CanUserResize=true, CanUserReorder=true, CreateTime=DateTime.Now },
                    new Base_CommonFormConfig(){ Id="Id3_16", Deleted = false, Table="Base_AppSecret", Type=1, Header="修改时间", PropertyName="ModifyTime", PropertyType="datetime", IsReadOnly = true, Visibility = 0, DisplayIndex=5, CanUserSort=true, CanUserResize=true, CanUserReorder=true, CreateTime=DateTime.Now },
                    new Base_CommonFormConfig(){ Id="Id3_17", Deleted = false, Table="Base_AppSecret", Type=1, Header="创建者", PropertyName="CreatorName", PropertyType="string", IsReadOnly = true, Visibility = 0, DisplayIndex=6, CanUserSort=true, CanUserResize=true, CanUserReorder=true, CreateTime=DateTime.Now },
                    new Base_CommonFormConfig(){ Id="Id3_18", Deleted = false, Table="Base_AppSecret", Type=1, Header="修改者", PropertyName="ModifyName", PropertyType="string", IsReadOnly = true, Visibility = 0, DisplayIndex=7, CanUserSort=true, CanUserResize=true, CanUserReorder=true, CreateTime=DateTime.Now },

                };

                var result = commonFormConfigBusiness.Insert(commonFormConfigs);
                logger.LogTrace($"Base_CommonFormConfig created");
            }
        }

        /// <summary>
        /// 初始化调度作业数据
        /// </summary>
        /// <param name="provider"></param>
        public static void EnsureSeedQuartzData(IServiceProvider provider)
        {
            var logger = provider.GetRequiredService<ILogger<SeedData>>();

            var quartz_TaskBusiness = provider.GetRequiredService<IQuartz_TaskBusiness>();
            var textJob = quartz_TaskBusiness.FirstOrDefault(p => p.TaskName == "TestJob");
            if (textJob == null)
            {
                textJob = new Quartz_Task()
                {
                    Id = IdHelper.GetId(),
                    TaskName = "TestJob",
                    GroupName = "Test",
                    ActionClass = nameof(TestJob),
                    Cron = "0/10 * * * * ?",
                    IsEnabled = true,
                    CreateTime = DateTime.Now,
                    CreatorId = "System",
                    CreatorName = "System",
                };

                var result = quartz_TaskBusiness.Insert(textJob);

                logger.LogDebug("TestJob created");
            }
        }

        /// <summary>
        /// 初始化工作流
        /// </summary>
        /// <param name="provider"></param>
        public static void EnsureSeedWorkflow(IServiceProvider provider)
        {
            //var logger = provider.GetRequiredService<ILogger<SeedData>>();

            //var oA_DefFormBusiness = provider.GetRequiredService<IOA_DefFormBusiness>();
            //var defformcount = oA_DefFormBusiness.GetIQueryable().Count();
            //if (defformcount == 0)
            //{
            //    var directory = AppContext.BaseDirectory;
            //    directory = directory.Replace("\\", "/");

            //    List<OA_DefForm> defs = new List<OA_DefForm>();
            //    string id = IdHelper.GetId();
            //    var def = new OA_DefForm()
            //    {
            //        Id = id,
            //        WorkflowJSON = File.ReadAllText($"{directory}/OA_Manage/Step/g6test1.json"),
            //        JSONId = "1274618511506804736",
            //        Type = "请假",
            //        Name = "请假流程",
            //        Text = "最简单的请假流程",
            //        Status = 1,
            //        CreateTime = DateTime.Now,
            //    };
            //    defs.Add(def);
            //    id = IdHelper.GetId();
            //    def = new OA_DefForm()
            //    {
            //        Id = id,
            //        WorkflowJSON = File.ReadAllText($"{directory}/OA_Manage/Step/g6test2.json"),
            //        JSONId = "1274620801831669760",
            //        Type = "报销",
            //        Name = "报销审批-与签",
            //        Text = "所有审批人都要同意",
            //        Status = 1,
            //        CreateTime = DateTime.Now,
            //    };
            //    defs.Add(def);
            //    id = IdHelper.GetId();
            //    def = new OA_DefForm()
            //    {
            //        Id = id,
            //        WorkflowJSON = File.ReadAllText($"{directory}/OA_Manage/Step/g6test3.json"),
            //        JSONId = "1274621154383892480",
            //        Type = "报销",
            //        Name = "报销审批-或签",
            //        Text = "只要有一个人审批就行",
            //        Status = 1,
            //        CreateTime = DateTime.Now,
            //    };
            //    defs.Add(def);
            //    id = IdHelper.GetId();
            //    def = new OA_DefForm()
            //    {
            //        Id = id,
            //        WorkflowJSON = File.ReadAllText($"{directory}/OA_Manage/Step/g6test4.json"),
            //        JSONId = "1274621654579810304",
            //        Type = "顺序",
            //        Name = "部门领导审批",
            //        Text = "根据申请人所在部门自动查找生成审批人",
            //        Status = 1,
            //        CreateTime = DateTime.Now,
            //    };
            //    defs.Add(def);
            //    id = IdHelper.GetId();
            //    def = new OA_DefForm()
            //    {
            //        Id = id,
            //        WorkflowJSON = File.ReadAllText($"{directory}/OA_Manage/Step/g6test5.json"),
            //        JSONId = "1274622508779180032",
            //        Type = "报销",
            //        Name = "并行流程",
            //        Text = "两个分管部门同时进行审批",
            //        Status = 1,
            //        CreateTime = DateTime.Now,
            //    };
            //    defs.Add(def);
            //    id = IdHelper.GetId();
            //    def = new OA_DefForm()
            //    {
            //        Id = id,
            //        WorkflowJSON = File.ReadAllText($"{directory}/OA_Manage/Step/g6test6.json"),
            //        JSONId = "1274623039325081600",
            //        Type = "顺序",
            //        Name = "有创建权限的流程",
            //        Text = "只有管理员能创建的流程",
            //        Status = 1,
            //        Value = $"^{AdminTypes.Admin.ToString()}^",
            //        CreateTime = DateTime.Now,
            //    };
            //    defs.Add(def);
            //    id = IdHelper.GetId();
            //    def = new OA_DefForm()
            //    {
            //        Id = id,
            //        WorkflowJSON = File.ReadAllText($"{directory}/OA_Manage/Step/g6test7.json"),
            //        JSONId = "1274623664695808000",
            //        Type = "请假",
            //        Name = "请假流程-条件",
            //        Text = "根据请假天数是否需要分管领导审批",
            //        Status = 1,
            //        CreateTime = DateTime.Now,
            //    };
            //    defs.Add(def);
            //    var result = oA_DefFormBusiness.Insert(defs);
            //    logger.LogDebug("OA_DefForm created");
            //}

       
        }
    }
}