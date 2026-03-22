using System.ComponentModel.DataAnnotations;

namespace WorkTrace.Models
{
    public class SystemSetting
    {
        [Key]
        public string Key { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}