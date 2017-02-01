using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.EnterpriseServices;

//[assembly: Guid("49E484AE-02C4-4c77-A36E-7B4ED0BCE11F")]
//[assembly: ApplicationActivation(ActivationOption.Server)]
[assembly: ApplicationID("49E484AE-02C4-4c77-FFFF-7B4ED0BCE11F")]
[assembly: ApplicationName("Server_Lab_2_KPP")]
[assembly: ApplicationActivation(ActivationOption.Server)]


namespace Server_Lab_2_KPP
{
    [ComVisible(false), Guid("611D38CE-6F35-4da3-8690-417020F5CDD8"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IMyFunctionClass
    {
        [DispId(0)]
        double GetPoint(double x);
        [DispId(1)]
        IEnumerable<double[]> GetAllPoint(int startX, int endX);
    }

    //этот будет зарегистрирован в реестре 
    [ComVisible(true), Guid("8AD40DB1-8AA4-4f8f-94A6-FAB22454E7A9"), ClassInterface(ClassInterfaceType.None), ProgId("Server_Lab_2_KPP.MainClass")]
    public class MainClass : IMyFunctionClass
    {
        public MainClass() { }
        public double GetPoint(double x)
        {
            return (4 * x * x - 1) / (x + 3);
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

