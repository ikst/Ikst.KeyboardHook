# Ikst.KeyboardHook
This is a library to realize global keyboard hooks in .net windows applications.  
Events can be triggered for applications that do not have a screen, or for keyboard operations outside the window.

## usege
- Create an instance and use the start method to start the hook.
- The event is triggered by a keyboard operation.

[e.g]Displays the key name entered in the window title.
```c#
using Ikst.KeyboardHook;

public partial class MainWindow : Window
{
    KeyboardHook kh = new KeyboardHook();

    public MainWindow()
    {
        InitializeComponent();

        kh.KeyDown += (sender, e) =>
        {
            Title = e.Key.ToString();
        };

        kh.Start();
    }
}
```

## nuget
https://www.nuget.org/packages/Ikst.KeyboardHook/