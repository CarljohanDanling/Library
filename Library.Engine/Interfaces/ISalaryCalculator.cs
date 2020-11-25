namespace Library.Engine.Interface
{
    public interface ISalaryCalculator
    {
        decimal RegularSalary(int rankNumber);
        decimal ManagerSalary(int rankNumber);
        decimal CEOSalary(int rankNumber);
    }
}