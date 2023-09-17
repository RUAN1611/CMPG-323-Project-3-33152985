using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Models;
using Data;
using EcoPower_Logistics.DataAccess.Repository.IRepository;

namespace Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrdersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            var superStoreContext = _unitOfWork.OrderRepository.GetAll(includeEntities:"Customer");
            return View(await superStoreContext);
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _unitOfWork.OrderRepository == null)
            {
                return NotFound();
            }

            var order = await _unitOfWork.OrderRepository.Get(m => m.OrderId == id, includeEntities:"Customer");
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        public async Task<IActionResult> Create()
        {
            ViewData["CustomerId"] = new SelectList(await _unitOfWork.CustomerRepository.GetAll(), "CustomerId", "CustomerId");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderId,OrderDate,CustomerId,DeliveryAddress")] Order order)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.OrderRepository.Add(order);
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(await _unitOfWork.CustomerRepository.GetAll(), "CustomerId", "CustomerId", order.CustomerId);
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _unitOfWork.OrderRepository == null)
            {
                return NotFound();
            }

            var order = await _unitOfWork.OrderRepository.GetById(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(await _unitOfWork.CustomerRepository.GetAll(), "CustomerId", "CustomerId", order.CustomerId);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderId,OrderDate,CustomerId,DeliveryAddress")] Order order)
        {
            if (id != order.OrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.OrderRepository.Update(order);
                    _unitOfWork.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(await _unitOfWork.CustomerRepository.GetAll(), "CustomerId", "CustomerId", order.CustomerId);
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _unitOfWork.OrderRepository == null)
            {
                return NotFound();
            }

            var order = await _unitOfWork.OrderRepository.Get(m => m.OrderId == id, includeEntities:"Customer");
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_unitOfWork.OrderRepository == null)
            {
                return Problem("Entity set 'SuperStoreContext.Orders'  is null.");
            }
            var order = await _unitOfWork.OrderRepository.GetById(id);
            if (order != null)
            {
                _unitOfWork.OrderRepository.Remove(order);
            }

            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return (_unitOfWork.OrderRepository.Exists(e => e.OrderId == id));
        }
    }
}
