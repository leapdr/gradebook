using System;
using Xunit;

namespace GradeBook.Tests
{
    public delegate string WriteLogDelegate(string logMessage);

    public class BookTests
    {
        int count = 0;

        [Fact]
        public void WriteLogDelegateCanPointToMethod()
        {
            WriteLogDelegate log = ReturnMessage;
            // log = new WriteLogDelegate(ReturnMessage);
            log += ReturnMessage; // combine with another method
            log += IncrementCount;

            var result = log("Hello!");
            // invokes ReturnMessage 2 times and IncrementCount 1 time

            Assert.Equal("Hello!", result);
        }

        string IncrementCount(string message)
        {
            count++;
            return message.ToLower();
        }

        string ReturnMessage(string message)
        {
            return message;
        }

        [Fact]
        public void BookCalculateAverageGrade()
        {
            // arrange
            var book = new InMemoryBook("");
            book.AddGrade(89.1);
            book.AddGrade(90.5);
            book.AddGrade(77.3);

            // act
            var result = book.GetStatistics();

            // assert
            Assert.Equal(85.6, result.Average, 1);
            Assert.Equal(90.5, result.High, 1);
            Assert.Equal(77.3, result.Low, 1);
            Assert.Equal("B", result.GradeLetter);
        }

        [Fact]
        public void SuccessfulAddGradeToBook()
        {
            // arrange
            var book = new InMemoryBook("");
            var isNegativeAdded = book.AddGrade(-1);
            var isGreaterAdded = book.AddGrade(1005);
            var isInRangeAdded = book.AddGrade(95);

            // act
            var result = book.GetStatistics();

            // assert
            Assert.False(isNegativeAdded);
            Assert.False(isGreaterAdded);
            Assert.True(isInRangeAdded);
        }
    }
}
