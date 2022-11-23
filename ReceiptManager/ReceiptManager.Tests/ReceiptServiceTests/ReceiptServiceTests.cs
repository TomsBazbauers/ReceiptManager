using FluentAssertions;
using ReceiptManager.Core.Models;
using ReceiptManager.Core.Services;
using ReceiptManager.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ReceiptManager.Tests
{
    public class ReceiptServiceTests : InMemoryDb
    {
        private readonly IReceiptService _sut;

        public ReceiptServiceTests()
        {
            _sut = new ReceiptService(_dbContext);
        }

        [Fact]
        public void CreateReceipt_InputValid_ReceiptCreatedCorrectly()
        {
            // Arrange
            var receiptCountInDb = _dbContext.Receipts.Count();
            var testReceiptItems = new List<Item>()
            {
                new Item("LG monitor"),
                new Item("Logitech mouse")
            };

            // Act
            var actual = _sut.CreateReceipt(testReceiptItems);

            // Assert
            actual.Success.Should().BeTrue();

            // Assert
            _dbContext.Receipts.Count().Should().Be(receiptCountInDb + 1);
            _dbContext.Receipts.Any(receipt => receipt.Items.Count == testReceiptItems.Count
                && receipt.CreatedOn.Date == actual.Created.Date
                && receipt.Items.First().ProductName == testReceiptItems.First().ProductName);

            _dbContext.Receipts.OrderByDescending(receipt => receipt.CreatedOn)
                .First().Items.Should().BeSameAs(testReceiptItems);
        }

        [Fact]
        public void DeleteReceipt_InputValid_CorrectReceiptDeleted()
        {
            // Act
            var receiptCountInDb = _dbContext.Receipts.Count();
            var testId = 1;
            var actual = _sut.DeleteReceipt(testId);

            // Assert
            actual.Success.Should().BeTrue();

            // Assert
            _dbContext.Receipts.Count().Should().Be(receiptCountInDb - 1);
            _dbContext.Receipts.Any(receipt => receipt.Id != testId).Should().BeFalse();
        }

        [Fact]
        public void GetReceiptById_InputValid_ReturnsCorrectReceipt()
        {
            // Arrange
            var testReceipt = _dbContext.Receipts.First();

            // Act
            var actual = _sut.GetReceiptById(testReceipt.Id);

            // Assert
            actual.Id.Should().Be(1);
            actual.Should().NotBeNull();
            actual.Should().BeSameAs(testReceipt);
            actual.Should().BeOfType<Receipt>();
        }

        [Theory]
        [InlineData(20)]
        [InlineData(20.5)]
        [InlineData(-20)]
        [InlineData(-20.5)]
        public void GetReceiptById_InputInvalid_ReturnsNull(long testReceiptId)
        {
            // Act
            var actual = _sut.GetReceiptById(testReceiptId);

            // Assert
            actual.Should().BeNull();
        }

        [Fact]
        public void GetAllReceipts_ReturnsAllReceipts()
        {
            // Act
            var actual = _sut.GetAllReceipts();

            // Assert
            actual.Should().NotBeNull();
            actual.Count.Should().Be(3);
            actual.Should().BeOfType<List<Receipt>>();
        }

        [Fact]
        public void FilterReceiptsByItem_InputValid_ReturnsCorrectReceipt()
        {
            // Arrange
            var testItemProductName = "Bose headphones";

            // Act
            var actual = _sut.FilterReceiptsByItem(testItemProductName);

            // Assert
            actual.Should().NotBeNull();
            actual.Count.Should().Be(1);
            actual.Should().BeOfType<List<Receipt>>();
            actual.First().Should().BeOfType<Receipt>();
            actual.First().Items.Should().BeOfType<List<Item>>();
            actual.First().Items.Any(item => item.ProductName == testItemProductName).Should().BeTrue();
        }

        [Theory]
        [InlineData("Chewing gums")]
        [InlineData("Len0v0 lapt0p")]
        [InlineData("Samsung monitor")]
        public void FilterReceiptsByItem_InputInvalid_ReturnsEmptyList(string testItemProductName)
        {
            // Act
            var actual = _sut.FilterReceiptsByItem(testItemProductName);

            // Assert
            actual.Any().Should().BeFalse();
            actual.Count.Should().Be(0);
            actual.Should().BeOfType<List<Receipt>>();
        }

        [Fact]
        public void FilterReceiptsByPeriod_InputValid_ReturnsCorrectReceipt()
        {
            // Arrange
            var testLowerRange = new DateTime(2022, 10, 30);
            var testUpperRange = new DateTime(2022, 11, 01);

            // Act
            var actual = _sut.FilterReceiptsByPeriod(testLowerRange, testUpperRange);

            // Assert
            actual.Should().NotBeNull();
            actual.Count.Should().Be(2);
            actual.Should().BeOfType<List<Receipt>>();
            actual.First().Should().BeOfType<Receipt>();
            actual.All(receipt => receipt.CreatedOn < testLowerRange
                    && receipt.CreatedOn > testUpperRange).Should().BeFalse();
        }

        [Fact]
        public void FilterReceiptsByPeriod_InputInvalid_ReturnsEmptyList()
        {
            // Arrange
            var testLowerRange = new DateTime(2021, 01, 01);
            var testUpperRange = new DateTime(2022, 01, 01);

            // Act
            var actual = _sut.FilterReceiptsByPeriod(testLowerRange, testUpperRange);

            // Assert
            actual.Any().Should().BeFalse();
            actual.Count.Should().Be(0);
            actual.Should().BeOfType<List<Receipt>>();
        }
    }
}
