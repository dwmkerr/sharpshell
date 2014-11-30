## Version 2.2

 * SharpShell is now built with Visual Studio 2013 Community Edition.
 * Overhauled the logging mechanism.
 * Fixed preview handler bugs #33, #58, #50, #52, #56.
 * Preview handlers no longer flicker.


### Breaking Changes

 * `Logging.DebugLog` and `Logging.DebugError` are no longer available. Logging is 
   enabled based on configuration in the registry rather than debug mode. Use 
   `Logging.Log` or `Logging.Error` instead.
 * `Logging` class has no facility to enable/disable logging.
 * Preview Handlers MUST be decorated with the `PreviewHandler` attribute. This
   pattern will be implemented for other extensions in time.

### srm

* Show SharpShell config on the machine with `srm config`.

### Development

* The SharpShell assembly embedded into SRM is updated automatically during the build.