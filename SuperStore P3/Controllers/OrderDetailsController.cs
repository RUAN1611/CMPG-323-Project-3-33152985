using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Data;
using Models;
using EcoPower_Logistics.DataAccess.Repository.IRepository;

namespace Controllers
{
    [Authorize]
    public class OrderDetailsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderDetailsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: OrderDetails
        public async Task<IActionResult> Index()
        {
            var superStoreContext = _unitOfWork.OrderDetailsRepository.GetAll(includeEntities:"Order,Product");
            return View(await superStoreContext);
        }

        // GET: OrderDetails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _unitOfWork.OrderDetailsRepository == null)
            {
                return NotFound();
            }

            var orderDetail = await _unitOfWork.OrderDetailsRepository.Get(x => x.OrderDetailsId == id, includeEntities:"Order,Product");
            if (orderDetail == null)
            {
                return NotFound();
            }

            return View(orderDetail);
        }

        // GET: OrderDetails/Create
        public async Task<IActionResult> Create()
        {
            ViewData["OrderId"] = new SelectList(await _unitOfWork.OrderRepository.GetAll(), "OrderId", "OrderId");
            ViewData["ProductId"] = new SelectList(await _unitOfWork.ProductRepository.GetAll(), "ProductId", "ProductId");
            return View();
        }

        // POST: OrderDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderDetailsId,OrderId,ProductId,Quantity,Discount")] OrderDetail orderDetail)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.OrderDetailsRepository.Add(orderDetail);
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            ViewData["OrderId"] = new SelectList(await _unitOfWork.OrderRepository.GetAll(), "OrderId", "OrderId", orderDetail.OrderId);
            ViewData["ProductId"] = new SelectList(await _unitOfWork.ProductRepository.GetAll(), "ProductId", "ProductId", orderDetail.ProductId);
            return View(orderDetail);
        }

        // GET: OrderDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _unitOfWork.OrderDetailsRepository == null)
            {
                return NotFound();
            }

            var orderDetail = await _unitOfWork.OrderDetailsRepository.GetById(id);
            if (orderDetail == null)
            {
                return NotFound();
            }
            ViewData["OrderId"] = new SelectList(await _unitOfWork.OrderRepository.GetAll(), "OrderId", "OrderId", orderDetail.OrderId);
            ViewData["ProductId"] = new SelectList(await _unitOfWork.ProductRepository.GetAll(), "ProductId", "ProductId", orderDetail.ProductId);
            return View(orderDetail);
        }

        // POST: OrderDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderDetailsId,OrderId,ProductId,Quantity,Discount")] OrderDetail orderDetail)
        {
            if (id != orderDetail.OrderDetailsId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.OrderDetailsRepository.Update(orderDetail);
                    _unitOfWork.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderDetailExists(orderDetail.OrderDetailsId))
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
            ViewData["OrderId"] = new SelectList(await _unitOfWork.OrderRepository.GetAll(), "OrderId", "OrderId", orderDetail.OrderId);
            ViewData["ProductId"] = new SelectList(await _unitOfWork.ProductRepository.GetAll(), "ProductId", "ProductId", orderDetail.ProductId);
            return View(orderDetail);
        }

        // GET: OrderDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _unitOfWork.OrderDetailsRepository == null)
            {
                return NotFound();
            }

            var orderDetail = _unitOfWork.OrderDetailsRepository.Get(x => x.OrderDetailsId == id, includeEntities: "Order,Product");
            if (orderDetail == null)
            {
                return NotFound();
            }

            return View(orderDetail);
        }

        // POST: OrderDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_unitOfWork.OrderDetailsRepository == null)
            {
                return Problem("Entity set 'SuperStoreContext.OrderDetails'  is null.");
            }
            var orderDetail = await _unitOfWork.OrderDetailsRepository.GetById(id);
            if (orderDetail != null)
            {
                _unitOfWork.OrderDetailsRepository.Remove(orderDetail);
            }

            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderDetailExists(int id)
        {
            return (_unitOfWork.OrderDetailsRepository.Exists(e => e.OrderDetailsId == id));
        }
    }
}
