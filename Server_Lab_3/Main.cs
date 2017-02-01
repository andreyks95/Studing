using System;
using System.Runtime.InteropServices;

namespace Server_Lab_3_KPP
{
    [ComVisible(true)]
    public partial class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            OnStart();
            OnStop();
            Console.ReadKey();    
        }

        private static int _cookie = 0;

        private static void OnStart()
        {
            Guid CLSID_MyObject = new Guid(Server.Guid);
            UInt32 hResult = ComAPI.CoRegisterClassObject(ref CLSID_MyObject, new ClassFactory(),
            ComAPI.CLSCTX_LOCAL_SERVER, ComAPI.REGCLS_MULTIPLEUSE, out _cookie);
            if (hResult != 0)
                throw new ApplicationException(
                 "CoRegisterClassObject failed" + hResult.ToString("X"));
            else
                Console.WriteLine("CoRegisterClassObject successfully!");
        }

        private static void OnStop()
        {
            if (_cookie != 0)
                ComAPI.CoRevokeClassObject(_cookie);
        }
    }
}
