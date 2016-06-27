namespace MyBitly.DAL.Filters
{
    using System.Collections.Generic;
    using System.Linq;
    using Entities;

    public class UrlListFilter : ListFilterBase<UrlEntity>
    {
        /// <summary>
        /// Хэши записей, которые необходимо извлечь
        /// </summary>
        public IEnumerable<string> Hashes { get; set; }

        public override bool IsCorrect()
        {
            if (!base.IsCorrect()) return false;

            return Hashes != null && Hashes.Any();
        }

        public override IQueryable<UrlEntity> ApplyCustom(IQueryable<UrlEntity> query)
        {
            return query.Where(x => Hashes.Contains(x.Hash));
        }
    }
}