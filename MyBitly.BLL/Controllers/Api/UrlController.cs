namespace MyBitly.BLL.Controllers.Api
{
    using System.Net.Http;
    using System.Net.Http.Formatting;
    using System.Web.Http;
    using Castle.Windsor;
    using Base;
    using Models;
    using Services;

    public class UrlController : ApiControllerBase
    {
        private readonly IUrlService _urlService;

        public UrlController(IWindsorContainer container, IUrlService urlService)
            : base(container)
        {
            _urlService = urlService;
        }

        [HttpPost]
        public HttpResponseMessage Shorten(string longUrl)
        {
            try
            {
                var response = _urlService.Shorten(longUrl);
                return new HttpResponseMessage()
                {
                    Content =
                        new ObjectContent<Response>(new Response { Data = response },
                            new JsonMediaTypeFormatter(), "application/json")
                };
            }
            finally
            {
                if (_urlService != null) Container.Release(_urlService);
            }
        }

        [HttpGet]
        public HttpResponseMessage Info(string hash)
        {
            try
            {
                var response = _urlService.Get(hash);
                return new HttpResponseMessage()
                {
                    Content =
                        new ObjectContent<Response>(new Response { Data = response },
                            new JsonMediaTypeFormatter(), "application/json")
                };
            }
            finally
            {
                if (_urlService != null) Container.Release(_urlService);
            }
        }

        [HttpGet]
        public HttpResponseMessage LinkHistory([FromUri]UrlHistoryRequest request)
        {
            try
            {
                var response = _urlService.LinkHistory(request);
                return new HttpResponseMessage()
                {
                    Content =
                        new ObjectContent<ListResponse>(response,
                            new JsonMediaTypeFormatter(), "application/json")
                };
            }
            finally
            {
                if (_urlService != null) Container.Release(_urlService);
            }
        }
    }
}