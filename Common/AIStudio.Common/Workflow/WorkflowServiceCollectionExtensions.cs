using AIStudio.Common.AppSettings;
using AIStudio.Common.Workflow.Middleware;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;

namespace AIStudio.Common.Workflow
{
    public static class WorkflowServiceCollectionExtensions
    {
        public static IServiceCollection AddWorkflow_(this IServiceCollection services, Action<WorkflowSetupOptions>? setupAction = null)
        {
            //services.AddWorkflow();
            //services.AddWorkflowDSL();

            switch ((DbType)Convert.ToInt32(Enum.Parse(typeof(DbType), AppSettingsConfig.ConnectionStringsOptions.DbConfigs[0].DbType)))
            {
                case DbType.SqlServer: services.AddWorkflow(x => x.UseSqlServer(AppSettingsConfig.ConnectionStringsOptions.DbConfigs[0].DbString, false, true)); break;
                //case DbType.MySql.ToString(): services.AddWorkflow(x => x.UseMySQL(AppSettingsConfig.ConnectionStringsOptions.DbConfigs[0].DbString, false, true)); break;
                //case DbType.PostgreSql.ToString(): services.AddWorkflow(x => x.UsePostgreSQL(AppSettingsConfig.ConnectionStringsOptions.DbConfigs[0].DbString, false, true)); break;
                //case DbType.SQLite.ToString(): services.AddWorkflow(x => x.UseSqlite(AppSettingsConfig.ConnectionStringsOptions.DbConfigs[0].DbString, true)); break;
                default: throw new Exception("暂不支持该数据库！");
            }
            services.AddWorkflowDSL();       

            // Add step middleware
            // Note that middleware will get executed in the order in which they were registered
            services.AddWorkflowStepMiddleware<AddMetadataToLogsMiddleware>();
            services.AddWorkflowStepMiddleware<PollyRetryMiddleware>();

            // Add some pre workflow middleware
            // This middleware will run before the workflow starts
            services.AddWorkflowMiddleware<AddDescriptionWorkflowMiddleware>();

            // Add some post workflow middleware
            // This middleware will run after the workflow completes
            services.AddWorkflowMiddleware<PrintWorkflowSummaryMiddleware>();

            services.AddHostedService<WorkflowHostedService>();

            if (setupAction != null) services.Configure(setupAction);
            return services;
        }
    }
}
