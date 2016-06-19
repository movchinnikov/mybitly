namespace MyBitly.BLL.Services
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Text.RegularExpressions;
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

            Uri urlResult;
            var canCreateUrl = Uri.TryCreate(longUrl, UriKind.Absolute, out urlResult);

            if (!canCreateUrl)
                throw new MyBitlyException(MyBitleResources.ShortenUrlException)
                {
                    Code = MyBitleResources.INVALID_ARG_URL,
                    StatusCode = 101
                };

            var title = GetPageTitle(longUrl);
            UrlEntity entity = null;
            try
            {
                var hash = Guid.NewGuid().GetHashCode().ToString("x");
                entity = this.Repository.Create(new UrlEntity { LongUrl = longUrl, Hash = hash, Title = title });
            }
            catch (Exception e)
            {
                throw new MyBitlyException(MyBitleResources.ShortenUrlException)
                {
                    Code = MyBitleResources.EXCEPTION_STORAGE_URL,
                    StatusCode = 102
                };
            }

            var response = (ShortenResponse) entity;
            response.ShortUrl = string.Format("{0}/{1}", "http://localhost:21460", response.Hash);

            return response;
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

            var response = (ShortenResponse)entity;
            response.ShortUrl = string.Format("{0}/{1}", "http://localhost:21460", response.Hash);

            return response;
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
                Data = entities.Data.Select(x => 
                {
                    var response = (ShortenResponse)x;
                    response.ShortUrl = string.Format("{0}/{1}", "http://localhost:21460", response.Hash);

                    return response;
                }),
                Count = entities.TotalCount
            };
        }

        public ShortenResponse Increment(string hash)
        {
            if (string.IsNullOrWhiteSpace(hash))
                throw new MyBitlyException(MyBitleResources.HashIsNullOrEmptyException)
                {
                    Code = MyBitleResources.EMPTY_SHORT_URL,
                    StatusCode = 103
                };

            var entity = this.Repository.Increment(hash);

            if (entity == null)
                throw new MyBitlyException(MyBitleResources.NotFoundException)
                {
                    Code = MyBitleResources.URL_NOT_FOUND,
                    StatusCode = 104
                };

            var response = (ShortenResponse)entity;
            response.ShortUrl = string.Format("{0}/{1}", "http://localhost:21460", response.Hash);

            return response;
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

        private static string GetPageTitle(string url)
        {
            StreamReader reader = null;
            Stream stream = null;
            HttpWebResponse pageResponse = null;
            try
            {
                var pageContent = WebRequest.Create(url);

                pageResponse = pageContent.GetResponse() as HttpWebResponse;
                if (pageResponse == null) return url;

                stream = pageResponse.GetResponseStream();
                if (stream == null) return url;

                var characterSet = "utf-8";

                if (pageResponse.CharacterSet != null) characterSet = pageResponse.CharacterSet;
                reader = new StreamReader(stream, Encoding.GetEncoding(characterSet));
                var readcontent = reader.ReadToEnd();

                var titleMatch = Regex.Match(readcontent, "<title>([^<]*)</title>",
                    RegexOptions.IgnoreCase | RegexOptions.Multiline);
                var pageTitle = titleMatch.Groups[1].Value;

                return pageTitle;
            }
            catch
            {
                return url;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                if (stream != null) stream.Dispose();
                if (pageResponse != null) pageResponse.Dispose();
            }
        }
    }
}