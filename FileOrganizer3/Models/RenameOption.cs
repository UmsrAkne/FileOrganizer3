namespace FileOrganizer3.Models
{
    public class RenameOption
    {
        public ExtractOption ExtractOption { get; set; }

        public string Text { get; set; } = string.Empty;

        public bool IsPrefix { get; set; } = true;
    }
}