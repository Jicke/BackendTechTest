namespace BackendTechTest.Services.Helpers
{
    public static class NhsNumberHelper
    {
        const int MaximumDigits = 10;

        public static void ValidateNhsNumber(long nhsNumber)
        {
            if (!IsValidNhsNumber(nhsNumber))
                throw new ArgumentException("Invalid NHS number.");
        }

        private static bool IsValidNhsNumber(long nhsNumber)
        {
            string nhsNumberString = nhsNumber.ToString();

            if (nhsNumberString.Length != MaximumDigits)
                return false;

            int sum = 0;

            for (int i = 0; i < MaximumDigits; i++)
            {
                int digit = nhsNumberString[i] - '0';
                int factor = MaximumDigits - i;
                sum += digit * factor;
            }

            int remainder = sum % 11;

            int checkDigit = 11 - remainder;

            if (checkDigit == 10)
                return false;

            if (checkDigit == 11)
                checkDigit = 0;

            return remainder == checkDigit;
        }
    }
}
