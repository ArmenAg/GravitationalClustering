using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GravitationalClustering.Algorithms
{
    class SquaredEuclideanDistance : IDistance
    {
        public double Compute(double[] left, double[] right)
        {
            Trace.Assert(left != null);
            Trace.Assert(right != null);

            Trace.Assert(left.Length == right.Length);

            double sum = 0;

            for (int i = 0; i < left.Length; i++)
            {
                sum += Math.Pow(left[i] - right[i], 2.0);
            }
            return sum;
        }
    }
}
