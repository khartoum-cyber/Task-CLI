using Task_CLI.Enums;

namespace Task_CLI.Models
{
    internal class CliTask
    {
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public Status TaskStatus { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
