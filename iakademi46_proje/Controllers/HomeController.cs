using iakademi46_proje.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PagedList.Core;
using System.Collections.Specialized;
using System.Text;

namespace iakademi46_proje.Controllers
{
    public class HomeController : Controller
    {
        /* 
         1 = slider
         2 = özel
         3 = yıldız
         4 = fırsat
         5 = dikkat çeken
         6= günün ürünü
         adddate = yeni ürünler
         discount = indirimli
        öne cıkanlar= highlighted kolonu
        coksatanlar =  topseller kolonu
         */

        iakademi46Context context = new iakademi46Context();
        MainPageModel mpm = new MainPageModel();
        cls_Product cp = new cls_Product();
        cls_Order o = new cls_Order();
        cls_Category c = new cls_Category();
        cls_Supplier s = new cls_Supplier();
        string url = "";


        //public HomeController()
        //{
        //    ViewBag.x = "38.436273";
        //    ViewBag.y = "27.142299";
        //    ViewBag.telefon = context.Settings.FirstOrDefault(s => s.SettingID == 1).Telephone;
        //}

        public IActionResult Index()
        {
            mpm.SliderProducts = cp.ProductSelect("SliderProducts", "index", 0); //slider
            mpm.NewProducts = cp.ProductSelect("NewProducts", "index", 0); //yeni
            mpm.ProductOfDay = cp.ProductSelect_OfDay(); //günün ürünü
            mpm.SpecialProducts = cp.ProductSelect("SpecialProducts", "index", 0); //özel
            mpm.StarProducts = cp.ProductSelect("StarProducts", "index", 0); //yıldız
            mpm.FeaturedProducts = cp.ProductSelect("FeaturedProducts", "index", 0); //fırsat
            mpm.DiscountedProducts = cp.ProductSelect("DiscountedProducts", "index", 0); //indirimli
            mpm.HighlightedProducts = cp.ProductSelect("HighlightedProducts", "index", 0); //öne çıkanlar
            mpm.TopsellerProducts = cp.ProductSelect("TopsellerProducts", "index", 0); //çok satanlar
            mpm.NotableProducts = cp.ProductSelect("NotableProducts", "index", 0); //dikkat çeken

            return View(mpm);
        }

        public IActionResult NewProducts()
        {
            mpm.NewProducts = cp.ProductSelect("NewProducts", "topmenu", 0); //alt sayfa , menüden en yeni ürünler tıklanınca
            return View(mpm);
        }

        public PartialViewResult _PartialNewProducts(string nextpagenumber)
        {
            //nextpagenumber * 4 = kacıncı üründen baslayacak Skip
            int pagenumber = Convert.ToInt32(nextpagenumber);
            mpm.NewProducts = cp.ProductSelect("NewProducts", "topmenuajax", pagenumber); //alt sayfa , daha fazla ürün tıklanınca

            return PartialView(mpm);
        }



        public IActionResult SpecialProducts()
        {
            mpm.SpecialProducts = cp.ProductSelect("SpecialProducts", "topmenu", 0); //alt sayfa , menüden en özel ürünler tıklanınca
            return View(mpm);
        }

        public PartialViewResult _PartialSpecialProducts(string nextpagenumber)
        {
            //nextpagenumber * 4 = kacıncı üründen baslayacak Skip
            int pagenumber = Convert.ToInt32(nextpagenumber);
            mpm.SpecialProducts = cp.ProductSelect("SpecialProducts", "topmenuajax", pagenumber); //alt sayfa , daha fazla ürün tıklanınca

            return PartialView(mpm);
        }


        public IActionResult DiscountedProducts()
        {
            mpm.DiscountedProducts = cp.ProductSelect("DiscountedProducts", "topmenu", 0); //alt sayfa , menüden indirimli ürünler tıklanınca
            if (HttpContext.Session.GetString("url") != null)
            {
                url = HttpContext.Session.GetString("url");
                HttpContext.Session.Remove("url");
                return Redirect(url);
            }
            else
            {
                HttpContext.Session.SetString("DiscountedProducts", "url");
            }
            return View(mpm);
        }

