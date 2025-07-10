using System.Net;
using System.Net.Http.Json;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using PoEGamblingHelper.Application.Exception.Body;
using PoEGamblingHelper.Web.Extensions;

namespace PoEGamblingHelper.Web.Services.Interfaces;

public abstract class HttpService(HttpClient httpClient, IToastService toastService)
{
    protected IToastService ToastService { get; } = toastService;
    protected HttpClient HttpClient { get; } = httpClient;

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

    protected async Task<T?> GetAsync<T>(string url, params (string name, string value)[] headers)
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);
            foreach (var (name, value) in headers) request.Headers.Add(name, value);
            var response = await HttpClient.SendAsync(request);

            if (response.IsSuccessStatusCode) return await response.Content.ReadFromJsonAsync<T>();

            if (response.StatusCode == HttpStatusCode.Unauthorized) return default;

            var exceptionBody = await response.Content.GetExceptionBody();
            ToastService.ShowError($"Error: {exceptionBody.Id.ToIdString()}");

            return default;
        }
        catch (HttpRequestException)
        {
            ToastService.ShowError("Cannot connect to Server.");
            return default;
        }
    }

    protected async Task GetAsync(string url, params (string name, string value)[] headers)
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);
            foreach (var (name, value) in headers) request.Headers.Add(name, value);
            var response = await HttpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.Unauthorized) return;
                var exceptionBody = await response.Content.GetExceptionBody();
                ToastService.ShowError($"Error: {exceptionBody.Id.ToIdString()}");
            }
        }
        catch (HttpRequestException)
        {
            ToastService.ShowError("Cannot connect to Server.");
        }
    }

    protected async Task<T?> PostAsync<T>(string url, object body)
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);
            request.Content = JsonContent.Create(body);
            var response = await HttpClient.SendAsync(request);

            if (response.IsSuccessStatusCode) return await response.Content.ReadFromJsonAsync<T>();
            if (response.StatusCode == HttpStatusCode.Unauthorized) return default;

            var exceptionBody = await response.Content.GetExceptionBody();
            ToastService.ShowError($"Error: {exceptionBody.Id.ToIdString()}");
            return default;
        }
        catch (HttpRequestException)
        {
            ToastService.ShowError("Cannot connect to Server.");
            return default;
        }
    }
}