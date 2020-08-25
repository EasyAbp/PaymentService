namespace EasyAbp.PaymentService
{
    public static class DecimalExtensions
    {
        public static decimal EnsureIsNonNegative(this decimal number)
        {
            if (number < decimal.Zero)
            {
                throw new UnexpectedNumberException(number);
            }
            
            return number;
        }
    }
}