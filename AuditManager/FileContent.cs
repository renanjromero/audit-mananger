namespace AuditManager
{
    public struct FileContent
    {
        public readonly string FileName;
        public readonly string[] Content;

        public FileContent(string fileName, string[] content)
        {
            FileName = fileName;
            Content = content;
        }
    }
}