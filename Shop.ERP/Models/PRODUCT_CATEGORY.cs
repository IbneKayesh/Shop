using System.ComponentModel.DataAnnotations;

namespace Shop.ERP.Models
{
    public class PRODUCT_CATEGORY
    {
        public int ID { get; set; }


        [StringLength(50,ErrorMessage ="Lenght Should be max 50 char", MinimumLength = 3)]
        public string CATEGORY_NAME { get; set; }
    }
}
