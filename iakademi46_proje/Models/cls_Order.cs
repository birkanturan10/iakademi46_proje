using Microsoft.Data.SqlClient;

namespace iakademi46_proje.Models
{
    public class cls_Order
    {
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public string? Sepet { get; set; } //10=1&20=1&30=1
        public string? ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Kdv { get; set; }
        public string? PhotoPath { get; set; }

        iakademi46Context context = new iakademi46Context();

        //sepete ekle
        public bool AddToCart(string id)
        {
            bool varmi = false;

            if (Sepet == "")
            {
                //sepete ilk defa ürün ekliyor
                Sepet = id + "=1";
            }
            else
            {
                //daha önceden sepete eklenmiş ürün(ler) var
                //10=1&20=1
                string[] sepetdizi = Sepet.Split('&'); //& = ampersand
                for (int i = 0; i < sepetdizi.Length; i++)
                {
                    //10=1  -- for ilk dönüşte -- sepetdizi[0]
                    //20=1  -- for ikinci dönüşte sepetdizi[1]
                    string[] sepetdizi2 = sepetdizi[i].Split('=');
                    if (sepetdizi2[0] == id)
                    {
                        //bu ürün zaten sepette var
                        //sepetdizi2[0] = ProductID
                        //sepetdizi2[1] = Adet
                        //sepetdizi2[1] + 1
                        // Sepet = Sepet + "&" + id + "=" + sepetdizi2[1] + 1; //ürün sepet adedini 1 arttır oldu
                        varmi = true;
                    }
                }

                if (varmi == false)
                {
                    //ürün sepete daha önceden eklenmemiş
                    Sepet = Sepet + "&" + id + "=1";
                }
            }

            return varmi;
        }


        //proje sag üst kösedeki sepet sayfası ve sil butonu tıklanınca yüklencek olan sayfa bu metodu cagıracak
        //List<cls_Order>     = property ler dönecek
       //siparişi onaylama metodu cağırıyor
        public List<cls_Order> SelectMyCart()
        {
            List<cls_Order> list = new List<cls_Order>(); //bütün ürünlerin

            string[] sepetdizi = Sepet.Split('&'); //ürünleri ayırıyorum  10=1      20=1       30=1
            if (sepetdizi[0] != "")
            {
                for (int i = 0; i < sepetdizi.Length; i++)
                {
                    string[] sepetdizi2 = sepetdizi[i].Split('='); //10     1
                    int sepetid = Convert.ToInt32(sepetdizi2[0]); //ProductID
                    int adet = Convert.ToInt32(sepetdizi2[1]); //Adet

                    Product? product  = context.Products.FirstOrDefault(p => p.ProductID == sepetid);

                    cls_Order p = new cls_Order();
                    //property = veritabanı
                    p.ProductID = product.ProductID; //veritabanından gelen bilgileri property lere gönderecegim
                    p.Quantity = adet;
                    p.UnitPrice = product.UnitPrice;
                    p.ProductName = product.ProductName;
                    p.Kdv = product.Kdv;
                    p.PhotoPath = product.PhotoPath;
                   //1 ürünün bilgileri property lerde
                   //listeye ekle
                   list.Add(p);
                }
            }
            return list;
        }


        public void DeleteFromMyCart(string id)
        {
            string[] sepetdizi = Sepet.Split('&'); //ürünleri ayırıyorum  10=1      20=1       30=1
            string yenisepet = "";

            for (int i = 0; i < sepetdizi.Length; i++)
            {
                string[] sepetdizi2 = sepetdizi[i].Split('=');
                int adet = Convert.ToInt32(sepetdizi2[1]); //Adet

                if (sepetdizi2[0] != id)
                {
                    //silimeyecek ürünler yakalandı
                    //10=1&20=1&30=1
                    if (yenisepet == "")
                    {
                        yenisepet = sepetdizi2[0] + "=" + adet;
                    }
                    else
                    {
                        yenisepet = yenisepet + "&" + sepetdizi2[0] + "=" + adet;
                    }
                }
            }
            Sepet = yenisepet;
        }


        public string WriteFromCookieToOrderTable(string Email)
        {
            string OrderGroupGUID = DateTime.Now.ToString().Replace(".", "").Replace(":", "").Replace(" ", "");
            DateTime OrderDate = DateTime.Now;

            List<cls_Order> orders = SelectMyCart();
            foreach (var item in orders)
            {
                Order order = new Order();
                order.OrderDate = OrderDate;
                order.OrderGroupGUID = OrderGroupGUID;
                order.UserID = context.Users.FirstOrDefault(u => u.Email == Email).UserID;
                order.ProductID = item.ProductID;
                order.Quantity = item.Quantity;
                context.Orders.Add(order);
                context.SaveChanges();
            }

            return OrderGroupGUID;
        }


        public List<VW_MyOrders> SelectMyOrders(string Email)
        {
            int UserID = context.Users.FirstOrDefault(u => u.Email == Email).UserID;

            List<VW_MyOrders> orders = context.vw_MyOrders.Where(o => o.UserID == UserID).ToList();
            return orders;
        }

        public List<cls_Order> Select_Products_DetailsSearch(string query)
        {
            List<cls_Order> products = new List<cls_Order>();

            SqlConnection sqlcon = connection.baglanti;
            SqlCommand sqlcmd = new SqlCommand(query,sqlcon);
            sqlcon.Open();
            SqlDataReader sqlDataReader = sqlcmd.ExecuteReader();
            while (sqlDataReader.Read())
            {
                cls_Order p = new cls_Order();
                p.ProductID = Convert.ToInt32(sqlDataReader["ProductID"]);
                p.ProductName = sqlDataReader["ProductName"].ToString();
                p.UnitPrice = Convert.ToDecimal(sqlDataReader["UnitPrice"]);
                p.PhotoPath = sqlDataReader["PhotoPath"].ToString();
                products.Add(p);
            }
            sqlcon.Close();
            return products;
        }
       

    }
}
