using Microsoft.AspNetCore.Mvc.Rendering;

namespace Shop.ERP.Services
{
    public class UnitsService
    {
        private readonly AppDbContext dbCtx;
        public UnitsService(AppDbContext dbContext)
        {
            dbCtx = dbContext;
        }
        public SelectList GetUnitListItems()
        {
            return new SelectList(dbCtx.UNITS.ToList(), "ID", "UNIT_NAME");
        }
        public List<UNITS> GetAll()
        {
            string sql = $@"SELECT * FROM UNITS ORDER BY UNIT_NAME";
            return dbCtx.Database.SqlQueryRaw<UNITS>(sql).ToList();
        }
        public EQResult Save(UNITS obj)
        {
            EQResult eQResult = new EQResult();
            eQResult.entities = "UNITS";
            if (obj.ID == Guid.Empty.ToString())
            {
                obj.ID = Guid.NewGuid().ToString();
                dbCtx.UNITS.Add(obj);
                eQResult.rows = dbCtx.SaveChanges();
                eQResult.success = true;
                eQResult.messages = NotifyService.SaveSuccess();
                return eQResult;
            }
            else
            {
                var entity = dbCtx.UNITS.Where(x => x.ID == obj.ID).FirstOrDefault();
                if (entity != null)
                {
                    entity.UNIT_NAME = obj.UNIT_NAME;
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


        public UNITS GetById(string id)
        {
            string sql = $@"SELECT * FROM UNITS WHERE ID = '{id}'";
            return dbCtx.Database.SqlQueryRaw<UNITS>(sql).ToList().FirstOrDefault();
        }

        public EQResult Delete(string id)
        {
            EQResult eQResult = new EQResult();
            eQResult.entities = "UNITS";
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
                var entity = dbCtx.UNITS.Find(id);
                if (entity != null)
                {
                    //TODO : Delete property
                    dbCtx.UNITS.Remove(entity);
                    eQResult.rows = dbCtx.SaveChanges();
                    eQResult.success = true;
                    eQResult.messages = NotifyService.DeletedSuccess(entity.UNIT_NAME!);
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
