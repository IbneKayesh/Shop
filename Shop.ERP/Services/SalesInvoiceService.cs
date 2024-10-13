namespace Shop.ERP.Services
{
    public class SalesInvoiceService
    {
        private readonly AppDbContext dbCtx;
        public SalesInvoiceService(AppDbContext dbContext)
        {
            dbCtx = dbContext;
        }
        public List<SALES_MASTER> GetAll()
        {
            string sql = $@"select * from SALES_MASTER order by TRN_DATE desc";
            return dbCtx.Database.SqlQueryRaw<SALES_MASTER>(sql).ToList();
        }
        public SALES_MD_VM GetById(string id)
        {
            SALES_MD_VM entity = new SALES_MD_VM();
            entity.SALES_MASTER = dbCtx.SALES_MASTER.Where(s => s.ID == id).FirstOrDefault();

            string sql = $"select sd.*,p.PRODUCT_NAME,u.UNIT_NAME from SALES_DETAIL sd Join PRODUCTS p on sd.PRODUCT_ID = p.ID Join UNITS u on sd.UNIT_ID = u.ID  Where sd.MASTER_ID = '{id}'";

            entity.SALES_DETAIL_VM = dbCtx.Database.SqlQueryRaw<SALES_DETAIL_VM>(sql).ToList();
            return entity;
        }
        public EQResult Save(SALES_MD_VM obj)
        {
            EQResult eQResult = new EQResult();
            eQResult.entities = "SALES_MD_VM";
            if (string.IsNullOrWhiteSpace(obj.SALES_MASTER.ID))
            {
                obj.SALES_MASTER.ID = Guid.NewGuid().ToString();
                obj.SALES_MASTER.SALES_NO = CreateSalesNo();
                dbCtx.SALES_MASTER.Add(obj.SALES_MASTER);

                List<SALES_DETAIL> detailList = new List<SALES_DETAIL>();
                int LineNo = 1;
                foreach (var item in obj.SALES_DETAIL_VM)
                {
                    SALES_DETAIL sd = new SALES_DETAIL();
                    sd.ID = Guid.NewGuid().ToString();
                    sd.MASTER_ID = obj.SALES_MASTER.ID;
                    sd.LINE_NO = LineNo;
                    sd.PRODUCT_ID = item.PRODUCT_ID;
                    sd.UNIT_ID = item.UNIT_ID;
                    sd.PRODUCT_RATE = item.PRODUCT_RATE;
                    sd.PRODUCT_QTY = item.PRODUCT_QTY;
                    sd.DISCOUNT_AMOUNT = 0;
                    sd.PRODUCT_AMOUNT = item.PRODUCT_AMOUNT;
                    sd.LINE_NOTE = "";
                    LineNo++;
                    detailList.Add(sd);
                }
                dbCtx.SALES_DETAIL.AddRange(detailList);
                eQResult.rows = dbCtx.SaveChanges();
                eQResult.success = true;
                eQResult.messages = obj.SALES_MASTER.SALES_NO;
                return eQResult;
            }
            else
            {
                var entity = dbCtx.SALES_MASTER.Where(x => x.ID == obj.SALES_MASTER.ID).FirstOrDefault();
                if (entity != null)
                {
                    //remove all detail
                    dbCtx.SALES_DETAIL.RemoveRange(dbCtx.SALES_DETAIL.Where(x => x.MASTER_ID == obj.SALES_MASTER.ID).ToList());
                    dbCtx.SaveChanges();

                    //modify master
                    entity.CUSTOMER_NAME = obj.SALES_MASTER.CUSTOMER_NAME;
                    entity.TRN_NOTE = obj.SALES_MASTER.TRN_NOTE;
                    dbCtx.Entry(entity).State = EntityState.Modified;

                    //add all detail
                    List<SALES_DETAIL> detailList = new List<SALES_DETAIL>();
                    int LineNo = 1;
                    foreach (var item in obj.SALES_DETAIL_VM)
                    {
                        SALES_DETAIL sd = new SALES_DETAIL();
                        sd.ID = Guid.NewGuid().ToString();
                        sd.MASTER_ID = obj.SALES_MASTER.ID;
                        sd.LINE_NO = LineNo;
                        sd.PRODUCT_ID = item.PRODUCT_ID;
                        sd.UNIT_ID = item.UNIT_ID;
                        sd.PRODUCT_RATE = item.PRODUCT_RATE;
                        sd.PRODUCT_QTY = item.PRODUCT_QTY;
                        sd.DISCOUNT_AMOUNT = 0;
                        sd.PRODUCT_AMOUNT = item.PRODUCT_AMOUNT;
                        sd.LINE_NOTE = "";
                        LineNo++;
                        detailList.Add(sd);
                    }
                    dbCtx.SALES_DETAIL.AddRange(detailList);

                    //save it into database
                    eQResult.rows = dbCtx.SaveChanges();
                    eQResult.success = true;
                    eQResult.messages = obj.SALES_MASTER.SALES_NO;
                    return eQResult;
                }
                else
                {
                    eQResult.messages = NotifyService.NotFoundString();
                    return eQResult;
                }
            }
            return eQResult;
        }
        private string CreateSalesNo()
        {
            int LastNo = dbCtx.SALES_MASTER.Count() + 1;
            return "INV-" + DateTime.Now.ToString("yyMMdd") + "-" + LastNo.ToString().PadLeft(5, '0');
        }
    }
}
