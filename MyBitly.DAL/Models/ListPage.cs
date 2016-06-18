namespace MyBitly.DAL.Models
{
    using System.Collections.Generic;
    using Entities;

    public class ListPage<TEntity>
        where TEntity : Entity
    {
        public IEnumerable<TEntity> Data { get; set; }
        public int TotalCount { get; set; }
    }
}