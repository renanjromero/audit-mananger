using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AuditManager
{
    public class Persister
    {
        public FileContent ReadFile(string fileName)
        {
            return new FileContent(fileName, File.ReadAllLines(fileName));
        }

        public FileContent[] ReadDirectory(string directoryName)
        {
            return Directory
                .GetFiles(directoryName)
                .Select(x => ReadFile(x))
                .ToArray();
        }

        public void ApplyChanges(IReadOnlyList<FileAction> actions)
        {
            foreach (FileAction action in actions)
            {
                switch (action.Type)
                {
                    case ActionType.Create:
                    case ActionType.Update:
                        File.WriteAllLines(action.FileName, action.Content);
                        break;

                    case ActionType.Delete:
                        File.Delete(action.FileName);
                        break;

                    default:
                        throw new InvalidOperationException();
                }
            }
        }

        public void ApplyChange(FileAction action)
        {
            ApplyChanges(new List<FileAction> { action });
        }
    }
}
