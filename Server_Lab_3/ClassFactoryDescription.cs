using System;
using System.Runtime.InteropServices;

namespace Server_Lab_3_KPP
{

    [ClassInterface(ClassInterfaceType.None)]

    [ComVisible(true)]

    public class ClassFactory : IClassFactoryServer
    {

        public int CreateInstance(IntPtr pUnkOuter, ref Guid riid, out IntPtr ppvObject)
        {

            ppvObject = IntPtr.Zero;

            if (pUnkOuter != IntPtr.Zero)
            {

                Marshal.ThrowExceptionForHR(ComAPI.CLASS_E_NOAGGREGATION);
            }

            if (riid == new Guid(Server.Guid) || riid == new Guid(ComAPI.guidIUnknown))
            {
                ppvObject = Marshal.GetComInterfaceForObject(new Server(), typeof(IServer));
            }

            else
            {
                Marshal.ThrowExceptionForHR(ComAPI.E_NOINTERFACE);
            }

            return 0;

        }

        public int LockServer(bool lockIt)
        {
            return 0;
        }

    }
}
