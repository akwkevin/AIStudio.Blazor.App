using AIStudio.Entity.Base_Manage;
using AIStudio.Util.Common;

namespace AIStudio.Entity.DTO.Base_Manage
{
    public class Base_CommonFormConfigDTO : Base_CommonFormConfig, IIdObject
    {
        public string VisibilityName
        {
            get
            {
                if (Visibility == 0)
                    return "显示";
                else
                    return "隐藏";
            }
        }

        public string HorizontalAlignmentName
        {
            get
            {
                if (HorizontalAlignment == 0)
                    return "左对齐";
                else if (HorizontalAlignment == 1)
                    return "居中";
                else if (HorizontalAlignment == 2)
                    return "右对齐";
                else
                    return "拉伸";
            }
        }
    }
}
