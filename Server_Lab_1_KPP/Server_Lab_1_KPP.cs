using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Server_Lab_1_KPP
{
    //Function
    [ComVisible(true), Guid("58834027-11F2-49c8-859F-0344C82A4A10")]
    public interface FunctionClass
    {
        [DispId(0)]
        double GetPoint(double x);

        [DispId(1)]
        IEnumerable<double[]> GetAllPoint(int startX, int endX);
    }

    // Events 
    [ComVisible(true), Guid("019DF86F-71EB-4ab2-A76B-3BEC5004F92D"), 
        InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface IMyEvents
    {
    }

    //Class
    [ComVisible(true), Guid("89F22426-5AA7-44f3-BB57-C1FA0A972740"), 
        ClassInterface(ClassInterfaceType.None), ComSourceInterfaces(typeof(IMyEvents)), 
        ProgId("Server_Lab_1_KPP.MainClass")]
    public class MainClass : FunctionClass
    {
        public MainClass() { }
        public double GetPoint(double x)
        {
            return (4 * x * x - 1) / (x + 3);
        }

        public IEnumerable<double[]> GetAllPoint(int begin, int end)
        {
            for (double x = begin; x <= end; x += 0.01)
            {
                yield return new double[] { x, GetPoint(x) };
            }
        }

    }
}
