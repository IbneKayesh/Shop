namespace Shop.ERP.ViewModels
{
    public class PRODUCTS_VM : PRODUCTS
    {
        //[NotMapped]
        [Display(Name = "Category Name")]
        public string? CATEGORY_NAME { get; set; }

        [Display(Name = "Unit Name")]
        public string? UNIT_NAME { get; set; }
    }
}