        public PartialViewResult _PartialDiscountedProducts(string nextpagenumber)
        {
            //nextpagenumber * 4 = kacıncı üründen baslayacak Skip
            int pagenumber = Convert.ToInt32(nextpagenumber);
            mpm.DiscountedProducts = cp.ProductSelect("DiscountedProducts", "topmenuajax", pagenumber); //alt sayfa , daha fazla ürün tıklanınca

            return PartialView(mpm);
        }


        //projenin herhangi bir sayfasından , sepete ekle butonu tıklanınca
        public IActionResult CartProcess(int id)
        {
            // highlighted kolonu 1 arttır
            cp.HighLightedPlus(id);

            o.ProductID = id;
            o.Quantity = 1;

            var cookieOptions = new CookieOptions();//nesne olusturduk.instance aldık
            //read
            //çerez politikası = cookies = tarayıcıda tutulur(google chrome,mozilla firefox,safari,opera)

            var cookie = Request.Cookies["sepetim"]; //tarayıcıda sepetim isminde cookie(çerez) varmı
            if (cookie == null)
            {
                //kullanıcı siteme ilk defa ürün ekleyecek
                cookieOptions = new CookieOptions();
                cookieOptions.Expires = DateTime.Now.AddDays(1);//1 günlük çerez cookie degeri tanımladık
                cookieOptions.Path = "/";
                o.Sepet = "";
                o.AddToCart(id.ToString());//sepete ekle metodu yazalım
                Response.Cookies.Append("sepetim", o.Sepet, cookieOptions); //tanımladıgımız çerez i tarayıcıya gönderdik
                HttpContext.Session.SetString("Message", "Ürün Sepetinize Eklendi.");
                TempData["Message"] = "Ürün Sepetinize Eklendi";
            }
            else
            {
                // kullanıcı daha önceden sepetine ürün eklemiş.tarayıcıdaki sepetim iceriğigini property ye gönderdim
                //aynı ürün daha önceden sepetim e eklenmişmi kontrolü yapıyorum
                o.Sepet = cookie;
                if (o.AddToCart(id.ToString()) == false)
                {
                    //eklenmemiş
                    Response.Cookies.Append("sepetim", o.Sepet, cookieOptions); //tanımladıgımız çerez i tarayıcıya gönderdik
                    cookieOptions.Expires = DateTime.Now.AddDays(1);//1 günlük çerez cookie degeri tanımladık
                                                                    // HttpContext.Session.SetString("Message", "Ürün Sepetinize Eklendi.");
                    TempData["Message"] = "Ürün Sepetinize Eklendi";
                }
                else
                {
                    // HttpContext.Session.SetString("Message", "Bu Ürün Sepetinizde Zaten Var.");
                    TempData["Message"] = "Bu Ürün Sepetinizde Zaten Var.";
                }
            }
            //ürünü sepete ekledikten sonra,hangi sayfadaysam,o sayfada kalmaya devam edecek
            //http:localhost:/Home/Newproduct/
            url = Request.Headers["Referer"].ToString();
            return Redirect(url);
        }

        public IActionResult Details(int id)
        {
            //efcore
            //mpm.ProductDetails = context.Products.FirstOrDefault(p => p.ProductID == id);

            //select * from Products where ProductID = id  ado.net , dapper

            //linq  - 4 nolu ürünün bütün kolon (sütün) bilgileri elimde
            mpm.ProductDetails = (from p in context.Products where p.ProductID == id select p).FirstOrDefault();

            //linq
            mpm.CategoryName = (from p in context.Products
                                join c in context.Categories
                              on p.CategoryID equals c.CategoryID
                                where p.ProductID == id
                                select c.CategoryName).FirstOrDefault();

            //linq
            mpm.BrandName = (from p in context.Products
                             join s in context.Suppliers
                           on p.SupplierID equals s.SupplierID
                             where p.ProductID == id
                             select s.BrandName).FirstOrDefault();

            //select * from Products where Related = 2 and ProductID != 4
            mpm.RelatedProducts = context.Products.Where(p => p.Related == mpm.ProductDetails!.Related && p.ProductID != id).ToList();


            // highlighted kolonu 1 arttır
            cp.HighLightedPlus(id);
            return View(mpm);
        }

