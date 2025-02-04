using Ecommerce.Models;
using Ecommerce.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Ecommerce.Controllers
{
    public class CustomerController : Controller
    {
        private readonly AppDbContext context;
        private readonly IWebHostEnvironment env;

        public CustomerController(AppDbContext _context, IWebHostEnvironment _env)
        {
            context = _context;
            env = _env;
        }

        public IActionResult Index()
        {
            List<Category> categories = context.tbl_Category.ToList();
            ViewData["category"] = categories;
            List<Product> products = context.tbl_Product.ToList();
            ViewData["product"] = products;
            ViewBag.checkSession = HttpContext.Session.GetString("customer_session");
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(Customer customer)
        {
            context.tbl_Customer.Add(customer);
            context.SaveChanges();
            return RedirectToAction("Login");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string Email, string Password)
        {
            var customer = context.tbl_Customer.FirstOrDefault(c => c.Email == Email);
            if (customer != null && customer.Password == Password)
            {
                HttpContext.Session.SetString("customer_session", customer.Id.ToString());
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.message = "Incorrect Username or Password";
                return View();
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("customer_session");
            return RedirectToAction("Index");
        }

        public IActionResult CustomerProfile()
        {
            var customerId = HttpContext.Session.GetString("customer_session");
            if (customerId == null) return RedirectToAction("Login");

            var ad = context.tbl_Customer.FirstOrDefault(a => a.Id == int.Parse(customerId));
            if (ad == null) return NotFound();
            return View(ad);
        }

        [HttpPost]
        public IActionResult CustomerProfile(Customer customer, IFormFile Image, string ExistingImage)
        {
            var customerId = HttpContext.Session.GetString("customer_session");
            if (customerId == null) return RedirectToAction("Login");

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
            return RedirectToAction("CustomerProfile");
        }
        public IActionResult Feedback()
        {
           
            var customerId = HttpContext.Session.GetString("customer_session");
            if (customerId == null) return RedirectToAction("Login");
            return View();
        }
        [HttpPost]
        public IActionResult Feedback(Feedback feedback)
        {
            var customerId = HttpContext.Session.GetString("customer_session");
            if (customerId == null) return RedirectToAction("Login");

            TempData["message"] = "Thankyou for your Feedback";
            context.tbl_Feedback.Add(feedback);
            context.SaveChanges();
            return RedirectToAction("Feedback");
        }
        public IActionResult ViewProducts()
        {
          

            List<Product> products = context.tbl_Product.ToList();
            ViewData["product"] = products;
            return View();
        }
        public IActionResult ProductDetails(int id)
        {
          
            var pro = context.tbl_Product.Include(x=>x.category).FirstOrDefault(x=>x.Id==id);
            if (pro != null)
            {
                return View(pro);
            }
            return RedirectToAction("Index");
        }
        public IActionResult AddToCart(int Product_ID,Cart cart)
        {
            string isLogin=HttpContext.Session.GetString("customer_session");
            if (isLogin != null) 
            {
                cart.Product_ID = Product_ID;
                cart.Customer_ID = int.Parse(isLogin);
                cart.Quantity = 1;
                cart.Status = 0;
                context.tbl_Cart.Add(cart);
                context.SaveChanges();
                TempData["message"] = "Product Sucesfully added in Cart";
                return RedirectToAction("ViewProducts");
               
            }
            else
            {
                return RedirectToAction("Login");
            }
            
        }
        public IActionResult FetchCart()
        {
            string customerId = HttpContext.Session.GetString("customer_session");
            if (string.IsNullOrEmpty(customerId))
            {
                return RedirectToAction("Login", "Customer"); // Redirect to login if customer session is not set
            }

            var carts = context.tbl_Cart.Include(c => c.Products)
                                        .Where(c => c.Customer_ID == int.Parse(customerId))
                                        .ToList();

            return View(carts);
        }
        public IActionResult removeProduct(int id)
        {
            var customerId = HttpContext.Session.GetString("customer_session");
            if (customerId == null) return RedirectToAction("Login");

            var cart = context.tbl_Cart.Find(id);
            context.tbl_Cart.Remove(cart);
            context.SaveChanges();
            return RedirectToAction("FetchCart");
        }


            public IActionResult Checkout(int id)
            {
            var customerId = HttpContext.Session.GetString("customer_session");
            if (customerId == null) return RedirectToAction("Login");

            var cart = context.tbl_Cart.Include(c => c.Products).FirstOrDefault(c => c.Id == id);
                if (cart == null)
                {
                    return NotFound();
                }
                return View(cart);
            }

            [HttpPost]
            public IActionResult Checkout(int id, string shippingAddress)
            {
            var customerId = HttpContext.Session.GetString("customer_session");
            if (customerId == null) return RedirectToAction("Login");

            var cart = context.tbl_Cart.Include(c => c.Products).FirstOrDefault(c => c.Id == id);
                if (cart == null)
                {
                    return NotFound();
                }

                // Simulate checkout process (e.g., create an order, remove the item from cart)
                var order = new Order
                {
                    Product_ID = cart.Product_ID,
                    Customer_ID = cart.Customer_ID,
                    Quantity = cart.Quantity,
                    TotalPrice = cart.Products.Price * cart.Quantity,
                    ShippingAddress = shippingAddress,
                    OrderDate = DateTime.Now
                };
                context.tbl_Order.Add(order);
                context.tbl_Cart.Remove(cart);
                context.SaveChanges();

                return RedirectToAction("OrderConfirmation", new { id = order.Id });
            }

            public IActionResult OrderConfirmation(int id)
            {
            var customerId = HttpContext.Session.GetString("customer_session");
            if (customerId == null) return RedirectToAction("Login");

            var order = context.tbl_Order.Include(o => o.Products).FirstOrDefault(o => o.Id == id);
                if (order == null)
                {
                    return NotFound();
                }
                return View(order);
            }
        public IActionResult ProcessPayment(string CardName, string CardNumber, string ExpiryDate, string CVV)
        {
            // Process the payment here (this is just a placeholder)
            // You might need to integrate with a payment gateway API

            // Assuming payment is successful, redirect to a confirmation page
            return RedirectToAction("PaymentSuccess");
        }

        public IActionResult PaymentSuccess()
        {
            var customerId = HttpContext.Session.GetString("customer_session");
            if (customerId == null) return RedirectToAction("Login");

            return View();
        }
        public IActionResult Search(string query)
        {
            var results = context.tbl_Product
                .Include(p => p.category)
                .Where(p => p.Name.Contains(query) || p.Description.Contains(query) || p.category.category_Name.Contains(query))
                .ToList();
            return View(results);
        }

    }
}

