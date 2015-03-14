using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GravitationalClustering.Algorithms
{
    public interface IDistance
    {
        double Compute(double[] left, double[] right);
    }
}
