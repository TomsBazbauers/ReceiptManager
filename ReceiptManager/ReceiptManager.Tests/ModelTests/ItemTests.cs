using FluentAssertions;
using ReceiptManager.Core.Models;
using Xunit;

namespace ReceiptManager.Tests
{
    public class ItemTests
    {
        [Theory]
        [InlineData("Lenovo laptop")]
        [InlineData("Razor pad")]
        [InlineData("Apple laptop")]
        [InlineData("LG monitor Extra-Wide 300x keyboard included")]
        public void CreateItemObject_InputsValid_ObjectCreatedCorrectly(string testProductName)
        {
            // Act
            var actual = new Item(testProductName);

            // Assert
            actual.Should().BeOfType<Item>();
            actual.ProductName.Should().BeOfType<string>();
            actual.ProductName.Should().Be(testProductName);
        }
    }
}