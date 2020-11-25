using Library.Engine.Interface;

namespace Library.Engine
{
    public class SalaryCalculator : ISalaryCalculator
    {
        public decimal RegularEmployee { get; set; } = 1.125M;
        public decimal Manager { get; set; } = 1.725M;
        public decimal CEO { get; set; } = 2.725M;

        public decimal RegularSalary(int rankNumber)
        {
            return rankNumber * RegularEmployee;
        }

        public decimal ManagerSalary(int rankNumber)
        {
            return rankNumber * Manager;
        }

        public decimal CEOSalary(int rankNumber)
        {
            return rankNumber * CEO;
        }
    }
}