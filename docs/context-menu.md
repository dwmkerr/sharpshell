# Shell Context Menus

Shell Context Menus are Shell Extensions that add to the context menu opened when the user right clicks on a Shell Item. A Shell Item might be a file, folder, drive, network share or so on.  

Shell Context Menus are fully supported in SharpShell. This section describes how to implement them.

## Creating Shell Context Menus

Create a C# class library project. Ensure the class library is signed with a strong name. Add a reference to 'SharpShell' (the SharpShell core library is available from the Downloads page, or can be found on Nuget with a search for 'SharpShell'). If the library has been referenced manually, also add references to:

- `System.Drawing`
- `System.Windows.Forms`


If the library has been installed with Nuget, these references will have been added automatically.  

Create a class that derives from `SharpContextMenu`. Ensure the class has the `ComVisible` attribute set to true. Also add a `COMServerAssociation` attribute to specify what types of shell item this context menu should be available for. You can find full documentation on this attribute on the page [COM Server Associations](./com-server-associations.md).  

At this stage, you should have a class that looks like this:  

```csharp
[ComVisible(true)]
[COMServerAssociation(AssociationType.ClassOfExtension, ".txt")]
public class ExampleShellExtension : SharpContextMenu
{
}
```

Finally, there are two override that must be implemented.  

**CanShowMenu**  

This function is called to determine whether the shell extension should be visible for a set of items. The items selected by the user are stored in the property `SelectedItemPaths`.  

```csharp
protected override bool CanShowMenu()
{
    //  Depending on the files in 'SelectedItemPaths' you might not show the menu.
    return true;
}
```

**CreateMenu**  

This function is called to actually create the context menu strip to add to the shell context menu. An example is below:  

```csharp
protected override ContextMenuStrip CreateMenu()
{
    //  Create the menu strip.
    var menu = new ContextMenuStrip();

    //  Create an item.
    var itemCountLines = new ToolStripMenuItem
                             {
                                 Text = "Do something ...",
                                 Image = Properties.Resources.CountLines
                             };

    //  Add a handler for the click event.
    itemCountLines.Click += (sender, args) => MessageBox.Show("Do something");

    //  Add the item to the context menu.
    menu.Items.Add(itemCountLines);

    //  Return the menu.
    return menu;
}
```

## Icons

As a note, if the source of a context menu item's icon is a file format that supports transparency, such as `*.png`, then the icon itself will render correctly with transparency, as the screenshot below shows (left side with the mouse out, right side with the mouse over):

![Context Menu Screenshot](./context-menu-screenshot.png)

## Troubleshooting

**I cannot see the context menu extension, or it does not activate properly, when the user has selected more than 15 items**

By default the Windows Shell disables context menu items when more than a certain number of items are specified. This is discussed here:

- https://support.microsoft.com/en-us/help/2022295/context-menus-are-shortened-when-more-than-15-files-are-selected

A potential work-around is to change the `MultipleInvokePromptMinimum` registry key, which is located at `HKCU\Software\Microsoft\Windows\CurrentVersion\Explorer`, to a value higher than 15. More details are noted here:

- https://www.sevenforums.com/tutorials/131470-context-menu-items-missing-fix-when-more-than-15-files-selected.html

However, please note that this workaround might have performance issues. A more detailed discussion is in issue [#4](https://github.com/dwmkerr/sharpshell/issues/4).
