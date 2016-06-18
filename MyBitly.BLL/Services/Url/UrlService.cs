namespace MyBitly.BLL.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Castle.Windsor;
    using DAL.Entities;
    using DAL.Filters;
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
            UrlEntity entity = null;
            try
            {
                hash = Guid.NewGuid().GetHashCode().ToString("x");
                entity = this.Repository.Create(new UrlEntity { LongUrl = longUrl, Hash = hash });
            }
            catch (Exception e)
            {
                throw new MyBitlyException(MyBitleResources.ShortenUrlException)
                {
                    Code = MyBitleResources.EXCEPTION_STORAGE_URL,
                    StatusCode = 102
                };
            }

            return new ShortenResponse { 
                Id = entity.Id, 
                LongUrl = entity.LongUrl, 
                Hash = entity.Hash, 
                ShortUrl = string.Format("{0}/{1}", "http://localhost:21460", hash) };
        }

        public ShortenResponse Get(string hash)
        {
            if (string.IsNullOrWhiteSpace(hash))
                throw new MyBitlyException(MyBitleResources.HashIsNullOrEmptyException)
                {
                    Code = MyBitleResources.EMPTY_SHORT_URL,
                    StatusCode = 103
                };

            var entity = this.Repository.Get(hash);

            if (entity == null)
                throw new MyBitlyException(MyBitleResources.NotFoundException)
                {
                    Code = MyBitleResources.URL_NOT_FOUND,
                    StatusCode = 104
                };

            return new ShortenResponse
            {
                Id = entity.Id, 
                LongUrl = entity.LongUrl, 
                Hash = entity.Hash, 
                ShortUrl = string.Format("{0}/{1}", "http://localhost:21460", entity.Hash)
            };
        }

        public ListResponse LinkHistory(UrlHistoryRequest request)
        {
            if (request == null)
                throw new MyBitlyException(MyBitleResources.InvalidRequestException)
                {
                    Code = MyBitleResources.INVALID_REQUEST,
                    StatusCode = 105
                };

            var filter = BuildListFilter(request);

            if (!filter.IsCorrect())
                throw new MyBitlyException(MyBitleResources.InvalidRequestException)
                {
                    Code = MyBitleResources.INVALID_REQUEST,
                    StatusCode = 105
                };

            var entities = this.Repository.GetList(filter);

            if (entities == null || entities.Data == null)
                throw new MyBitlyException(MyBitleResources.UNKNOW_EXCEPTION)
                {
                    Code = MyBitleResources.UNKNOW_EXCEPTION,
                    StatusCode = 106
                };

            return new ListResponse
            {
                Data = entities.Data.Select(x => new ShortenResponse
                {
                    Hash = x.Hash,
                    Id = x.Id,
                    LongUrl = x.LongUrl,
                    ShortUrl = string.Format("{0}/{1}", "http://localhost:21460", x.Hash)
                }),
                Count = entities.TotalCount
            };
        }

        private static UrlListFilter BuildListFilter(UrlHistoryRequest request)
        {
            return new UrlListFilter
            {
                Offset = request.Offset, 
                Limit = request.Limit, 
                Hashes = request.Hashes
            };
        }
    }
}