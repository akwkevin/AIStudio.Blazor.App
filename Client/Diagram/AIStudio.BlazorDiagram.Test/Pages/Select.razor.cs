using System.ComponentModel.DataAnnotations;

namespace AIStudio.BlazorDiagram.Test.Pages
{
    public partial class Select
    {
    }

    public class Color
    {
        public Color()
        {
            SelectedColors = new string[] { };
        }
        [Required, MinLength(2), MaxLength(3)]
        public string[] SelectedColors { get; set; }
    }
}
