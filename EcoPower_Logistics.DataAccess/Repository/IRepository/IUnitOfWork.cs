using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoPower_Logistics.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        ICustomerRepository CustomerRepository { get; }
        void Save();
    }
}
