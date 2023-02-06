using System.Net.Http.Json;
using Shared.Exception;

namespace PoEGamblingHelper3.Service.Util;

public static class ExtensionMethods
{
    public static async Task<PoeGamblingHelperExceptionBody> GetExceptionBody(this HttpContent content)
    {
        return await content.ReadFromJsonAsync<PoeGamblingHelperExceptionBody>() ??
               new PoeGamblingHelperExceptionBody(ExceptionType.InternalError, ExceptionId.CannotParseBackendException);
    }
}