using Becomex.Robo.Application.Exceptions;
using Becomex.Robo.Application.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using System.Threading.Tasks;

namespace Becomex.Robo.Application.Filters
{
    public class ApplicationExceptionFilter : IAsyncExceptionFilter
    {
        public Task OnExceptionAsync(ExceptionContext context)
        {
            var exception = context.Exception;

            var error = new ExceptionModel()
            {
                Code = exception.GetType() == typeof(BusinessException) ? exception.Source : "EX000",
                Message = exception.Message,
                Type = exception.GetType().Name.ToString()
            };

            HttpResponse response = context.HttpContext.Response;
            response.StatusCode = (int)HttpStatusCode.InternalServerError;
            response.ContentType = "application/json";
            context.Result = new JsonResult(error);

            return Task.CompletedTask;
        }
    }
}
