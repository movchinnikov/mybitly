namespace MyBitly.DAL.Factory
{
    using System.Data.Entity;

    public interface ISessionFactory
    {
        DbContext OpenSession();
    }
}