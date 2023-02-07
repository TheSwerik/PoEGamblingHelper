using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Backend.Exceptions;

internal class HttpResponseExceptionFilter : IActionFilter, IOrderedFilter
{
    public void OnActionExecuting(ActionExecutingContext context) { }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Exception is not HttpException httpException) return;
        context.Result = new ObjectResult(httpException.Body)
                         { StatusCode = httpException.StatusCode };
        context.ExceptionHandled = true;
    }

    public int Order => int.MaxValue - 10;
}