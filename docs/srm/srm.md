# The Server Registration Manager Tool

The Server Registration Manager tool (`srm.exe`) is a standalone console application which can be used to perform administrative tasks for SharpShell, such as installing or uninstalling servers, and changing configuration.

<!-- vim-markdown-toc GFM -->

* [Installing / Uninstalling Servers](#installing--uninstalling-servers)
* [Showing SharpShell Config](#showing-sharpshell-config)
* [Setting SharpShell Config](#setting-sharpshell-config)

<!-- vim-markdown-toc -->

## Installing / Uninstalling Servers

Install:

```
srm install <path.dll>
```

Install (without using the GAC)

```
srm install <path.dll> -codebase
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

These configuration settings are described in more detail in the [Logging Documentation](../logging/logging.md).

