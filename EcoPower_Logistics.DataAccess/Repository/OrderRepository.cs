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
    public class OrderRepository : Repository<Order>, IOrderRepository
    {

        public OrderRepository(SuperStoreContext db) : base(db)
        {
        }

        public void Update(Order order)
        {
            dbSet.Update(order);
        }
    }
}
