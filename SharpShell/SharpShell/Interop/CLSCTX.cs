using System;

namespace SharpShell.Interop
{
    /// <summary>
    /// Values that are used in activation calls to indicate the execution contexts in which an object is to be run. These values are also used in calls to CoRegisterClassObject to indicate the set of execution contexts in which a class object is to be made available for requests to construct instances.
    /// </summary>
    [Flags]
    public enum CLSCTX : uint
    {
        /// <summary>
        /// The code that creates and manages objects of this class is a DLL that runs in the same process as the caller of the function specifying the class context.
        /// </summary>
        CLSCTX_INPROC_SERVER = 0x1,

        /// <summary>
        /// The code that manages objects of this class is an in-process handler. This is a DLL that runs in the client process and implements client-side structures of this class when instances of the class are accessed remotely.
        /// </summary>
        CLSCTX_INPROC_HANDLER = 0x2,

        /// <summary>
        /// The EXE code that creates and manages objects of this class runs on same machine but is loaded in a separate process space.
        /// </summary>
        CLSCTX_LOCAL_SERVER = 0x4,

        /// <summary>
        /// Obsolete.
        /// </summary>
        CLSCTX_INPROC_SERVER16 = 0x8,

        /// <summary>
        /// A remote context. The LocalServer32 or LocalService code that creates and manages objects of this class is run on a different computer.
        /// </summary>
        CLSCTX_REMOTE_SERVER = 0x10,

        /// <summary>
        /// Obsolete.
        /// </summary>
        CLSCTX_INPROC_HANDLER16 = 0x20,

        /// <summary>
        /// Reserved.
        /// </summary>
        CLSCTX_RESERVED1 = 0x40,

        /// <summary>
        /// Reserved.
        /// </summary>
        CLSCTX_RESERVED2 = 0x80,

        /// <summary>
        /// Reserved.
        /// </summary>
        CLSCTX_RESERVED3 = 0x100,

        /// <summary>
        /// Reserved.
        /// </summary>
        CLSCTX_RESERVED4 = 0x200,

        /// <summary>
        /// Disaables the downloading of code from the directory service or the Internet. This flag cannot be set at the same time as CLSCTX_ENABLE_CODE_DOWNLOAD.
        /// </summary>
        CLSCTX_NO_CODE_DOWNLOAD = 0x400,

        /// <summary>
        /// Reserved.
        /// </summary>
        CLSCTX_RESERVED5 = 0x800,

        /// <summary>
        /// Specify if you want the activation to fail if it uses custom marshalling.
        /// </summary>
        CLSCTX_NO_CUSTOM_MARSHAL = 0x1000,

        /// <summary>
        /// Enables the downloading of code from the directory service or the Internet. This flag cannot be set at the same time as CLSCTX_NO_CODE_DOWNLOAD.
        /// </summary>
        CLSCTX_ENABLE_CODE_DOWNLOAD = 0x2000,

        /// <summary>
        /// The CLSCTX_NO_FAILURE_LOG can be used to override the logging of failures in CoCreateInstanceEx.
        /// </summary>
        CLSCTX_NO_FAILURE_LOG = 0x4000,

        /// <summary>
        /// Disables activate-as-activator (AAA) activations for this activation only. This flag overrides the setting of the EOAC_DISABLE_AAA flag from the EOLE_AUTHENTICATION_CAPABILITIES enumeration. This flag cannot be set at the same time as CLSCTX_ENABLE_AAA. Any activation where a server process would be launched under the caller's identity is known as an activate-as-activator (AAA) activation. Disabling AAA activations allows an application that runs under a privileged account (such as LocalSystem) to help prevent its identity from being used to launch untrusted components. Library applications that use activation calls should always set this flag during those calls. This helps prevent the library application from being used in an escalation-of-privilege security attack. This is the only way to disable AAA activations in a library application because the EOAC_DISABLE_AAA flag from the EOLE_AUTHENTICATION_CAPABILITIES enumeration is applied only to the server process and not to the library application.
        ///     Windows 2000:  This flag is not supported.
        /// </summary>
        CLSCTX_DISABLE_AAA = 0x8000,

        /// <summary>
        /// Enables activate-as-activator (AAA) activations for this activation only. This flag overrides the setting of the EOAC_DISABLE_AAA flag from the EOLE_AUTHENTICATION_CAPABILITIES enumeration. This flag cannot be set at the same time as CLSCTX_DISABLE_AAA. Any activation where a server process would be launched under the caller's identity is known as an activate-as-activator (AAA) activation. Enabling this flag allows an application to transfer its identity to an activated component.
        ///     Windows 2000:  This flag is not supported.
        /// </summary>
        CLSCTX_ENABLE_AAA = 0x10000,

        /// <summary>
        /// Begin this activation from the default context of the current apartment.
        /// </summary>
        CLSCTX_FROM_DEFAULT_CONTEXT = 0x20000,

        /// <summary>
        /// Activate or connect to a 32-bit version of the server; fail if one is not registered.
        /// </summary>
        CLSCTX_ACTIVATE_32_BIT_SERVER = 0x40000,

        /// <summary>
        /// Activate or connect to a 64 bit version of the server; fail if one is not registered.
        /// </summary>
        CLSCTX_ACTIVATE_64_BIT_SERVER = 0x80000,

        /// <summary>
        /// Inproc combination.
        /// </summary>
        CLSCTX_INPROC = CLSCTX_INPROC_SERVER | CLSCTX_INPROC_HANDLER,

        /// <summary>
        /// Server combination.
        /// </summary>
        CLSCTX_SERVER = CLSCTX_INPROC_SERVER | CLSCTX_LOCAL_SERVER | CLSCTX_REMOTE_SERVER,

        /// <summary>
        /// All.
        /// </summary>
        CLSCTX_ALL = CLSCTX_SERVER | CLSCTX_INPROC_HANDLER
    }
}