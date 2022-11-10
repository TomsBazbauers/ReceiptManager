using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ReceiptManager.Core.Models;
using ReceiptManager.Core.Services;
using ReceiptManager.Core.Validations;
using ReceiptManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ReceiptManager.Controllers
{
    [Route("user-menu")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IReceiptService _receiptService;
        private readonly IEnumerable<IItemValidator> _itemValidators;
        private readonly IRequestValidator _requestValidator;
        private readonly IMapper _autoMapper;

        public UserController(IReceiptService receiptService,
            IEnumerable<IItemValidator> itemValidators, IRequestValidator requestValidator, IMapper autoMapper)
        {
            _receiptService = receiptService;
            _itemValidators = itemValidators;
            _requestValidator = requestValidator;
            _autoMapper = autoMapper;
        }

        [HttpPost, Route("create-receipt")]
        public IActionResult CreateReceipt(ReceiptRequest request)
        {
            var newReceipt = _autoMapper.Map<Receipt>(request);

            if (!_itemValidators.All(validator => validator.IsValid(newReceipt.Items)))
            {
                return BadRequest();
            }

            var methodResult = _receiptService.CreateReceipt(newReceipt.Items);

            if (methodResult.Success)
            {
                request = _autoMapper.Map<ReceiptRequest>(newReceipt);
                return Created("", request);
            }

            return Problem(methodResult.FormattedErrors);
        }

        [HttpDelete, Route("delete-receipt/{id}")]
        public IActionResult DeleteReceipt(long id)
        {
            var receiptToDelete = _receiptService.GetReceiptById(id);

            if (receiptToDelete == null)
            {
                return NotFound(id);
            }

            var methodResult = _receiptService.DeleteReceipt(receiptToDelete.Id);

            if (methodResult.Success)
            {
                return Ok(id);
            }

            return Problem();
        }

        [HttpGet, Route("get-receipt/{id}")]
        public IActionResult GetReceipt(long id)
        {
            var requestedReceipt = _receiptService.GetReceiptById(id);

            if (requestedReceipt == null)
            {
                return NotFound(id);
            }

            var foundReceipt = _autoMapper.Map<ReceiptRequest>(requestedReceipt);

            return Ok(foundReceipt);
        }

        [HttpGet, Route("get-receipt/all")]
        public IActionResult GetAllReceipts()
        {
            var requestedReceipts = _receiptService.GetAllReceipts();

            if (!requestedReceipts.Any())
            {
                return NotFound();
            }

            var foundReceipts = requestedReceipts
                .Select(receipt => _autoMapper.Map<ReceiptRequest>(receipt)).ToList();

            return Ok(foundReceipts);
        }

        [HttpGet, Route("filter-receipts/{productName}")]
        public IActionResult FilterReceiptsByItem(string productName)
        {
            if (!_requestValidator.IsValid(productName))
            {
                return BadRequest(productName);
            }

            var requestedReceipts = _receiptService.FilterReceiptsByItem(productName);

            if (!requestedReceipts.Any())
            {
                return NotFound();
            }

            var foundReceipts = requestedReceipts
                .Select(receipt => _autoMapper.Map<ReceiptRequest>(receipt)).ToList();

            return Ok(requestedReceipts);
        }

        [HttpGet, Route("filter-receipts/date")]
        public IActionResult FilterReceiptsByPeriod(DateTime lowerRange, DateTime upperRange)
        {
            if (!_requestValidator.IsValidDate(lowerRange, upperRange))
            {
                return BadRequest();
            }

            var requestedReceipts = _receiptService.FilterReceiptsByPeriod(lowerRange, upperRange);

            if (!requestedReceipts.Any())
            {
                return NotFound();
            }

            var foundReceipts = requestedReceipts
                .Select(receipt => _autoMapper.Map<ReceiptRequest>(receipt)).ToList();

            return Ok(requestedReceipts);
        }
    }
}