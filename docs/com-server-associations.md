# COM Server Associations

COM Server Associations are attributes that are applied to a SharpShell COM Server to specify what file types, classes or special objects the COM server should be associated with. A COM Server Association is specified on the COM Server class, just like the example below:

```csharp
[COMServerAssociation(AssociationType.ClassOfExtension, ".dll")]
public class DllIconHandler : SharpIconHandler
```

In this example, the `DllIconHandler` COM server is associated with the class of `*.dll` files.

Each different association type is described below.

## File Extensions

You can associate a COM server with a specific file extension or set of file extensions only. Examples:

```csharp
// Associate the CustomIconHandler server with batch files.
[COMServerAssociation(AssociationType.FileExtension, ".bat")]
public class CustomIconHanlder : SharpIconHandler
```

and:

```csharp
// Associate the CustomIconHandler server with jpeg files.
[COMServerAssociation(AssociationType.FileExtension, ".jpg". ".jpeg")]
public class CustomIconHanlder : SharpIconHandler
```

**Important** Testing on Windows 7 and Windows 8 shows that the system does not normally respect such associations - they must be made on the _class_ of an extension, rather than the extension itself.

## Classes of Files

This is the most typical type of association that is made. In this case, we ask the system to register the server for a class, and rather than specifying the class, specify a file in it. For example, if we want to register for JPEG types, we could register for the class of `*.jpg`, which will resolve to `jpgfile` - meaning the server will register for `*.jpg`, `*.jpeg` and any other extensions that are classified as `jpgfile`.

Here are the examples:

```csharp
// Associate the CustomIconHandler server with the class of jpg files.
[COMServerAssociation(AssociationType.ClassOfExtension, ".jpg")]
public class CustomIconHanlder : SharpIconHandler
```

and:

```csharp
// Associate the CustomIconHandler server with the class of jpg and png.
[COMServerAssociation(AssociationType.ClassOfExtension, ".jpg". ".png")]
public class CustomIconHanlder : SharpIconHandler
```

## Specific Classes

If you know the class you want to associate with it, you can use the 'Class' association, as in the example below.

```csharp
[COMServerAssociation(AssociationType.Class, "dllfile")]
public class CustomIconHanlder : SharpIconHandler
```

## All Files

To associate a server with all files in the system, use the `AllFiles` association:

```csharp
[COMServerAssociation(AssociationType.AllFiles)]
public class CustomIconHanlder : SharpIconHandler
```

## Directories

To associate a server with all folders in the system, use the `Directory` association:

```csharp
[COMServerAssociation(AssociationType.Directory)]
public class CustomIconHanlder : SharpIconHandler
```

## Directory Background

To associate a server with the background of a folder, use the `DirectoryBackground` association:

```csharp
[COMServerAssociation(AssociationType.DirectoryBackground)]
public class CustomIconHanlder : SharpIconHandler
```

## Desktop Background

To associate a server with the background of the desktop, use the `DesktopBackground` association:

```csharp
[COMServerAssociation(AssociationType.DesktopBackground)]
public class CustomIconHanlder : SharpIconHandler
```

## Drives

To associate a server with all drives in the system, use the `Drive` association:

```csharp
[COMServerAssociation(AssociationType.Drive)](COMServerAssociation(AssociationType.Drive))
public class CustomIconHanlder : SharpIconHandler
```

## Unknown Files

To associate a server with all unknown file types, use the `UnknownFiles` association:

```csharp
// Associate the CustomIconHandler server with the all unknown file types.
[COMServerAssociation(AssociationType.UnknownFiles)](COMServerAssociation(AssociationType.UnknownFiles))
public class CustomIconHanlder : SharpIconHandler
```
