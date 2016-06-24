namespace MyBitly.DAL.Filters
{
    using System.ComponentModel;
    using System.Data.Entity;
    using System.Linq;
    using Entities;
    using Extensions;

    public class ListFilterBase<TEntity>
        where TEntity : EntityBase
    {
        /// <summary>
        /// Количество записей, которое необходимо вернуть с результатом
        /// </summary>
        public int Limit { get; set; }

        /// <summary>
        /// Количество записей, которые необходимо пропустить
        /// </summary>
        public int Offset { get; set; }

        /// <summary>
        /// Параметры сортировки
        /// </summary>
        public SortParam[] SortParams { get; set; }

        public ListFilterBase()
        {
            SortParams = new[] {new SortParam {Direction = ListSortDirection.Descending, Field = "Id"}};
        }

        public virtual bool IsCorrect()
        {
            if (this.Limit < 0) return false;

            return this.Offset >= 0;
        }

        public virtual IQueryable<TEntity> Apply(DbSet<TEntity> dbSet)
        {
            IQueryable<TEntity> result = this.ApplyCustom(dbSet);

            foreach (var sortParam in this.SortParams)
            {
                result = sortParam.Direction == ListSortDirection.Ascending ?
                    result.OrderBy(sortParam.Field) :
                    result.OrderByDescending(sortParam.Field);
            }

            if (this.Offset > 0)
                result = result.Skip(this.Offset);
            if (this.Limit > 0)
                result = result.Take(this.Limit);

            return result;
        }

        public virtual IQueryable<TEntity> ApplyCustom(IQueryable<TEntity> query)
        {
            return query;
        }
    }

    public struct SortParam
    {
        /// <summary>
        /// Наименование поля для сортировки
        /// </summary>
        public string Field { get; set; }

        /// <summary>
        /// Направление сортировки
        /// </summary>
        public ListSortDirection Direction { get; set; }
    }
}