using AIStudio.Entity.D_Manage;
using System;

namespace AIStudio.Entity.DTO
{
    public class GroupData
    {
        public int Total { get; set; }
        public string CreatorId { get; set; }
        public string CreatorName { get { return D_UserMessage?.CreatorName; } }
        public string Avatar { get; set; }
        public string GroupId { get { return D_UserMessage?.GroupId; } }
        public string GroupName { get { return D_UserMessage?.GroupName; } }
        public string UserIds { get { return D_UserMessage?.UserIds; } }
        public string UserNames { get { return D_UserMessage?.UserNames; } }
        public string Text { get { return D_UserMessage?.Text; } }
        public DateTime? CreateTime { get { return D_UserMessage?.CreateTime; } }

        public D_UserMessage D_UserMessage { get; set; }
    }
}
