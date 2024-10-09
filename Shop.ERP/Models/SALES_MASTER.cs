namespace Shop.ERP.Models
{
    public class SALES_MASTER
    {
        public SALES_MASTER()
        {
            ID = Guid.Empty.ToString();
            CUSTOMER_NAME = "Walking Customer";
            TRN_DATE = DateTime.Now;
        }
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "ID")]
        [StringLength(50, ErrorMessage = "{0} length is {2} between {1}", MinimumLength = 1)]
        public string ID { get; set; }

        [Display(Name = "Sales No")]
        [StringLength(50, ErrorMessage = "{0} length is {2} between {1}", MinimumLength = 0)]
        [Required(ErrorMessage = "{0} is required")]
        public string? SALES_NO { get; set; }

        [Display(Name = "Trn Date")]
        [Required(ErrorMessage = "{0} is required")]
        public DateTime TRN_DATE { get; set; }

        [Display(Name = "Customer Name")]
        [StringLength(50, ErrorMessage = "{0} length is {2} between {1}", MinimumLength = 0)]
        [Required(ErrorMessage = "{0} is required")]
        public string? CUSTOMER_NAME { get; set; }

        [Display(Name = "Note")]
        [StringLength(250, ErrorMessage = "{0} length is {2} between {1}", MinimumLength = 0)]
        public string? TRN_NOTE { get; set; }


    }
}
