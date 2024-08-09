using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Shop.ERP.Models
{
    public class PRODUCTS
    {
        public PRODUCTS()
        {
            ID = Guid.Empty.ToString();
        }
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "ID")]
        [StringLength(50, ErrorMessage = "{0} length is {2} between {1}", MinimumLength = 1)]
        public string ID { get; set; }

        [Display(Name = "Category")]
        [StringLength(50, ErrorMessage = "{0} length is {2} between {1}", MinimumLength = 0)]
        [Required(ErrorMessage = "{0} is required")]
        public string? CATEGORY_ID { get; set; }

        [Display(Name = "Product Name")]
        [StringLength(50, ErrorMessage = "{0} length is {2} between {1}", MinimumLength = 3)]
        [Required(ErrorMessage = "{0} is required")]
        public string? PRODUCT_NAME { get; set; }

        [Display(Name = "Rate")]
        [Column(TypeName = "decimal(18, 6)")]
        public decimal PRODUCT_RATE { get; set; }


        [NotMapped]
        [Display(Name = "Category Name")]
        public string? CATEGORY_NAME { get; set; }
    }
}