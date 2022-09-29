using System.ComponentModel;

namespace AIStudio.Entity
{
    public enum UserMessageType
    {
        [Description("文本")]
        Text = 1,
        [Description("图片")]
        Image = 2,
        [Description("视频")]
        Video = 3,
        [Description("音频")]
        Audio = 4,
        [Description("文件")]
        File = 5,
        [Description("红包")]
        Money = 6,
        [Description("网页")]
        Html = 10000,
    }
}
