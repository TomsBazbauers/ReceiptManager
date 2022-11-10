using System;

namespace ReceiptManager.Core.Validations
{
    public class RequestValidator : IRequestValidator
    {
        public bool IsValid(string request)
        {
            return !string.IsNullOrEmpty(request.Trim());
        }

        public bool IsValidDate(DateTime lowerRange, DateTime upperRange)
        {
            return lowerRange != DateTime.MinValue
                && upperRange != DateTime.MinValue
                && lowerRange < upperRange;
        }
    }
}
