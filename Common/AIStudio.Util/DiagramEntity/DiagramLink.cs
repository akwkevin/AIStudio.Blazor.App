namespace AIStudio.Util.DiagramEntity
{
    /// <summary>
    /// 
    /// </summary>
    public class DiagramLink
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public string? Id { get; set; }
        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        /// <value>
        /// The color.
        /// </value>
        public string? Color { get; set; }
        /// <summary>
        /// Gets or sets the color of the selected.
        /// </summary>
        /// <value>
        /// The color of the selected.
        /// </value>
        public string? SelectedColor { get; set; }
        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>
        /// The width.
        /// </value>
        public double Width { get; set; }
        /// <summary>
        /// Gets or sets the label.
        /// </summary>
        /// <value>
        /// The label.
        /// </value>
        public string? Label { get; set; }//TODO

        /// <summary>
        /// Gets or sets the source identifier.
        /// </summary>
        /// <value>
        /// The source identifier.
        /// </value>
        public string? SourceId { get; set; }
        /// <summary>
        /// Gets or sets the target identifier.
        /// </summary>
        /// <value>
        /// The target identifier.
        /// </value>
        public string? TargetId { get; set; }

        /// <summary>
        /// Gets or sets the source port alignment.
        /// </summary>
        /// <value>
        /// The source port alignment.
        /// </value>
        public string? SourcePortAlignment { get; set; }
        /// <summary>
        /// Gets or sets the target port alignment.
        /// </summary>
        /// <value>
        /// The target port alignment.
        /// </value>
        public string? TargetPortAlignment { get; set; }
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public string? Type { get; set; }

        /// <summary>
        /// Gets or sets the router.
        /// </summary>
        /// <value>
        /// The router.
        /// </value>
        public string? Router { get; set; }

        /// <summary>
        /// Gets or sets the path generator.
        /// </summary>
        /// <value>
        /// The path generator.
        /// </value>
        public string? PathGenerator { get; set; }

        /// <summary>
        /// Gets or sets the source marker path.
        /// </summary>
        /// <value>
        /// The source marker path.
        /// </value>
        public string? SourceMarkerPath { get; set; }
        /// <summary>
        /// Gets or sets the width of the source marker.
        /// </summary>
        /// <value>
        /// The width of the source marker.
        /// </value>
        public double? SourceMarkerWidth { get; set; }

        /// <summary>
        /// Gets or sets the target marker path.
        /// </summary>
        /// <value>
        /// The target marker path.
        /// </value>
        public string? TargetMarkerPath { get; set; }
        /// <summary>
        /// Gets or sets the width of the target marker.
        /// </summary>
        /// <value>
        /// The width of the target marker.
        /// </value>
        public double? TargetMarkerWidth { get; set; }
    }
}
