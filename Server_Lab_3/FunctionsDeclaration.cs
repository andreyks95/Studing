using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Server_Lab_3_KPP
{
    [ComVisible(true),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
    Guid("A50602C0-5BB7-4b51-9534-D479FFBF1CE4")]

    public interface IServer
    {
        double GetPoint(double x);
        IEnumerable<double[]> GetAllPoint(int start, int end);
    }

}
