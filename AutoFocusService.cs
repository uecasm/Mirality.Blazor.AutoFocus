using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Mirality.Blazor.AutoFocus;

/// <summary>A service that will set input focus to an element with the <c>autofocus</c> attribute/class.</summary>
/// <remarks>Call from the <see cref="ComponentBase.OnAfterRenderAsync(bool)"/> handler of some component
/// or some other event where the elements are known to exist already.</remarks>
public class AutoFocusService : IAsyncDisposable
{
    /// <summary>Service injection constructor</summary>
    public AutoFocusService(IJSRuntime js)
    {
        _Module = new(() => js.InvokeAsync<IJSObjectReference>("import", "./_content/Mirality.Blazor.AutoFocus/autofocus.js").AsTask());
    }

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        if (_Module.IsValueCreated)
        {
            var module = await _Module.Value;
            await module.DisposeAsync();
        }
    }

    private readonly Lazy<Task<IJSObjectReference>> _Module;

    /// <summary>Sets input focus to the first descendant of the specified element that has the <c>autofocus</c> attribute/class.</summary>
    /// <param name="parent">The parent element, or <c>null</c> to search the whole page.</param>
    /// <returns>Awaitable</returns>
    public async Task AutoFocus(ElementReference? parent = null)
    {
        var module = await _Module.Value;
        await module.InvokeVoidAsync("autofocus", parent);
    }
}