        //projenin sag üst kösesinden sepet sayfama git tıklanınca
        //sayfada ürün silerken sil butonu tıklanınca

        //Cast = Casting 
        /*  int yas = Convert.ToInt32(51);
          List<cls_Order> urunler = urun as List<cls_Order>;
          List<cls_Order> urunler2 = (List<cls_Order>)urun;
        */

        public IActionResult Cart()
        {
            if (HttpContext.Request.Query["scid"].ToString() != "")
            {
                //sayfada ürün silerken sil butonu tıklanınca scid gönderecegim
                //sepetim cookie (çerez) sinden ürün silerek Cart.cshtml sayfası yükleyeceğim
                int scid = Convert.ToInt32(HttpContext.Request.Query["scid"].ToString());
                o.Sepet = Request.Cookies["sepetim"]; //tarayıcıdan aldım,property ye koydum
                o.DeleteFromMyCart(scid.ToString());

                var cookieOptions = new CookieOptions();
                cookieOptions.Expires = DateTime.Now.AddDays(1);//1 günlük çerez cookie degeri tanımladık
                Response.Cookies.Append("sepetim", o.Sepet, cookieOptions); //tanımladıgımız çerez i tarayıcıya 
                TempData["Message"] = "Ürün Sepetinizden Silindi.";

                //cart.cshmml ürünleri foreach ile dönüp gösterecek.o bilgiyi hazırlıyorum
                List<cls_Order> sepet = o.SelectMyCart();
                ViewBag.Sepetim = sepet;
                ViewBag.sepet_tablo_detay = sepet;
            }
            else
            {
                // projenin sag üst kösesinden sepet sayfama git tıklanınca
                //sepetim cookie (çerez) sinden hiçbirşey değiştirmeden Cart.cshtml sayfası yükleyeceğim
                var cookie = Request.Cookies["sepetim"];
                List<cls_Order> sepet;
                var cookieOptions = new CookieOptions();
                if (cookie == null)
                {
                    o.Sepet = "";
                    sepet = o.SelectMyCart();
                    ViewBag.Sepetim = sepet;
                    ViewBag.sepet_tablo_detay = sepet;
                }
                else
                {
                    o.Sepet = Request.Cookies["sepetim"];
                    sepet = o.SelectMyCart();
                    ViewBag.Sepetim = sepet;
                    ViewBag.sepet_tablo_detay = sepet;
                }
            }
            return View();
        }


        public IActionResult HighlightedProducts()
        {
            mpm.HighlightedProducts = cp.ProductSelect("HighlightedProducts", "topmenu", 0); //alt sayfa , menüden indirimli ürünler tıklanınca
            return View(mpm);
        }

        public PartialViewResult _PartialHighlightedProducts(string nextpagenumber)
        {
            //nextpagenumber * 4 = kacıncı üründen baslayacak Skip
            int pagenumber = Convert.ToInt32(nextpagenumber);
            mpm.HighlightedProducts = cp.ProductSelect("HighlightedProducts", "topmenuajax", pagenumber); //alt sayfa , daha fazla ürün tıklanınca

            return PartialView(mpm);
        }


        public IActionResult TopsellerProducts(int page = 1, int pageSize = 4)
        {
            PagedList<Product> model = new PagedList<Product>(context.Products.OrderByDescending(p => p.TopSeller), page, pageSize);

            return View("TopsellerProducts", model);
        }


