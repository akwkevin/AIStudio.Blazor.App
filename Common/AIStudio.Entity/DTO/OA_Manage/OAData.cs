using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AIStudio.Entity.DTO.OA_Manage
{
    //aistudio
    public class OAData
    {
        public string Id { get; set; }
        public int Version { get; set; }
        public string DataType { get; set; }
        public bool FirstStart { get; set; } = true;
        public List<OAStep> Steps { get; set; } = new List<OAStep>();
        public List<CurrentStepId> CurrentStepIds { get; set; } = new List<CurrentStepId>();
        public MyEvent MyEvent { get; set; }
        public double Flag { get; set; }

        #region g6editor
        public nodes[] nodes { get; set; }
        public edges[] edges { get; set; }
        public groups[] groups { get; set; }
        #endregion
    }

    public class OAStep
    {
        public string Id { get; set; }
        public string Label { get; set; }
        public string StepType { get; set; }
        public List<string> PreStepId { get; set; }
        public string NextStepId { get; set; }
        public int Status { get; set; }

        public ActRule ActRules { get; set; }
        public Dictionary<string, string> SelectNextStep { get; set; } = new Dictionary<string, string>();
    }

    public class CurrentStepId
    {
        public string StepId { get; set; }

        public string StepLabel { get; set; }
        public ActRule ActRules { get; set; }
    }

    public class ActRule
    {
        public List<string> UserIds { get; set; }
        public List<string> UserNames { get; set; }
        public List<string> RoleIds { get; set; }
        public List<string> RoleNames { get; set; }
        public string ActType { get; set; }
    }


    public class MyEvent
    {
        [Required]
        public string EventName { get; set; }
        [Required]
        public string EventKey { get; set; }
        public int Status { get; set; }
        [Required]
        public string Remarks { get; set; }
    }

    public class nodes
    {
        public string id { get; set; }
        public string name { get; set; }
        public string backImage { get; set; }
        public string color { get; set; }
        public string image { get; set; }
        public string label { get; set; }
        public int offsetX { get; set; }
        public int offsetY { get; set; }
        public string shape { get; set; }
        public int[] size { get; set; }
        public string stateImage { get; set; }
        public string type { get; set; }
        public double x { get; set; }
        public double y { get; set; }
        public double[][] inPoints { get; set; }
        public double[][] outPoints { get; set; }
        public bool isDoingStart { get; set; }
        public bool isDoingEnd { get; set; }
        public List<string> UserIds { get; set; }
        public List<string> RoleIds { get; set; }
        public string ActType { get; set; }
    }

    public class edges
    {
        public string id { get; set; }
        public point end { get; set; }
        public point endPoint { get; set; }
        public point start { get; set; }
        public point startPoint { get; set; }
        public string shape { get; set; }
        public string source { get; set; }
        public string sourceId { get; set; }
        public string target { get; set; }
        public string targetId { get; set; }
        public string type { get; set; }
        public string label { get; set; }
        public string textColor { get; set; }
    }

    public class groups
    {

    }

    public class point
    {
        public double x { get; set; }
        public double y { get; set; }
    }

    public static class StepType
    {
        public readonly static string Start = "AIStudio.Business.OA_Manage.Steps.OAStartStep, AIStudio.Business";
        public readonly static string Middle = "AIStudio.Business.OA_Manage.Steps.OAMiddleStep, AIStudio.Business";
        public readonly static string End = "AIStudio.Business.OA_Manage.Steps.OAEndStep, AIStudio.Business";
        public readonly static string Normal = "AIStudio.Business.OA_Manage.Steps.OANormalStep, AIStudio.Business";
        public readonly static string Data = "Coldairarrow.Util.OAData, Coldairarrow.Util";
        public readonly static string Decide = "AIStudio.Business.OA_Manage.Steps.OADecideStep, AIStudio.Business"; //"WorkflowCore.Primitives.Decide, WorkflowCore";
        public readonly static string COBegin = "AIStudio.Business.OA_Manage.Steps.OACOBeginStep, AIStudio.Business";
        public readonly static string COEnd = "AIStudio.Business.OA_Manage.Steps.OACOEndStep, AIStudio.Business";
    }

    public static class stateImage
    {
        public readonly static string start = "/assets/start.e502ed95.svg";
        public readonly static string end = "/assets/end.dfe4a5ab.svg";
        public readonly static string none = "/assets/none.d18d59d7.svg";
        public readonly static string jiahao = "/assets/jiahao.ecf71c51.svg";
        public readonly static string wenhao = "/assets/wenhao.71b31d27.svg";
        public readonly static string ok = "/assets/ok.463ab0e4.svg";
        public readonly static string no = "/assets/no.0ca40fee.svg";
    }

    public enum OAStatus
    {
        [Description("未开始")]
        Default = 0,
        [Description("审批中")]
        Being = 1,
        [Description("驳回上一级")]
        Goback = 2,
        [Description("驳回重提")]
        Restart = 3,
        [Description("否决")]
        Reject = 4,
        [Description("废弃")]
        Discard = 5,
        [Description("挂起")]
        Suspend = 6,
        [Description("恢复")]
        Resume = 7,
        [Description("操作失败")]
        Fail = 8,
        [Description("部分审批")]
        PartialApproval = 99,
        [Description("通过")]
        Approve = 100,
    }
}
