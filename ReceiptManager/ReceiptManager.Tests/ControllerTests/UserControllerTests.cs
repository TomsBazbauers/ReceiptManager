using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.AutoMock;
using ReceiptManager.Controllers;
using ReceiptManager.Core.Models;
using ReceiptManager.Core.Services;
using ReceiptManager.Core.Validations;
using ReceiptManager.Models;
using ReceiptManager.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ReceiptManager.Tests
{
    public class UserControllerTests : InMemoryDb
    {
        private AutoMocker _mocker;
        private readonly UserController _sut;
        private readonly IReceiptService _receiptService;
        private readonly IEnumerable<IItemValidator> _itemValidators;
        private readonly IRequestValidator _requestValidator;
        private readonly Mock<IMapper> _autoMapperMock;

        public UserControllerTests()
        {
            _mocker = new AutoMocker();

            _receiptService = new ReceiptService(_dbContext);
            _itemValidators = new List<IItemValidator>() { new ItemCountValidator(), new ItemNameValidator() };
            _requestValidator = new RequestValidator();
            _autoMapperMock = _mocker.GetMock<IMapper>();

            _sut = new UserController(_receiptService, _itemValidators, _requestValidator, _autoMapperMock.Object);
        }

        [Fact]
        public void CreateReceipt_InputValid_ReceiptCreated()
        {
            // Arrange
            var testItemList = new List<Item>() { new Item("Apple laptop"), new Item("Logitech mouse") };
            var testReceiptRequest = new ReceiptRequest(testItemList);

            _autoMapperMock
                .Setup(m => m.Map<Receipt>(testReceiptRequest))
                .Returns(new Receipt(testItemList));

            // Act
            var actionResult = _sut.CreateReceipt(testReceiptRequest);

            // Assert
            actionResult.Should().NotBeNull();
            actionResult.Should().BeOfType<CreatedResult>();

            // Assert
            var lastCreatedReceipt = _dbContext.Receipts.OrderByDescending(receipt => receipt.CreatedOn).ToList().First();
            lastCreatedReceipt.Items.Should().BeSameAs(testItemList);
        }

        [Fact]
        public void CreateReceipt_InputInvalidItemName_ReceiptNotCreated()
        {
            // Arrange
            var testItemList = new List<Item>() { new Item("Apple laptop"), new Item("") };
            var testReceiptRequest = new ReceiptRequest(testItemList);

            _autoMapperMock
                .Setup(m => m.Map<Receipt>(testReceiptRequest))
                .Returns(new Receipt(testItemList));

            // Act
            var actionResult = _sut.CreateReceipt(testReceiptRequest);

            // Assert
            actionResult.Should().BeOfType<BadRequestResult>();

            // Assert
            var lastCreatedReceipt = _dbContext.Receipts.OrderByDescending(receipt => receipt.CreatedOn).ToList().First();
            lastCreatedReceipt.Items.Should().NotBeSameAs(testItemList);
        }


        [Fact]
        public void DeleteReceipt_InputValid_CorrectReceiptDeleted()
        {
            // Arrange
            var receiptCountInDb = _dbContext.Receipts.Count();
            var testId = 2;

            // Act
            var actionResult = _sut.DeleteReceipt(testId);

            // Assert
            var actualId = (actionResult as ObjectResult).Value;
            actionResult.Should().BeOfType<OkObjectResult>();
            actualId.Should().Be(2);

            // Assert
            _dbContext.Receipts.Any(receipt => receipt.Id != testId).Should().BeTrue();
            _dbContext.Receipts.Count().Should().Be(receiptCountInDb - 1);
        }

        [Fact]
        public void DeleteReceipt_InputInvalidReceiptId_ReceiptNotDeleted()
        {
            // Arrange
            var receiptCountInDb = _dbContext.Receipts.Count();
            var testId = 12;

            // Act
            var actionResult = _sut.DeleteReceipt(testId);

            // Assert
            var actualId = (actionResult as ObjectResult).Value;
            actionResult.Should().BeOfType<NotFoundObjectResult>();
            actualId.Should().Be(12);

            // Assert
            _dbContext.Receipts.Count().Should().Be(receiptCountInDb);
        }

        [Fact]
        public void GetReceipt_InputValid_ReturnsCorrectReceipt()
        {
            // Arrange
            var testId = 2;
            var expectedReceipt = _receiptService.GetReceiptById(2);

            _autoMapperMock
                .Setup(m => m.Map<ReceiptRequest>(expectedReceipt))
                .Returns(new ReceiptRequest(expectedReceipt.Items));

            // Act
            var actionResult = _sut.GetReceipt(testId);

            // Assert
            actionResult.Should().BeOfType<OkObjectResult>();

            // Assert
            var actualValue = (actionResult as ObjectResult).Value;
            var actualReceipt = actualValue as Models.ReceiptRequest;
            actualValue.Should().BeOfType<ReceiptRequest>();
            actualReceipt.Items.Should().BeSameAs(expectedReceipt.Items);
        }

        [Fact]
        public void GetReceipt_InputInvalid_ReceiptNotReturned()
        {
            // Arrange
            var testId = 22;

            // Act
            var actionResult = _sut.GetReceipt(testId);

            // Assert
            var actualId = (actionResult as NotFoundObjectResult).Value;
            actionResult.Should().BeOfType<NotFoundObjectResult>();
            actualId.Should().Be(22);
        }

        [Fact]
        public void GetAllReceipts_InputValid_CorrectReceiptsReturned()
        {
            // Act
            var actionResult = _sut.GetAllReceipts();
            var actualReceiptCountInDb = _dbContext.Receipts.Count();

            // Assert
            var actualValue = (actionResult as OkObjectResult).Value;
            var actualReceipts = actualValue as List<Models.ReceiptRequest>;
            actionResult.Should().BeOfType<OkObjectResult>();
            actualReceipts.Count.Should().Be(actualReceiptCountInDb);
        }

        [Fact]
        public void FilterReceiptsByItem_InputValid_CorrectReceiptsReturned()
        {
            // Arrange
            var testReceipt = _dbContext.Receipts.First();
            var testItem = testReceipt.Items.First().ProductName;
            var receiptsContainingThisItemInDb = _dbContext.Receipts
                .Where(receipt => receipt.Items.Any(item => item.ProductName == testItem))
                .Count();

            // Act
            var actionResult = _sut.FilterReceiptsByItem(testItem);

            // Assert
            actionResult.Should().BeOfType<OkObjectResult>();

            // Assert
            var actualValue = (actionResult as OkObjectResult).Value;
            var actualReceipts = actualValue as List<Receipt>;
            actualReceipts.Count.Should().Be(receiptsContainingThisItemInDb);
            actualReceipts.First().Should().BeSameAs(testReceipt);
            actualReceipts.All(receipts => receipts.Items
                .Any(item => item.ProductName == testItem)).Should().BeTrue();
        }

        [Fact]
        public void FilterReceiptsByItem_InputInvalid_NoReceiptReturned()
        {
            // Act
            var actionResult = _sut.FilterReceiptsByItem("Apple personal computer");

            // Assert
            actionResult.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public void FilterReceiptsByPeriod_InputValid_ReceiptsReturned()
        {
            // Arrange
            var testLowerRange = new DateTime(2022, 10, 20);
            var testUpperRange = new DateTime(2022, 10, 30);

            var testReceipt = _dbContext.Receipts
                .First(receipt => receipt.CreatedOn.Date == testUpperRange);

            var receiptsCreatedOnThisPeriod = _dbContext.Receipts
                .Where(receipt => receipt.CreatedOn >= testLowerRange && receipt.CreatedOn <= testUpperRange)
                .Count();

            // Act
            var actionResult = _sut.FilterReceiptsByPeriod(testLowerRange, testUpperRange);

            // Assert
            actionResult.Should().BeOfType<OkObjectResult>();

            // Assert
            var actualValue = (actionResult as OkObjectResult).Value;
            var actualReceipts = actualValue as List<Receipt>;
            actualReceipts.Count.Should().Be(receiptsCreatedOnThisPeriod);
            actualReceipts.First().Should().BeSameAs(testReceipt);
            actualReceipts.All(receipt => receipt.CreatedOn >= testLowerRange
                && receipt.CreatedOn <= testUpperRange).Should().BeTrue();
        }

        [Fact]
        public void FilterReceiptsByPeriod_InputInvalid_NoReceiptsReturned()
        {
            // Arrange
            var testLowerRange = DateTime.Now;
            var testUpperRange = DateTime.MinValue;

            // Act
            var actionResult = _sut.FilterReceiptsByPeriod(testLowerRange, testUpperRange);

            // Assert
            actionResult.Should().BeOfType<BadRequestResult>();
        }
    }
}
