namespace MyBitly.BLL.Controllers.Api
{
    using System.Net.Http;
    using System.Net.Http.Formatting;
    using System.Web.Http;
    using Base;
    using Castle.Windsor;
    using Models;
    using Services;

    public class UrlController : BaseApiController
    {
        private readonly IUrlService _urlService;

        public UrlController(IWindsorContainer container, IUrlService urlService)
            : base(container)
        {
            this._urlService = urlService;
        }

        [HttpPost]
        public HttpResponseMessage Shorten(string longUrl)
        {
            try
            {
                var response = this._urlService.Shorten(longUrl);
                return new HttpResponseMessage()
                {
                    Content =
                        new ObjectContent<Response>(new Response { Data = response },
                            new JsonMediaTypeFormatter(), "application/json")
                };
            }
            finally
            {
                if (this._urlService != null) this.Container.Release(this._urlService);
            }
        }

        [HttpGet]
        public HttpResponseMessage Info(string hash)
        {
            try
            {
                var response = this._urlService.Get(hash);
                return new HttpResponseMessage()
                {
                    Content =
                        new ObjectContent<Response>(new Response { Data = response },
                            new JsonMediaTypeFormatter(), "application/json")
                };
            }
            finally
            {
                if (this._urlService != null) this.Container.Release(this._urlService);
            }
        }

        [HttpGet]
        public HttpResponseMessage LinkHistory(UrlHistoryRequest request)
        {
            try
            {
                var response = this._urlService.LinkHistory(request);
                return new HttpResponseMessage()
                {
                    Content =
                        new ObjectContent<Response>(new Response { Data = response },
                            new JsonMediaTypeFormatter(), "application/json")
                };
            }
            finally
            {
                if (this._urlService != null) this.Container.Release(this._urlService);
            }
        }
    }
}