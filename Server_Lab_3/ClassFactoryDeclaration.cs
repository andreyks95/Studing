using System;
using System.Runtime.InteropServices;

namespace Server_Lab_3_KPP
{
    [ComVisible(true), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid(ComAPI.guidIClassFactory)]

    public interface IClassFactoryServer
    {
        [PreserveSig]
        int CreateInstance(IntPtr pUnkOuter, ref Guid riid, out IntPtr ppvObject);

        [PreserveSig]
        int LockServer(bool fLock);

    }

}
