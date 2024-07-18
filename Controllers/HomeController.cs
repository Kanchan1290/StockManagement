using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StockManagementSystem.Models;
using StockManagementSystem.Security;
using System.Diagnostics;

namespace StockManagementSystem.Controllers
{
    public class HomeController : Controller
    {
       
        private readonly StockmanagementContext _context;
        private readonly IDataProtector _protector;
        private readonly IWebHostEnvironment _env;

        public HomeController(StockmanagementContext context, DataSecurityKey key, IDataProtectionProvider provider, IWebHostEnvironment env)
        {
            _context = context;
            _protector = provider.CreateProtector(key.key);
            _env = env;
        }

        public IActionResult Index()
        {
            var prod = _context.Products.ToList();
            var u = prod.Select(e => new ProductEdit
            {
                PId = e.PId,
                PName = e.PName,
                PPrice = e.PPrice,
                Pimage = e.Pimage,
                SaleQuantity = e.SaleQuantity,
                PurchaseOuantity = e.PurchaseOuantity,
                Stock = e.Stock,
/*                EncID = _protector.Protect(e.PId.ToString())
*/            }).ToList();
            return View(u);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            var productEdit = new ProductEdit();

            // Fetch categories from the database
            var prodCategories = _context.PodCategories.ToList();
            ViewData["cate"] = new SelectList(prodCategories, nameof(PodCategory.PcId), nameof(PodCategory.PcName));

            // Fetch suppliers from the database
            var suppliers = _context.Suppliers.ToList();
            ViewData["suppliers"] = new SelectList(suppliers, nameof(Supplier.SId), nameof(Supplier.SName));

            return View(productEdit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ProductEdit m)
        {
            short maxid;

            if (_context.Products.Any())
                maxid = Convert.ToInt16(_context.Products.Max(x => x.PId) + 1);
            else
                maxid = 1;
            m.PId = maxid;

            if (m.ProductFile != null)
            {
                string fileName = "ProductImage" + Guid.NewGuid() + Path.GetExtension(m.ProductFile.FileName);
                string filePath = Path.Combine(_env.WebRootPath, "ProductImage", fileName);
                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    m.ProductFile.CopyTo(stream);
                }
                m.Pimage = fileName;
            }

            Product product = new()
            {
                PId = m.PId,
                PName = m.PName,
                PPrice = m.PPrice,
                Pimage = m.Pimage,
                SaleQuantity = m.SaleQuantity,
                PurchaseOuantity = m.PurchaseOuantity,
                Stock = m.Stock,
                CategoryId = m.CategoryId,
                SupplierId = m.SupplierId
            };

            _context.Products.Add(product);
            _context.SaveChanges();
            return RedirectToAction("Index");

            
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var product= _context.Products.Where(e=> e.PId == id).FirstOrDefault();
            if (product == null)
            {
                return NotFound();
            }

            var prodCategories = _context.PodCategories.ToList();
            ViewData["cate"] = new SelectList(prodCategories, nameof(PodCategory.PcId), nameof(PodCategory.PcName));

            // Fetch suppliers from the database
            var suppliers = _context.Suppliers.ToList();
            ViewData["suppliers"] = new SelectList(suppliers, nameof(Supplier.SId), nameof(Supplier.SName));


            ProductEdit model = new ProductEdit
            {
                PId = product.PId,
                PName = product.PName,
                PPrice = product.PPrice,
                Pimage = product.Pimage,
                SaleQuantity = product.SaleQuantity,
                PurchaseOuantity = product.PurchaseOuantity,
                Stock = product.Stock,
                CategoryId = product.CategoryId,
                SupplierId = product.SupplierId
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ProductEdit m)
        {
            var product = _context.Products.Find(m.PId);
            if (product == null)
            {
                return NotFound();
            }

            if (m.ProductFile != null)
            {
                string fileName = "ProductImage" + Guid.NewGuid() + Path.GetExtension(m.ProductFile.FileName);
                string filePath = Path.Combine(_env.WebRootPath, "ProductImage", fileName);
                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    m.ProductFile.CopyTo(stream);
                }
                m.Pimage = fileName;
            }

            product.PName = m.PName;
            product.PPrice = m.PPrice;
            product.Pimage = m.Pimage;
            product.SaleQuantity = m.SaleQuantity;
            product.PurchaseOuantity = m.PurchaseOuantity;
            product.Stock = m.Stock;
            product.CategoryId = m.CategoryId;
            product.SupplierId = m.SupplierId;

            _context.Products.Update(product);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            
            var u = _context.Products.Where(x => x.PId == id).FirstOrDefault();

            if (u == null)
            {
                return NotFound();
            }
            _context.Products.Remove(u);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult ProductList()
        {

            ViewData["cat"] = new SelectList(_context.PodCategories, nameof(PodCategory.PcId), nameof(PodCategory.PcName));
            return View();

        }


        public IActionResult GetProductList(int PcId)
        {
            var product = _context.Products.Where(x => x.CategoryId == PcId).ToList();
            return PartialView("_ProductList", product);
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
