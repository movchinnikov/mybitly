namespace MyBitly.Tests
{
    using System;
    using System.Data.Entity;
    using System.Diagnostics;
    using System.Linq;
    using Base;
    using BLL.Models;
    using BLL.Resources;
    using BLL.Services;
    using Castle.MicroKernel.Registration;
    using DAL;
    using DAL.Entities;
    using DAL.Repositories;
    using DAL.UnitOfWork;
    using NUnit.Framework;

    [TestFixture]
    public class UrlServiceTest : BaseTest
    {
        private IUrlService _urlService;

        private DbSet<UrlEntity> _dbSet;
        private DbContext _dbContext;
        
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            this.Container.Register(
                Component.For<IUrlService>().ImplementedBy<UrlService>().LifestyleTransient(),
                Component.For<DbContext>().ImplementedBy<MyBitlyContext>().LifestylePerThread(),
                Component.For<EfUnitOfWorkInterceptor>().LifestyleTransient(),
                Component.For<IUrlRepository>().ImplementedBy<UrlRepository>()
                    .Interceptors<EfUnitOfWorkInterceptor>().LifestylePerThread()
                );

            this._urlService = this.Container.Resolve<IUrlService>();

            this._dbContext = this.Container.Resolve<DbContext>();
            this._dbSet = this._dbContext.Set<UrlEntity>();

            this._dbContext.Database.Log = sql => Debug.WriteLine(sql);
        }

        [TestCase("localhost")]
        [TestCase("1254")]
        [TestCase("www.localhost")]
        public void Shorten_NoFormat_Negative(string longUrl)
        {
            var ex = InvokeAndAssertException(() => this._urlService.Shorten(longUrl), MyBitleResources.ShortenUrlException);
            Assert.AreEqual(MyBitleResources.INVALID_ARG_URL, ex.Code);
            Assert.AreEqual(101, ex.StatusCode);
        }

        [TestCase("")]
        [TestCase(null)]
        public void Shorten_Empty_Negative(string longUrl)
        {
            var ex = InvokeAndAssertException(() => this._urlService.Shorten(longUrl), MyBitleResources.UrlIsNullOrEmptyException);
            Assert.AreEqual(MyBitleResources.EMPTY_ARG_URL, ex.Code);
            Assert.AreEqual(100, ex.StatusCode);
        }

        [TestCase("localhost:154")]
        [TestCase("www.localhost:154")]
        [TestCase("http://www.localhost:154")]
        [TestCase("http://www.localhost")]
        [TestCase("http://www.localhost/#")]
        [TestCase("http://www.localhost/#/")]
        [TestCase("http://www.localhost/#/foo")]
        [TestCase("http://www.localhost/#/foo/1")]
        [TestCase("http://www.localhost/#/foo?bar=dds&sfds")]
        [TestCase("http://www.localhost/#/foo/1?bar=dds&sfds")]
        [TestCase("http://www.localhost/#/foo?bar=dds&baz=sfds")]
        [TestCase("http://www.localhost/#/foo/1?bar=dds&baz=sfds")]
        [TestCase("http://www.localhost/#/foo/некий_русский_текст")]
        [TestCase("https://translate.google.ru/#ru/en/%D0%BD%D0%B5%20%D1%83%D0%B4%D0%B0%D0%BB%D0%BE%D1%81%D1%8C%20%D1%83%D0%BA%D0%BE%D1%80%D0%BE%D1%82%D0%B8%D1%82%D1%8C%20%D1%81%D1%81%D1%8B%D0%BB%D0%BA%D1%83")]
        [TestCase("https://www.localhost:154")]
        [TestCase("https://www.localhost")]
        [TestCase("ftp://www.localhost:154")]
        [TestCase("ftp://www.localhost")]
        public void Shorten_Positive(string longUrl)
        {
            var response = this._urlService.Shorten(longUrl);
            Assert.NotNull(response);
            Assert.AreEqual(longUrl, response.LongUrl);
            Assert.IsNotNull(response.Hash);
            Assert.IsNotEmpty(response.Hash);
            
            var dbResponse = this._dbSet.FirstOrDefault(x => x.Hash == response.Hash);
            Assert.NotNull(dbResponse);
            Assert.AreEqual(response.Hash, dbResponse.Hash);
            Assert.AreEqual(response.LongUrl, dbResponse.LongUrl);
        }

        [Test]
        public void Get_NotExist_Negative()
        {
            var ex = InvokeAndAssertException(() => this._urlService.Get("1"), MyBitleResources.NotFoundException);
            Assert.AreEqual(MyBitleResources.URL_NOT_FOUND, ex.Code);
            Assert.AreEqual(104, ex.StatusCode);
        }

        [TestCase("")]
        [TestCase(null)]
        public void Get_HashIsEmpty_Negative(string hash)
        {
            var ex = InvokeAndAssertException(() => this._urlService.Get(hash), MyBitleResources.HashIsNullOrEmptyException);
            Assert.AreEqual(MyBitleResources.EMPTY_SHORT_URL, ex.Code);
            Assert.AreEqual(103, ex.StatusCode);
        }

        [Test]
        public void Get_Positive()
        {
            const string hash = "тестовый";

            var entity = new UrlEntity {Hash = hash, LongUrl = "http://google.ru"};
            this._dbSet.Add(entity);
            this._dbContext.SaveChanges();

            var fromDb = this._urlService.Get(hash);

            Assert.NotNull(fromDb);
            Assert.AreEqual(entity.Hash, fromDb.Hash);
            Assert.AreEqual(entity.LongUrl, fromDb.LongUrl);
        }

        [Test]
        public void GetList_NegativeOffset_Negative()
        {
            var request = new UrlHistoryRequest {Offset = -1, Hashes = new[] {"googleru", "yandexru"}};

            var ex = InvokeAndAssertException(() => this._urlService.LinkHistory(request), MyBitleResources.InvalidRequestException);
            Assert.AreEqual(MyBitleResources.INVALID_REQUEST, ex.Code);
            Assert.AreEqual(105, ex.StatusCode);
        }

        [Test]
        public void GetList_NullRequest_Negative()
        {
            var ex = InvokeAndAssertException(() => this._urlService.LinkHistory(null), MyBitleResources.InvalidRequestException);
            Assert.AreEqual(MyBitleResources.INVALID_REQUEST, ex.Code);
            Assert.AreEqual(105, ex.StatusCode);
        }

        [Test]
        public void GetList_NegativeLimit_Negative()
        {
            var request = new UrlHistoryRequest { Limit = -1, Hashes = new[] { "googleru", "yandexru" } };

            var ex = InvokeAndAssertException(() => this._urlService.LinkHistory(request), MyBitleResources.InvalidRequestException);
            Assert.AreEqual(MyBitleResources.INVALID_REQUEST, ex.Code);
            Assert.AreEqual(105, ex.StatusCode);
        }

        [Test]
        public void GetList_NullHashes_Negative()
        {
            var request = new UrlHistoryRequest { Limit = -1, Hashes = null };

            var ex = InvokeAndAssertException(() => this._urlService.LinkHistory(request), MyBitleResources.InvalidRequestException);
            Assert.AreEqual(MyBitleResources.INVALID_REQUEST, ex.Code);
            Assert.AreEqual(105, ex.StatusCode);
        }

        [Test]
        public void GetList_EmptyHashes_Negative()
        {
            var request = new UrlHistoryRequest { Limit = -1, Hashes = new String[0] };

            var ex = InvokeAndAssertException(() => this._urlService.LinkHistory(request), MyBitleResources.InvalidRequestException);
            Assert.AreEqual(MyBitleResources.INVALID_REQUEST, ex.Code);
            Assert.AreEqual(105, ex.StatusCode);
        }

        [Test]
        public void GetList_Positive()
        {
            var entity = new UrlEntity { Hash = "googleru", LongUrl = "http://google.ru" };
            this._dbSet.Add(entity);
            entity = new UrlEntity { Hash = "vkrulink", LongUrl = "http://vk.ru" };
            this._dbSet.Add(entity);
            entity = new UrlEntity { Hash = "yandexru", LongUrl = "http://ya.ru" };
            this._dbSet.Add(entity);
            this._dbContext.SaveChanges();

            var response = this._urlService.LinkHistory(new UrlHistoryRequest { Hashes = new[] { "googleru", "yandexru" } });
            Assert.NotNull(response);
            Assert.NotNull(response.Data);
            Assert.AreEqual(2, response.Data.Count());
            Assert.AreEqual(2, response.Count);

            var yandexDbResponse = response.Data.First() as ShortenResponse;
            var googleDbResponse = response.Data.Last() as ShortenResponse;

            Assert.NotNull(googleDbResponse);
            Assert.NotNull(yandexDbResponse);

            Assert.AreEqual("googleru", googleDbResponse.Hash);
            Assert.AreEqual("yandexru", yandexDbResponse.Hash);

            Assert.AreEqual("http://google.ru", googleDbResponse.LongUrl);
            Assert.AreEqual("http://ya.ru", yandexDbResponse.LongUrl);
        }

        [Test]
        public void GetList_HasLimit_Positive()
        {
            var entity = new UrlEntity { Hash = "googleru", LongUrl = "http://google.ru" };
            this._dbSet.Add(entity);
            entity = new UrlEntity { Hash = "vkrulink", LongUrl = "http://vk.ru" };
            this._dbSet.Add(entity);
            entity = new UrlEntity { Hash = "yandexru", LongUrl = "http://ya.ru" };
            this._dbSet.Add(entity);
            this._dbContext.SaveChanges();

            var response = this._urlService.LinkHistory(new UrlHistoryRequest { Limit = 1, Hashes = new[] { "googleru", "yandexru" } });
            Assert.NotNull(response);
            Assert.NotNull(response.Data);
            Assert.AreEqual(1, response.Data.Count());
            Assert.AreEqual(2, response.Count);

            var yandexDbResponse = response.Data.First() as ShortenResponse;

            Assert.NotNull(yandexDbResponse);

            Assert.AreEqual("yandexru", yandexDbResponse.Hash);

            Assert.AreEqual("http://ya.ru", yandexDbResponse.LongUrl);
        }

        [Test]
        public void GetList_HasOffset_Positive()
        {
            var entity = new UrlEntity { Hash = "googleru", LongUrl = "http://google.ru" };
            this._dbSet.Add(entity);
            entity = new UrlEntity { Hash = "vkrulink", LongUrl = "http://vk.ru" };
            this._dbSet.Add(entity);
            entity = new UrlEntity { Hash = "yandexru", LongUrl = "http://ya.ru" };
            this._dbSet.Add(entity);
            this._dbContext.SaveChanges();

            var response = this._urlService.LinkHistory(new UrlHistoryRequest { Offset = 1, Hashes = new[] { "googleru", "yandexru" } });
            Assert.NotNull(response);
            Assert.NotNull(response.Data);
            Assert.AreEqual(1, response.Data.Count());
            Assert.AreEqual(2, response.Count);

            var googleDbResponse = response.Data.First() as ShortenResponse;

            Assert.NotNull(googleDbResponse);

            Assert.AreEqual("googleru", googleDbResponse.Hash);

            Assert.AreEqual("http://google.ru", googleDbResponse.LongUrl);
        }

        [Test]
        public void GetList_HasOffsetAndLimit_Positive()
        {
            var entity = new UrlEntity { Hash = "googleru", LongUrl = "http://google.ru" };
            this._dbSet.Add(entity);
            entity = new UrlEntity { Hash = "vkrulink", LongUrl = "http://vk.ru" };
            this._dbSet.Add(entity);
            entity = new UrlEntity { Hash = "yandexru", LongUrl = "http://ya.ru" };
            this._dbSet.Add(entity);
            this._dbContext.SaveChanges();

            var response = this._urlService.LinkHistory(new UrlHistoryRequest { Limit = 1, Offset = 1, Hashes = new[] { "googleru", "yandexru" } });
            Assert.NotNull(response);
            Assert.NotNull(response.Data);
            Assert.AreEqual(1, response.Data.Count());
            Assert.AreEqual(2, response.Count);

            var googleDbResponse = response.Data.First() as ShortenResponse;

            Assert.NotNull(googleDbResponse);

            Assert.AreEqual("googleru", googleDbResponse.Hash);

            Assert.AreEqual("http://google.ru", googleDbResponse.LongUrl);
        }

        [Test]
        public void GetList_EmptyResult_Positive()
        {
            var response = this._urlService.LinkHistory(new UrlHistoryRequest { Hashes = new[] { "тестовый" } });
            Assert.NotNull(response);
            Assert.NotNull(response.Data);
            Assert.AreEqual(0, response.Data.Count());
            Assert.AreEqual(0, response.Count);
        }
    }
}