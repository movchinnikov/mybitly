namespace MyBitly.BLL.ExceptionHandling
{
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Formatting;
    using System.Web.Http.Filters;
    using Common.Exceptions;
    using Models;

    public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            var e = actionExecutedContext.Exception;
            var response = new ExceptionResponse {Message = e.Message};

            var exception = e as MyBitlyException;
            if (exception != null)
            {
                response.Code = exception.Code;
                response.StatusCode = exception.StatusCode;
            }

            actionExecutedContext.Response =
                new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content =
                        new ObjectContent<Response>(new Response { Data = response },
                            new JsonMediaTypeFormatter(), "application/json")
                };
        }
    }
}