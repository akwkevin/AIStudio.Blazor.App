namespace AIStudio.Util.Common
{
    /// <summary>
    /// 前端SelectOption
    /// </summary>
    public class SelectOption : ISelectOption
    {
        public string? Value { get; set; }
        public string? Text { get; set; }

        public override string ToString()
        {
            return $"{Value}-{Text}";
        }
    }
}
