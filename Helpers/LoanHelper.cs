using LoanSharkBlazor.Models;

namespace LoanSharkBlazor.Helpers
{
    public static class LoanHelper
    {
        /// <summary>
        /// Caculate the monthly payment schedule for a loan
        /// </summary>
        /// <param name="loan"></param>
        /// <returns>Returns a loan object</returns>
        public static Loan GetPayments(Loan loan)
        {
            double balance = loan.PurchaseAmount;
            double totalInterest = 0.0;
            double monthlyPrincipal = 0.0;
            double monthlyInterest = 0.0;
            double monthlyRate = CalculateMonthlyRate(loan.Rate);
            var loanMonths = loan.Term * 12;

            loan.Payment = CalculatePayment(loan.PurchaseAmount, loan.Rate, loan.Term);
            loan.AmortizationSchedule.Clear();

            for (int month = 1; month <= loanMonths; month++)
            {
                monthlyInterest = CalculateMonthlyInterest(balance, monthlyRate);
                totalInterest += monthlyInterest;
                monthlyPrincipal = loan.Payment - monthlyInterest;
                balance -= monthlyPrincipal;

                var loanPayment = new LoanPayment
                {
                    Month = month,
                    Payment = loan.Payment,
                    MonthlyPrincipal = monthlyPrincipal,
                    MonthlyInterest = monthlyInterest,
                    TotalInterest = totalInterest,
                    Balance = balance < 0 ? 0 : balance
                };

                loan.AmortizationSchedule.Add(loanPayment);
            }

            loan.TotalInterest = totalInterest;
            loan.TotalCost = loan.PurchaseAmount + totalInterest;

            return loan;
        }

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

        private static double CalculateMonthlyInterest(double balance, double monthlyRate)
        {
            return balance * monthlyRate;
        }
    }
}