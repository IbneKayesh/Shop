using System.ComponentModel.DataAnnotations;

namespace Shop.ERP.Models
{
    public class PRODUCT_CATEGORY
    {

        public int ID { get; set; }


        [Display(Name ="Category Name")]
        [StringLength(50,ErrorMessage ="{0} length is between max {1} char and min {2}", MinimumLength = 3)]
        [Required(ErrorMessage ="{0} is required")]
        public string CATEGORY_NAME { get; set; }


        //[StringLength(50,ErrorMessage ="Lenght Should be max 50 char", MinimumLength = 3)]
        //[Required(ErrorMessage ="Category Description is required")]
        //public string CATEGORY_DESC { get; set; }
    }
}
