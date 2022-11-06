using SqlSugar;
using System.Data.Common;
using WorkflowCore.Interface;
using WorkflowCore.Models;
using WorkflowCore.Persistence.SqlSugar.Services;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWorkflow_(this IServiceCollection services)
        {
            services.AddWorkflow(x => x.UseSqlSugar(false, true));
            services.AddWorkflowDSL();

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
