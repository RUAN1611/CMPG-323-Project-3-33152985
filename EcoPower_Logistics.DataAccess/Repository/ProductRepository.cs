using Data;
using EcoPower_Logistics.DataAccess.Repository.IRepository;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoPower_Logistics.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(SuperStoreContext db) : base(db)
        {
        }

        public void Update(Product product)
        {
            dbSet.Update(product);
        }
    }
}
