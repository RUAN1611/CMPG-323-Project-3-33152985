using Data;
using EcoPower_Logistics.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoPower_Logistics.DataAccess.Repository
{
    public class UnitOfWork:IUnitOfWork
    {
        private readonly SuperStoreContext _db;
        public ICustomerRepository CustomerRepository { get; private set; }
        public IProductRepository ProductRepository { get; private set; }

        public UnitOfWork(SuperStoreContext db)
        {
            _db = db;
            CustomerRepository = new CustomerRepository(_db);
            ProductRepository = new ProductRepository(_db);
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
