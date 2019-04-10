# Troubleshooting SharpShell Servers

Creating Shell Extension Servers can be extremely difficult, as diagnosing issues is very hard. The most common problem encountered is that a server seems to work with the ServerManager tool, but simply doesn't show up at all in Windows Explorer. This page pulls together a detailed guide of how to troubleshoot servers.

**My Server Doesn't Show Up in Windows Explorer**

First, check the troubleshooting guide in the [Installing](../installing/installing.md) documentation.

**Important:** For most SharpShell servers to work on anything other than a development machine, they MUST be built in Release Mode against the Release Mode SharpShell binary. This binary uses an unmanaged C++ component that has a dependency to MSVCRTD100.dll in debug mode - this will NOT be present on none-development machines.

**Shell Thumbnail Handlers**

Shell thumbnail handlers have some specific things to be aware of:

1.  Do NOT use 'ClassOfExtension' COM server associations - you must register to a file extension.
2.  The Server Manager tool doesn't seem to respect alpha channels, but the system does.

**Shell Icon Overlay Handlers**

If you are not seeing your Shell Icon Overlay handlers after registering, double check which other icon handlers are registered, if you have more than a few then yours may not show up - the shell only supports about ten or so.

Overlays are registered at:

- `HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\ShellIconOverlayIdentifiers`
- `HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\explorer\ShellIconOverlayIdentifiers`

Check these nodes to make sure you do not have too many overlays already registered.

You can also use the `RegistrationName` attribute on a server to force a name to be used for the registration which will move the server to a high position in the list. For example:

```csharp
 [RegistrationName("  ReadOnlyFileIconOverlayHandler")] // push our way up the list by putting spaces in the name...
public class ReadOnlyFileIconOverlayHandler : SharpIconOverlayHandler
```

On 32 bit systems, it normally takes a restart if explorer.exe for the icon to be loaded. Apparently, this can actually be done programatically via SHChangeNotify.

If you see a cross icon instead of your icon, make sure the build action for the icon is set to "Resource". If this doesn't work, try copying it to the resource folder manually. (See [http://www.codeproject.com/Articles/545781/NET-Shell-Extensions-Shell-Icon-Overlay-Handlers?msg=4972010#xx4972010xx](http://www.codeproject.com/Articles/545781/NET-Shell-Extensions-Shell-Icon-Overlay-Handlers?msg=4972010#xx4972010xx)).

**Shell Property Sheet Extensions**

For property sheet extensions to work, make sure that the destination machine has the Visual C++ 2012 Redistributables installed (vcredist_x86 or vcredist_x64).

It seems that under some circumstances, Tab Controls in property sheet extensions can lead to unpredictable behaviour and crashes - currently I recommend against using them until the route cause of this issue is identified. More detail on this issue and the one above can be found here: [https://sharpshell.codeplex.com/discussions/470807](https://sharpshell.codeplex.com/discussions/470807)

**Context Menu Extensions**

It seems that sometimes creating the [ComServerAssociation] is not enough. <span style="font-size:10pt">When you add the registry key for the given extension, the context menu may be got from a different part of the registry for the users preference. For example, for 'png', u</span><span style="font-size:10pt">ou can determine this by checking the user choice:</span>

HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\FileExts.png\UserChoice   

This will give you the progid gives you the keyname to associate with the server. In this case, it might be WindowsLive.PhotoGallery.png.16.4.  

I did this manually and hey presto it works.

**The DirectoryBackground ComServerAssociation**

Note that if you have a `DirectoryBackground` COM Server Association, then the directory you are in will *not* be in the `SelectedItemPaths` (as it is not actually selected). To get this directory, simply use the `FolderPath` property. (See [#68](https://github.com/dwmkerr/sharpshell/issues/68#issuecomment-442073262) for more details).

**Preview Handlers**

If you find your preview handler for Directories is not working on a client machine, trying installing and registering it both **both **x86 and x64\. Thanks [Umut Ozel](https://github.com/umutozel)!

**The File Type Verifier**

The File Type Verifier tool from Microsoft can also be used to get some diagnostic reports on Shell Extensions associated with file types. Details are here: [http://msdn.microsoft.com/en-us/library/windows/desktop/hh127466](http://msdn.microsoft.com/en-us/library/windows/desktop/hh127466)
