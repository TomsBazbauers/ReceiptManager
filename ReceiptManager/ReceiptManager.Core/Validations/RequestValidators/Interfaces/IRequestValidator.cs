using System;

namespace ReceiptManager.Core.Validations
{
    public interface IRequestValidator
    {
        bool IsValid(string request);

        bool IsValidDate(DateTime lowerRange, DateTime upperRange);
    }
}
