namespace MyBitly.BLL.Services
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Castle.Windsor;
    using Common.Params;
    using Common.Resources;
    using DAL.Entities;
    using DAL.Filters;
    using DAL.Repositories;
    using Exceptions;
    using Models;

    public class UrlService : BaseService, IUrlService
    {
        private readonly IParamsHelper _paramsHelper;

        public UrlService(IWindsorContainer container, IParamsHelper paramsHelper)
            : base(container)
        {
            this._paramsHelper = paramsHelper;
        }

        private IUrlRepository Repository
        {
            get { return this.Container.Resolve<IUrlRepository>(); }
        }

        #region

        public ShortenResponse Shorten(string longUrl)
        {
            try
            {
                BeforeShorten(longUrl);

                UrlEntity entity = null;
                try
                {
                    var hash = Guid.NewGuid().GetHashCode().ToString("x");
                    entity = this.Repository.Create(new UrlEntity { LongUrl = longUrl, Hash = hash, Title = longUrl });
                }
                catch (Exception e)
                {
                    throw new MyBitlyException(MyBitlyResources.ShortenUrlException)
                    {
                        Code = MyBitlyResources.EXCEPTION_STORAGE_URL,
                        StatusCode = 102
                    };
                }

                this.SetPageTitle(entity);

                var response = (ShortenResponse) entity;
                response.ShortUrl = string.Format("{0}/{1}", this._paramsHelper.ShortDomen, response.Hash);

                return response;
            }
            catch (MyBitlyException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new MyBitlyException(MyBitlyResources.ShortenUrlException)
                {
                    Code = MyBitlyResources.TECH_EXCEPTION,
                    StatusCode = 106
                };
            }
        }

        private void SetPageTitle(UrlEntity entity)
        {
            Task.Run(() =>
            {
                entity.Title = GetPageTitle(entity.LongUrl);
                this.Repository.SetPageTitle(entity);
            });
        }

        private static void BeforeShorten(string longUrl)
        {
            if (string.IsNullOrWhiteSpace(longUrl))
                throw new MyBitlyException(MyBitlyResources.ShortenUrlException)
                {
                    Code = MyBitlyResources.EMPTY_ARG_URL,
                    StatusCode = 100
                };

            Uri urlResult;
            var canCreateUrl = Uri.TryCreate(longUrl, UriKind.Absolute, out urlResult);

            if (!canCreateUrl)
                throw new MyBitlyException(MyBitlyResources.ShortenUrlException)
                {
                    Code = MyBitlyResources.INVALID_ARG_URL,
                    StatusCode = 101
                };
        }

        #endregion

        #region Get

        public ShortenResponse Get(string hash)
        {
            try
            {
                BeforeGet(hash);

                var entity = this.Repository.Get(hash);

                if (entity == null)
                    throw new MyBitlyException(MyBitlyResources.GetException)
                    {
                        Code = MyBitlyResources.URL_NOT_FOUND,
                        StatusCode = 104
                    };

                var response = (ShortenResponse) entity;
                response.ShortUrl = string.Format("{0}/{1}", this._paramsHelper.ShortDomen, response.Hash);

                return response;
            }
            catch (MyBitlyException)
            {
                throw;
            }
            catch (Exception)
            {
                throw new MyBitlyException(MyBitlyResources.GetException)
                {
                    Code = MyBitlyResources.TECH_EXCEPTION,
                    StatusCode = 106
                };
            }
        }

        private static void BeforeGet(string hash)
        {
            if (string.IsNullOrWhiteSpace(hash))
                throw new MyBitlyException(MyBitlyResources.GetException)
                {
                    Code = MyBitlyResources.EMPTY_SHORT_URL,
                    StatusCode = 103
                };
        }

        #endregion

        #region GetList

        public ListResponse LinkHistory(UrlHistoryRequest request)
        {
            try
            {
                var filter = BeforeGetList(request);

                var entities = this.Repository.GetList(filter);

                if (entities == null || entities.Data == null)
                    throw new MyBitlyException(MyBitlyResources.GetListException)
                    {
                        Code = MyBitlyResources.TECH_EXCEPTION,
                        StatusCode = 106
                    };

                return new ListResponse
                {
                    Data = entities.Data.Select(x =>
                    {
                        var response = (ShortenResponse) x;
                        response.ShortUrl = string.Format("{0}/{1}", this._paramsHelper.ShortDomen, response.Hash);

                        return response;
                    }),
                    Count = entities.TotalCount
                };
            }
            catch (MyBitlyException)
            {
                throw;
            }
            catch (Exception)
            {
                throw new MyBitlyException(MyBitlyResources.GetListException)
                {
                    Code = MyBitlyResources.TECH_EXCEPTION,
                    StatusCode = 106
                };
            }
        }

        private static UrlListFilter BeforeGetList(UrlHistoryRequest request)
        {
            if (request == null)
                throw new MyBitlyException(MyBitlyResources.GetListException)
                {
                    Code = MyBitlyResources.INVALID_REQUEST,
                    StatusCode = 105
                };

            var filter = BuildListFilter(request);

            if (!filter.IsCorrect())
                throw new MyBitlyException(MyBitlyResources.GetListException)
                {
                    Code = MyBitlyResources.INVALID_REQUEST,
                    StatusCode = 105
                };

            return filter;
        }

        #endregion

        public ShortenResponse Increment(string hash)
        {
            if (string.IsNullOrWhiteSpace(hash))
                throw new MyBitlyException(MyBitlyResources.GetException)
                {
                    Code = MyBitlyResources.EMPTY_SHORT_URL,
                    StatusCode = 103
                };

            var entity = this.Repository.Increment(hash);

            if (entity == null)
                throw new MyBitlyException(MyBitlyResources.NotFoundException)
                {
                    Code = MyBitlyResources.URL_NOT_FOUND,
                    StatusCode = 104
                };

            var response = (ShortenResponse)entity;
            response.ShortUrl = string.Format("{0}/{1}", this._paramsHelper.ShortDomen, response.Hash);

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