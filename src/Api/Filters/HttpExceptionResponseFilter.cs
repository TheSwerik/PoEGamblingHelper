using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PoEGamblingHelper.Application.Exception.Abstract;

namespace PoEGamblingHelper.Api.Filters;

// ReSharper disable once ClassNeverInstantiated.Global
internal class HttpExceptionResponseFilter : IActionFilter, IOrderedFilter
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