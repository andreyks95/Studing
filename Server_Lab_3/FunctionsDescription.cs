using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Server_Lab_3_KPP
{

    [ClassInterface(ClassInterfaceType.None),
    Guid("BB78D399-0145-426c-8473-A04CCAA36811"),
    ComVisible(true),
    ProgId("Program.Server")]

    public class Server : IServer
    {
        public Server(){}
        ~Server(){}

        [ComVisible(true)]
        public static string Guid = "BB78D399-0145-426c-8473-A04CCAA36811";

        public double GetPoint(double x)
        {
            return (4*x*x-1)/(x+3);
        }

        public IEnumerable<double[]> GetAllPoint(int start, int end)
        {
            for (double x = start; x <= end; x += 0.01)
            {
                yield return new double[] { x, GetPoint(x) };
            }
        }

    }

}
