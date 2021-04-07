namespace AuditManager
{
    public struct FileAction
    {
        public readonly string FileName;
        public readonly string[] Content;
        public readonly ActionType Type;

        public FileAction(string fileName, string[] content, ActionType type)
        {
            FileName = fileName;
            Content = content;
            Type = type;
        }
    }
}