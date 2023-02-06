using Shared.Exception;

namespace Backend.Exceptions;

public abstract class HttpException : PoeGamblingHelperException
{
    //PoeGamblingHelper 
    //Schüsselvawaltung
    //YouTubeDownloader
    protected HttpException(int statusCode, PoeGamblingHelperExceptionBody? body)
    {
        (StatusCode, Body) = (statusCode, body);
    }

    public int StatusCode { get; }
    public PoeGamblingHelperExceptionBody? Body { get; }
}