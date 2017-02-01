using System;
using ZedGraph;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Server_Lab_3_KPP;


namespace Client_Lab_3
{
        public partial class Form1 : Form
        {
            object obj;
            public Form1()
            {
                InitializeComponent();
                Type type = Type.GetTypeFromProgID("Program.Server");
                ClassFactory cF = new ClassFactory();
                IntPtr ptr_object;
                Guid guid = type.GUID;
                cF.CreateInstance(IntPtr.Zero, ref guid, out ptr_object);
                obj = Marshal.GetObjectForIUnknown(ptr_object);
                List<double[]> list = new List<double[]>();
                object[] param = new object[] { -2, 10 };
                list.AddRange((IEnumerable<double[]>)obj.GetType().InvokeMember("GetAllPoint", BindingFlags.InvokeMethod, null, obj, param));
                PointPairList pList = new PointPairList();
                for (int i = 0; i < list.Count; i++)
                {
                    pList.Add(list[i][0], list[i][1]);
                }
                #region DrawGraph
                GraphPane pane = zedGraphControl.GraphPane;
                pane.CurveList.Clear();
                pane.XAxis.Scale.Min = pList[0].X;
                pane.XAxis.Scale.Max = pList[pList.Count - 1].X;
                LineItem myCurve = pane.AddCurve("Значения функции f(x) от x", pList, Color.Blue, SymbolType.None);
                pane.XAxis.Title.Text = "Значениe x";
                pane.YAxis.Title.Text = "Значение функции";
                pane.Title.Text = "Значения функции на отрезке от -2 до 10";
                pane.Fill = new Fill(Color.White, Color.LightSkyBlue, 45.0f);
                pane.Chart.Fill.Type = FillType.None;
                zedGraphControl.AxisChange();
                zedGraphControl.Invalidate();
                #endregion

            }

            private void zedGraphControl_MouseClick(object sender, MouseEventArgs e)
            {
                CurveItem curve;
                int index;
                GraphPane pane = zedGraphControl.GraphPane;
                int count = pane.CurveList.Count;
                if (count != 0)
                {
                    CurveItem item = pane.CurveList[0];
                    pane.CurveList.Clear();
                    pane.CurveList.Add(item);
                }
                GraphPane.Default.NearestTol = 10;
                bool result = pane.FindNearestPoint(e.Location, out curve, out index);
                if (result)
                {
                    PointPairList point = new PointPairList();
                    point.Add(curve[index]);
                    LineItem curvePount = pane.AddCurve("", new double[] { curve[index].X }, new double[] { curve[index].Y }, Color.Red, SymbolType.Circle);
                    curvePount.Line.IsVisible = false;
                    curvePount.Symbol.Fill.Color = Color.Red;
                    curvePount.Symbol.Fill.Type = FillType.Solid;
                    curvePount.Symbol.Size = 7;
                zedGraphControl.Invalidate();
                    double x = Math.Round(curve[index].X, 5);
                    object[] param = new object[] { x };
                    double y = (double)obj.GetType().InvokeMember("GetPoint", BindingFlags.InvokeMethod, null, obj, param);
                    label.BackColor = pane.Fill.Color;
                    label.Text = string.Format("x = {0}, y = {1}", x, y);
                }
            }
        }
    }


