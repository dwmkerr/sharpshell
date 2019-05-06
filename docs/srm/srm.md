# The Server Registration Manager Tool

The Server Registration Manager tool (`srm.exe`) is a standalone console application which can be used to perform administrative tasks for SharpShell, such as installing or uninstalling servers, and changing configuration.

<!-- vim-markdown-toc GFM -->

* [Installing the SRM](#installing-the-srm)
* [Installing / Uninstalling Servers](#installing--uninstalling-servers)
* [Showing SharpShell Config](#showing-sharpshell-config)
* [Setting SharpShell Config](#setting-sharpshell-config)

<!-- vim-markdown-toc -->

## Installing the SRM

You can get the tool from the [ServerRegistrationManager NuGet Package](https://www.nuget.org/packages/ServerRegistrationManager).

It is also available in all [SharpShell Releases](https://github.com/dwmkerr/sharpshell/releases).

## Installing / Uninstalling Servers

Install:

```
srm install <path.dll>
```

Install (without using the GAC):

```
srm install <path.dll> -codebase
```

Install (with specific bitness, see [Installing and Uninstalling SharpShell Servers](https://github.com/dwmkerr/sharpshell/blob/master/docs/installing/installing.md)):

```
srm install <path.dll> [-codebase] -os32
srm install <path.dll> [-codebase] -os64
```

Uninstall:

```
srm uninstall <path.dll>
```

## Showing SharpShell Config

SharpShell config is stored locally in the registry. It is only used for diagnostic purposes, typically SharpShell config will not be present unless diagnosing issues. Show current config with:

```
srm config
```

## Setting SharpShell Config

Set SharpShell settings with the verb `config`:

```
srm config <setting> <value>
```

The following settings are supported:

| Setting              | Value                                |
| -------------------- | ------------------------------------ |
| `LoggingMode`        | Disabled, Debug, EventLog, File      |
| `LogPath`            | *Any file path*                      |

These configuration settings are described in more detail in the [Logging Documentation](../logging/logging.md). Note that logging options will normally require a restart of the `explorer.exe` process to take effect.

