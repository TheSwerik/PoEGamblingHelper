using PoEGamblingHelper.Web.Services.Implementations;

namespace PoEGamblingHelper.Web.Services.Interfaces;

public interface IUpdateService : IAsyncDisposable
{
    TimeSpan? UpdateInterval { get; set; }
    UpdateService.AsyncEventHandler? OnUpdate { get; set; }
    UpdateService.AsyncEventHandler? OnUiUpdate { get; set; }
    void Init();
    Task Update();
    DateTime? NextUpdate();
    DateTime LastUpdate();
    bool IsUpdating();
}