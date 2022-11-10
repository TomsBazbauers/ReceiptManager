using FluentAssertions;
using ReceiptManager.Core.Models;
using System;
using System.Collections.Generic;
using Xunit;

namespace ReceiptManager.Tests
{
    public class ReceiptTests
    {
        [Fact]
        public void CreateReceiptObject_InputValidSmallList_ObjectCreatedCorrectly()
        {
            // Arrange
            var testItem = new Item("Samsung monitor");
            var testList = new List<Item>() { testItem };

            // Act
            var actual = new Receipt(testList);

            // Assert
            actual.Should().BeOfType<Receipt>();
            actual.Items.Should().BeOfType<List<Item>>();
            actual.CreatedOn.Date.Should().Be(DateTime.Now.Date);
            actual.Items.Should().BeSameAs(testList);
        }

        [Fact]
        public void CreateReceiptObject_InputValidLargeList_ObjectCreatedCorrectly()
        {
            // Arrange
            var testItemA = new Item("Lenovo laptop");
            var testItemB = new Item("Samsung monitor");
            var testItemC = new Item("Logitech mouse");
            var testItemD = new Item("Bose headphones");
            var testList = new List<Item>() { testItemA, testItemB, testItemC, testItemD };

            // Act
            var actual = new Receipt(testList);

            // Assert
            actual.Should().BeOfType<Receipt>();
            actual.Items.Should().BeOfType<List<Item>>();
            actual.CreatedOn.Date.Should().Be(DateTime.Now.Date);
            actual.Items.Should().BeSameAs(testList);
            actual.Items.Count.Should().Be(testList.Count);
        }
    }
}