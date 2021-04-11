using System;
using Xunit;

namespace AuditManager.Tests
{
    public class ResultTests
    {
        [Fact]
        public void Result_is_success()
        {
            var result = Result.Ok();

            Assert.True(result.IsSuccess);
        }

        [Fact]
        public void Result_is_failure()
        {
            var result = Result.Fail("Something went wrong");

            Assert.True(result.IsFailure);
            Assert.Equal("Something went wrong", result.Error);
        }

        [Fact]
        public void Failures_must_contain_a_error_message()
        {
            Assert.Throws<InvalidOperationException>(() => Result.Fail(null));
        }

        [Fact]
        public void Result_is_success_and_contains_a_value()
        {
            var result = Result.Ok(1);

            Assert.True(result.IsSuccess);
            Assert.Equal(1, result.Value);
        }

        [Fact]
        public void Failure_result_value_is_not_accessible()
        {
            var result = Result.Fail<int>("Something went wrong");

            Assert.Throws<InvalidOperationException>(() => result.Value);
        }
    }
}
