namespace MyBitly.DAL.Filters
{
    using System.Collections.Generic;
    using System.Linq;
    using Entities;

    public class UrlListFilter : BaseListFilter<UrlEntity>
    {
        public IEnumerable<string> Hashes { get; set; }

        public override bool IsCorrect()
        {
            if (!base.IsCorrect()) return false;

            return this.Hashes != null && this.Hashes.Any();
        }

        public override IQueryable<UrlEntity> ApplyCustom(IQueryable<UrlEntity> query)
        {
            return query.Where(x => this.Hashes.Contains(x.Hash));
        }
    }
}