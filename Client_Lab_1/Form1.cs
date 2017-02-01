using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using ZedGraph;

namespace Client_Lab_1
{
    public partial class Form1 : Form
    {
        object myObject = COMCreateObject("Server_Lab_1_KPP.MainClass");
        public Form1()
        {
            InitializeComponent();
            object[] param = new object[] {-2, 10};
            List<double[]> list = new List<double[]>();
            list.AddRange(
                (IEnumerable<double[]>)
                    myObject.GetType().InvokeMember("GetAllPoint", BindingFlags.InvokeMethod, null, myObject, param));
            PointPairList pList = new PointPairList(); //для x и y
            for (int i = 0; i < list.Count; i++)
            {
                pList.Add(list[i][0], list[i][1]); //впихиваем  туда значение с COM-сервера х и у
            }

            #region DrawGraph

            GraphPane pane = zedGraphControl.GraphPane;
            pane.CurveList.Clear();
            pane.XAxis.Scale.Min = pList[0].X;
            pane.XAxis.Scale.Max = pList[pList.Count - 1].X;
            LineItem myCurve = pane.AddCurve("Значения функции f(x) от x", pList, Color.Blue, SymbolType.None);
            pane.XAxis.Title.Text = "Значениe x";
            pane.YAxis.Title.Text = "Значение функции";
            pane.Title.Text = "Значения функции на отрезке [-2;10] ";
            pane.Fill = new Fill(Color.White, Color.LightSkyBlue, 45.0f);
            pane.Chart.Fill.Type = FillType.None;
            zedGraphControl.AxisChange();
            zedGraphControl.Invalidate();

            #endregion

        }

       

        public static object COMCreateObject(string sProgID)
        {
            Type oType = Type.GetTypeFromProgID(sProgID);
            if (oType != null)
            {
                return Activator.CreateInstance(oType);
            }
            return null;
        }

        //для обоозначение точек на графике и узнать по точкам координаты (для одной точки)
        private void zedGraphControl_MouseClick(object sender, MouseEventArgs e)
        {
            CurveItem curve;
            int index;
            GraphPane pane = zedGraphControl.GraphPane;
            GraphPane.Default.NearestTol = 10;
            bool result = pane.FindNearestPoint(e.Location, out curve, out index);
            if (result)
            {
                PointPairList point = new PointPairList();
                point.Add(curve[index]);
                LineItem curvePount = pane.AddCurve("", new double[] {curve[index].X}, new double[] {curve[index].Y},
                    Color.Red, SymbolType.Circle);
                curvePount.Line.IsVisible = false;
                curvePount.Symbol.Fill.Color = Color.Red;
                curvePount.Symbol.Fill.Type = FillType.Solid;
                curvePount.Symbol.Size = 5;
                zedGraphControl.Invalidate();
                double x = Math.Round(curve[index].X, 5);
                object[] param = new object[] {x};
                //узнаём координаты точки с com сервера
                double y =
                    (double)
                        myObject.GetType().InvokeMember("GetPoint", BindingFlags.InvokeMethod, null, myObject, param);
                label.BackColor = pane.Fill.Color;
                label.Text = string.Format("x = {0}, y = {1}", x, y);
            }

        }
    }
}
