using Domain.QueryParameters;

namespace Application.Util;

public static class ExtensionMethods
{
    public static (int skipSize, int takeSize) ConvertToSizes(this PageRequest pageRequest)
    {
        return (pageRequest.PageSize * pageRequest.PageNumber, pageRequest.PageSize);
    }
}