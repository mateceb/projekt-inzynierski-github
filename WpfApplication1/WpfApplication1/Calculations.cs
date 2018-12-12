using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1
{
    class Calculations
    {
        public static decimal Average(List<int> list)
        {
            int sum = 0;
            int elementsNumber = 0;
            foreach (int x in list)
            {
                elementsNumber++;
                sum += x;
            }
            return (decimal)sum / (decimal)elementsNumber;
        }

        public static decimal Variance(List<int> list)
        {
            int elementsNumber = 0;
            double avg = (double)Average(list);
            double sumofSquares = 0;

            foreach (int x in list)
            {
                elementsNumber++;
                sumofSquares += Math.Pow((x - avg), 2.0);
            }
            return (decimal)sumofSquares / (decimal)elementsNumber;
        }

        public static decimal Standard_deviation(List<int> list)
        {
            double variance = (double)Variance(list);

            return (decimal)Math.Sqrt(variance);
        }

        public static decimal AverageFound(List<TestResults> list)
        {
            int sum = 0;
            int elementsNumber = 0;
            foreach (TestResults oTest in list)
            {
                elementsNumber++;
                sum += oTest.Results;
            }
            return (decimal)sum / (decimal)elementsNumber;
        }

        public static decimal VarianceFound(List<TestResults> list)
        {
            int elementsNumber = 0;
            double avg = (double)AverageFound(list);
            double sumofSquares = 0;

            foreach (TestResults oTest in list)
            {
                elementsNumber++;
                sumofSquares += Math.Pow((oTest.Results - avg), 2.0);
            }
            return (decimal)sumofSquares / (decimal)elementsNumber;
        }

        public static decimal Standard_deviationFound(List<TestResults> list)
        {
            double variance = (double)VarianceFound(list);

            return (decimal)Math.Sqrt(variance);
        }
    }
}
