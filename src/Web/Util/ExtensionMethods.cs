using System.Net.Http.Json;
using Domain.Exception.Body;

namespace Web.Util;

public static class ExtensionMethods
{
    public static async Task<PoeGamblingHelperExceptionBody> GetExceptionBody(this HttpContent content)
    {
        return await content.ReadFromJsonAsync<PoeGamblingHelperExceptionBody>() ??
               new PoeGamblingHelperExceptionBody(ExceptionType.InternalError, ExceptionId.CannotParseBackendException);
    }
}