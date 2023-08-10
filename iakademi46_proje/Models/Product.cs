using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iakademi46_proje.Models
{
    public class Product
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductID { get; set; }

        [StringLength(100)]
        [Required]
        [DisplayName("Ürün Adı")]
        public string? ProductName { get; set; }

        [DisplayName("Fiyat")]
        public decimal UnitPrice { get; set; }

        [DisplayName("Kategori")]
        public int CategoryID { get; set; }

        [DisplayName("Marka")]
        public int SupplierID { get; set; }

        [DisplayName("Stok")]
        public int Stock { get; set; }

        [DisplayName("İndirim")]
        public int Discount { get; set; }

        [DisplayName("Statüs")]
        public int StatusID { get; set; }
        public DateTime AddDate { get; set; }

        [DisplayName("Anahtar Kelimeler")]
        public string? Keywords { get; set; }


        //encapsulation (kapsülleme)
        private int _Kdv { get; set; }
        public int Kdv
        {
            get { return _Kdv; }
            set
            {
                _Kdv = Math.Abs(value);
            }
        }

        public int HighLighted { get; set; } //öne cıkanlar
        public int TopSeller { get; set; } //CokSatanlar

        
        [DisplayName("Buna Bakanlar")]
        public int Related { get; set; } //BunaBakanlar

        [DisplayName("Not")]
        public string? Notes { get; set; }

        [DisplayName("Resim")]
        public string? PhotoPath { get; set; }

        [DisplayName("Aktif/Pasif")]
        public bool Active { get; set; }


    }
}
