using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Mirality.Blazor.AutoFocus;

/// <summary>When this component is first rendered, it will automatically focus
/// the first child element with the <c>autofocus</c> attribute/class.</summary>
public class AutoFocus : ComponentBase
{
    [Inject] private AutoFocusService Service { get; set; } = default!;

    /// <summary>Child content for this component</summary>
    [Parameter] public RenderFragment? ChildContent { get; set; }

    /// <summary>Additional custom attributes for the rendered div</summary>
    [Parameter(CaptureUnmatchedValues = true)] public Dictionary<string, object>? Attributes { get; set; }

    private ElementReference _Self;

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await Service.AutoFocus(_Self);
        }

        await base.OnAfterRenderAsync(firstRender);
    }

    /// <inheritdoc />
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenElement(1, "div");
        builder.AddMultipleAttributes(2, Attributes);
        builder.AddElementReferenceCapture(3, r => _Self = r);
        builder.AddContent(4, ChildContent);
        builder.CloseElement();
    }
}
