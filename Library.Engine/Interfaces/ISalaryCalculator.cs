namespace Library.Engine.Interface
{
    public interface ISalaryCalculator
    {
        int CalculateRankForRegular(decimal salary);
        int CalculateRankForManager(decimal salary);
        int CalculateRankForCEO(decimal salary);
        decimal CalculateRegularSalary(int rankNumber);
        decimal CalculateManagerSalary(int rankNumber);
        decimal CalculateCEOSalary(int rankNumber);
    }
}