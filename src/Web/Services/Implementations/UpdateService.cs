using Web.Services.Interfaces;

namespace Web.Services.Implementations;

public class UpdateService : IUpdateService
{
    private readonly CancellationTokenSource _tokenSource = new();
    private bool _isUpdating;
    private DateTime _lastUpdate = DateTime.MinValue;
    private TimeSpan? _updateInterval = TimeSpan.FromSeconds(1);
    private Task _updateTask = null!;
    private event AsyncEventHandler? _onUpdate;
    private event AsyncEventHandler? OnUiUpdate;

    #region public methods

    public void Init()
    {
        _updateTask = Task.Run(async () =>
                               {
                                   while (true)
                                   {
                                       if (UpdateInterval is not null)
                                           while (_isUpdating || NextUpdate() > DateTime.Now)
                                           {
                                               _tokenSource.Token.ThrowIfCancellationRequested();
                                               if (OnUiUpdate is not null) await OnUiUpdate.Invoke(this);
                                               await Task.Delay(1000);
                                           }

                                       _tokenSource.Token.ThrowIfCancellationRequested();
                                       await Update();
                                   }
                               });
    }

    public async Task Update()
    {
        _isUpdating = true;
        if (_onUpdate is not null) await _onUpdate.Invoke(this);
        _lastUpdate = DateTime.Now;
        _isUpdating = false;
        if (OnUiUpdate is not null) await OnUiUpdate.Invoke(this);
    }

    AsyncEventHandler? IUpdateService.OnUpdate
    {
        get => _onUpdate;
        set => _onUpdate = value;
    }

    AsyncEventHandler? IUpdateService.OnUiUpdate
    {
        get => OnUiUpdate;
        set => OnUiUpdate = value;
    }

    public TimeSpan? UpdateInterval
    {
        get => _updateInterval;
        set => _updateInterval = value;
    }

    public DateTime? NextUpdate() { return _updateInterval is null ? null : _lastUpdate.Add(_updateInterval.Value); }
    public DateTime LastUpdate() { return _lastUpdate; }
    public bool IsUpdating() { return _isUpdating; }

    public delegate Task AsyncEventHandler(object sender);

    #endregion

    #region Dispose

    public async ValueTask DisposeAsync()
    {
        await DisposeAsyncCore().ConfigureAwait(false);
        GC.SuppressFinalize(this);
    }

    protected virtual async ValueTask DisposeAsyncCore()
    {
        _tokenSource.Cancel();
        try
        {
            await _updateTask;
        }
        catch (OperationCanceledException)
        {
        }
        finally
        {
            _updateTask.Dispose();
            _tokenSource.Dispose();
        }
    }

    #endregion
}