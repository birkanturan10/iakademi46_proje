using Microsoft.AspNetCore.Mvc;
using iakademi46_proje.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace iakademi46_proje.Controllers
{
    public class AdminController : Controller
    {
        cls_User u = new cls_User();
        cls_Category c = new cls_Category();
        iakademi46Context context = new iakademi46Context();
        cls_Supplier s = new cls_Supplier();
        cls_Status st = new cls_Status();
        cls_Product p = new cls_Product();

        

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("Email,Password,NameSurname")] User user)
        {
            if (ModelState.IsValid)
            {
                User? usr = await u.loginControl(user);
                if (usr != null)
                {
                    return RedirectToAction("Index");
                }
            }
            else
            {
                ViewBag.error = "Login ve/veya şifre yanlış";
            }
            return View();
        }


        public IActionResult Index()
        {
            return View();
        }

       

        public async Task<IActionResult> CategoryIndex()
        {
            List<Category> categories = await c.CategorySelect();
            return View(categories);
        }

        [HttpGet]
        public IActionResult CategoryCreate()
        {
            CategoryFill();
            return View();
        }

        void CategoryFill()
        {
            List<Category> categories = c.CategorySelectMain();
            ViewData["categoryList"] = categories.Select(c => new SelectListItem { Text = c.CategoryName, Value = c.CategoryID.ToString() });
        }

        async void SupplierFill()
        {
            List<Supplier> suppliers = await s.SupplierSelect();
            ViewData["supplierList"] = suppliers.Select(s => new SelectListItem { Text = s.BrandName, Value = s.SupplierID.ToString() });
        }

        async void StatusFill()
        {
            List<Status> statuses = await st.StatusSelect();
            ViewData["statusList"] = statuses.Select(s => new SelectListItem { Text = s.StatusName, Value = s.StatusID.ToString() });
        }

        [HttpPost]
        public IActionResult CategoryCreate(Category category)
        {
            bool answer = cls_Category.CategoryInsert(category);

            if (answer)
            {
                TempData["Message"] = "Kategori Eklendi";
               // return RedirectToAction(nameof(CategoryCreate));
            }
            else
            {
                TempData["Message"] = "HATA!!! Kategori Eklenemedi";
            }
            return RedirectToAction(nameof(CategoryCreate));
        }


        public async Task<IActionResult> CategoryEdit(int? id)
        {
            CategoryFill();
            if (id == null || context.Categories == null)
            {
                return NotFound();
            }

            var category = await c.CategoryDetails(id);
            return View(category);
        }


        [HttpPost]
        public IActionResult CategoryEdit(Category category)
        {
            bool answer = cls_Category.CategoryUpdate(category);

            if (answer)
            {
                TempData["Message"] = "Kategori Güncellendi";
                return RedirectToAction(nameof(CategoryIndex));
            }
            else
            {
                TempData["Message"] = "HATA!!! Kategori Güncellenemedi";
            }
            return RedirectToAction(nameof(CategoryEdit));
          //  return RedirectToAction("CategoryEdit"));
        }


        public async Task<IActionResult> CategoryDetails(int? id)
        {
            var category = await c.CategoryDetails(id);
            //menuden tekrar tıklanamacak
            ViewBag.category = category?.CategoryName;
            return View(category);
        }

        [HttpGet]
        public async Task<IActionResult> CategoryDelete(int? id)
        {
            if (id == null || context.Categories == null)
            {
                return NotFound();
            }

            var category = await context.Categories.FirstOrDefaultAsync(c => c.CategoryID == id);

            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }


        [HttpPost,ActionName("CategoryDelete")]
        public async Task<IActionResult> CategoryDeleteConfirmed(int id)
        {
            bool answer = cls_Category.CategoryDelete(id);

            if (answer)
            {
                TempData["Message"] = "Kategori Silindi";
                return RedirectToAction(nameof(CategoryIndex));
            }
            else
            {
                TempData["Message"] = "HATA!!! Kategori Silinemedi";
            }
            return RedirectToAction(nameof(CategoryDelete));
        }


        public async Task<IActionResult> SupplierIndex()
        {
            List<Supplier> suppliers = await s.SupplierSelect();
            return View(suppliers);
        }


        [HttpGet]
        public IActionResult SupplierCreate()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SupplierCreate(Supplier supplier)
        {
            bool answer = cls_Supplier.SupplierInsert(supplier);

            if (answer)
            {
                TempData["Message"] = "Marka Eklendi";
            }
            else
            {
                TempData["Message"] = "HATA!!! Marka Eklenemedi";
            }
            return RedirectToAction(nameof(SupplierCreate));
        }


        private static int? staticID = 0;
        public async Task<IActionResult> SupplierEdit(int? id)
        {
            if (id == null || context.Suppliers == null)
            {
                return NotFound();
            }

            var supplier = await s.SupplierDetails(id);

            return View(supplier);
        }

        [HttpPost]
        public IActionResult SupplierEdit(Supplier supplier)
        {
            if (supplier.PhotoPath == null)
            {
                string? PhotoPath = context.Suppliers.FirstOrDefault(s => s.SupplierID == supplier.SupplierID).PhotoPath;
                supplier.PhotoPath = PhotoPath;
            }

            bool answer = cls_Supplier.SupplierUpdate(supplier);
            if (answer == true)
            {
                //türkce karakter sorunu icin,Program.cs icinde,
                //builder.Services.AddWebEncoders   eklendi
                TempData["Message"] = "Güncellendi";
                return RedirectToAction("SupplierIndex");
            }
            else
            {
                TempData["Message"] = "HATA";
                return RedirectToAction(nameof(SupplierEdit));
            }
        }


        public async Task<IActionResult> SupplierDetails(int? id)
        {
            if (id != null)
            {
                staticID = id;
            }

            if (id == null)
            {
                id = staticID;
            }
            var supplier = await s.SupplierDetails(id);
            //menuden tekrar tıklanamayacak , onun icin ? kullandım
            ViewBag.supplier = supplier?.BrandName;
            return View(supplier);
        }


        [HttpGet]
        public async Task<IActionResult> SupplierDelete(int? id)
        {
            if (id == null || context.Suppliers == null)
            {
                return NotFound();
            }

            var supplier = await context.Suppliers.FirstOrDefaultAsync(c => c.SupplierID == id);

            if (supplier == null)
            {
                return NotFound();
            }
            return View(supplier);
        }


        [HttpPost, ActionName("SupplierDelete")]
        public async Task<IActionResult> SupplierDeleteConfirmed(int id)
        {
            bool answer = cls_Supplier.SupplierDelete(id);

            if (answer)
            {
                TempData["Message"] = "Marka Silindi";
                return RedirectToAction(nameof(SupplierIndex));
            }
            else
            {
                TempData["Message"] = "HATA!!! Marka Silinemedi";
            }
            return RedirectToAction(nameof(SupplierIndex));
        }







        public async Task<IActionResult> StatusIndex()
        {
            List<Status> statuses = await st.StatusSelect();
            return View(statuses);
        }




        [HttpGet]
        public IActionResult StatusCreate()
        {
            return View();
        }

        [HttpPost]
        public IActionResult StatusCreate(Status status)
        {
            bool answer = cls_Status.StatusInsert(status);

            if (answer)
            {
                TempData["Message"] = "Durum Eklendi";
            }
            else
            {
                TempData["Message"] = "HATA!!! Durum Eklenemedi";
            }
            return RedirectToAction(nameof(StatusCreate));
        }


        public async Task<IActionResult> StatusEdit(int? id)
        {
            if (id == null || context.Statuses == null)
            {
                return NotFound();
            }

            var status = await st.StatusDetails(id);

            return View(status);
        }

        [HttpPost]
        public IActionResult StatusEdit(Status status)
        {
            bool answer = cls_Status.StatusUpdate(status);
            if (answer == true)
            {
                TempData["Message"] = "Güncellendi";
                return RedirectToAction("StatusIndex");
            }
            else
            {
                TempData["Message"] = "HATA";
                return RedirectToAction(nameof(StatusEdit));
            }
        }



        public async Task<IActionResult> StatusDetails(int? id)
        {
            if (id != null)
            {
                staticID = id;
            }

            if (id == null)
            {
                id = staticID;
            }
            var status = await st.StatusDetails(id);
            ViewBag.status = status?.StatusName;
            return View(status);
        }


        [HttpGet]
        public async Task<IActionResult> StatusDelete(int? id)
        {
            if (id == null || context.Statuses == null)
            {
                return NotFound();
            }

            var status = await context.Statuses.FirstOrDefaultAsync(c => c.StatusID == id);

            if (status == null)
            {
                return NotFound();
            }
            return View(status);
        }


        [HttpPost, ActionName("StatusDelete")]
        public async Task<IActionResult> StatusDeleteConfirmed(int id)
        {
            bool answer = cls_Status.StatusDelete(id);

            if (answer)
            {
                TempData["Message"] = "Durum Silindi";
                return RedirectToAction(nameof(StatusIndex));
            }
            else
            {
                TempData["Message"] = "HATA!!! Durum Silinemedi";
            }
            return RedirectToAction(nameof(StatusIndex));
        }



        public async Task<IActionResult> ProductIndex()
        {
            List<Product> products = await p.ProductSelect();
            return View(products);
        }



        [HttpGet]
        public async Task<IActionResult> ProductCreate()
        {
            CategoryFill();

            List<Supplier> suppliers = await s.SupplierSelect();
            ViewData["supplierList"] = suppliers.Select(s => new SelectListItem { Text = s.BrandName, Value = s.SupplierID.ToString() });

            List<Status> statuses = await st.StatusSelect();
            ViewData["statusList"] = statuses.Select(s => new SelectListItem { Text = s.StatusName, Value = s.StatusID.ToString() });

            return View();
        }

        [HttpPost]
        public IActionResult ProductCreate(Product product)
        {
            if (ModelState.IsValid)
            {
                bool answer = cls_Product.ProductInsert(product);

                if (answer)
                {
                    TempData["Message"] = "Ürün Eklendi";
                }
                else
                {
                    TempData["Message"] = "HATA!!! Ürün Eklenemedi";
                }
            }
            else
            {
                TempData["Message"] = "Zorunlu Alanları Doldurunuz.";
            }
            return RedirectToAction(nameof(ProductCreate)); //[HttpGet]
        }

        //ctrl + M + O = metodları kapatır
        //ctrl + M + L = acar


        [HttpGet]
        public async Task<IActionResult> ProductEdit(int? id)
        {
            CategoryFill();
            SupplierFill();
            StatusFill();
            if (id == null || context.Products == null)
            {
                return NotFound();
            }

            var product = await p.ProductDetails(id);
            return View(product);
        }


        [HttpPost]
        public IActionResult ProductEdit(Product product)
        {
            if (product.PhotoPath == null)
            {
                //eski resim aynen yerinde kalacak
                //veritabanından eski resmi alıp gelelim
                product.PhotoPath = context.Products.FirstOrDefault(p => p.ProductID == product.ProductID).PhotoPath;
            }

            bool answer = cls_Product.ProductUpdate(product);

            if (answer)
            {
                TempData["Message"] = "Ürün Güncellendi";
                return RedirectToAction(nameof(ProductIndex));
            }
            else
            {
                TempData["Message"] = "HATA!!! Ürün Güncellenemedi";
            }
            return RedirectToAction(nameof(ProductEdit)); // [HttpGet]
           // return View(); // [HttpPost]
            //  return RedirectToAction("CategoryEdit"));
        }



        [HttpGet]
        public async Task<IActionResult> ProductDelete(int? id)
        {
            if (id == null || context.Products == null)
            {
                return NotFound();
            }

            var product = await context.Products.FirstOrDefaultAsync(c => c.ProductID == id);

            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }


        [HttpPost, ActionName("ProductDelete")]
        public async Task<IActionResult> ProductDeleteConfirmed(int id)
        {
            bool answer = cls_Product.ProductDelete(id);

            if (answer)
            {
                TempData["Message"] = "Ürün Silindi";
                return RedirectToAction(nameof(ProductIndex));
            }
            else
            {
                TempData["Message"] = "HATA!!! Ürün Silinemedi";
            }
            return RedirectToAction(nameof(ProductDelete));
        }


        public async Task<IActionResult> ProductDetails(int? id)
        {
            var product = await p.ProductDetails(id);
            //menuden tekrar tıklanamacak
            ViewBag.product = product?.ProductName;
            return View(product);
        }


    }
}
