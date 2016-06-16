namespace MyBitly.BLL.Controllers.Api
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Formatting;
    using System.Web.Http;

    public class UrlController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage Shorten(string longUrl)
        {
            Uri uriResult;
            var result = Uri.TryCreate(longUrl, UriKind.Absolute, out uriResult);
            if (result != false)
                return new HttpResponseMessage()
                {
                    Content =
                        new ObjectContent<Response>(new Response {Data = new {url = "http://bit.ly/ze6poY"}},
                            new JsonMediaTypeFormatter(), "application/json")
                };
            else
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError) { Content = new StringContent("Не удалось сократить эту ссылку") });
            }
        }
    }

    public class Response
    {
        public object Data { get; set; }
    }
}