using EasyTrufi.Core.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EasyTrufi.Infraestructure.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        //private readonly ILogger<GlobalExceptionFilter> _logger;

        public void OnException(ExceptionContext context)
        { 
            if (context.Exception is BussinesException exception)
            {
                var validation = new
                {
                    Status = 400,
                    Title = "Bad Request",
                    Detail = exception.Message
                };

                var json = new
                {
                    errors = new[] { validation }
                };

                context.Result = new BadRequestObjectResult(json);
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
            else
            {
                // handle other exception types, e.g.:
                var validation = new
                {
                    Status = 500,
                    Title = "Internal Server Error",
                    Detail = context.Exception.Message
                };
                var json = new { errors = new[] { validation } };
                context.Result = new ObjectResult(json) { StatusCode = 500 };
            }
            context.ExceptionHandled = true;
        }
    }
}
