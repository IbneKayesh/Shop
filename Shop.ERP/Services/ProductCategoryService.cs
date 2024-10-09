using Microsoft.AspNetCore.Mvc.Rendering;

namespace Shop.ERP.Services
{
    public class ProductCategoryService
    {
        private readonly AppDbContext dbCtx;
        public ProductCategoryService(AppDbContext dbContext)
        {
            dbCtx = dbContext;
        }

        public SelectList GetCategoryListItems()
        {
            return new SelectList(dbCtx.PRODUCT_CATEGORY.OrderBy(x=>x.CATEGORY_NAME).ToList(), "ID", "CATEGORY_NAME");
        }
        public List<PRODUCT_CATEGORY> GetAll()
        {
            string sql = $@"SELECT * FROM PRODUCT_CATEGORY ORDER BY CATEGORY_NAME";
            return dbCtx.Database.SqlQueryRaw<PRODUCT_CATEGORY>(sql).ToList();
        }
        public EQResult Save(PRODUCT_CATEGORY obj)
        {
            EQResult eQResult = new EQResult();
            eQResult.entities = "PRODUCT_CATEGORY";
            if (obj.ID == Guid.Empty.ToString())
            {
                obj.ID = Guid.NewGuid().ToString();
                dbCtx.PRODUCT_CATEGORY.Add(obj);
                eQResult.rows = dbCtx.SaveChanges();
                eQResult.success = true;
                eQResult.messages = NotifyService.SaveSuccess();
                return eQResult;
            }
            else
            {
                var entity = dbCtx.PRODUCT_CATEGORY.Where(x => x.ID == obj.ID).FirstOrDefault();
                if (entity != null)
                {
                    entity.CATEGORY_NAME = obj.CATEGORY_NAME;
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


        public PRODUCT_CATEGORY GetById(string id)
        {
            string sql = $@"SELECT * FROM PRODUCT_CATEGORY WHERE ID = '{id}'";
            return dbCtx.Database.SqlQueryRaw<PRODUCT_CATEGORY>(sql).ToList().FirstOrDefault();
        }

        public EQResult Delete(string id)
        {
            EQResult eQResult = new EQResult();
            eQResult.entities = "PRODUCT_CATEGORY";
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
                var entity = dbCtx.PRODUCT_CATEGORY.Find(id);
                if (entity != null)
                {
                    //TODO : Delete property
                    dbCtx.PRODUCT_CATEGORY.Remove(entity);
                    eQResult.rows = dbCtx.SaveChanges();
                    eQResult.success = true;
                    eQResult.messages = NotifyService.DeletedSuccess(entity.CATEGORY_NAME!);
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
