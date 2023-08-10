using Microsoft.EntityFrameworkCore;

namespace iakademi46_proje.Models
{
    public class cls_Product
    {
        iakademi46Context context = new iakademi46Context();
        #region MyRegion
        //bütün ürün listesi
        #endregion
        public async Task<List<Product>> ProductSelect()
        {
            List<Product> products = await context.Products.ToListAsync();
            return products;
        }

        public static bool ProductInsert(Product product)
        {
            //metod static oldugu icin , context direk gelmez , using tanımı metod icinde
            using (iakademi46Context context = new iakademi46Context())
            {
                try
                {
                  //  product.ProductName = product.ProductName.ToLower().Trim();
                    if (product.Notes == null)
                    {
                        product.Notes = "";
                    }
                    product.AddDate = DateTime.Now;

                    bool exists = context.Products.Any(p => p.ProductName.ToLower().Trim().Equals(product.ProductName.ToLower().Trim()));
                    if (exists == false)
                    {
                        context.Add(product);
                        context.SaveChanges();
                        return true;
                    }
                    return false;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        public async Task<Product> ProductDetails(int? id)
        {
            Product? product = await context.Products.FirstOrDefaultAsync(c => c.ProductID == id);
            return product;
        }


        public static bool ProductUpdate(Product product)
        {
            //metod static oldugu icin , context direk gelmez
            using (iakademi46Context context = new iakademi46Context())
            {
                try
                {
                    context.Update(product);
                    context.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        public static bool ProductDelete(int id)
        {
            //metod static oldugu icin , context direk gelmez
            using (iakademi46Context context = new iakademi46Context())
            {
                try
                {
                    Product product = context.Products.FirstOrDefault(c => c.ProductID == id);
                    product.Active = false;

                    context.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }


        public  List<Product> ProductSelect(string mainPageName,string subPageName,int pagenumber)
        {
            List<Product> productsList;
            if (mainPageName == "SliderProducts")
            {
                //slider 
                 productsList = context.Products.Where(p => p.StatusID == 1).Take(8).ToList();
            }

            else if (mainPageName == "NewProducts")
            {
                //yeni ürünler
                if (subPageName == "index")
                {
                    //home/index
                    productsList = context.Products.OrderByDescending(p => p.AddDate).Take(8).ToList();
                }
                else
                {
                    //home/NewProducts  
                    if (subPageName == "topmenu")
                    {
                       // --menüden new  butonu tıklanınca
                        productsList = context.Products.OrderByDescending(p => p.AddDate).Take(4).ToList();
                    }
                    else
                    {
                        //ajax
                        productsList = context.Products.OrderByDescending(p => p.AddDate).Skip(pagenumber * 4).Take(4).ToList();
                    }
                }
            }

          
            else if (mainPageName == "SpecialProducts")
            {
                //özel ürünler
                if (subPageName == "index")
                {
                    //home/index
                    productsList = context.Products.Where(p => p.StatusID == 2).Take(8).ToList();
                }
                else
                {
                    //home/SpecialProducts  
                    if (subPageName == "topmenu")
                    {
                        // --menüden new  butonu tıklanınca
                        // productsList = context.Products.OrderByDescending(p => p.AddDate).Take(4).ToList();
                        productsList = context.Products.Where(p => p.StatusID == 2).Take(4).ToList();
                    }
                    else
                    {
                        //ajax
                        //  productsList = context.Products.OrderByDescending(p => p.AddDate).Skip(pagenumber * 4).Take(4).ToList();
                        productsList = context.Products.Where(p => p.StatusID == 2).Skip(pagenumber * 4).Take(4).ToList();
                    }
                }
            }


            else if (mainPageName == "DiscountedProducts")
            {
                //indirimli
                if (subPageName == "index")
                {
                    //home/index
                    productsList = context.Products.OrderByDescending(p => p.Discount).Take(8).ToList();
                }
                else
                {
                    //home/DiscountedProducts  
                    if (subPageName == "topmenu")
                    {
                        // --menüden new  butonu tıklanınca
                        productsList = context.Products.OrderByDescending(p => p.Discount).Take(4).ToList();
                    }
                    else
                    {
                        //ajax
                        productsList = context.Products.OrderByDescending(p => p.Discount).Skip(pagenumber * 4).Take(4).ToList();
                    }
                }
            }




            else if (mainPageName == "HighlightedProducts")
            {
                //indirimli
                if (subPageName == "index")
                {
                    //home/index
                    productsList = context.Products.OrderByDescending(p => p.HighLighted).Take(8).ToList();
                }
                else
                {
                    //home/HighlightedProducts  
                    if (subPageName == "topmenu")
                    {
                        // --menüden new  butonu tıklanınca
                        productsList = context.Products.OrderByDescending(p => p.HighLighted).Take(4).ToList();
                    }
                    else
                    {
                        //ajax
                        productsList = context.Products.OrderByDescending(p => p.HighLighted).Skip(pagenumber * 4).Take(4).ToList();
                    }
                }
            }







            else if (mainPageName == "StarProducts")
            {
                //yıldız
                productsList = context.Products.Where(p => p.StatusID == 3).Take(8).ToList();
            }

            else if (mainPageName == "FeaturedProducts")
            {
                //fırsat
                productsList = context.Products.Where(p => p.StatusID == 4).Take(8).ToList();
            }

            

            else if (mainPageName == "HighlightedProducts")
            {
                //öne çıkanlar
                productsList = context.Products.OrderByDescending(p => p.HighLighted).Take(8).ToList();
            }

            else if (mainPageName == "TopsellerProducts")
            {
                //çok satanlar
                productsList = context.Products.OrderByDescending(p => p.TopSeller).Take(8).ToList();
            }

            else if (mainPageName == "NotableProducts")
            {
                //dikkat çeken
                productsList = context.Products.Where(p => p.StatusID == 5).Take(8).ToList();
            }

            else
            {
                productsList = null;
            }
            return productsList;
        }


        public Product ProductSelect_OfDay()
        {
            Product  product= context.Products.FirstOrDefault(p => p.StatusID == 6);
            return product;
        }


        public  List<Product> ProductsSelectByCategoryID(int id)
        {
            //select * from Products where CategoryID = id order by ProductName
            List<Product> products = context.Products.Where(p => p.CategoryID == id).OrderBy(p => p.ProductName).ToList();
            return products;
        }

        public List<Product> ProductsSelectBySupplierID(int id)
        {
            //select * from Products where SupplierID = id order by ProductName
            List<Product> products = context.Products.Where(p => p.SupplierID == id).OrderBy(p => p.ProductName).ToList();
            return products;
        }

        public  void HighLightedPlus(int? id)
        {
            using (iakademi46Context context = new iakademi46Context())
            {
                Product? product = context.Products.FirstOrDefault(c => c.ProductID == id);
                product.HighLighted = product.HighLighted + 1;
                context.Update(product);
                context.SaveChanges();
            }
           
        }

        public static List<sp_arama> gettingSearchProducts(string id)
        {
            using (iakademi46Context context = new iakademi46Context())
            {
                var products = context.sp_Aramas.FromSql($"sp_arama {id}").ToList();
                return products;
            }
        }


    }
}
