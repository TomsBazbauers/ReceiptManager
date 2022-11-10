using FluentAssertions;
using ReceiptManager.Core.Models;
using ReceiptManager.Core.Validations;
using System.Collections.Generic;
using Xunit;

namespace ReceiptManager.Tests
{
    public class ItemCountValidatorTests
    {
        private readonly IItemValidator _sut;

        public ItemCountValidatorTests()
        {
            _sut = new ItemCountValidator();
        }

        [Fact]
        public void IsValid_InputValidCount_ReturnsTrue()
        {
            // Arrange
            var testList = new List<Item>()
            {
                new Item(""),
                new Item(""),
                new Item("")
            };

            // Act
            var actual = _sut.IsValid(testList);

            // Assert
            actual.Should().BeTrue();
        }

        [Fact]
        public void IsValid_InputInvalidCount_ReturnsFalse()
        {
            // Arrange
            var testList = new List<Item>();

            // Act
            var actual = _sut.IsValid(testList);

            // Assert
            actual.Should().BeFalse();
        }
    }
}