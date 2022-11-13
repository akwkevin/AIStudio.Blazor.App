using AIStudio.Api.Controllers.Test.SamplesWorkflow;
using AIStudio.Common.Service;
using AIStudio.Common.Swagger;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using WorkflowCore.Interface;

namespace AIStudio.Api.Controllers.Test
{
    /// <summary>
    /// 工作流测试
    /// </summary>
    [ApiExplorerSettings(GroupName = nameof(ApiVersionInfo.Test))]
    [ApiController]
    [Route("[controller]/[action]")]
    public class WorkflowTestController : ControllerBase
    {
        private IWorkflowHost _workflowHost { get { return ServiceLocator.Instance.GetRequiredService<IWorkflowHost>(); } }
        private readonly ILogger _logger;

        /// <summary>
        /// 工作流测试控制器
        /// </summary>
        /// <param name="logger"></param>
        public WorkflowTestController(ILogger<WorkflowTestController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// HelloWorld
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string Sample01()
        {
            try
            {
                _workflowHost.RegisterWorkflow<HelloWorldWorkflow>();
            }
            catch { }

            _workflowHost.StartWorkflow("HelloWorld");

            return "HelloWorld";
        }

        /// <summary>
        /// 条件分支
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string Sample02()
        {
            try
            {
                _workflowHost.RegisterWorkflow<SimpleDecisionWorkflow>();
            }
            catch { }

            _workflowHost.StartWorkflow("Simple Decision Workflow");

            return "Simple Decision Workflow";
        }

        /// <summary>
        /// 输入输出
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string Sample03()
        {
            try
            {
                _workflowHost.RegisterWorkflow<PassingDataWorkflow, MyDataClass>();
                _workflowHost.RegisterWorkflow<PassingDataWorkflow2, Dictionary<string, int>>();
            }
            catch { }

            var initialData = new MyDataClass
            {
                Value1 = 2,
                Value2 = 3
            };

            _workflowHost.StartWorkflow("PassingDataWorkflow", 1, initialData);

            var initialData2 = new Dictionary<string, int>
            {
                ["Value1"] = 7,
                ["Value2"] = 2
            };

            _workflowHost.StartWorkflow("PassingDataWorkflow2", 1, initialData2);

            return "PassingDataWorkflow";
        }

        /// <summary>
        /// 等待事件Event触发
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<string> Sample04()
        {
            try
            {
                _workflowHost.RegisterWorkflow<EventSampleWorkflow, MyDataClass>();
            }
            catch { }

            var initialData = new MyDataClass();
            var workflowId = await _workflowHost.StartWorkflow("EventSampleWorkflow", 1, initialData);

            _logger.LogInformation("MyEvent publish 1s later");
            Thread.Sleep(1000);
            Random rnd = new Random();
            string value = rnd.Next(100).ToString();

            _workflowHost.PublishEvent("MyEvent", workflowId, value);

            return "EventSampleWorkflow";
        }

        /// <summary>
        /// 等待3s执行
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<string> Sample05()
        {
            try
            {
                _workflowHost.RegisterWorkflow<DeferSampleWorkflow>();
            }
            catch { }

            _workflowHost.StartWorkflow("DeferSampleWorkflow", 1, null, null);

            return "DeferSampleWorkflow";
        }


        /// <summary>
        /// 多结果测试
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<string> Sample06()
        {
            try
            {
                _workflowHost.RegisterWorkflow<MultipleOutcomeWorkflow>();
            }
            catch { }

            _workflowHost.StartWorkflow("MultipleOutcomeWorkflow", 1, null, null);

            return "MultipleOutcomeWorkflow";
        }

        /// <summary>
        /// Foreach循环
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string Sample07()
        {
            try
            {
                _workflowHost.RegisterWorkflow<ForEachWorkflow>();
            }
            catch { }

            string workflowId = _workflowHost.StartWorkflow("Foreach").Result;

            return "ForEachWorkflow";
        }

        /// <summary>
        /// 异步Foreach循环
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string Sample08()
        {
            try
            {
                _workflowHost.RegisterWorkflow<ForEachSyncWorkflow>();
            }
            catch { }

            string workflowId = _workflowHost.StartWorkflow("ForeachSync").Result;

            return "ForEachSyncWorkflow";
        }

        /// <summary>
        /// While循环
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string Sample09()
        {
            try
            {
                _workflowHost.RegisterWorkflow<WhileWorkflow, MyData>();
            }
            catch { }

            string workflowId = _workflowHost.StartWorkflow("While", new MyData { Counter = 0 }).Result;

            return "WhileWorkflow";
        }

        /// <summary>
        /// If分支
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string Sample10()
        {
            try
            {
                _workflowHost.RegisterWorkflow<IfWorkflow, MyData>();
            }
            catch { }

            string workflowId = _workflowHost.StartWorkflow("if-sample", new MyData { Counter = 4 }).Result;

            return "IfWorkflow";
        }

        /// <summary>
        /// 条件复杂分支
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string Sample11()
        {
            try
            {
                _workflowHost.RegisterWorkflow<OutcomeWorkflow, MyData>();
            }
            catch { }

            string workflowId = _workflowHost.StartWorkflow("outcome-sample", new MyData { Value = 2 }).Result;

            return "OutcomeWorkflow";
        }

        /// <summary>
        /// 并行运行
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string Sample12()
        {
            try
            {
                _workflowHost.RegisterWorkflow<ParallelWorkflow, MyData>();
            }
            catch { }

            string workflowId = _workflowHost.StartWorkflow("parallel-sample", new MyData()).Result;

            return "ParallelWorkflow";
        }

        /// <summary>
        /// 重复执行
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string Sample13()
        {
            try
            {
                _workflowHost.RegisterWorkflow<RecurSampleWorkflow, MyData>();
            }
            catch { }

            string workflowId = _workflowHost.StartWorkflow("recur-sample").Result;

            return "RecurSampleWorkflow";
        }

        /// <summary>
        /// 执行作业
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string Sample14()
        {
            try
            {
                _workflowHost.RegisterWorkflow<ScheduleWorkflow>();
            }
            catch { }

            string workflowId = _workflowHost.StartWorkflow("schedule-sample").Result;

            return "ScheduleWorkflow";
        }

        /// <summary>
        /// 报错重试
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string Sample15()
        {
            try
            {
                _workflowHost.RegisterWorkflow<CompensatingWorkflow>();
            }
            catch { }

            string workflowId = _workflowHost.StartWorkflow("compensate-sample").Result;

            return "CompensatingWorkflow";
        }

        /// <summary>
        /// 等待激活
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string Sample16()
        {
            try
            {
                _workflowHost.RegisterWorkflow<ActivityWorkflow, MyData>();
            }
            catch { }

            var workflowId = _workflowHost.StartWorkflow("activity-sample", new MyData { Request = "Spend $1,000,000" }).Result;

            var approval = _workflowHost.GetPendingActivity("get-approval", "worker1", TimeSpan.FromMinutes(1)).Result;

            if (approval != null)
            {
                _logger.LogInformation("Approval required for " + approval.Parameters);
                _workflowHost.SubmitActivitySuccess(approval.Token, "John Smith");
            }

            return "ActivityWorkflow";
        }


        /// <summary>
        /// 中间件，如日志打印，重试等
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string Sample17()
        {
            try
            {
                _workflowHost.RegisterWorkflow<FlakyConnectionWorkflow, FlakyConnectionParams>();
            }
            catch { }

            var workflowParams = new FlakyConnectionParams
            {
                Description = "Flaky connection workflow"
            };
            var workflowId = _workflowHost.StartWorkflow("flaky-sample", workflowParams).Result;

            return "FlakyConnectionWorkflow";
        }
    }
}