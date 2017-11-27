using AussieLink.Contracts.Repositories;

namespace AussieLink.Contracts.UnitOfWork
{
    public interface IJobPostUnitOfWork
    {
        IJobDayRepository JobDays { get; }
        IJobPostRepository JobPosts { get; }
        ISalaryRepository Salaries { get; }

        int Complete();
    }
}