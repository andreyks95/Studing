using System;
using System.Runtime.InteropServices;

namespace Server_Lab_3_KPP
{
    [ComVisible(true)]
    public class ComAPI
    {
        [DllImport("OLE32.DLL")]
        public static extern UInt32 CoInitializeSecurity(
            IntPtr securityDescriptor,
            Int32 cAuth,
            IntPtr asAuthSvc,
            IntPtr reserved,
            UInt32 AuthLevel,
            UInt32 ImpLevel,
            IntPtr pAuthList,
            UInt32 Capabilities,
            IntPtr reserved3);

        [DllImport("ole32.dll")]
        public static extern UInt32 CoRegisterClassObject(
        ref Guid rclsid,
        [MarshalAs(UnmanagedType.Interface)]IClassFactoryServer pUnkn, int dwClsContext,
        int flags, out int lpdwRegister);

        [DllImport("ole32.dll")]
        public static extern UInt32 CoRevokeClassObject(int dwRegister);
        public const int RPC_C_AUTHN_LEVEL_PKT_PRIVACY = 6; // Encrypted DCOM communication
        public const int RPC_C_IMP_LEVEL_IDENTIFY = 2; // No impersonation really required
        public const int CLSCTX_LOCAL_SERVER = 4;
        public const int REGCLS_MULTIPLEUSE = 1;
        public const int EOAC_DISABLE_AAA = 0x1000; // Disable Activate-as-activator
        public const int EOAC_NO_CUSTOM_MARSHAL = 0x2000; // Disable custom marshalling
        public const int EOAC_SECURE_REFS = 0x2; // Enable secure DCOM references
        public const int CLASS_E_NOAGGREGATION = unchecked((int)0x80040110);
        public const int E_NOINTERFACE = unchecked((int)0x80004002);
        public const string guidIClassFactory = "00000001-0000-0000-C000-000000000046";
        public const string guidIUnknown = "00000000-0000-0000-C000-000000000046";
    }
}
