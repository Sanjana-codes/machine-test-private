using MachineTestProject.Data;
using MachineTestProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;

namespace MachineTestProject.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductDbContext _context ;

        public ProductController(ProductDbContext context)
        {
            this._context = context;
        }


        public ActionResult Index(int page = 1, int pageSize = 10)
        {
            
            var totalProducts = _context.Products.Count();

            var products = _context.Products
                                    .Include(p => p.Category)
                                    .OrderBy(p => p.ProductId)
                                    .Skip((page - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToList();

            
            var totalPages = (int)Math.Ceiling((double)totalProducts / pageSize);

            var model = new ProductListViewModel
            {
                Products = products,
                CurrentPage = page,
                TotalPages = totalPages,
                PageSize = pageSize
            };

            return View(model);
        }

        public ActionResult Create()
        {
            var product = new Product();
            ViewBag.Categories = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            return View(product);
        }


        [HttpPost]
        public ActionResult Create(Product product)
        {
            if (!ModelState.IsValid)
            {
                foreach (var state in ModelState)
                {
                    Console.WriteLine($"Key: {state.Key}, Error: {string.Join(", ", state.Value.Errors.Select(e => e.ErrorMessage))}");
                }

              
                ViewBag.Categories = new SelectList(_context.Categories, "CategoryId", "CategoryName", product.CategoryId);

                return View(product);
            }

            
                _context.Products.Add(product);
                _context.SaveChanges();
                return RedirectToAction("Index");
           
        }



        public ActionResult Edit(int id)
        {
            var product = _context.Products.Find(id);
            ViewBag.Categories = new SelectList(_context.Categories, "CategoryId", "CategoryName", product.CategoryId);
            return View(product);
        }

        [HttpPost]
        public ActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Entry(product).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Categories = new SelectList(_context.Categories, "CategoryId", "CategoryName", product.CategoryId);
            return View(product);
        }

        public ActionResult Delete(int id)
        {
            var product = _context.Products.Find(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
