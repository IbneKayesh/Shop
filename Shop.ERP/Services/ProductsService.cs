using Shop.ERP.ViewModels;

namespace Shop.ERP.Services
{
    public class ProductsService
    {
        private readonly AppDbContext dbCtx;
        public ProductsService(AppDbContext dbContext)
        {
            dbCtx = dbContext;
        }
        public List<PRODUCTS_VM> GetAll()
        {
            string sql = $@"select p.*,pc.CATEGORY_NAME,un.UNIT_NAME
                            from PRODUCTS p
                            join PRODUCT_CATEGORY pc on p.CATEGORY_ID = pc.ID
                            join UNITS un on p.UNIT_ID = un.ID
                            order by p.PRODUCT_NAME";
            return dbCtx.Database.SqlQueryRaw<PRODUCTS_VM>(sql).ToList();
        }
        public EQResult Save(PRODUCTS obj)
        {
            EQResult eQResult = new EQResult();
            eQResult.entities = "PRODUCTS";
            if (obj.ID == Guid.Empty.ToString())
            {
                obj.ID = Guid.NewGuid().ToString();
                dbCtx.PRODUCTS.Add(obj);
                eQResult.rows = dbCtx.SaveChanges();
                eQResult.success = true;
                eQResult.messages = NotifyService.SaveSuccess();
                return eQResult;
            }
            else
            {
                var entity = dbCtx.PRODUCTS.Where(x => x.ID == obj.ID).FirstOrDefault();
                if (entity != null)
                {
                    entity.CATEGORY_ID = obj.CATEGORY_ID;
                    entity.UNIT_ID = obj.UNIT_ID;
                    entity.PRODUCT_NAME = obj.PRODUCT_NAME;
                    entity.PRODUCT_RATE = obj.PRODUCT_RATE;
                    dbCtx.Entry(entity).State = EntityState.Modified;

                    eQResult.rows = dbCtx.SaveChanges();
                    eQResult.success = true;
                    eQResult.messages = NotifyService.EditSuccess();
                    return eQResult;
                }
                else
                {
                    eQResult.messages = NotifyService.NotFound();
                    return eQResult;
                }
            }
        }


        public PRODUCTS GetById(string id)
        {
            string sql = $@"SELECT * FROM PRODUCTS WHERE ID = '{id}'";
            return dbCtx.Database.SqlQueryRaw<PRODUCTS>(sql).ToList().FirstOrDefault();
        }

        public EQResult Delete(string id)
        {
            EQResult eQResult = new EQResult();
            eQResult.entities = "PRODUCTS";
            if (string.IsNullOrWhiteSpace(id))
            {
                eQResult.messages = NotifyService.InvalidRequest();
                return eQResult;
            }
            try
            {
                //check child entity
                //int anyChild = dbCtx.BANK_BRANCH.Where(x => x.BANK_ID == id).Count();
                //if (anyChild > 0)
                //{
                //    eQResult.messages = NotifyService.DeleteHasChildString("Branch", anyChild, "Bank");
                //    return eQResult;
                //}

                //old entity
                var entity = dbCtx.PRODUCTS.Find(id);
                if (entity != null)
                {
                    //TODO : Delete property
                    dbCtx.PRODUCTS.Remove(entity);
                    eQResult.rows = dbCtx.SaveChanges();
                    eQResult.success = true;
                    eQResult.messages = NotifyService.DeletedSuccess(entity.PRODUCT_NAME!);
                    return eQResult;
                }
                else
                {
                    eQResult.messages = NotifyService.NotFound();
                    return eQResult;
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message == string.Empty ? ex.InnerException.Message : ex.Message;
                eQResult.messages = NotifyService.Error(msg.Replace("'", ""));
                return eQResult;
            }
            finally
            {
                dbCtx.Dispose();
            }
        }

    }
}
