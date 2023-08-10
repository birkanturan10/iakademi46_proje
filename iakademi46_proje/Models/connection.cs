using Microsoft.Data.SqlClient;

namespace iakademi46_proje.Models
{
    public class connection
    {
        public static SqlConnection baglanti
        {
            get
            {
                SqlConnection sqlcon = new SqlConnection("Server=.;Trusted_Connection=True;Database=iakademi46Core_proje;TrustServerCertificate=True;");
                return sqlcon;
            }
        }
    }
}
