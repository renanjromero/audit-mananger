using System;

namespace AuditManager
{
    public struct AuditEntry
    {
        public readonly int Number;
        public readonly string Visitor;
        public readonly DateTime TimeOfVisit;

        public AuditEntry(int number, string visitor, DateTime timeOfVisit)
        {
            Number = number;
            Visitor = visitor;
            TimeOfVisit = timeOfVisit;
        }
    }
}
