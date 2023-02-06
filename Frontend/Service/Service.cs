using System.Net.Http.Json;
using Blazored.Toast.Services;
using PoEGamblingHelper3.Service.Util;
using Shared.Exception;

namespace PoEGamblingHelper3.Service;

public abstract class Service
{
    protected Service(HttpClient httpClient, IToastService toastService)
    {
        HttpClient = httpClient;
        ToastService = toastService;
    }

    protected IToastService ToastService { get; }
    protected HttpClient HttpClient { get; }

    protected async Task<HttpResponseMessage> GetAsync(string url)
    {
        try
        {
            return await HttpClient.GetAsync(url);
        }
        catch (HttpRequestException)
        {
            ToastService.ShowError("Cannot connect to Server.");
            throw;
        }
    }

    protected async Task<T?> GetAsync<T>(string url)
    {
        try
        {
            var response = await HttpClient.GetAsync(url);

            if (response.IsSuccessStatusCode) return await response.Content.ReadFromJsonAsync<T>();

            var exceptionBody = await response.Content.GetExceptionBody();
            ToastService.ShowError($"Error: {exceptionBody.Id.ToIdString()}");
            return default;
        }
        catch (HttpRequestException)
        {
            ToastService.ShowError("Cannot connect to Server.");
            throw;
        }
    }
}