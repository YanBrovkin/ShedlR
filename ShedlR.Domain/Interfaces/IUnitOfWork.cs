using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShedlR.Domain.Repository;

namespace ShedlR.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        //TRepo Repository<TRepo, TItem>() where TRepo : IRepository<TItem>;

        void Commit();
        void RollBack();
    }
}
