using System;
using System.Collections.Generic;
using Xunit;

namespace AuditManager.Tests
{
    public class AuditManagerTests
    {
        [Fact]
        public void AddRecord_adds_a_record_to_an_existing_file_if_not_overflowed()
        {
            var sut = new AuditManager(3);
            FileContent fileContent = new FileContent("Audit_1.txt", new []
            {
                "1;Peter Peterson;2021-04-06T16:30:00",
            });

            FileAction action = sut.AddRecord(fileContent, "Renan Romero", new DateTime(2021, 4, 7, 9, 5, 0));

            Assert.Equal("Audit_1.txt", action.FileName);
            Assert.Equal(ActionType.Update, action.Type);
            Assert.Equal(new[]
            {
                "1;Peter Peterson;2021-04-06T16:30:00",
                "2;Renan Romero;2021-04-07T09:05:00"
            }, action.Content);
        }

        [Fact]
        public void AddRecord_adds_a_record_to_a_new_file_if_overflowed()
        {
            var sut = new AuditManager(1);
            FileContent fileContent = new FileContent("Audit_1.txt", new[]
            {
                "1;Peter Peterson;2020-04-06T16:30:00",
            });

            FileAction action = sut.AddRecord(fileContent, "Renan Romero", new DateTime(2021, 4, 7, 9, 5, 0));

            Assert.Equal("Audit_2.txt", action.FileName);
            Assert.Equal(ActionType.Create, action.Type);
            Assert.Equal(new[]
            {
                "1;Renan Romero;2021-04-07T09:05:00"
            }, action.Content);
        }

        [Fact]
        public void RemoveMentionsAbout_removes_mentions_from_files_in_the_directory()
        {
            var sut = new AuditManager(10);
            FileContent file = new FileContent("Audit_1.txt", new[]
            {
                "1;Peter Peterson;2021-04-06T16:30:00",
                "2;Jane Doe;2021-04-06T16:40:00",
                "3;Jack Rick;2021-04-06T17:00:00",
            });

            IReadOnlyList<FileAction> actions = sut.RemoveMentionsAbout("Peter Peterson", new[] { file });

            Assert.Equal(1, actions.Count);
            Assert.Equal("Audit_1.txt", actions[0].FileName);
            Assert.Equal(ActionType.Update, actions[0].Type);
            Assert.Equal(new[]
            {
                "1;Jane Doe;2021-04-06T16:40:00",
                "2;Jack Rick;2021-04-06T17:00:00"
            }, actions[0].Content);
        }

        [Fact]
        public void RemoveMentionsAbout_removes_whole_file_if_it_doesnt_contain_anything_else()
        {
            var sut = new AuditManager(10);
            var file = new FileContent("Audit_1.txt", new[]
            {
                "1;Peter Peterson;2021-04-06T16:30:00"
            });

            IReadOnlyList<FileAction> actions = sut.RemoveMentionsAbout("Peter Peterson", new[] { file });

            Assert.Equal(1, actions.Count);
            Assert.Equal("Audit_1.txt", actions[0].FileName);
            Assert.Equal(ActionType.Delete, actions[0].Type);
        }

        [Fact]
        public void RemoveMentionsAbout_does_not_do_anything_in_case_no_mentions_found()
        {
            var sut = new AuditManager(10);
            var file = new FileContent("Audit_1.txt", new[]
            {
                "1;Jane Smith;2021-04-06T16:30:00"
            });

            IReadOnlyList<FileAction> actions = sut.RemoveMentionsAbout("Peter Peterson", new[] { file });

            Assert.Equal(0, actions.Count);
        }
    }
}
