using AussieLink.Contracts.Repositories;
using AussieLink.Contracts.UnitOfWork;
using AussieLink.DAL.Data;
using AussieLink.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AussieLink.DAL.UnitOfWork
{
    public class JobPostUnitOfWork : IJobPostUnitOfWork
    {
        private readonly DataContext Context;
        public IJobPostRepository JobPosts { get; }
        public IJobDayRepository JobDays { get; }
        public ISalaryRepository Salaries { get; }

        public JobPostUnitOfWork(DataContext context)
        {
            this.Context = context;
            JobPosts = new JobPostRepository(context);
            JobDays = new JobDayRepository(context);
            Salaries = new SalaryRepository(context);
        }

        public int Complete()
        {
            return Context.SaveChanges();
        }

        public void Dispose()
        {
            Context.Dispose();
        }
    }
}
