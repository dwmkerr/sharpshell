# Installing and Uninstalling SharpShell Servers

Internally, SharpShell servers are simply assemblies which contain a COM server. This means that SharpShell servers can be installed using standard tools like `regasm`. However, to simplify this process, there is also a dedicated tool which has been developed to manage the installation of SharpShell servers.

This guide covers how to install and uninstall SharpShell Servers.


<!-- vim-markdown-toc GFM -->

* [Core Concepts](#core-concepts)
* [Installing with regasm](#installing-with-regasm)
    * [Install (using the GAC)](#install-using-the-gac)
    * [Install (not using the GAC)](#install-not-using-the-gac)
    * [Uninstall](#uninstall)
* [Installing with srm](#installing-with-srm)
    * [Install (using the GAC)](#install-using-the-gac-1)
    * [Install (not using the GAC)](#install-not-using-the-gac-1)
    * [Uninstall](#uninstall-1)
* [Troubleshooting Server Installation](#troubleshooting-server-installation)
    * [Get Installation Logs](#get-installation-logs)
    * [Ensure your server is COM Visible](#ensure-your-server-is-com-visible)
    * [Ensure you register with the correct bitness](#ensure-you-register-with-the-correct-bitness)

<!-- vim-markdown-toc -->

## Core Concepts

For a SharpShell server to be installed, it must be registered. Servers can be registered in two ways:

1. By installing them into the Global Assembly Cache, then registering them as COM servers.
2. By leaving them as loose files in the filesystem, then registering them with the `/codebase` option.

Often teams prefer the second option as it allows them to keep all of their application files together in the Program Files folder.

COM Servers can be 32 or 64 bit. Most shell extensions will run as part of the `explorer.exe` process (i.e. they are 'in-proc' servers). This means that in general, you must register your server with the same bitness as the operating system.

## Installing with regasm

You can install a SharpShell server with the [`regasm`](https://docs.microsoft.com/en-us/dotnet/framework/tools/regasm-exe-assembly-registration-tool) tool:

### Install (using the GAC)

First install the assembly into the GAC with [`gacutil`](https://docs.microsoft.com/en-us/dotnet/framework/app-domains/how-to-install-an-assembly-into-the-gac), then register:

```
gacutil -i ExampleContextMenuExtension.dll
regasm ExampleContextMenuExtension.dll
```

### Install (not using the GAC)

Install, using the `/codebase` flag:

```
regasm /codebase ExampleContextMenuExtension.dll
```

### Uninstall

Uninstall:

```
regasm /u ExampleContextMenuExtension.dll
```

## Installing with srm

The Server Registration Manager (`srm.exe`) is a command line tool that allows you to perform various installation and diagnostic activities for SharpShell servers. You can use `srm` to install or uninstall servers, check diagnostic settings and change the SharpShell settings.

See also: [The Server Registration Manager Tool](../srm/srm.md)

### Install (using the GAC)

Install servers with `gacutil` then use the verb `install`:

```
gacutil -i <serverpath>
srm install <serverpath>
```

### Install (not using the GAC)

Install using the `-codebase` flag:

```
srm install <serverpath> -codebase
```

### Uninstall

To uninstall a server, just use the `uninstall` verb:

```
srm uninstall <serverpath>
```

## Troubleshooting Server Installation

There are a number of issues which can occur which can lead to server installation failing. A failed installation will mean the shell extension is not loaded and not visible.

The following steps should be taken to troubleshoot server installations.

### Get Installation Logs

Use `srm` to enable logging, then attempt to run the installation again, then check the log output:

```
srm config LoggingMode File
srm config LogPath '%TEMP%\SharpShell.log'
srm install <path.dll>
srm config LoggingMode Disable
```

See also: [Logging](../logging/logging.md)

### Ensure your server is COM Visible

If your server is not COM visible, the `regasm` (or `srm`, which uses `regasm` under the hood) will show an error message like:

```
RA0000 : No types were registered error.
```

There are number of things you must do to make sure your server is COM visible.

**Make sure that your server class has the `[ComVisible(true)]` attribute**

Good:

```csharp
[ComVisible(true)]
[COMServerAssociation(AssociationType.ClassOfExtension, ".txt")]
public class CountLinesExtension : SharpContextMenu
```

Bad - no `ComVisible` attribute:

```csharp
[COMServerAssociation(AssociationType.ClassOfExtension, ".txt")]
public class CountLinesExtension : SharpContextMenu
```

**Make sure your assembly is COM Visibile**

This is *not required* if you are explicitly making your server visible, as described above. However, if you choose to make all types COM visible, you can set this option in `Properties\AssemblyInfo.cs`:

```csharp
[assembly: ComVisible(true)]
```

**Make sure your server has a public default constructor**

COM will always call the public default constructor to instantiate the type. If this is not present, or the default constructor is not public, the server will not register.

Good:

```csharp
[ComVisible(true)]
[COMServerAssociation(AssociationType.ClassOfExtension, ".txt")]
public class CountLinesExtension : SharpContextMenu
{
    //  Good - this class has an implicit public constructor.
}
```

Bad:

```csharp
[ComVisible(true)]
[COMServerAssociation(AssociationType.ClassOfExtension, ".txt")]
public class CountLinesExtension : SharpContextMenu
{
    protected CountLinesExtension()
    {
        //  Bad - there is no public constructor.
    }
}
```

**Double check the Microsoft Guidelines**

If none of the above solutions has worked, double check you have followed all of the guidelines at:

[Microsoft - Exposing .NET Framework Components to COM](https://docs.microsoft.com/en-us/dotnet/framework/interop/exposing-dotnet-components-to-com)

### Ensure you register with the correct bitness

If you are on Windows 32 bit, make sure you register the server with an x86 version of `regasm`, e.g:  

```
C:\Windows\Microsoft.NET\Framework\v4.0.30319\regasm 
```

If you are on Windows 64 bit, make sure you register the server with an x64 version of `regasm`, e.g:

```
C:\Windows\Microsoft.NET\Framework64\v4.0.30319\regasm
```

If you are using `srm`, the bitness will be assumed based on the bitness of the calling program. You can also enforce a specific bitness with the `-os32` or `-os64` flags:

```
srm install -os32
```
