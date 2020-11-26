using Library.Engine.Interface;

namespace Library.Engine
{
    public class SalaryCalculator : ISalaryCalculator
    {
        // This is the salary calculator. I filled it with two types of calculations.
        // One to convert the rank from the salary. This is because its nice to see it
        // in the views form. The other type is the standard salary calculation that is 
        // on the the criteries.

        public decimal RegularEmployee { get; set; } = 1.125M;
        public decimal Manager { get; set; } = 1.725M;
        public decimal CEO { get; set; } = 2.725M;

        public int CalculateRankForRegular(decimal salary)
        {
            return decimal.ToInt32(salary / RegularEmployee);
        }

        public int CalculateRankForManager(decimal salary)
        {
            return decimal.ToInt32(salary / Manager);
        }

        public int CalculateRankForCEO(decimal salary)
        {
            return decimal.ToInt32(salary / CEO);
        }

        public decimal CalculateRegularSalary(int rankNumber)
        {
            return rankNumber * RegularEmployee;
        }

        public decimal CalculateManagerSalary(int rankNumber)
        {
            return rankNumber * Manager;
        }

        public decimal CalculateCEOSalary(int rankNumber)
        {
            return rankNumber * CEO;
        }
    }
}