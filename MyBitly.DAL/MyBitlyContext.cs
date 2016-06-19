namespace MyBitly.DAL
{
    using System.Data.Entity;
    using Common.Params;
    using Entities;

    public class MyBitlyContext : DbContext
    {
        public MyBitlyContext(IParamsHelper paramsHelper)
        {
            this.Database.Connection.ConnectionString = paramsHelper.ConnectionString;
        }

        public virtual DbSet<UrlEntity> Urls { get; set; } 
    }
}