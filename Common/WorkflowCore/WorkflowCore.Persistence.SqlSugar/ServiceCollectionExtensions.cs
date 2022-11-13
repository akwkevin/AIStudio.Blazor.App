using AIStudio.Common.Workflow;
using AIStudio.Common.Workflow.Middleware;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;
using WorkflowCore.Interface;
using WorkflowCore.Models;
using WorkflowCore.Persistence.SqlSugar;
using WorkflowCore.Persistence.SqlSugar.Services;

namespace WorkflowCore.Persistence.SqlSugar
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWorkflow_(this IServiceCollection services, Action<WorkflowSetupOptions>? setupAction = null)
        {
            //services.AddWorkflow(x => x.UseSqlSugar(false, true));
            //services.AddWorkflowDSL();

            services.AddWorkflow();
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

        public static WorkflowOptions UseSqlSugar(this WorkflowOptions options, bool canCreateDB, bool canMigrateDB)
        {
            var db = options.Services.BuildServiceProvider().GetService<ISqlSugarClient>();
            options.UsePersistence(sp => new SqlSugarPersistenceProvider(db, canCreateDB, canMigrateDB));
            options.Services.AddTransient<IWorkflowPurger>(sp => new WorkflowPurger(db));
            return options;
        }
    }
}
