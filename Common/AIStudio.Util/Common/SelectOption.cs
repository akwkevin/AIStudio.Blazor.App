namespace AIStudio.Util.Common
{
    /// <summary>
    /// 前端SelectOption
    /// </summary>
    /// <seealso cref="AIStudio.Util.Common.ISelectOption" />
    public class SelectOption : ISelectOption
    {
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public string? Value { get; set; }
        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>
        /// The text.
        /// </value>
        public string? Text { get; set; }

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return $"{Value}-{Text}";
        }
    }
}
