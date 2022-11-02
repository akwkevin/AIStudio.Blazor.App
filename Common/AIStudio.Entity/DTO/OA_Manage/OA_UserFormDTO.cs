using AIStudio.Entity.OA_Manage;
using AIStudio.Util.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIStudio.Entity.DTO.OA_Manage
{
    [Map(typeof(OA_UserForm))]
    public class OA_UserFormDTO : OA_UserForm
    {
        public string ApplicantUserAndDepartment { get => ApplicantUser + "-" + ApplicantDepartment; }
        public string UserNamesAndRoles
        {
            get
            {
                var users = (UserNames ?? "").Replace("^", " ").Trim().Replace(" ", ",");
                var roles = (UserRoleNames ?? "").Replace("^", " ").Trim().Replace(" ", ",");
                return users + (string.IsNullOrEmpty(roles) ? "" : "-" + roles);
            }
        }

        public string Current
        {
            get { return CurrentNode?.Replace("^", "").Trim().Replace(" ", ","); }
        }


        public string ExpectedDateString { get => ExpectedDate?.ToString("yyyy-MM-dd"); }

        public string WorkflowJSON { get; set; }

        public List<OA_UserFormStepDTO> Comments { get; set; }

        public List<OAStep> Steps { get; set; }

        public int CurrentStepIndex { get; set; }
        public string CurrentStepId { get; set; }
        public string Avatar { get; set; }
    }
}
