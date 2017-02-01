using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Server_Lab_2_KPP;
using ZedGraph;


namespace Client__Lab_2
{
    public partial class Form1 : Form
    {
        MainClass obj;
        public Form1()
        {
            InitializeComponent();
            //вынимаем наши методы с MainClass obj
            obj = (MainClass)Activator.CreateInstance(Type.GetTypeFromProgID("Server_Lab_2_KPP.MainClass"));
            List<double[]> list = new List<double[]>();
            list.AddRange(obj.GetAllPoint(-2, 10));
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
            pane.Title.Text = "Значения функции на отрезке [-2;10]";
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
                double y = obj.GetPoint(Math.Round(curve[index].X, 5));
                label.BackColor = pane.Fill.Color;
                label.Text = string.Format("x = {0}, y = {1}", Math.Round(curve[index].X, 5), y);
            }
        }
    }

}
