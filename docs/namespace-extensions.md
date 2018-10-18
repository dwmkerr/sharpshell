# Namespace Extensions

Namespace extensions are currently work in progress.

## Notes

When I change the GitHub namespace extension to have MyCoumpter as the virtual junction point but for current user, an exception is thrown trying to open the correct key to create the registry entries as children of.

This is possibly because there are no per-user namespace extensions already installed, so part of the path to the required location might be missing - or it might be that some junction points are not supported for the current user. If that's the case, we need to throw an exception when attempting to register with such a configuration, informing the caller that it's not supported. If it's just that path elements are missing, we can create them as needed (and if that's the case, this bug will crop up for others when installing on vanilla machines).

---

Shell namespace extensions have more options than are exposed for registration, e.g:

WantsFORPARSING
HideAsDelete
HideAsDeletePerUser
QueryForOverlay
support custom verbs with a 'Shell' subkey.
support custom shortcut menu handler with ShellEx.
tie in support for a property sheet handler.

---

Currently we only support virtual junction points for shell namespace extensions. We can support file system junction points too, see the:

`SharpNamespaceExtension.CustomRegisterFunction`

And take it from there.

