using System.Net.Http.Json;
using PoEGamblingHelper.Application.Exception.Body;

namespace PoEGamblingHelper.Web.Extensions;

public static class HttpContentExtensions
{
    public static async Task<PoeGamblingHelperExceptionBody> GetExceptionBody(this HttpContent content)
    {
        return await content.ReadFromJsonAsync<PoeGamblingHelperExceptionBody>() ??
               new PoeGamblingHelperExceptionBody(ExceptionType.InternalError, ExceptionId.CannotParseBackendException);
    }
}