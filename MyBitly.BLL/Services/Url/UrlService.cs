namespace MyBitly.BLL.Services
{
    using System;
    using Castle.Windsor;
    using DAL.Entities;
    using DAL.Repositories;
    using Exceptions;
    using Models;
    using Resources;

    public class UrlService : BaseService, IUrlService
    {
        public UrlService(IWindsorContainer container)
            : base(container)
        {
        }

        private IUrlRepository Repository
        {
            get { return this.Container.Resolve<IUrlRepository>(); }
        }

        public ShortenResponse Shorten(string longUrl)
        {
            if (string.IsNullOrWhiteSpace(longUrl))
                throw new MyBitlyException(MyBitleResources.UrlIsNullOrEmptyException)
                {
                    Code = MyBitleResources.EMPTY_ARG_URL,
                    StatusCode = 100
                };

            Uri uriResult;
            var canCreateUrl = Uri.TryCreate(longUrl, UriKind.Absolute, out uriResult);

            if (!canCreateUrl)
                throw new MyBitlyException(MyBitleResources.ShortenUrlException)
                {
                    Code = MyBitleResources.INVALID_ARG_URL,
                    StatusCode = 101
                };

            string hash;
            try
            {
                hash = Guid.NewGuid().GetHashCode().ToString("x");
                this.Repository.Create(new UrlEntity {LongUrl = longUrl, Hash = hash});
            }
            catch (Exception e)
            {
                throw new MyBitlyException(MyBitleResources.ShortenUrlException)
                {
                    Code = MyBitleResources.EXCEPTION_STORAGE_URL,
                    StatusCode = 102
                };
            }

            return new ShortenResponse {LongUrl = longUrl, Hash = hash};
        }
    }
}