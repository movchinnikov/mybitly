namespace MyBitly.Tests
{
	using System;
	using System.Linq;
	using Base;
	using BLL.Models;
	using BLL.Services;
	using Castle.MicroKernel.Registration;
	using Common.Resources;
	using DAL.Entities;
	using Fake;
	using NUnit.Framework;

	[TestFixture]
	public class UrlServiceTest : TestBase
	{
		private IUrlService _urlService;
		
		[SetUp]
		public override void SetUp()
		{
			base.SetUp();

			Container.Register(
				Component.For<IUrlService>().ImplementedBy<UrlService>().LifestyleTransient()
				);
		}

		protected override void RegisterParamsHelper(ParamsRaw paramsRaw)
		{
			base.RegisterParamsHelper(paramsRaw);
			_urlService = Container.Resolve<IUrlService>();
		}

		[TestCase("localhost")]
		[TestCase("1254")]
		[TestCase("www.localhost")]
		public void Shorten_NoFormat(string longUrl)
		{
			RegisterParamsHelper(new ParamsRaw());

			var ex = InvokeAndAssertException(() => _urlService.Shorten(longUrl), MyBitlyResources.ShortenUrlException);
			Assert.AreEqual(MyBitlyResources.INVALID_ARG_URL, ex.Code);
			Assert.AreEqual(101, ex.StatusCode);
		}

		[TestCase("")]
		[TestCase(null)]
		public void Shorten_Empty(string longUrl)
		{
			RegisterParamsHelper(new ParamsRaw());

			var ex = InvokeAndAssertException(() => _urlService.Shorten(longUrl), MyBitlyResources.ShortenUrlException);
			Assert.AreEqual(MyBitlyResources.EMPTY_ARG_URL, ex.Code);
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
		public void Shorten(string longUrl)
		{
			var paramsRaw = new ParamsRaw();
			RegisterParamsHelper(paramsRaw);
			RegisterDbDependencies();

			var response = _urlService.Shorten(longUrl);
			Assert.NotNull(response);
			Assert.AreEqual(longUrl, response.LongUrl);
			Assert.IsNotNull(response.Hash);
			Assert.IsNotEmpty(response.Hash);

			using (var context = Factory.OpenSession())
			{
				var dbSet = context.Set<UrlEntity>();

				var dbResponse = dbSet.FirstOrDefault(x => x.Hash == response.Hash);
				Assert.NotNull(dbResponse);
				Assert.AreEqual(response.Hash, dbResponse.Hash);
				Assert.AreEqual(response.LongUrl, dbResponse.LongUrl);
				Assert.AreEqual(response.ShortUrl, string.Format("{0}/{1}", paramsRaw.ShortDomen, response.Hash));
			}
		}

		[Test]
		public void Get_NotExist()
		{
			RegisterParamsHelper(new ParamsRaw());
			RegisterDbDependencies();

			var ex = InvokeAndAssertException(() => _urlService.Get("1"), MyBitlyResources.GetException);
			Assert.AreEqual(MyBitlyResources.URL_NOT_FOUND, ex.Code);
			Assert.AreEqual(104, ex.StatusCode);
		}

		[TestCase("")]
		[TestCase(null)]
		public void Get_HashIsEmpty(string hash)
		{
			RegisterParamsHelper(new ParamsRaw());

			var ex = InvokeAndAssertException(() => _urlService.Get(hash), MyBitlyResources.GetException);
			Assert.AreEqual(MyBitlyResources.EMPTY_SHORT_URL, ex.Code);
			Assert.AreEqual(103, ex.StatusCode);
		}

		[Test]
		public void Get()
		{
			RegisterParamsHelper(new ParamsRaw());
			RegisterDbDependencies();

			const string hash = "тестовый";
			UrlEntity entity = null;
			using (var context = Factory.OpenSession())
			{
				var dbSet = context.Set<UrlEntity>();
				entity = new UrlEntity { Hash = hash, LongUrl = "http://google.ru", Title = "Google" };
				dbSet.Add(entity);
				context.SaveChanges();
			}
			var fromDb = _urlService.Get(hash);

			Assert.NotNull(fromDb);
			Assert.AreEqual(entity.Hash, fromDb.Hash);
			Assert.AreEqual(entity.LongUrl, fromDb.LongUrl);
		}

		[Test]
		public void GetList_NegativeOffset()
		{
			RegisterParamsHelper(new ParamsRaw());

			var request = new UrlHistoryRequest {Offset = -1, Hashes = new[] {"googleru", "yandexru"}};

			var ex = InvokeAndAssertException(() => _urlService.LinkHistory(request), MyBitlyResources.GetListException);
			Assert.AreEqual(MyBitlyResources.INVALID_REQUEST, ex.Code);
			Assert.AreEqual(105, ex.StatusCode);
		}

		[Test]
		public void GetList_NullRequest()
		{
			RegisterParamsHelper(new ParamsRaw());

			var ex = InvokeAndAssertException(() => _urlService.LinkHistory(null), MyBitlyResources.GetListException);
			Assert.AreEqual(MyBitlyResources.INVALID_REQUEST, ex.Code);
			Assert.AreEqual(105, ex.StatusCode);
		}

		[Test]
		public void GetList_NegativeLimit()
		{
			RegisterParamsHelper(new ParamsRaw());

			var request = new UrlHistoryRequest { Limit = -1, Hashes = new[] { "googleru", "yandexru" } };

			var ex = InvokeAndAssertException(() => _urlService.LinkHistory(request), MyBitlyResources.GetListException);
			Assert.AreEqual(MyBitlyResources.INVALID_REQUEST, ex.Code);
			Assert.AreEqual(105, ex.StatusCode);
		}

		[Test]
		public void GetList_NullHashes()
		{
			RegisterParamsHelper(new ParamsRaw());

			var request = new UrlHistoryRequest { Limit = -1, Hashes = null };

			var ex = InvokeAndAssertException(() => _urlService.LinkHistory(request), MyBitlyResources.GetListException);
			Assert.AreEqual(MyBitlyResources.INVALID_REQUEST, ex.Code);
			Assert.AreEqual(105, ex.StatusCode);
		}

		[Test]
		public void GetList_EmptyHashes()
		{
			RegisterParamsHelper(new ParamsRaw());

			var request = new UrlHistoryRequest { Limit = -1, Hashes = new String[0] };

			var ex = InvokeAndAssertException(() => _urlService.LinkHistory(request), MyBitlyResources.GetListException);
			Assert.AreEqual(MyBitlyResources.INVALID_REQUEST, ex.Code);
			Assert.AreEqual(105, ex.StatusCode);
		}

		[Test]
		public void GetList()
		{
			RegisterParamsHelper(new ParamsRaw());
			RegisterDbDependencies();

			using (var context = Factory.OpenSession())
			{
				var dbSet = context.Set<UrlEntity>();

				var entity = new UrlEntity {Hash = "googleru", LongUrl = "http://google.ru", Title = "Google"};
				dbSet.Add(entity);
				entity = new UrlEntity {Hash = "vkrulink", LongUrl = "http://vk.ru", Title = "Вконтакте"};
				dbSet.Add(entity);
				entity = new UrlEntity {Hash = "yandexru", LongUrl = "http://ya.ru", Title = "Яндекс"};
				dbSet.Add(entity);
				context.SaveChanges();
			}

			var response = _urlService.LinkHistory(new UrlHistoryRequest { Hashes = new[] { "googleru", "yandexru" } });
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
		public void GetList_HasLimit()
		{
			RegisterParamsHelper(new ParamsRaw());
			RegisterDbDependencies();
			
			using (var context = Factory.OpenSession())
			{
				var dbSet = context.Set<UrlEntity>();

				var entity = new UrlEntity {Hash = "googleru", LongUrl = "http://google.ru", Title = "Google"};
				dbSet.Add(entity);
				entity = new UrlEntity {Hash = "vkrulink", LongUrl = "http://vk.ru", Title = "Вконтакте"};
				dbSet.Add(entity);
				entity = new UrlEntity {Hash = "yandexru", LongUrl = "http://ya.ru", Title = "Яндекс"};
				dbSet.Add(entity);
				context.SaveChanges();
			}

			var response = _urlService.LinkHistory(new UrlHistoryRequest { Limit = 1, Hashes = new[] { "googleru", "yandexru" } });
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
		public void GetList_HasOffset()
		{
			RegisterParamsHelper(new ParamsRaw());
			RegisterDbDependencies();

			using (var context = Factory.OpenSession())
			{
				var dbSet = context.Set<UrlEntity>();

				var entity = new UrlEntity {Hash = "googleru", LongUrl = "http://google.ru", Title = "Google"};
				dbSet.Add(entity);
				entity = new UrlEntity {Hash = "vkrulink", LongUrl = "http://vk.ru", Title = "Вконтакте"};
				dbSet.Add(entity);
				entity = new UrlEntity {Hash = "yandexru", LongUrl = "http://ya.ru", Title = "Яндекс"};
				dbSet.Add(entity);
				context.SaveChanges();
			}

			var response = _urlService.LinkHistory(new UrlHistoryRequest { Offset = 1, Hashes = new[] { "googleru", "yandexru" } });
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
		public void GetList_HasOffsetAndLimit()
		{
			RegisterParamsHelper(new ParamsRaw());
			RegisterDbDependencies();


			using (var context = Factory.OpenSession())
			{
				var dbSet = context.Set<UrlEntity>();

				var entity = new UrlEntity {Hash = "googleru", LongUrl = "http://google.ru", Title = "Google"};
				dbSet.Add(entity);
				entity = new UrlEntity {Hash = "vkrulink", LongUrl = "http://vk.ru", Title = "Вконтакте"};
				dbSet.Add(entity);
				entity = new UrlEntity {Hash = "yandexru", LongUrl = "http://ya.ru", Title = "Яндекс"};
				dbSet.Add(entity);
				context.SaveChanges();
			}

			var response = _urlService.LinkHistory(new UrlHistoryRequest { Limit = 1, Offset = 1, Hashes = new[] { "googleru", "yandexru" } });
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
		public void GetList_EmptyResult()
		{
			RegisterParamsHelper(new ParamsRaw());
			RegisterDbDependencies();
			
			var response = _urlService.LinkHistory(new UrlHistoryRequest { Hashes = new[] { "тестовый" } });
			Assert.NotNull(response);
			Assert.NotNull(response.Data);
			Assert.AreEqual(0, response.Data.Count());
			Assert.AreEqual(0, response.Count);
		}

		[Test]
		public void Increment_NotExisitsHash()
		{
			RegisterParamsHelper(new ParamsRaw());
			RegisterDbDependencies();

			var ex = InvokeAndAssertException(() => _urlService.Increment("тестовый"), MyBitlyResources.NotFoundException);
			Assert.AreEqual(MyBitlyResources.URL_NOT_FOUND, ex.Code);
			Assert.AreEqual(104, ex.StatusCode);
		}

		[TestCase("")]
		[TestCase(null)]
		public void Increment_HashIsEmpty(string hash)
		{
			RegisterParamsHelper(new ParamsRaw());

			var ex = InvokeAndAssertException(() => _urlService.Increment(hash), MyBitlyResources.GetException);
			Assert.AreEqual(MyBitlyResources.EMPTY_SHORT_URL, ex.Code);
			Assert.AreEqual(103, ex.StatusCode);
		}

		[Test]
		public void Increment()
		{
			RegisterParamsHelper(new ParamsRaw());
			RegisterDbDependencies();

			using (var context = Factory.OpenSession())
			{
				var dbSet = context.Set<UrlEntity>();

				var entity = new UrlEntity {Hash = "yandexru", LongUrl = "http://ya.ru", Title = "Яндекс"};
				dbSet.Add(entity);
				context.SaveChanges();
			}

			var response = _urlService.Increment("yandexru");
			Assert.NotNull(response);
			Assert.AreEqual(1, response.Clicks);
			
			response = _urlService.Increment("yandexru");
			Assert.NotNull(response);
			Assert.AreEqual(2, response.Clicks);

			response = _urlService.Increment("yandexru");
			Assert.NotNull(response);
			Assert.AreEqual(3, response.Clicks);
		}
	}
}