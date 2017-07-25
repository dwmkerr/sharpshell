COM Server Assocations are attributes that are applied to a SharpShell COM Server to specify what file types, classes or special objects the COM server should be associated with. A COM Server Association is specified on the COM Server class, just like the example below:

{{
[COMServerAssociation(AssociationType.ClassOfExtension, ".dll")](COMServerAssociation(AssociationType.ClassOfExtension,-_.dll_))
public class DllIconHandler : SharpIconHandler
}}

In this example, the DllIconHandler COM server is associated with the class of *.dll files.

Each different association type is described below.

## File Extensions

You can associate a COM server with a specific file extension or set of file extensions only. Examples:

{{
// Associate the CustomIconHandler server with batch files.
[COMServerAssociation(AssociationType.FileExtension, ".bat")](COMServerAssociation(AssociationType.FileExtension,-_.bat_))
public class CustomIconHanlder : SharpIconHandler
}}

{{
// Associate the CustomIconHandler server with jpeg files.
[COMServerAssociation(AssociationType.FileExtension, ".jpg". ".jpeg")](COMServerAssociation(AssociationType.FileExtension,-_.jpg_.-_.jpeg_))
public class CustomIconHanlder : SharpIconHandler
}}

**Important** Testing on Windows 7 and Windows 8 shows that the system does not normally respect such associations - they must be made on the _class_ of an extension, rather than the extension itself.

## Classes of Files

This is the most typical type of association that is made. In this case, we ask the system to register the server for a class, and rather than specifying the class, specify a file in it. For example, if we want to register for JPEG types, we could register for the class of '**.jpg', which will resolve to 'jpgfile' - meaning the server will register for **.jpg, **.jpeg and any other extensions that are classified as jpgfile.

Here are the examples:

{{
// Associate the CustomIconHandler server with the class of jpg files.
[COMServerAssociation(AssociationType.ClassOfExtension, ".jpg")](COMServerAssociation(AssociationType.ClassOfExtension,-_.jpg_))
public class CustomIconHanlder : SharpIconHandler
}}

{{
// Associate the CustomIconHandler server with the class of jpg and png.
[COMServerAssociation(AssociationType.ClassOfExtension, ".jpg". ".png")](COMServerAssociation(AssociationType.ClassOfExtension,-_.jpg_.-_.png_))
public class CustomIconHanlder : SharpIconHandler
}}

## Specific Classes

If you know the class you want to associate with it, you can use the 'Class' association, as in the example below.

{{
// Associate the CustomIconHandler server with the dlls.
[COMServerAssociation(AssociationType.Class, "dllfile")](COMServerAssociation(AssociationType.Class,-_dllfile_))
public class CustomIconHanlder : SharpIconHandler
}}

## All Files

To associate a server with all files in the system, use the 'AllFiles' association:

{{
// Associate the CustomIconHandler server with the all files.
[COMServerAssociation(AssociationType.AllFiles)](COMServerAssociation(AssociationType.AllFiles))
public class CustomIconHanlder : SharpIconHandler
}}

## Directories

To associate a server with all folders in the system, use the 'Directory' association:

{{
// Associate the CustomIconHandler server with the all folders.
[COMServerAssociation(AssociationType.Directory)](COMServerAssociation(AssociationType.Directory))
public class CustomIconHanlder : SharpIconHandler
}}

## Drives

To associate a server with all drives in the system, use the 'Drive' association:

{{
// Associate the CustomIconHandler server with the all drives.
[COMServerAssociation(AssociationType.Drive)](COMServerAssociation(AssociationType.Drive))
public class CustomIconHanlder : SharpIconHandler
}}

## Unknown Files

To associate a server with all unknown file types, use the 'UnknownFiles' association:

{{
// Associate the CustomIconHandler server with the all unknown file types.
[COMServerAssociation(AssociationType.UnknownFiles)](COMServerAssociation(AssociationType.UnknownFiles))
public class CustomIconHanlder : SharpIconHandler
}}