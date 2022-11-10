using FluentAssertions;
using ReceiptManager.Core.Models;
using ReceiptManager.Core.Validations;
using System.Collections.Generic;
using Xunit;

namespace ReceiptManager.Tests
{
    public class ItemNameValidatorTests
    {
        private readonly IItemValidator _sut;

        public ItemNameValidatorTests()
        {
            _sut = new ItemNameValidator();
        }

        [Fact]
        public void IsValid_InputValidName_ReturnsTrue()
        {
            // Arrange
            var testList = new List<Item>()
            {
                new Item("Lenovo laptop"),
                new Item("Logitech mouse"),
                new Item("Bose headphones")
            };

            // Act
            var actual = _sut.IsValid(testList);

            // Assert
            actual.Should().BeTrue();
        }

        [Fact]
        public void IsValid_InputInvalidNameSingle_ReturnsFalse()
        {
            // Arrange
            var testList = new List<Item>()
            {
                new Item("Lenovo laptop"),
                new Item(""),
                new Item("Bose headphones")
            };

            // Act
            var actual = _sut.IsValid(testList);

            // Assert
            actual.Should().BeFalse();
        }

        [Theory]
        [InlineData(" ", "     ", null, false)]
        public void IsValid_InputInvalidNameMultiple_ReturnsFalse(string testProductNameA,
            string testProductNameB, string testProductNameC, bool expected)
        {
            // Arrange
            var testList = new List<Item>()
            {
                new Item("Lenovo laptop"),
                new Item(testProductNameA),
                new Item(testProductNameB),
                new Item("Samsung monitor"),
                new Item(testProductNameC),
                new Item("Bose headphones")
            };

            // Act
            var actual = _sut.IsValid(testList);

            // Assert
            actual.Should().Be(expected);
        }
    }
}