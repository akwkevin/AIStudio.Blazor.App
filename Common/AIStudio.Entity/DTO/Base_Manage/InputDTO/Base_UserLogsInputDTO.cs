using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIStudio.Entity.DTO.Base_Manage.InputDTO
{
    public class Base_UserLogsInputDTO
    {
        public string logContent { get; set; }
        public string logType { get; set; }
        public string opUserName { get; set; }
        public DateTime? startTime { get; set; }
        public DateTime? endTime { get; set; }
    }
}
