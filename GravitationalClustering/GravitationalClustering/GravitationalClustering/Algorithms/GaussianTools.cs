using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GravitationalClustering.Algorithms
{
    public static class GaussianTools
    {
        private const double invsqrt2 = 1.0 / (2 * Math.PI);

        public static double CalculatePDFNormalized(double r,double sig)
        {
            return GaussianTools.invsqrt2 * Math.Exp(-Math.Pow(r, 2) / (2 * sig * sig)) / sig;
        }
        public static double CalculateRelativePDF(double r,double sig)
        {
            return Math.Exp(-Math.Pow(r, 2) / (2 * sig * sig));
        }
    }
}
