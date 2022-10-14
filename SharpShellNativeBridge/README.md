# SharpShellNativeBridge

The SharpShellNativeBridge is a Win32 library that is used to host APIs that cannot be directly hosted in the .NET SharpShell library. At runtime, the core SharpShell Assembly loads this native bridge into memory and makes calls to it.

<!-- vim-markdown-toc GFM -->

* [Building](#building)

<!-- vim-markdown-toc -->

If you find this project useful, please consider [Sponsoring](https://github.com/sponsors/dwmkerr)!

This project uses the Windows 10 SDK. Windows 8.1 is no longer supported. However, if you install legacy SDKs you can re-target this project and manually build the bridge if needed.

SharpShell is currently developed in Visual Studio 2017, and can be built using the [Community Edition](https://visualstudio.microsoft.com/vs/community/).

In order to maximize compatibility, we do not use the latest version of each SDK. The following components are needed:

- Windows Universal CRT SDK
- Windows 10 SDK
- Windows Universal C Runtime

## Building

As long as the correct components have be installed for Visual Studio, you should be able to just open the main `SharpShellNativeBridge.sln` solution to build.

To build using Powershell run:

```ps1
./build.ps1
```

The CI/CD processes also use the `./build.ps1` script to build the project.

Be aware of the following nuances of the build process.

- `SharpNativeBridge` should be built in `x64` mode. When successful, the `x64` build will trigger a `x32` build, and both 32/64 bit binaries are copied to the `artifacts/build/SharpNativeBridge` folder.
- The core `SharpShell` assembly no longer takes the latest build of the native bridge automatically - you must build the project and embed the files in [../SharpShell/SharpShell/NativeBridge](../SharpShell/NativeBridge) to update the native bridge
