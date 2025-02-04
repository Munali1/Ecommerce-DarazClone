using System.IO;
using Ecommerce.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Linq;
using Ecommerce.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext context;
        private readonly IWebHostEnvironment env;

        public AdminController(AppDbContext _context, IWebHostEnvironment _env)
        {
            context = _context;
            env = _env;
        }

        public IActionResult Index()
        {
            string adminSession = HttpContext.Session.GetString("admin_session");
            if (adminSession != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string adminEmail, string adminPassword)
        {
            var admin = context.tbl_admin.FirstOrDefault(c => c.Email == adminEmail);
            if (admin != null && admin.Password == adminPassword)
            {
                HttpContext.Session.SetString("admin_session", admin.Id.ToString());
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Message = "Incorrect Username or Password";
                return View();
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("admin_session");
            return RedirectToAction("Login");
        }

        public IActionResult Profile()
        {
            var adminId = HttpContext.Session.GetString("admin_session");
            if (adminId == null) return RedirectToAction("Login");

            var ad = context.tbl_admin.FirstOrDefault(a => a.Id == int.Parse(adminId));
            if (ad == null) return NotFound();
            return View(ad);
        }

        [HttpPost]
        public IActionResult Profile(Admin admin, IFormFile Image)
        {
            if (Image != null)
            {
                string imagePath = Path.Combine(env.WebRootPath, "admin_Img", Path.GetFileName(Image.FileName));
                using (FileStream fs = new FileStream(imagePath, FileMode.Create))
                {
                    Image.CopyTo(fs);
                }
                admin.Image = Path.GetFileName(Image.FileName);
            }

            context.tbl_admin.Update(admin);
            context.SaveChanges();

            return RedirectToAction("Profile");
        }

        public IActionResult FetchCustomer()
        {
            var customers = context.tbl_Customer.ToList();
            return View(customers);
        }

        public IActionResult CustomerDetails(int id)
        {
            var cust = context.tbl_Customer.FirstOrDefault(x => x.Id == id);
            if (cust == null) return NotFound();
            return View(cust);
        }

        public IActionResult UpdateCustomer(int id)
        {
            var customer = context.tbl_Customer.FirstOrDefault(x => x.Id == id);
            if (customer == null) return NotFound();
            return View(customer);
        }
        [HttpPost]
        public IActionResult UpdateCustomer(Customer customer, IFormFile Image, string ExistingImage)
        {
            if (Image != null)
            {
                string imagePath = Path.Combine(env.WebRootPath, "customer_image", Path.GetFileName(Image.FileName));
                using (FileStream fs = new FileStream(imagePath, FileMode.Create))
                {
                    Image.CopyTo(fs);
                }
                customer.Image = Path.GetFileName(Image.FileName);
            }
            else
            {
                customer.Image = ExistingImage;
            }
            context.tbl_Customer.Update(customer);
            context.SaveChanges();
            return RedirectToAction("FetchCustomer");
        }


        public IActionResult DeleteCustomer(int id)
        {
            var customer = context.tbl_Customer.FirstOrDefault(x => x.Id == id);
            if (customer != null)
            {
                context.tbl_Customer.Remove(customer);
                context.SaveChanges();
            }
            return RedirectToAction("FetchCustomer");
        }
        public IActionResult FetchCategory()
        {
            return View(context.tbl_Category.ToList());
        }
        public IActionResult AddCategory()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddCategory(Category category)
        {
            context.tbl_Category.Add(category);
            context.SaveChanges();
            return RedirectToAction("FetchCategory");
        }
        public IActionResult UpdateCategory(int Id)
        {
            var cat = context.tbl_Category.FirstOrDefault(x => x.Id == Id);
            return View(cat);
        }

        [HttpPost]
        public IActionResult UpdateCategory(Category cat)
        {
            context.tbl_Category.Update(cat);
            context.SaveChanges();
            return RedirectToAction("FetchCategory");
        }
        public IActionResult DeleteCategory(int id)
        {
            var category = context.tbl_Category.FirstOrDefault(y => y.Id == id);
            context.tbl_Category.Remove(category);
            context.SaveChanges();
            return RedirectToAction("FetchCategory");
        }
        public IActionResult FetchProduct()
        {
            return View(context.tbl_Product.ToList());
        }
        public IActionResult AddProduct()
        {
            List<Category> categories = context.tbl_Category.ToList();
            ViewData["category"] = categories;
            return View();
        }
        [HttpPost]
        public IActionResult AddProduct(Product product, IFormFile Image)
        {
            if (Image != null)
            {
                string imagePath = Path.Combine(env.WebRootPath, "product_image", Path.GetFileName(Image.FileName));
                using (FileStream fs = new FileStream(imagePath, FileMode.Create))
                {
                    Image.CopyTo(fs);
                }
                product.Image = Path.GetFileName(Image.FileName);
            }
            context.tbl_Product.Add(product);
            context.SaveChanges();
            return RedirectToAction("FetchProduct");
        }
        public IActionResult UpdateProduct(int id)
        {
            List<Category> categories = context.tbl_Category.ToList();
            ViewData["category"] = categories;
            var product = context.tbl_Product.FirstOrDefault(x => x.Id == id);
            ViewBag.selectedCategory = product.CategoryId;
            return View(product);
        }

        [HttpPost]
        public IActionResult UpdateProduct(Product product, IFormFile Image, string ExistingImage)
        {
            if (Image != null)
            {
                string imagePath = Path.Combine(env.WebRootPath, "product_image", Path.GetFileName(Image.FileName));
                using (FileStream fs = new FileStream(imagePath, FileMode.Create))
                {
                    Image.CopyTo(fs);
                }
                product.Image = Path.GetFileName(Image.FileName);
            }
            else
            {
                product.Image = ExistingImage;
            }
            context.tbl_Product.Update(product);
            context.SaveChanges();
            return RedirectToAction("FetchProduct");
        }
        public IActionResult DeleteProduct(int id)
        { 
            var product=context.tbl_Product.FirstOrDefault( x => x.Id == id);
            context.tbl_Product.Remove(product);
            context.SaveChanges();
            return RedirectToAction("FetchProduct");
        }
        public IActionResult ProductDetails(int id)
        {
            return View(context.tbl_Product.Include(p=>p.category).FirstOrDefault(p => p.Id == id));//include to add foreign key data to view
        }
        public IActionResult fetchFeedback()
        {
            return View(context.tbl_Feedback.ToList());
        }
        public IActionResult DeleteFeedback(int id)
        {
            var feed=context.tbl_Feedback.FirstOrDefault( x => x.Id == id); 
            context.tbl_Feedback.Remove(feed);
            context.SaveChanges();
            return RedirectToAction("fetchFeedback");
        }
        public IActionResult FetchCart()
        {
            var cart = context.tbl_Cart.Include(c => c.Products).Include(c => c.Customers).ToList();
            return View(cart);
        }
        public IActionResult DeleteCart(int id)
        {
            var cart = context.tbl_Cart.FirstOrDefault(x => x.Id == id);
            context.tbl_Cart.Remove(cart);
            context.SaveChanges();
            return RedirectToAction("FetchCart");
        }
        public IActionResult UpdateCart(int id)
        {
            var cart = context.tbl_Cart.FirstOrDefault(x => x.Id == id);
            if (cart == null)
            {
                return NotFound();
            }
            return View(cart);
        }

        [HttpPost]
        public IActionResult UpdateCart(Cart cart, int status)
        {
            var existingCart = context.tbl_Cart.FirstOrDefault(x => x.Id == cart.Id);
            if (existingCart == null)
            {
                return NotFound();
            }

            existingCart.Status = status;
            existingCart.Product_ID = cart.Product_ID;
            existingCart.Customer_ID = cart.Customer_ID;
            existingCart.Quantity = cart.Quantity;

            context.tbl_Cart.Update(existingCart);
            context.SaveChanges();

            return RedirectToAction("FetchCart");
        }
        public IActionResult FetchOrder()
        {
            var order= context.tbl_Order.ToList();
            return View(order);
        }


    }
}
