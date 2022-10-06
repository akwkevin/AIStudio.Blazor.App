using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIStudio.Common.Quartz.Models
{
    public class JobExcuteResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }

        public JobExcuteResult(bool success, string message)
        {
            Success = success;
            Message = message;
        }
    }
}
