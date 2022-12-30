using AIStudio.Util.DiagramEntity;
using Google.Protobuf.WellKnownTypes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AIStudio.Entity.DTO.OA_Manage
{
    /// <summary>
    /// OA_Data
    /// </summary>
    public class OA_Data
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public string? Id { get; set; }
        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        public int Version { get; set; }
        /// <summary>
        /// Gets or sets the type of the data.
        /// </summary>
        /// <value>
        /// The type of the data.
        /// </value>
        public string? DataType { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [first start].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [first start]; otherwise, <c>false</c>.
        /// </value>
        public bool FirstStart { get; set; } = true;
        /// <summary>
        /// Gets or sets the steps.
        /// </summary>
        /// <value>
        /// The steps.
        /// </value>
        public List<OA_Step> Steps { get; set; } = new List<OA_Step>();
        /// <summary>
        /// Gets or sets the current step ids.
        /// </summary>
        /// <value>
        /// The current step ids.
        /// </value>
        public List<CurrentStepId> CurrentStepIds { get; set; } = new List<CurrentStepId>();
        /// <summary>
        /// Gets or sets my event.
        /// </summary>
        /// <value>
        /// My event.
        /// </value>
        public MyEvent MyEvent { get; set; }
        /// <summary>
        /// Gets or sets the flag.
        /// </summary>
        /// <value>
        /// The flag.
        /// </value>
        public double Flag { get; set; }

        //#region g6editor
        //public nodes[] nodes { get; set; }
        //public edges[] edges { get; set; }
        //public groups[] groups { get; set; }
        //#endregion

        #region Diagrams
        /// <summary>
        /// Gets or sets the nodes.
        /// </summary>
        /// <value>
        /// The nodes.
        /// </value>
        public FlowchartNode[]? Nodes { get; set; }
        /// <summary>
        /// Gets or sets the links.
        /// </summary>
        /// <value>
        /// The links.
        /// </value>
        public FlowchartLink[]? Links { get; set; }
        /// <summary>
        /// Gets or sets the groups.
        /// </summary>
        /// <value>
        /// The groups.
        /// </value>
        public FlowchartGroup[]? Groups { get; set; }
        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public class OA_Step
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public string? Id { get; set; }
        /// <summary>
        /// Gets or sets the label.
        /// </summary>
        /// <value>
        /// The label.
        /// </value>
        public string? Label { get; set; }
        /// <summary>
        /// Gets or sets the type of the step.
        /// </summary>
        /// <value>
        /// The type of the step.
        /// </value>
        public string? StepType { get; set; }
        /// <summary>
        /// Gets or sets the pre step identifier.
        /// </summary>
        /// <value>
        /// The pre step identifier.
        /// </value>
        public List<string>? PreStepId { get; set; }
        /// <summary>
        /// Gets or sets the next step identifier.
        /// </summary>
        /// <value>
        /// The next step identifier.
        /// </value>
        public string? NextStepId { get; set; }
        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public int Status { get; set; }

        /// <summary>
        /// Gets or sets the act rules.
        /// </summary>
        /// <value>
        /// The act rules.
        /// </value>
        public ActRule? ActRules { get; set; }
        /// <summary>
        /// Gets or sets the select next step.
        /// </summary>
        /// <value>
        /// The select next step.
        /// </value>
        public Dictionary<string, string> SelectNextStep { get; set; } = new Dictionary<string, string>();
    }

    /// <summary>
    /// 
    /// </summary>
    public class CurrentStepId
    {
        /// <summary>
        /// Gets or sets the step identifier.
        /// </summary>
        /// <value>
        /// The step identifier.
        /// </value>
        public string? StepId { get; set; }

        /// <summary>
        /// Gets or sets the step label.
        /// </summary>
        /// <value>
        /// The step label.
        /// </value>
        public string? StepLabel { get; set; }
        /// <summary>
        /// Gets or sets the act rules.
        /// </summary>
        /// <value>
        /// The act rules.
        /// </value>
        public ActRule? ActRules { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ActRule
    {
        /// <summary>
        /// Gets or sets the user ids.
        /// </summary>
        /// <value>
        /// The user ids.
        /// </value>
        public List<string>? UserIds { get; set; }
        /// <summary>
        /// Gets or sets the user names.
        /// </summary>
        /// <value>
        /// The user names.
        /// </value>
        public List<string>? UserNames { get; set; }
        /// <summary>
        /// Gets or sets the role ids.
        /// </summary>
        /// <value>
        /// The role ids.
        /// </value>
        public List<string>? RoleIds { get; set; }
        /// <summary>
        /// Gets or sets the role names.
        /// </summary>
        /// <value>
        /// The role names.
        /// </value>
        public List<string>? RoleNames { get; set; }
        /// <summary>
        /// Gets or sets the type of the act.
        /// </summary>
        /// <value>
        /// The type of the act.
        /// </value>
        public string? ActType { get; set; }

        public override string? ToString()
        {
            return string.Join(".", RoleNames??new List<string>()) + " " + string.Join(".", UserNames ?? new List<string>());
        }
    }


    /// <summary>
    /// 
    /// </summary>
    public class MyEvent
    {
        /// <summary>
        /// Gets or sets the name of the event.
        /// </summary>
        /// <value>
        /// The name of the event.
        /// </value>
        [Required]
        public string? EventName { get; set; }
        /// <summary>
        /// Gets or sets the event key.
        /// </summary>
        /// <value>
        /// The event key.
        /// </value>
        [Required]
        public string? EventKey { get; set; }
        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public int Status { get; set; }
        /// <summary>
        /// Gets or sets the remarks.
        /// </summary>
        /// <value>
        /// The remarks.
        /// </value>
        [Required]
        public string? Remarks { get; set; }

        public string? UserId { get; set; }

        public string? UserName { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class StepType
    {
        /// <summary>
        /// The start
        /// </summary>
        public readonly static string Start = "AIStudio.Business.OA_Manage.Steps.OAStartStep, AIStudio.Business";
        /// <summary>
        /// The middle
        /// </summary>
        public readonly static string Middle = "AIStudio.Business.OA_Manage.Steps.OAMiddleStep, AIStudio.Business";
        /// <summary>
        /// The end
        /// </summary>
        public readonly static string End = "AIStudio.Business.OA_Manage.Steps.OAEndStep, AIStudio.Business";
        /// <summary>
        /// The normal
        /// </summary>
        public readonly static string Normal = "AIStudio.Business.OA_Manage.Steps.OANormalStep, AIStudio.Business";
        /// <summary>
        /// The data
        /// </summary>
        public readonly static string Data = "AIStudio.Entity.DTO.OA_Manage.OA_Data, AIStudio.Entity";
        /// <summary>
        /// The decide
        /// </summary>
        public readonly static string Decide = "AIStudio.Business.OA_Manage.Steps.OADecideStep, AIStudio.Business"; //"WorkflowCore.Primitives.Decide, WorkflowCore";
        /// <summary>
        /// The co begin
        /// </summary>
        public readonly static string COBegin = "AIStudio.Business.OA_Manage.Steps.OACOBeginStep, AIStudio.Business";
        /// <summary>
        /// The co end
        /// </summary>
        public readonly static string COEnd = "AIStudio.Business.OA_Manage.Steps.OACOEndStep, AIStudio.Business";
    }

    /// <summary>
    /// 
    /// </summary>
    public enum OA_Status
    {
        /// <summary>
        /// The default
        /// </summary>
        [Description("未开始")]
        Default = 0,
        /// <summary>
        /// The being
        /// </summary>
        [Description("审批中")]
        Being = 1,
        /// <summary>
        /// The goback
        /// </summary>
        [Description("驳回上一级")]
        Goback = 2,
        /// <summary>
        /// The restart
        /// </summary>
        [Description("驳回重提")]
        Restart = 3,
        /// <summary>
        /// The reject
        /// </summary>
        [Description("否决")]
        Reject = 4,
        /// <summary>
        /// The discard
        /// </summary>
        [Description("废弃")]
        Discard = 5,
        /// <summary>
        /// The suspend
        /// </summary>
        [Description("挂起")]
        Suspend = 6,
        /// <summary>
        /// The resume
        /// </summary>
        [Description("恢复")]
        Resume = 7,
        /// <summary>
        /// The fail
        /// </summary>
        [Description("操作失败")]
        Fail = 8,
        /// <summary>
        /// The partial approval
        /// </summary>
        [Description("部分审批")]
        PartialApproval = 99,
        /// <summary>
        /// The approve
        /// </summary>
        [Description("通过")]
        Approve = 100,
    }
}