        public IActionResult CategoryPage(int id)
        {
            //tip = type (List<Product>)
            List<Product> products = cp.ProductsSelectByCategoryID(id);
            return View(products);
        }

        public IActionResult SupplierPage(int id)
        {
            List<Product> products = cp.ProductsSelectBySupplierID(id);
            return View(products);
        }


        [HttpGet]
        public IActionResult Order()
        {

            //kullanıcı giriş yapmışmı
            if (HttpContext.Session.GetString("Email") != null)
            {
                //bu kullanıcı login girişi yapmış, ve benden Email isminde oturum almuş
                User? user = cls_User.SelectUserInfo(HttpContext.Session.GetString("Email"));
                return View(user);
            }
            else
            {
                HttpContext.Session.SetString("url", Request.Headers["Referer"].ToString());

                return RedirectToAction("Login");
            }
        }


        [HttpPost]
        public IActionResult Order(Order order, IFormCollection frm)
        {
            if (Request.Form["txt_tckimlikno"] != "")
            {
                string? tckimlikno = Request.Form["txt_tckimlikno"];
            }
            if (frm["txt_vergino"] != "")
            {
                string? vergino = Request.Form["txt_vergino"];
            }

            string? kredikartno = Request.Form["kredikartno"];
            string? kredikartay = Request.Form["kredikartay"];
            string? kredikartyil = Request.Form["kredikartyil"];
            string? kredikartcvc = Request.Form["kredikartcvc"];

            /*
            //payu = iyzico

            NameValueCollection data = new NameValueCollection();
            string payu_url = "https://www.sedattefci.com/backref";
            data.Add("BACK_REF", payu_url);
            data.Add("CC_CVV", kredikartcvc);
            data.Add("CC_NUMBER", kredikartno);
            data.Add("CC_MONTH", kredikartay);
            data.Add("CC_YEAR", kredikartyil);

            var deger = "";
            foreach (var item in data)
            {
                var value = item as string;
                var byteCount = Encoding.UTF8.GetByteCount(data.Get(value));
                deger += byteCount + data.Get(value);
            }

            var signatureKey = "size verilen SECREY_KEY burada yazılacak";
            var hash = HashWithSignature(deger, signatureKey);
            data.Add("ORDER_HASH", hash);
            var x = POSTFormPayu("https://secure.payu.com.tr/order/...", data);

            if (x.Contains("<STATUS>SUCCESS</STATUS>") && x.Contains("<RETURN_CODE>3DS_ENROLLED</RETURN_CODE>"))
            {
                //sanal kart ile alısveriş yaptı ok.
            }
            else
            {
                //gerçek kart
               if(x.Contains("<STATUS>SUCCESS</STATUS>") && x.Contains("<RETURN_CODE>AUTHORIZED</RETURN_CODE"))
                    {
                    //gerçek kart ile alısveriş yaptı ok.
                }
            }
            */
            return RedirectToAction("beckref");
        }

        public string POSTFormPayu(string url, NameValueCollection data)
        {
            return "";
        }

        public string HashWithSignature(string deger, string signatureKey)
        {
            return "";
        }

        public IActionResult beckref()
        {
            OrderConfirm();
            return RedirectToAction("Confirm");
        }

        public static string OrderGroupGUID = "";//20230513113445345


        public IActionResult OrderConfirm()
        {
            //cookie sepetindeki siparişi Order tablosuna yazacagız,sepeti sileceğiz
            var cookieOptions = new CookieOptions();
            var cookie = Request.Cookies["sepetim"]; //tarayıcıda sepetim isminde cookie(çerez) varmı
            if (cookie != null)
            {
                o.Sepet = cookie;
                OrderGroupGUID = o.WriteFromCookieToOrderTable(HttpContext.Session.GetString("Email").ToString());
                cookieOptions.Expires = DateTime.Now.AddDays(1);//1 günlük çerez cookie degeri tanımladık
                Response.Cookies.Delete("sepetim");
                cls_User.Send_Sms(OrderGroupGUID);
                cls_User.Send_Email(OrderGroupGUID);
            }
            return RedirectToAction("Confirm");
        }

