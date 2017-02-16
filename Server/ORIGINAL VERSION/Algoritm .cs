using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Numerics;

namespace Server
{
    public class Algoritm 
    {
        private static double GetMax(List<double> array)
        {
            double max = double.IsNaN(array[0]) ? 0 : array[0];
            for (int i = 0; i < array.Count(); i++)
            {
                if (max < array[i])
                    max = array[i];
            }
            return max;
        }

        private static List<double> GetValuesXY(BigInteger startX, BigInteger endX, double stepX, BigInteger startY, BigInteger endY, double stepY)
        {
            List<double> valuesYX = new List<double>();
            for (double x = (double)startX; x <= (double)endX; x += stepX)
                valuesYX.AddRange(GetValuesY(startY, endY, stepY, x));
            return valuesYX;
        }

        private static List<double> GetValuesY(BigInteger startY, BigInteger endY, double stepY, double valueX)
        {
            List<double> valuesY = new List<double>();
            for (double y = (double)startY; y <= (double)endY; y += stepY)
                valuesY.Add(GetValueFunc(valueX, y));
            return valuesY;
        }

        private static double GetValueFunc(double x, double y)
        {
            return x * x + 2 * y;
        } 
    }
}