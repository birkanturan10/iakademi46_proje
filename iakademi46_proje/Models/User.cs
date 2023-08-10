using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iakademi46_proje.Models
{
    public class User
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserID { get; set; }

        [StringLength(100)]
        [Required]
        public string? NameSurname { get; set; }

        [StringLength(100)]
        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [StringLength(100)]
        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }  //d375af34cc08aba9a1cc9b6596a70c36

        public string? Telephone { get; set; }

        public string? InvoicesAddres { get; set; }

        public bool IsAdmin { get; set; }

        public bool Active { get; set; }
    }
}
