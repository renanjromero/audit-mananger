using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AuditManager
{
    public class AuditManager
    {
        private readonly int _maxEntriesPerFile;

        public AuditManager(int maxEntriesPerFile)
        {
            _maxEntriesPerFile = maxEntriesPerFile;
        }

        public FileAction AddRecord(FileContent currentFile, string visitorName, DateTime timeOfVisit)
        {
            List<AuditEntry> entries = Parse(currentFile.Content);

            if(entries.Count < _maxEntriesPerFile)
            {
                entries.Add(new AuditEntry(entries.Count + 1, visitorName, timeOfVisit));
                string[] newContent = Serialize(entries);

                return new FileAction(currentFile.FileName, newContent, ActionType.Update);
            } 
            else
            {
                var entry = new AuditEntry(1, visitorName, timeOfVisit);
                string[] newContent = Serialize(new List<AuditEntry> { entry });
                string newFileName = GetNewFileName(currentFile.FileName);

                return new FileAction(newFileName, newContent, ActionType.Create);
            }
        }

        public IReadOnlyList<FileAction> RemoveMentionsAbout(string visitorName, FileContent[] directoryFiles)
        {
            return directoryFiles
                .Select(file => RemoveMentionsIn(file, visitorName))
                .Where(action => action.HasValue)
                .Select(action => action.Value)
                .ToList();
        }
        
        private string GetNewFileName(string currentFileName)
        {
            string fileName = Path.GetFileNameWithoutExtension(currentFileName);
            int index = int.Parse(fileName.Split('_')[1]);
            return "Audit_" + (index + 1) + ".txt";
        }

        private List<AuditEntry> Parse(string[] content)
        {
            var result = new List<AuditEntry>();

            foreach (var line in content)
            {
                string[] data = line.Split(';');
                result.Add(new AuditEntry(int.Parse(data[0]), data[1], DateTime.Parse(data[2])));
            }

            return result;
        }

        private string[] Serialize(List<AuditEntry> entries)
        {
            return entries
                .Select(entry => entry.Number.ToString() + ';' + entry.Visitor + ';' + entry.TimeOfVisit.ToString("s"))
                .ToArray();
        }

        private FileAction? RemoveMentionsIn(FileContent file, string visitorName)
        {
            List<AuditEntry> entries = Parse(file.Content);

            List<AuditEntry> newContent = entries
                .Where(x => x.Visitor != visitorName)
                .Select((entry, index) => new AuditEntry(index + 1, entry.Visitor, entry.TimeOfVisit))
                .ToList();

            if (newContent.Count == entries.Count)
                return null;

            if(newContent.Count == 0)
                return new FileAction(file.FileName, new string[0], ActionType.Delete);

            return new FileAction(file.FileName, Serialize(newContent), ActionType.Update);
        }
    }
}
