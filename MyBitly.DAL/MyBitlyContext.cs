namespace MyBitly.DAL
{
    using System.Data.Entity;
    using Entities;
    using Utils.Helpers;

    public class MyBitlyContext : DbContext
    {
        public MyBitlyContext()
            : base(ConnectionStringHelper.ConnectionString)
        {
           
        }

        public virtual DbSet<UrlEntity> Urls { get; set; } 
    }
}