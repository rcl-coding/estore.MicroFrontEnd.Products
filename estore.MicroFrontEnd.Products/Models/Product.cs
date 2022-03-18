#nullable disable
using System.ComponentModel.DataAnnotations;

namespace estore.MicroFrontEnd.Products.Models
{
    public class Product
    {
        [Key]
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Sort Code")]
        [MaxLength(50)]
        public string SortCode { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Name")]
        [MaxLength(250)]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Description")]
        [MaxLength(350)]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Display(Name = "Cost")]
        public Decimal Cost { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Image Name")]
        public string ImageName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Category")]
        [MaxLength(250)]
        public string Category { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Payment Button Code")]
        public string PaymentButtonCode { get; set; }
    }
}
