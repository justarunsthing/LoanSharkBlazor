namespace LoanSharkBlazor.Helpers
{
    public static class LoanHelper
    {
        /// <summary>
        /// Calculates a payment for a simple interest loan.
        /// </summary>
        /// <param name="amount">Loan Amount</param>
        /// <param name="rate">Anualized rate as a double</param>
        /// <param name="term">Term in years</param>
        /// <returns>A monthly payment as a double</returns>
        public static double CalculatePayment(double amount, double rate, double term)
        {
            var monthlyRate = CalculateMonthlyRate(rate);
            var months = term * 12;
            var payment = (amount * monthlyRate) / (1 - Math.Pow(1 + monthlyRate, -months));

            return payment;
        }

        private static double CalculateMonthlyRate(double rate)
        {
            return rate / 1200;
        }
    }
}