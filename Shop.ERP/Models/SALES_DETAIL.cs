namespace Shop.ERP.Models
{
    public class SALES_DETAIL
    {
        public SALES_DETAIL()
        {
            ID = Guid.Empty.ToString();
        }
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "ID")]
        [StringLength(50, ErrorMessage = "{0} length is {2} between {1}", MinimumLength = 1)]
        public string ID { get; set; }


        [Display(Name = "Trn Id")]
        [StringLength(50, ErrorMessage = "{0} length is {2} between {1}", MinimumLength = 0)]
        public string? MASTER_ID { get; set; }


        [Display(Name = "Line No")]
        public int LINE_NO { get; set; }


        [Display(Name = "Product Id")]
        [StringLength(50, ErrorMessage = "{0} length is {2} between {1}", MinimumLength = 0)]
        public string? PRODUCT_ID { get; set; }

        [Display(Name = "Unit Id")]
        [StringLength(50, ErrorMessage = "{0} length is {2} between {1}", MinimumLength = 2)]
        [Required(ErrorMessage = "{0} is required")]
        public string? UNIT_ID { get; set; }

        [Display(Name = "Rate")]
        [Column(TypeName = "decimal(18, 6)")]
        public decimal PRODUCT_RATE { get; set; } = 0;

        [Display(Name = "Qty")]
        [Column(TypeName = "decimal(18, 6)")]
        public decimal PRODUCT_QTY { get; set; }

        [Display(Name = "Discount Amount")]
        [Column(TypeName = "decimal(18, 6)")]
        public decimal DISCOUNT_AMOUNT { get; set; } = 0;

        [Display(Name = "Amount")]
        [Column(TypeName = "decimal(18, 6)")]
        public decimal PRODUCT_AMOUNT { get; set; }

        [Display(Name = "Line Note")]
        [StringLength(50, ErrorMessage = "{0} length is {2} between {1}", MinimumLength = 0)]
        public string? LINE_NOTE { get; set; }
    }
}
