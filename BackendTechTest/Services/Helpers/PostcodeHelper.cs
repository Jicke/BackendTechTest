namespace BackendTechTest.Services.Helpers
{
    public static class PostcodeHelper
    {
        public static void ValidatePostcode(string postcode)
        {
            if (!IsValidPostcode(postcode))
                throw new ArgumentException("Invalid postcode format.");
        }

        private static bool IsValidPostcode(string postcode)
        {
            var postcodeParts = postcode.Split(' ', 2);
            if (postcodeParts.Length != 2)
                return false;

            var outerPostcode = postcodeParts[0];
            if (outerPostcode.Length < 2 || outerPostcode.Length > 4 || !char.IsLetter(outerPostcode[0]))
                return false;

            var innerPostcode = postcodeParts[1];
            if (innerPostcode.Length != 3 || !char.IsDigit(innerPostcode[0]))
                return false;

            return true;
        }
    }
}
