using PoEGamblingHelper.Application.QueryParameters;
using PoEGamblingHelper.Domain.Entity.Gem;

namespace PoEGamblingHelper.Application.Util;

public static class ExtensionMethods
{
    public static (int skipSize, int takeSize) ConvertToSizes(this PageRequest pageRequest)
    {
        return (pageRequest.PageSize * pageRequest.PageNumber, pageRequest.PageSize);
    }

    public static int LevelModifier(this ResultCase resultCase) { return (int)resultCase - 1; }
}