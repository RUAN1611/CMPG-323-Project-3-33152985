﻿using Data;
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
        private readonly SuperStoreContext _db; // Unit of Work must have DbContext present.
        public ICustomerRepository CustomerRepository { get; private set; }
        public IOrderRepository OrderRepository { get; private set; }
        public IOrderDetailsRepository OrderDetailsRepository { get; private set; }
        public IProductRepository ProductRepository { get; private set; }

        public UnitOfWork(SuperStoreContext db)
        {
            _db = db;
            CustomerRepository = new CustomerRepository(_db);
            OrderRepository = new OrderRepository(_db);
            OrderDetailsRepository = new OrderDetailsRepository(_db);
            ProductRepository = new ProductRepository(_db);
        }

        public void Save()
        {
            _db.SaveChanges(); // Because this is in Unit of Work, any controller accessing Unit of Work may save changes in database.
        }
    }
}