        public IActionResult Confirm()
        {
            ViewBag.OrderGroupGUID = OrderGroupGUID;
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(User user)
        {
            string answer = cls_User.AddUser(user);

            if (answer == "Başarılı")
            {
                TempData["Message"] = "Bilgileriniz Başarıyla Kaydedildi.";
                return RedirectToAction("Login");
            }
            else if (answer == "Email zaten var")
            {
                TempData["Message"] = "EMail daha önceden kayıtlı.Tekrar Deneyiniz.";
                return View();
            }
            TempData["Message"] = "HATA";
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(User user)
        {
            string answer = cls_User.UserControl(user);

            if (answer == "yanlış")
            {
                //email ve şifre kayıtlı değil
                TempData["Message"] = "HATA...EMail , Şifreniz yanlış.Tekrar deneyiniz.";
                return View();
            }
            else if (answer == "admin")
            {
                return RedirectToAction("Login", "Admin");
            }
            else if (answer == "HATA")
            {
                TempData["Message"] = "Bir HATA oluştu.Tekrar deneyiniz.";
                return View();
            }
            else
            {
                HttpContext.Session.SetString("Email", answer);

                if (HttpContext.Session.GetString("url") != null)
                {
                    url = HttpContext.Session.GetString("url");
                    HttpContext.Session.Remove("url");
                    return Redirect(url);
                }

                return RedirectToAction("Index", "Home");
            }
        }


        public IActionResult Logout()
        {
            HttpContext.Session.Remove("Email");
            HttpContext.Session.Remove("Admin");
            return RedirectToAction("Index");
        }


        public IActionResult MyOrders()
        {
            if (HttpContext.Session.GetString("Email") != null)
            {
                List<VW_MyOrders> orders = o.SelectMyOrders(HttpContext.Session.GetString("Email").ToString());
                return View(orders);
            }

            return RedirectToAction("Login");
        }


        public async Task<IActionResult> DetailedSearch()
        {
            ViewBag.Categories = await c.CategorySelect();
            ViewBag.Suppliers = await s.SupplierSelect();
            return View();
        }


        public IActionResult DpProducts(int CategoryID, string[] SupplierID, string price, string isInStock)
        {
            price = price.Replace(" ", "");
            string[] PriceArray = price.Split("-");
            string startmoney = PriceArray[0];
            string endmoney = PriceArray[1];

            string sign = ">";
            if (isInStock == "0")
            {
                sign = ">=";
            }

            int count = 0;
            string SupplierValue = "";

            for (int i = 0; i < SupplierID.Length; i++)
            {
                if (count == 0)
                {
                    //tek marka seçilmiş
                    //select * from Products where (SupplierID = 1)
                    SupplierValue = "SupplierID = " + SupplierID[i];
                    count++;
                }
                else
                {
                    //birden fazla marka seçilmiş
                    //select * from Products where (SupplierID = 1 or SupplierID = 5)
                    SupplierValue += " or SupplierID = " + SupplierID[i];
                }
            }

            string query = "select * from Products where  CategoryID = " + CategoryID + " and (" + SupplierValue + ") and (UnitPrice > " + startmoney + " and UnitPrice < " + endmoney + ") and Stock " + sign + " 0 order by ProductName";

            ViewBag.Products = o.Select_Products_DetailsSearch(query);

            return View();
        }

        public PartialViewResult gettingProducts(string id)
        {
            id = id.ToUpper(new System.Globalization.CultureInfo("tr-TR"));
            List<sp_arama> ulist = cls_Product.gettingSearchProducts(id);
            string json = JsonConvert.SerializeObject(ulist);
            var response = JsonConvert.DeserializeObject<List<Search>>(json);
            return PartialView(response);
        }


        public IActionResult ContactUs()
        {
            return View();
        }

    }
}
