using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Mail;
using System.Text;
using XSystem.Security.Cryptography;

namespace iakademi46_proje.Models
{
    public class cls_User
    {
        iakademi46Context context = new iakademi46Context();

        public async Task<User> loginControl(User user)
        {
            string md5sifrele = MD5Sifrele(user.Password);

            User? usr = await context.Users.FirstOrDefaultAsync(u => u.Email == user.Email  && u.Password == md5sifrele && u.IsAdmin == true && u.Active == true);

            return usr;
        }

        //User? user = cls_User.SelectUserInfo(HttpContext.Session.GetString("Email"));

        public static User? SelectUserInfo(string Email)
        {
           using (iakademi46Context context = new iakademi46Context())
            {
                User? user = context.Users.FirstOrDefault(u => u.Email == Email);
                return user;
            }
        }

        //bool answer = cls_User.AddUser(user);
        public static string AddUser(User user)
        {
            using (iakademi46Context context = new iakademi46Context())
            {
                try
                {
                    User? usr = context.Users.FirstOrDefault(u => u.Email == user.Email);
                    if (usr != null)
                    {
                        //bu email daha önceden kayıtlı
                        return "Email zaten var";
                    }
                    else
                    {
                        user.Active = true;
                        user.IsAdmin = false;
                        user.Password = MD5Sifrele(user.Password);
                        context.Users.Add(user);
                        context.SaveChanges();
                        return "Başarılı";
                    }
                }
                catch (Exception)
                {
                    return "Başarısız";
                }
            }
        }


        public static string MD5Sifrele(string value)
        {
            //using XSystem.Security.Cryptography;
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] btr = Encoding.UTF8.GetBytes(value);
            btr = md5.ComputeHash(btr);

            StringBuilder sb = new StringBuilder();
            foreach (byte item in btr)
            {
                sb.Append(item.ToString("x2").ToLower());
            }
            return sb.ToString();
        }

        //ctrl + M O = metodları kapalı gösterir
        //ctrm + M L = metodları açık gösterir

        // string answer = cls_User.UserControl(user);
        public static string UserControl(User user)
        {
            using (iakademi46Context context = new iakademi46Context())
            {
                string answer = "";
                try
                {
                    string md5sifrele = MD5Sifrele(user.Password);
                    User? us = context.Users.FirstOrDefault(u => u.Email == user.Email && u.Password == md5sifrele);

                    if (us == null)
                    {
                        //email ve/veya şifre yanlış
                        answer = "yanlış";
                    }
                    else
                    {
                        //email ve/veya şifre doğru
                        //adminmi? normal kullanıcımı?
                        if (us.IsAdmin == true)
                        {
                            answer = "admin";
                        }
                        else
                        {
                            answer = us.Email;
                        }
                    }
                }
                catch (Exception)
                {
                    answer = "HATA";
                }
                return answer;
            }
        }

        public static void Send_Sms(string OrderGroupGUID)
        {
            using (iakademi46Context context = new iakademi46Context())
            {
            string ss = "";
            ss += "<?xml version='1.0' encoding='UTF-8'>";
            ss += "<mainbody>";
            ss += "<header>";
            ss += "<company dil='TR' üye olunca size verilen gönderen bilgisi></company>";
            ss += "<usercode>size verilen 850 li numara</usercode>";
            ss += "<password>size verilen şifre</password>";
            ss += "<msgheader>başlık buraya</msgheader>";
            ss += "</header>";
            ss += "<body>";

                Order? order = context.Orders.FirstOrDefault(o => o.OrderGroupGUID == OrderGroupGUID);
                User user = context.Users.FirstOrDefault(u => u.UserID == order.UserID);

                //Sayın Sedat Tefçi , 13052013 tarihinde , 13052023115818 nolu siparişiniz alınmıştır.
                string content = "Sayın " + user.NameSurname + " , " + order.OrderDate + " tarihinde , " + OrderGroupGUID + " nolu siparişiniz alınmıştır.";

                ss += "<mp><msg><![CDATA[" + content + "]]></msg><no>90" + user.Telephone + "</no></mp>";
                ss += "</body>";
                ss += "</mainbody>";
                string answer = SMSXMLPOST("http://api.netgsm.com.tr/vfnfndbvdfb", ss);
                if (answer != "-1")
                {
                    //sms gitti
                }
                else
                {
                   // sms gitmedi , ilgili personeli bilgilendir.
                }
            }
        }


        public static string SMSXMLPOST(string url,string ss)
        {
            try
            {
                WebClient webClient = new WebClient();
                HttpWebRequest httpWebRequest = WebRequest.Create(url) as HttpWebRequest;
                httpWebRequest.Method = "POST";
                httpWebRequest.ContentType = "application/x-www-form-urlencoded";

                Byte[] bytes = Encoding.UTF8.GetBytes(ss);
                Byte[] response = webClient.UploadData(url, "POST", bytes);

                Char[] chars = Encoding.UTF8.GetChars(response);
                string sWebPage = new string(chars);
                return sWebPage;
            }
            catch (Exception)
            {
                return "-1";
            }
        }

        public static void Send_Email(string OrderGroupGUID)
        {
            using (iakademi46Context context = new iakademi46Context())
            {
                Order? order = context.Orders.FirstOrDefault(o => o.OrderGroupGUID == OrderGroupGUID);
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress("info@iakademi.com", "Bilgi");

                User user = context.Users.FirstOrDefault(u => u.UserID == order.UserID);
                mailMessage.To.Add(user.Email);

                string content = "Sayın " + user.NameSurname + " , " + order.OrderDate + " tarihinde , " + OrderGroupGUID + " nolu siparişiniz alınmıştır.";
                mailMessage.Body = content;

                string subject = "Siparişiniz Hakkında";
                mailMessage.Subject = subject;

                SmtpClient smtp = new SmtpClient();
                smtp.Credentials = new NetworkCredential("login bilgisi burada", "şifre bilgisi burada");
                smtp.Port = 587;
                smtp.Host = "smtp.iakademi.com";
                try
                {
                    smtp.Send(mailMessage);
                }
                catch (Exception)
                {
                    //email gönderilemedi,ilgili personeli bilgilendir.
                }
            }
        }


    }
}
