![.NET 6](https://img.shields.io/static/v1?label=.NET&message=6&color=blue) [![NuGet version (Mirality.Blazor.AutoFocus)](https://img.shields.io/nuget/v/Mirality.Blazor.AutoFocus.svg?logo=nuget)](https://www.nuget.org/packages/Mirality.Blazor.AutoFocus/)

This is a simple Blazor interop service and component to support automatically setting input focus to an element without need of manually capturing an `ElementReference` and using the `Focus` method on it.

It's useful both to set an initial focus on navigation (where your pages are sufficiently different that the generic `FocusOnNavigate` is insufficient), and when displaying some kind of modal dialog popup.

# Basic Setup

1. Either use the precompiled package from nuget.org; or optionally, download this project and unpack it to a suitable location in your source tree (or reference it as a git submodule).

2. Add it to your solution if needed, and add it as a project/nuget reference to your application.

3. Optionally, add `@using Mirality.Blazor.AutoFocus` to your `_Imports.razor` file (otherwise you'll need to add this to any file that uses components from this library).

4. In your `Program.cs`, add this to your services (and add the corresponding `using` to the top of the file):

   ```c#
   builder.Services.AddAutoFocus();
   ```


# Selecting focus element

You can either apply the HTML5 `autofocus` attribute:

```html
<InputText autofocus ... />
```

Or the `autofocus` class:

```html
<InputText class="autofocus" ... />
```

Or using some condition on the attribute:

```html
<InputText autofocus="@_SomeBool" ... />
```

The first method is recommended for the common case, since it's shorter.  One of the latter may be more convenient when there is some dynamic component to selecting the focus element.

Neither attribute or class is preferred over the other; the first match of either will win.

You may apply these to more than one element, but focus will always be set to the first matching element.  This may be useful if some of your elements are only conditionally rendered.

# Usage

### Via wrapper component

1. Enclose your content in an `<AutoFocus>` element.
2. Select a child element to focus, as above.

```html
<EditForm Model="...">
    <AutoFocus class="...">
        <InputText autofocus ... />
        <InputText ... />
    </AutoFocus>
</EditForm>
```

Note that the `<AutoFocus>` component itself is rendered as a `<div>`; you may want to specify additional classes or styles for it to achieve the layout that you desire.  It need not appear inside a form nor target input elements; that's just a convenient example.

The element will be focused on first render only; after that it's up to the user where focus goes.  This means that this method may not work if you want to determine focus based on some asynchronous data, as the first render will occur before asynchronous actions complete.

Dynamic usage is similar (provided that the state is known by first render):

```html
<EditForm Model="...">
    <AutoFocus class="...">
        <InputText autofocus="@_FocusFirst" ... />
        <InputText autofocus ... />
    </AutoFocus>
</EditForm>

@code {
	private bool _FocusFirst;
}
```

This will focus the first field if `_FocusFirst` has been set to `true` by first render, or the second field otherwise (the second needs no condition, since the first matching element will win).

### Via service

If you need more control than the above provides (for example, to set focus when dynamically showing/hiding modals, tabs, or other such content), you can instead inject the `AutoFocusService`:

```c#
@inject AutoFocusService AutoFocus
```

or

```c#
[Inject] private AutoFocusService AutoFocus { get; set; } = default!;
```

At the time when you want to set focus, call the `AutoFocus` method on this service:

```c#
await AutoFocus.AutoFocus(parent);
```

The `parent` should be an `ElementReference` of the parent element to search within for an `autofocus` element (*not* the element to be focused itself!).  You may pass `null` (or omit the parameter entirely) to search the entire page instead of a subtree.

Note that this should typically be called from `OnAfterRenderAsync` or another event where you know that the element that you want to focus has already been rendered -- and you need to ensure that this only happens once (or once per some user-significant action, such as opening a modal) to avoid getting focus "stuck" at one specific element instead of letting the user move it around as desired.