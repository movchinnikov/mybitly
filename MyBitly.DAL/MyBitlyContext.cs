namespace MyBitly.DAL
{
    using System;
    using System.Data.Entity;
    using Common.Exceptions;
    using Common.Params;
    using Common.Resources;
    using Entities;

    public class MyBitlyContext : DbContext
    {
        public MyBitlyContext(IParamsHelper paramsHelper)
        {
            try
            {
                this.Database.Connection.ConnectionString = paramsHelper.ConnectionString;
            }
            catch (Exception e)
            {
                throw new MyBitlyException(MyBitlyResources.TechnicalException)
                {
                    Code = MyBitlyResources.TECH_EXCEPTION,
                    StatusCode = 106
                };
            }
            
        }

        public virtual DbSet<UrlEntity> Urls { get; set; } 
    }
}