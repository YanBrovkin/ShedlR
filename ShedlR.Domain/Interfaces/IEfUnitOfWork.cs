using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShedlR.Domain.Interfaces
{
    public interface IEfUnitOfWork: IUnitOfWork
    {
        DbContext Session { get; }
    }
}
